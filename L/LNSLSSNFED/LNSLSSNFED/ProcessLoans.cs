using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace LNSLSSNFED
{
    public class ProcessLoans : Uheaa.Common.Scripts.ScriptBase
    {
        private const string pslfPattern = @"PSLF Transfer - R4*";

        private const string splitPattern = @"Split Loan Transfer - R4*";

        private const string scriptId = "LNSLSSNFED";
        Portfolios portfolio { get; set; }
        List<KeyValuePair<string, string>> loans { get; set; }
        private bool isDloPortfolio { get; set; }
        private bool hasErrored { get; set; }
        private string fileToDelete { get; set; }
        private ProcessLogData PLD { get; set; }
        private ProcessLogRun logRun { get; set; }

        public ProcessLoans(Uheaa.Common.Scripts.ReflectionInterface refInt)
            : base(refInt, scriptId, DataAccessHelper.Region.CornerStone)
        {
            portfolio = new Portfolios();
            logRun = new ProcessLogRun(this.ProcessLogData.ProcessLogId, scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false, false);
        }

        public override void Main()
        {
             
            DataSet ds = new DataSet();

            using (UserInput ui = new UserInput())
            {
                ui.ShowDialog();
                ds.saleId = ui.formSaleId;
                ds.splitChecked = ui.formSplit;
                ds.pslfChecked = ui.formPslf;
                ds.dloLoan = ui.formDlo;
                ds.lncLoan = ui.formLnc;
            }

            if (ds.saleId.IsNullOrEmpty() || ds.saleId.Length == 0)
            {
                MessageBox.Show($"Operation cancelled.");
                logRun.LogEnd();
                RI.CloseSession();
                return;
            }


            isDloPortfolio = ds.dloLoan ? true : false;

            string searchPattern = ds.splitChecked ? splitPattern : pslfPattern;
            string filePath = ds.splitChecked ? "LNSLSSNFEDSPLT" : "LNSLSSNFEDPSLF";

            loans = getExcelLoanData(searchPattern, filePath);
            if (loans.Count == 0)
            {
                logError($"No loans returned for file pattern {searchPattern}. Error reading file, or empty file.");
                MessageBox.Show($"Script has completed with errors. Please check Process Log entries under script LNSLSSNFED, logId {logRun.ProcessLogId}.");
                return;
            }

            RI.FastPath("TX3Z/CTS4P");
            if (RI.GetText(1, 4, 4) != "TS4P")
            {
                logError($"TS4P not available to current user, program will terminate.");
                return;
            }

            RI.PutText(5, 26, ds.saleId);
            RI.Hit(ReflectionInterface.Key.Enter);

            if (RI.GetText(22, 2, 5) == "01020")
            {
                logError($"Loan Sale Id {ds.saleId} was not found. Program will terminate.");
                return;
            }

            string portId = RI.GetText(12, 16, 20);
            if (isDloPortfolio)
            {
                if (!portId.Contains("DLO"))
                    logError($"User indicated DLO portfolio type, but the loan sale they selected was not of that portfolio type.");
            }
            else
            {
                
                if (!portId.Contains("LNC"))
                    logError($"User indicated LNC portfolio type, but the loan sale they selected was not of that portfolio type.");
            }


            RI.Hit(ReflectionInterface.Key.F4);
            if (RI.GetText(21, 20, 1) != "U")
            {
                logError($"The selection status for this loan sale is not Unapproved, so SSNs cannot be added. Change the selection status and then rerun the script.");
            }

            RI.Hit(ReflectionInterface.Key.F4);
            if (RI.GetText(22, 2, 5) == "01108")
            {
                logError($"Unable to go to Select Specific Loans screen.Loan sale may have been set up incorrectly.");
                return;
            }

            processSaleId(loans);

            if (hasErrored)
                MessageBox.Show($"Script has completed with errors. Please check Process Log ID {logRun.ProcessLogId} for LNSLSSNFED entry.");
            else
                MessageBox.Show("Script has completed.");

#if !DEBUG
            Repeater.TryRepeatedly(() => File.Delete(fileToDelete));
#endif
            logRun.LogEnd();
            RI.CloseSession();
        }


        // Main loop through loans.
        private void processSaleId(List<KeyValuePair<string, string>> loans)
        {
            foreach(var loan in loans)
            {
                if (!loan.Key.IsNumeric() || loan.Key.Length != 9)
                {
                    logError($"Data {loan.Key} is not a valid ssn.");
                    continue;
                }

                RI.Hit(ReflectionInterface.Key.F2);
                RI.PutText(10, 39, loan.Key);
                RI.Hit(ReflectionInterface.Key.Enter);
                if(RI.GetText(22, 2, 20) == "03914 NO LOANS FOUND")
                {
                    logError($"No loans were found for listed account {loan.Key}.");
                }

                bool success = markLoanSequence(loan.Key, loan.Value);

                RI.Hit(ReflectionInterface.Key.F12);
                RI.Hit(ReflectionInterface.Key.F4);

            }
        }


        // Loop through borrower loans and mark required sequences.
        private bool markLoanSequence(string ssn, string sequence)
        {
            int row = 9;
            int page = 1;
            
            while(row < 21)
            {

                if (RI.GetText(row, 17, 2) == "")   // Run out of loans for borrower.
                {
                    logError($"Ssn {ssn} sequence {sequence} not found. Search complete.");
                    return false;
                }

                if (RI.GetText(row, 17, 2) == sequence)
                {
                    // If the loan program matches the portfolio ID, add the loan.
                    string currentBalance = RI.GetText(row, 72, 5);
                    if (currentBalance == "$0.00")
                    {
                        logError($"SSN {ssn} sequence {sequence} has a zero balance.");
                        return false;
                    }

                    string portfolioListed = RI.GetText(row, 20, 6);
                    if (!correctPortfolio(portfolioListed))
                    {
                        string msg = isDloPortfolio ? " is NOT part of the DLO portfolio." : " is NOT part of the LNC portfolio.";
                        logError($"SSN {ssn} sequence {sequence} loan type {portfolioListed}: {msg}.");
                        return false;
                    }

                    RI.PutText(row, 3, "S");  
                    RI.Hit(ReflectionInterface.Key.Enter);
                    if(RI.GetText(22, 2, 5) != "02114") // Failure if not 02114
                    {
                        logError(RI.GetText(22, 2, 15));
                        return false;
                    }

                    return true;
                }

                row++;
                if(row == 21) 
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 9;
                    page++;
                }

                if(page == 21)
                {
                    RI.Hit(ReflectionInterface.Key.Enter);
                    if (RI.GetText(22, 2, 29) == "90007 NO MORE DATA TO DISPLAY")
                    {
                        logError($"Ssn {ssn} for sequence {sequence} not found. Search complete.");
                        return false;
                    }
                    else
                        page = 1;
                }
            }
            return false;
        }

        private void logError(string msg)
        {
            hasErrored = true;
            logRun.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }


        // Checks for portfolio match.
        private bool correctPortfolio(string currentPortfolio)
        {
            if (isDloPortfolio)
                return portfolio.DLO.ContainsKey(currentPortfolio);
            else
                return portfolio.LNC.ContainsKey(currentPortfolio);
        }

        private void hitF4Twice()
        {
            RI.Hit(ReflectionInterface.Key.F4);
            RI.Hit(ReflectionInterface.Key.F4);
        }


        // Check for and gather excel data.
        private List<KeyValuePair<string, string>> getExcelLoanData(string searchPattern, string pathKey)
        {
            List<KeyValuePair<string, string>> loans = new List<KeyValuePair<string, string>>();
            string loanSalePath = EnterpriseFileSystem.GetPath(pathKey);
            DirectoryInfo dir = new DirectoryInfo(loanSalePath);
            FileInfo[] files = dir.GetFiles(searchPattern);
            if(files.Length > 1)
            {
                logError($"Search for Excel file pattern {searchPattern} found {files.Length} files. Only a single file is permitted.");
                return loans;
            }
            else if (files.Length == 0)
            {
                logError($"Search for Excel file pattern {searchPattern} found 0 files. Please check there is an input file and it is in the right format, then rerun the script.");
                return loans;
            }

            Excel.Application excelApp = new Excel.Application();
            //excelApp.Visible = true;
            fileToDelete = files[0].FullName;
            Excel._Workbook workBook = excelApp.Workbooks.Open(files[0].FullName);
            foreach (Excel._Worksheet sheet in workBook.Worksheets)
            {
                Excel.Range range = sheet.UsedRange;
                loans = GetLoansForSheet(sheet);
            }

            workBook.Close(0);
            excelApp.Quit();
            return loans;
        }


        // Gather worksheets from excel.
        private List<KeyValuePair<string, string>> GetLoansForSheet(Excel._Worksheet sheet)
        {
            List<KeyValuePair<string, string>> loans = new List<KeyValuePair<string, string>>();
            string parse = (sheet.Cells[1, 1] as Excel.Range).Value.ToString();

            int rowStart = 2;
            try
            {
                int parsedInt = Int32.Parse(parse);
                if (parse.Length != 9)
                    return loans;

                rowStart = 1;
            }
            catch (Exception ex)
            {
            }


            for (int row = rowStart; sheet.Rows.CurrentRegion.EntireRow.Count >= row; row++)
            {
                string ssn = (sheet.Cells[row, 1] as Excel.Range).Value.ToString();
                string seq = (sheet.Cells[row, 2] as Excel.Range).Value.ToString();

                if (ssn.IsPopulated() && seq.IsPopulated())
                {
                    loans.Add(new KeyValuePair<string, string>(ssn, seq));
                }
            }

            return loans;
        }


    }
    }
