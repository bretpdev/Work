using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Excel = Microsoft.Office.Interop.Excel;

namespace PRECONADJ
{
    public class PreConversionAdjustment
    {
        private string ScriptId { get; set; }
        public ReflectionInterface RI { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public PreConversionAdjustment(ReflectionInterface ri, string scriptId)
        {
            RI = ri;
            ScriptId = scriptId;
            LogRun = ri.LogRun;
            string userId = RI.UserId;
        }

        public void Process()
        {
            if (!RI.IsLoggedIn)
                Dialog.Warning.Ok("Please log in before you continue.", "Not Logged In");

            QueueHandler queueHandler = new QueueHandler(RI, LogRun);
            do
            {
                SelectedFile file = GetFileToRead();
                if (file.FilePath.IsNullOrEmpty())
                    return;
                //OpenQueue
                queueHandler.UnassignOpenQueueTask();
                if (!queueHandler.CheckQueueAccess())
                    return;

                Console.WriteLine("Beginning to process file at path: " + file.FilePath);
                //Process Preconversion Adjustment From File
                if (!ReadAndUpdateFromExcel(file, queueHandler))
                    break;
            }
            while (Dialog.Info.YesNo("Would you like to process another file?", "Process Another File"));
        }

        public SelectedFile GetFileToRead()
        {
            SelectedFile file = new SelectedFile();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"Q:\UHEAA Loan Servicing\Accounting\_Adjustments\Pre-conv\UHEAA\";
#if DEBUG
                openFileDialog.InitialDirectory = @"T:\";
#endif
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    file.FilePath = openFileDialog.FileName;
                    file.Ssn = Path.GetFileNameWithoutExtension(openFileDialog.SafeFileName);
                }
            }
            if ((file.Ssn.IsNullOrEmpty() || file.Ssn.Length != 9) && file.FilePath.IsPopulated())
            {
                Dialog.Error.Ok("No Filename found or filename is not in SSN format.", "Invalid File");
                return new SelectedFile();
            }

            return file;
        }

        public bool ReadAndUpdateFromExcel(SelectedFile file, QueueHandler queueHandler)
        {
            bool recordProcessed = false;
            int startTabLoanSequence = 0;
            //TODO: WHEN THE BU ASKS FOR THIS UNCOMMENT IT
            //StartTabOverride sto = new StartTabOverride();
            //DialogResult dr = sto.ShowDialog();
            //if(dr == DialogResult.OK)
            //{
            //    if(sto.StartAfterValue.HasValue)
            //    {
            //        startTabLoanSequence = sto.StartAfterValue;
            //    }
            //}
            Excel.Application excel = new Excel.Application();
            Excel.Workbook workBook = excel.Workbooks.Open(file.FilePath);
            Excel.Sheets sheets = workBook.Worksheets;
            int i;
            //ignore the first sheet, it does not contain loan information
            for (i = startTabLoanSequence + 1; i <= sheets.Count; i++)
            {
                //The first page should be skipped, loan sequence 0 does not match MainData so it will skip the first tab
                int loanSeq = i - 1;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.Item[i];
                if (workSheet.Name == ExcelHelper.TranslateToTabName(loanSeq) + 'Y')
                {
                    if (!queueHandler.OpenQueueTask(file.Ssn, loanSeq))//open queue and put in working mode
                        return false;
                    Console.WriteLine("Worksheet name for sheet number: " + i + " name:" + workSheet.Name);
                    recordProcessed = true;
                    List<LineData> data = ExcelHelper.ReadWorksheetToList(workSheet, ExcelHelper.TranslateToTabName(loanSeq) + 'Y');
                    if (data == null)
                        return false;
                    if (!AdjustAts44(data, file.Ssn, loanSeq.ToString()))//i-1 is loan sequence
                        return false;
                    Console.WriteLine("Loan Sequence: " + loanSeq + " Processed Successfully");
                    if (queueHandler.FoundLoan)
                        queueHandler.CloseQueueTask(file.Ssn); //CloseQueue
                }
            }

            //Clean Up Excel Application
            workBook.Close(0);
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            if (!recordProcessed)
                Dialog.Warning.Ok("No records to process. Queue task will not be closed", "Nothing to Process");
            return recordProcessed;
        }

        public bool AdjustAts44(List<LineData> data, string ssn, string loanSeq)
        {
            string loanType = "";
            RI.FastPath($"TX3Z/ATS44{ssn}");
            if (RI.MessageCode != "01021" || RI.ScreenCode != "TSX42")
            {
                LogAndAlert($"Encountered unexpected session code when accessing ATS44 for borrower: {ssn} Message: {RI.Message}");
                return false;
            }
            //Type 3 with a comment of "Precon Adj"
            RI.PutText(8, 43, "3");
            RI.PutText(16, 9, "Precon Adj");
            //Target loans
            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);

            bool adjustmentExists = false;
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 11;
            settings.MaxRow = 21;
            PageHelper.Iterate(RI, (row, s) =>
            {
                if (RI.GetText(row, 24, 3).Trim() == loanSeq)
                {
                    loanType = RI.GetText(row, 29, 6);
                    RI.PutText(22, 18, RI.GetText(row, 3, 2), ReflectionInterface.Key.Enter);
                    if (RI.MessageCode == "04033")
                    {
                        LogAndAlert($"Pending adjustment already exists for ssn: {ssn}. Please correct the issue and try running again. Ending script");
                        adjustmentExists = true;
                        settings.ContinueIterating = false;
                    }
                    settings.ContinueIterating = false;
                }
            }, settings);
            if (adjustmentExists)
                return false;

            return HandleTs6c(data, loanType);
        }

        private bool HandleTs6c(List<LineData> data, string loanType)
        {
            int row = 9; // row begin 9 end 20
                         //Track disbursment/payment dates
            DateTime? disbursementDate = null;
            DateTime? paymentDate = null;
            //Handle TS6C adjustments from document
            bool firstRow = true;
            int recordNum = 0;
            while (recordNum < data.Count)
            {
                double pmtAmt = data[recordNum].PaymentAmount.ToDouble();
                //Update the payment date if there is ever one sooner than the previous one
                if (!firstRow && pmtAmt != 0 && paymentDate.HasValue && paymentDate.Value < data[recordNum].PaymentDates)
                    paymentDate = data[recordNum].PaymentDates;
                else if (!firstRow && pmtAmt != 0 && !paymentDate.HasValue)
                    paymentDate = data[recordNum].PaymentDates;

                if (AdjustTs6cRow(data[recordNum], row, firstRow, pmtAmt))
                {
                    if (firstRow)
                    {
                        firstRow = false;
                        disbursementDate = data[recordNum].PaymentDates;
                    }
                    row++;
                    if (row > 20)
                    {
                        RI.Hit(ReflectionInterface.Key.Enter);
                        //ERROR HERE IF CODE IS NOT SUCCESS
                        if (RI.MessageCode != "04072")
                        {
                            LogAndAlert($"Unexpected session error encountered: {RI.Message}.\r\n\r\nPlease review the error in the session and adjust the spreadsheet to correct it then try again.");
                            return false;
                        }
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 9;
                    }
                }
                //If the date of the payment is before the first disbursement error
                if (disbursementDate.HasValue && paymentDate.HasValue && paymentDate.Value < disbursementDate.Value)
                {
                    Dialog.Info.Ok("Please contact the unit manager to review the account. Borrower has payment dates before the first disbursement", "CRITICAL ERROR");
                    return false;
                }
                recordNum++;
            }

            //Enter Sub Interest if loan type is in STFFRD, SUBCNS, SUBSPC, DLSCNS, DLSCSC, DLSSPL, DLSTFD
            if (loanType.IsIn("STFFRD", "SUBCNS", "SUBSPC", "DLSCNS", "DLSCSC", "DLSSPL", "DLSTFD"))
                RI.PutText(22, 17, "0.00");
            //Hit f10, then f6 to commit transaction
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode != "04072")
            {
                LogAndAlert("Please review the error in the session and adjust the spreadsheet to correct it then try again.");
                return false;
            }

            Dialog.Info.Ok("Please review the transactions to make sure everything looks correct. When you are finished, push Insert to continue.", "Please review");
            RI.PauseForInsert();
            if (RI.ScreenCode != "TSX57")
            {
                Dialog.Error.Ok("You have moved away from the PRE_CONVERSION FINANCIAL TRANSACTIONS (TSX57) screen and the form has been reset. Please run the application again for this file.");
                return false;
            }
            if (Dialog.Info.YesNo("Commit?", "Commit"))
            {
                RI.Hit(ReflectionInterface.Key.F10);
                RI.Hit(ReflectionInterface.Key.F6);
                if (RI.MessageCode != "02107")
                {
                    string message = $"Screen code not recognized expected: 02107 got: {RI.MessageCode}{Environment.NewLine}{Environment.NewLine}Message: {RI.Message}";
                    LogAndAlert(message);
                    return false;
                }
                RI.Hit(ReflectionInterface.Key.F10);
                RI.Hit(ReflectionInterface.Key.F11);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Does the input for the TS6C page according to the BU rules regarding PRECONADJ
        /// </summary>
        /// <returns>a bool saying if the row was added or not</returns>
        public bool AdjustTs6cRow(LineData rowData, int row, bool firstRow, double pmtAmt)
        {
            Types type = new Types().GetTypeSubtype(rowData.AdditionalDisbursement, rowData.Cap);
            if (type.Type.IsNullOrEmpty() || type.SubType.IsNullOrEmpty())
                return false;

            if (firstRow)
            {
                if (rowData.AdditionalDisbursement == null || rowData.AdditionalDisbursement == "")
                    return false;
                else
                {
                    HandleFirstRow(rowData, row);
                    return true;
                }
            }
            else
            {
                if (type.Type == "10" && type.SubType == "10" && pmtAmt != 0)
                {
                    HandleType1010(rowData, row, type, pmtAmt);
                    return true;
                }
                else if (type.Type == "01" && type.SubType == "01")
                {
                    HandleType0101(rowData, row, type);
                    return true;
                }
                else if (type.Type == "70" && type.SubType == "01")
                {
                    HandleType7001(rowData, row, type);
                    return true;
                }
            }
            return false;
        }

        public void HandleFirstRow(LineData rowData, int row)
        {
            RI.PutText(row, 4, rowData.PaymentDates.Month.ToString().PadLeft(2, '0')); //Effective Date 2 char first 2
            RI.PutText(row, 7, rowData.PaymentDates.Day.ToString().PadLeft(2, '0')); //Effective Date 2 char second 2
            RI.PutText(row, 10, rowData.PaymentDates.ToString("yy")); //Effective Date 2 char third 2
            RI.PutText(row, 34, rowData.AdditionalDisbursement);
            RI.PutText(row, 45, "0.00"); // INTEREST APPL 9 char
            RI.PutText(row, 55, "0.00"); // INELG PRI APPL 10 char
            RI.PutText(row, 66, "0.00"); //LTE FEE APPL 8 char
        }

        public void HandleType0101(LineData rowData, int row, Types type)
        {
            RI.PutText(row, 4, rowData.PaymentDates.Month.ToString().PadLeft(2, '0')); //Effective Date 2 char first 2
            RI.PutText(row, 7, rowData.PaymentDates.Day.ToString().PadLeft(2, '0')); //Effective Date 2 char second 2
            RI.PutText(row, 10, rowData.PaymentDates.ToString("yy")); //Effective Date 2 char third 2
            RI.PutText(row, 13, type.Type); //Type 2 char
            RI.PutText(row, 16, type.SubType); //Subtype 2 char
            RI.PutText(row, 19, "A"); //Need an A for every nonfirst row in the C/A field
            RI.PutText(row, 21, rowData.AdditionalDisbursement);
        }

        public void HandleType1010(LineData rowData, int row, Types type, double pmtAmt)
        {
            RI.PutText(row, 4, rowData.PaymentDates.Month.ToString().PadLeft(2, '0')); //Effective Date 2 char first 2
            RI.PutText(row, 7, rowData.PaymentDates.Day.ToString().PadLeft(2, '0')); //Effective Date 2 char second 2
            RI.PutText(row, 10, rowData.PaymentDates.ToString("yy")); //Effective Date 2 char third 2
            RI.PutText(row, 13, type.Type); //Type 2 char
            RI.PutText(row, 16, type.SubType); //Subtype 2 char
            RI.PutText(row, 19, "A"); //Need an A for every nonfirst row in the C/A field
            RI.PutText(row, 21, string.Format("{0:0.00}", (-pmtAmt))); //Transaction amount 10 char we want to put the negative payment amount into the session
        }

        public void HandleType7001(LineData rowData, int row, Types type)
        {
            RI.PutText(row, 4, rowData.PaymentDates.Month.ToString().PadLeft(2, '0')); //Effective Date 2 char first 2
            RI.PutText(row, 7, rowData.PaymentDates.Day.ToString().PadLeft(2, '0')); //Effective Date 2 char second 2
            RI.PutText(row, 10, rowData.PaymentDates.ToString("yy")); //Effective Date 2 char third 2
            RI.PutText(row, 13, type.Type); //Type 2 char
            RI.PutText(row, 16, type.SubType); //Subtype 2 char
            RI.PutText(row, 19, "A"); //Need an A for every nonfirst row in the C/A field
        }

        private void LogAndAlert(string message)
        {
            LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            Dialog.Error.Ok(message);
        }
    }
}