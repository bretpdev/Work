using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace PMTHIST
{
    public class PaymentHistory : ScriptBase
    {
        public const decimal EDTotalCurrent = 0.0009999m;
        public ProcessLogRun LogRun { get; set; }
        public string SSN { get; set; } = null;
        public PaymentHistory(ReflectionInterface ri) : base(ri, "PMTHIST", DataAccessHelper.Region.Uheaa)
        {
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
        }

        public PaymentHistory(ReflectionInterface ri, string ssn) : this(ri)
        {
            SSN = ssn;
        }

        public override void Main()
        {
            MessageBox.Show("This will run the csharp PMTHIST script.");
            try
            {
                Process();
            }
            finally
            {
                LogRun.LogEnd();
            }
        }

        public void Process()
        {
            RI.FastPath("LC41I");
            RI.PutText(7, 36, "X", ReflectionInterface.Key.Enter);
            
            if(RI.CheckForText(21,3,"40004"))
            {
                MessageBox.Show("Please enter an account number then hit insert.");
                RI.PauseForInsert();
            }

            while (RI.CheckForText(22, 3, "47004"))
            {
                RI.Hit(ReflectionInterface.Key.Insert);
                Thread.Sleep(1000);
                RI.Hit(ReflectionInterface.Key.Enter);
            }

            if (RI.CheckForText(22, 3, "47004"))
            {
                MessageBox.Show("There are no payments to report.", "No Payments", MessageBoxButtons.OK);
                LogRun.AddNotification("There are no payments to report.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return;
            }

            //Get balance information from LC10
            RI.FastPath("LC10I");

            //SSN will be set if the script is called through Collections MD
            if (SSN.IsNullOrEmpty())
            {
                SSN = RI.GetText(1, 9, 9);
            }
            LC10Data lc10Data = new LC10Data();

            if (RI.CheckForText(9, 10, "SOC"))
            {
                //This value is used when the account is assigned to ED and does not have a current balance
                lc10Data.TotalCur = EDTotalCurrent;
            }
            else
            {
                lc10Data.GetValuesFromLC10(RI);
            }

            //get purchase amount from LC05
            List<LC05Loan> loans = GetLoanInformation();

            //unable to get loans, message logged in GetLoanInformation
            if(loans == null)
            {
                return;
            }

            var demos = RI.GetDemographicsFromLP22(SSN);

            //This adds the rows to the rows object and returns an object containing some counters
            List<ExcelRow> rows = new List<ExcelRow>();
            var lc41Vars = ProcessLC41(loans, demos, rows);
            if (lc41Vars == null)
            {
                //failure, message logged inside method
                return;
            }

            while (lc41Vars.Pii != loans.Count && lc41Vars.Aii != lc41Vars.EDInd)
            {
                lc41Vars.EffectiveDate = "12/31/3000".ToDate();
                AddPurchaseOrAssignmentTransactions(lc41Vars, loans, rows);
            }
            //Write rows to excel
            CreateExcelFile(lc41Vars, rows, lc10Data);
        }

        public LC41Variables ProcessLC41(List<LC05Loan> loans, SystemBorrowerDemographics demos, List<ExcelRow> rows)
        {
            //Get payment information from LC41
            RI.FastPath("LC41I");
            RI.PutText(7, 36, "X", ReflectionInterface.Key.Enter);
            if (RI.CheckForText(1, 75, "SELECT"))
            {
                var lastPage = RI.GetText(2, 79, 2).Trim().Length == 1 ? "0" + RI.GetText(2, 79, 2).Trim() : RI.GetText(2, 79, 2).Trim();
                RI.PutText(2, 73, lastPage, Key.Enter); //Go to the last page
                RI.PutText(21, 13, "01", Key.Enter);
                while (!RI.CheckForText(22, 3, "46004"))
                {
                    RI.Hit(Key.F8);
                }
            }

            TransactionData transactionData = null;
            //rows = new List<ExcelRow>();
            LC41Variables vars = new LC41Variables();
            vars.OriginalPrincipal = 0;
            vars.LastEffectiveDate = "01/01/1900".ToDate();
            vars.Name = RI.GetText(2, 2, 40).Trim();
            vars.EDInd = loans.Where(p => p.EDDate.HasValue).Count();
            vars.AccountNumber = demos.AccountNumber;

            while (!RI.CheckForText(22, 3, "46003"))
            {
                //skip the first payment of no repur loan
                //This while seems irrelevant, going to remove for now
                while (RI.GetText(4, 72, 10).ToDate() < loans.First().PurchaseDate)
                {
                    RI.Hit(Key.F7);
                    if (RI.CheckForText(22, 3, "46003"))
                    {
                        MessageBox.Show("There are no payments to report for the current occurence of the loan(s).");
                        LogRun.AddNotification("There are no payments to report for the current occurence of the loan(s).", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        return null;
                    }
                }

                //skip bogus advices
                while (RI.CheckForText(4, 32, "ADVICE") && RI.GetText(15, 68, 12).ToInt() == 0 && !RI.CheckForText(22, 3, "46003"))
                {
                    RI.Hit(Key.F7);
                }
                if (RI.CheckForText(22, 3, "46003"))
                {
                    if (rows.Count == 0)
                    {
                        MessageBox.Show("The only transactions found for this borrower are advices that do not affect the account balance.  As a result, a payment history cannot be generated.", "Unable to Generate", MessageBoxButtons.OK);
                        LogRun.AddNotification("The only transactions found for this borrower are advices that do not affect the account balance.  As a result, a payment history cannot be generated.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        return null;
                    }
                    break;
                }

                vars.EffectiveDate = RI.GetText(4, 72, 8).ToDate();
                if (RI.CheckForText(6, 28, "POST ERROR") || RI.CheckForText(6, 28, "BAD CHECK"))
                {
                    //FullReversal
                    transactionData = FullReversal();
                }
                else if (RI.CheckForText(4, 28, "FEDERL TXO") && RI.CheckForText(6, 28, "INJ SPOUSE"))
                {
                    RI.Hit(Key.F7);
                    if (RI.CheckForText(4, 28, "FEDERL TXO") && RI.GetText(6, 28, 10).Trim() == "" && RI.GetText(4, 72, 8).ToDate() == vars.EffectiveDate)
                    {
                        RI.Hit(Key.F8);
                        var partType = "FOBC";
                        //PartialReversal
                        transactionData = PartialReversal(partType);
                    }
                    else if (RI.CheckForText(22, 3, "46003"))
                    {
                        //NormalTransaction
                        transactionData = NormalTransaction();
                    }
                    else
                    {
                        RI.Hit(Key.F8);
                        //FullReversal
                        transactionData = FullReversal();
                    }
                }
                else if (RI.CheckForText(4, 28, "FEDERL TXO") && RI.GetText(6, 28, 10).Trim() == "")
                {
                    RI.Hit(Key.F7);
                    if (RI.CheckForText(4, 28, "FEDERL TXO") && RI.CheckForText(6, 28, "INJ SPOUSE") && RI.GetText(4, 72, 8).ToDate() == vars.EffectiveDate)
                    {
                        var partType = "FO";
                        //PartialReversal
                        transactionData = PartialReversal(partType);
                        RI.Hit(Key.F7);
                    }
                    else if (RI.CheckForText(22, 3, "46003"))
                    {
                        //NormalTransaction
                        transactionData = NormalTransaction();
                    }
                    else
                    {
                        RI.Hit(Key.F8);
                        //NormalTransaction
                        transactionData = NormalTransaction();
                    }
                }
                else
                {
                    //NormalTransaction
                    transactionData = NormalTransaction();
                }

                //Check to see if purchase or assignment transactions need to be inserted
                AddPurchaseOrAssignmentTransactions(vars, loans, rows);

                //Go to previous payment
                RI.Hit(Key.F7);

                //add the payment to the spreadsheet
                //AddRow
                ExcelRow row = new ExcelRow();
                row.EffectiveDate = vars.EffectiveDate.Value.ToString("MM/dd/yyyy");
                row.PaymentType = transactionData.PmtType;
                row.PaymentAmount = transactionData.PmtAmt;
                row.OrigianlPrincipal = vars.OriginalPrincipal;
                row.Principal = transactionData.PrincCol;
                row.Interest = transactionData.IntCol;
                row.Legal = transactionData.Legal;
                row.Other = transactionData.Other;
                row.CollectionCosts = transactionData.CC;
                row.Overpayment = transactionData.OV;
                row.ReversalType = transactionData.RevType;
                rows.Add(row);

                //Subtract the principal collected from the beginning principal to get the new beginning principal balance
                vars.OriginalPrincipal = vars.OriginalPrincipal + transactionData.PrincCol; //princCol is negative so it is added
                vars.LastEffectiveDate = vars.EffectiveDate;
            }

            return vars;
        }

        public void AddPurchaseOrAssignmentTransactions(LC41Variables vars, List<LC05Loan> loans, List<ExcelRow> rows)
        {
            //insert purchase rows if the purchase insert indicator is less than the number of purchases
            if (vars.Pii < loans.Count)
            {
                for (int i = 0; i < loans.Count; i++)
                {
                    if (loans[i].PurchaseDate >= vars.LastEffectiveDate && loans[i].PurchaseDate <= vars.EffectiveDate)
                    {
                        ExcelRow row = new ExcelRow();
                        row.EffectiveDate = loans[i].PurchaseDate.ToString("MM/dd/yyyy");
                        row.PaymentType = "CLAIM PURCHASE";
                        row.PaymentAmount = loans[i].PurchAmt;
                        row.Interest = 0;
                        row.Legal = 0;
                        row.Other = 0;
                        row.CollectionCosts = 0;
                        row.Overpayment = 0;
                        row.ReversalType = "";
                        rows.Add(row);
                        //Update loop variables
                        vars.Pii++;
                        vars.OriginalPrincipal += loans[i].PurchAmt;
                        vars.LastEffectiveDate = loans[i].PurchaseDate;

                    }
                }
            }
            if (vars.EDInd != 0)
            {
                for (int i = 0; i < vars.EDInd; i++)
                {
                    if (loans[i].EDDate >= vars.LastEffectiveDate && loans[i].EDDate <= vars.EffectiveDate)
                    {
                        ExcelRow row = new ExcelRow();
                        row.EffectiveDate = loans[i].EDDate.Value.ToString("MM/dd/yyyy");
                        row.PaymentType = "ASSIGNED ED";
                        row.PaymentAmount = -1 * loans[i].EDAmt.Value;
                        row.Interest = -1 * loans[i].EDPrinc.Value;
                        row.Legal = -1 * loans[i].EDLegal.Value;
                        row.Other = -1 * loans[i].EDOther.Value;
                        row.CollectionCosts = -1 * loans[i].EDCC.Value;
                        row.Overpayment = 0;
                        row.ReversalType = "";
                        rows.Add(row);
                        //Update loop variables
                        vars.Aii++;
                        vars.OriginalPrincipal -= loans[i].EDAmt.Value;
                        vars.LastEffectiveDate = loans[i].EDDate.Value;
                    }
                }
            }
            //insert assignment rows if the ed indicator is yes (there are loan assigned to ED)

        }

        //Get information for a full reversal of a transaction
        public TransactionData FullReversal()
        {
            TransactionData data = new TransactionData();
            data.PmtType = RI.GetText(4, 28, 10).Trim();
            data.RevType = RI.GetText(6, 28, 10);
            data.PmtAmt = -1 * RI.GetText(3, 26, 12).ToDecimal();

            data.PrincCol = 0;
            data.IntCol = 0;
            data.Legal = 0;
            data.Other = 0;
            data.CC = 0;
            data.OV = RI.GetText(3, 26, 12).ToDecimal();

            return data;
        }

        //Get information for a partial reversal of a federal tax offset (injured spouse refund)
        public TransactionData PartialReversal(string partialType)
        {
            TransactionData data = new TransactionData();
            data.PmtType = RI.GetText(4, 28, 10).Trim();
            data.RevType = RI.GetText(6, 28, 10);
            data.PmtAmt = -1 * RI.GetText(3, 26, 12).ToDecimal();
            data.OV = RI.GetText(9, 26, 12).ToDecimal();

            if (partialType == "FOBC")
            {
                RI.Hit(Key.F7);
            }
            else
            {
                RI.Hit(Key.F8);
            }

            data.PrincCol = -1 * RI.GetText(15, 68, 12).ToDecimal();
            data.IntCol = -1 * RI.GetText(16, 68, 12).ToDecimal();
            data.Legal = -1 * RI.GetText(17, 68, 12).ToDecimal();
            data.Other = -1 * RI.GetText(18, 68, 12).ToDecimal();
            data.CC = -1 * RI.GetText(19, 68, 12).ToDecimal();

            return data;
        }

        //Get information for a non-reversed transaction (normal)
        public TransactionData NormalTransaction()
        {
            TransactionData data = new TransactionData();
            data.PmtType = RI.GetText(4, 28, 10).Trim();
            if (RI.GetText(20, 68, 12).ToInt() != 0)
            {
                data.RevType = "REFUND";
            }
            else
            {
                data.RevType = "";
            }

            data.PmtAmt = -1 * RI.GetText(3, 26, 12).ToDecimal();
            data.PrincCol = -1 * RI.GetText(15, 68, 12).ToDecimal();
            data.IntCol = -1 * RI.GetText(16, 68, 12).ToDecimal();
            data.Legal = -1 * RI.GetText(17, 68, 12).ToDecimal();
            data.Other = -1 * RI.GetText(18, 68, 12).ToDecimal();
            data.CC = -1 * RI.GetText(19, 68, 12).ToDecimal();
            data.OV = RI.GetText(20, 68, 12).ToDecimal();

            return data;
        }

        public List<LC05Loan> GetLoanInformation()
        {
            RI.FastPath("LC05I");

            if(RI.CheckForText(22,3,"47004"))
            {
                LogRun.AddNotification($"No loan on LC05 for account", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                MessageBox.Show("No loan on LC05 for account.", "Failure", MessageBoxButtons.OK);
                return null;
            }

            List<LC05Loan> loanList = new List<LC05Loan>();

            RI.PutText(21, 13, "01",Key.Enter);

            //review each loan
            while (!RI.CheckForText(22, 3, "46004"))
            {
                LC05Loan loan = new LC05Loan();
                loan.PurchaseDate = RI.GetText(4, 73, 8).ToDate();
                loan.PurchAmt = RI.GetText(9, 32, 12).ToDecimal();

                //Get assignment information if loan was assigned to ED
                if (!RI.CheckForText(19, 73, "MM") && RI.CheckForText(19, 70, "03"))
                {
                    loan.EDDate = RI.GetText(19, 72, 10).ToDate();
                    loan.EDAmt = RI.GetText(20, 32, 12).ToDecimal();
                    loan.EDPrinc = RI.GetText(9, 32, 12).ToDecimal() - RI.GetText(10, 32, 12).ToDecimal();
                    loan.EDInt = RI.GetText(11, 32, 12).ToDecimal() - RI.GetText(12, 32, 12).ToDecimal();
                    loan.EDLegal = RI.GetText(13, 32, 12).ToDecimal() - RI.GetText(14, 32, 12).ToDecimal();
                    loan.EDOther = RI.GetText(15, 32, 12).ToDecimal() - RI.GetText(16, 32, 12).ToDecimal();
                    loan.EDCC = RI.GetText(17, 32, 12).ToDecimal() - RI.GetText(18, 32, 12).ToDecimal() + RI.GetText(18, 32, 12).ToDecimal();//TODO This does not make sense, I think the last one should be row 19 probably. Will verify this in test when we have an account set up
                }
                //go to page 3
                RI.Hit(Key.F10);
                RI.Hit(Key.F10);
                loan.CLID = RI.GetText(3, 13, 19);
                //compare the unique ID to the other loans nad only keep the most current purchase of the loan
                bool matching = false;
                foreach (var l in loanList)
                {
                    if (l.CLID == loan.CLID)
                    {
                        l.PurchaseDate = loan.PurchaseDate;
                        l.PurchAmt = loan.PurchAmt;
                        l.EDDate = loan.EDDate;
                        l.EDAmt = loan.EDAmt;
                        l.CLID = loan.CLID;
                        matching = true;
                        break;
                    }

                }

                if (!matching)
                {
                    loanList.Add(loan);
                }
                //goto next loan
                RI.Hit(Key.F8);
            }

            //Sort loans by purchase date LPOD
            loanList = loanList.OrderBy(p => p.PurchaseDate).ToList();
            return loanList;
        }

        public void CreateExcelFile(LC41Variables vars, List<ExcelRow> rows, LC10Data data)
        {
            var filePath = EnterpriseFileSystem.GetPath("PaddGeneral", DataAccessHelper.Region.Uheaa);
            using (ExcelGenerator excel = new ExcelGenerator($"{filePath}TransHistroy_{vars.AccountNumber.ToString()}.xls"))
            {
                Font largeBoldFont = new Font("Calibri", 16.0f, FontStyle.Bold);
                Font normalBoldFont = new Font("Calibri", 9.0f, FontStyle.Bold);

                excel.SetActiveWorksheet(1, "Sheet1");

                //Create title
                excel.MergeCells("A1", "K1");
                excel.InsertData("A1", "A1", "UHEAA BORROWER TRANSACTION HISTORY",largeBoldFont, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);

                //Add header names
                excel.InsertData("A2", "A2", "Name:", normalBoldFont, false);
                excel.InsertData("A3", "A3", "Account #:", normalBoldFont,false);

                //Create the header information
                excel.InsertData("B2", "B2", vars.Name);
                excel.InsertData("B3", "B3", vars.AccountNumber);

                //Add outlines for header row
                excel.SetBorder("A5", "A7");
                excel.SetBorder("B5", "B7");
                excel.SetBorder("C5", "C7");
                excel.SetBorder("D5", "D7");

                excel.MergeCells("E5", "I5");
                excel.SetBorder("E5", "I5");

                excel.SetBorder("E6", "E7");
                excel.SetBorder("F6", "F7");
                excel.SetBorder("G6", "G7");
                excel.SetBorder("H6", "H7");
                excel.SetBorder("I6", "I7");

                excel.MergeCells("J5", "K5");
                excel.SetBorder("J5", "K5");

                excel.SetBorder("J6", "J7");
                excel.SetBorder("K6", "K7");

                //add header row values
                excel.InsertData("A6", "A6", "Date", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("B6", "B6", "Transaction Type", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("C6", "C6", "Beginning Principal Balance", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("D6", "D6", "Transaction Amount", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("E6", "E6", "Principal", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("F6", "F6", "Interest", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("G6", "G6", "Legal Costs", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("H6", "H6", "Other Costs", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("I6", "I6", "Collection Costs", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("J6", "J6", "Amount", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("K6", "K6", "Type", normalBoldFont, true, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);

                excel.InsertData("E5", "E5", "Transaction Applied", normalBoldFont, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);
                excel.InsertData("J5", "J5", "Refund or Reversal", normalBoldFont, false, ExcelGenerator.HCellAlignment.Center, ExcelGenerator.VCellAlignment.Center);             

                for (int i = 0; i < rows.Count; i++)
                {
                    string iplus8 = (i + 8).ToString();
                    excel.InsertData($"A{iplus8}", $"A{iplus8}", rows[i].EffectiveDate);                
                    excel.InsertData($"B{iplus8}", $"B{iplus8}", rows[i].PaymentType);
                    excel.InsertData($"C{iplus8}", $"C{iplus8}", rows[i].OrigianlPrincipal);
                    excel.ToAccountingFormat($"C{iplus8}", $"C{iplus8}");

                    excel.InsertData($"D{iplus8}", $"D{iplus8}", rows[i].PaymentAmount);
                    excel.ToAccountingFormat($"D{iplus8}", $"D{iplus8}");

                    excel.InsertData($"E{iplus8}", $"E{iplus8}", rows[i].Principal);
                    excel.ToAccountingFormat($"E{iplus8}", $"E{iplus8}");

                    excel.InsertData($"F{iplus8}", $"F{iplus8}", rows[i].Interest);
                    excel.ToAccountingFormat($"F{iplus8}", $"F{iplus8}");

                    excel.InsertData($"G{iplus8}", $"G{iplus8}", rows[i].Legal);
                    excel.ToAccountingFormat($"G{iplus8}", $"G{iplus8}");

                    excel.InsertData($"H{iplus8}", $"H{iplus8}", rows[i].Other);
                    excel.ToAccountingFormat($"H{iplus8}", $"H{iplus8}");

                    excel.InsertData($"I{iplus8}", $"I{iplus8}", rows[i].CollectionCosts);
                    excel.ToAccountingFormat($"I{iplus8}", $"I{iplus8}");

                    excel.InsertData($"J{iplus8}", $"J{iplus8}", rows[i].Overpayment);
                    excel.ToAccountingFormat($"J{iplus8}", $"J{iplus8}");


                    excel.InsertData($"K{iplus8}", $"K{iplus8}", rows[i].ReversalType);
                }              

                AddBalanceInfo(excel, rows.Count + 10, data);

                //adjust widths as necessary
                excel.AdjustWidth("A6", "A6", 10);
                excel.AdjustWidth("B6", "B6", 20);
                excel.AdjustWidth("C6", "C6", 10);
                excel.AdjustWidth("D6", "D6", 10);
                excel.AdjustWidth("E6", "E6", 10);
                excel.AdjustWidth("F6", "F6", 10);
                excel.AdjustWidth("G6", "G6", 10);
                excel.AdjustWidth("I6", "I6", 10);
                excel.AdjustWidth("J6", "J6", 10);
                excel.AdjustWidth("K6", "K6", 10);
            }
        }

        public void AddBalanceInfo(ExcelGenerator excel, int row, LC10Data lc10)
        {
            if (lc10.TotalCur == EDTotalCurrent)
            {
                excel.InsertData($"A{row}", $"A{row}", "Account assigned to ED, current balance not available");
            }
            else
            {
                excel.InsertData($"A{row}", $"A{row}", "Current Balance");
            }
            row++;

            excel.InsertData($"A{row}", $"A{row}", "Principal");
            excel.InsertData($"C{row}", $"C{row}", lc10.PrincCur);
            excel.ToAccountingFormat($"C{row}", $"C{row}");
            row++;

            excel.InsertData($"A{row}", $"A{row}", "Interest");
            excel.InsertData($"C{row}", $"C{row}", lc10.IntCur);
            excel.ToAccountingFormat($"C{row}", $"C{row}");
            row++;

            excel.InsertData($"A{row}", $"A{row}", "Legal Costs");
            excel.InsertData($"C{row}", $"C{row}", lc10.LegalCur);
            excel.ToAccountingFormat($"C{row}", $"C{row}");
            row++;

            excel.InsertData($"A{row}", $"A{row}", "Other Costs");
            excel.InsertData($"C{row}", $"C{row}", lc10.OtherCur);
            excel.ToAccountingFormat($"C{row}", $"C{row}");
            row++;

            excel.InsertData($"A{row}", $"A{row}", "Collection Costs");
            excel.InsertData($"C{row}", $"C{row}", lc10.CCCur);
            excel.ToAccountingFormat($"C{row}", $"C{row}");
            row++;

            excel.InsertData($"A{row}", $"A{row}", "Projected Collection Costs");
            excel.InsertData($"C{row}", $"C{row}", lc10.CCProj);
            excel.ToAccountingFormat($"C{row}", $"C{row}");
            row++;

            excel.InsertData($"C{row}", $"C{row}", "'--------------");
            row++;

            excel.InsertData($"A{row}", $"A{row}", "Total Balance");
            excel.InsertData($"C{row}", $"C{row}", lc10.TotalCur);
            excel.ToAccountingFormat($"C{row}", $"C{row}");
        }
    }
}
