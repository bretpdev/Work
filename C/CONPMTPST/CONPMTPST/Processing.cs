using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CONPMTPST
{
    public partial class ConsolPaymentPosting
    {
        public enum FileType
        {
            Comma,
            Flat,
            Direct
        }

        public PaymentPostingData Post { get; set; }
        public List<PaymentTypes> Types { get; set; }
        public bool ErrorsFound { get; set; }
        public DataAccess DA { get; set; }

        private void ProcessPayment(DataAccess da)
        {
            DA = da;
            if (!VerifyFilePath())
                return;

            Types = DA.GetPaymentTypes();

            if (LoadFileData())
            {
                if (Post.PaymentSource.Type != FileType.Direct)
                    DA.SetOverpayment(Post.BorData);
                RemoveACH();
                AddArcs();
                string batchNumber = CreateBatch();
                AddDataToBatch(batchNumber); //Creates a new batch and then adds the file data
                VerifyBatch(batchNumber);
                FileHelper.DeleteFile(Post.PaymentSource.FilePath, ProcessLogData.ProcessLogId, ProcessLogData.ExecutingAssembly);

                if (ErrorsFound)
                    Dialog.Info.Ok("Process Complete\r\n\r\nThere were errors in processing. Please see the errors in Process Logger.", "Process Complete");
                else
                    Dialog.Info.Ok("Process Complete", "Process Complete");

                Recovery.Delete();
            }
        }

        /// <summary>
        /// Check the file to make sure there is only one, it does exist and it is no empty.
        /// </summary>
        /// <returns>True: File is valid and ready to process; False: File not verified</returns>
        private bool VerifyFilePath()
        {
            string[] files = Directory.GetFiles(EnterpriseFileSystem.TempFolder, Post.PaymentSource.FileName + "*");
            if (files.Count() == 0) //Verify file exists
            {
                string message = string.Format("The {0} file does not exist or is missing.", Post.PaymentSource.FileName);
                Dialog.Warning.Ok(message, "File Missing");
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.NoFile, NotificationSeverityType.Warning);
                return false;
            }

            if (files.Count() > 1) //Check for multiple files
            {
                string message = string.Format("Multiple {0} files were found. Make sure the current file is the only file in your T: drive.", Post.PaymentSource.FileName);
                Dialog.Warning.Ok(message, "Multiple Files");
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }

            if (new FileInfo(files[0]).Length == 0) //Check for emtpy file
            {
                string message = string.Format("The {0} file is empty.", Post.PaymentSource.FileName);
                Dialog.Warning.Ok(message, "Emtpy File");
                LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Warning);
                return false;
            }

            Post.PaymentSource.FilePath = files[0];

            return true; //The file is valid
        }

        /// <summary>
        /// Loads the data from the selected servicer file into the PaymentPostingData obect.
        /// </summary>
        private bool LoadFileData()
        {
            using (StreamReader sr = new StreamReader(Post.PaymentSource.FilePath))
            {
                if (Post.PaymentSource.Type == FileType.Comma)
                    return CommaDelimited(sr);
                else if (Post.PaymentSource.Type == FileType.Direct)
                    return DirectFile(sr);
                else if (Post.PaymentSource.Type == FileType.Flat)
                    return FlatFile(sr);
                else
                {
                    string message = "Error loading file. The File Type is missing in the Payment Source table";
                    Dialog.Error.Ok(message, "No File Type");
                    LogRun.AddNotification(message, NotificationType.EmptyFile, NotificationSeverityType.Critical);
                    return false;
                }
            }
        }

        /// <summary>
        /// Loads all the file data from a comma delimited file into a BorrowerData object
        /// </summary>
        /// <param name="sr"></param>
        private bool CommaDelimited(StreamReader sr)
        {
            string header = sr.ReadLine();
            List<string> lineData = new List<string>();
            while (!sr.EndOfStream)
            {
                lineData = sr.ReadLine().SplitAndRemoveQuotes(",");

                if (lineData.Count == 1)
                    return false;
                if (lineData.Count > 1)
                {
                    BorrowerData data = new BorrowerData();
                    data.ManifestNumber = lineData[1];
                    data.EffectiveDate = DateTime.Parse(lineData[2]);
                    data.Ssn = lineData[4];
                    data.FirstName = lineData[5];
                    data.LastName = lineData[6];
                    data.NsldsId = lineData[7];
                    data.OriginatorId = lineData[8];
                    data.FirstDisbursement = DateTime.Parse(lineData[10]);
                    data.LoanType = lineData[11];
                    data.PayoffAmount = Decimal.Parse(lineData[18]);
                    data.LoanSequence = GetLoanSequence(data);
                    data.AccountNumber = GetAccountNumber(data);
                    Post.BorData.Add(data);
                }
            }
            return true;
        }

        /// <summary>
        /// Loads the file data from a flat file into a BorrowerData object
        /// </summary>
        /// <param name="sr"></param>
        private bool FlatFile(StreamReader sr)
        {
            string header = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string lineData = sr.ReadLine();
                if (lineData.SplitAndRemoveQuotes(",").Count > 1)
                    return false;
                //The last line does not contain data so do a peek to see if the next line is empty, if it is, don't process the current line.
                if (sr.Peek() > 0)
                {
                    BorrowerData data = new BorrowerData();
                    data.Ssn = lineData.SafeSubString(39, 9);
                    data.FirstName = lineData.SafeSubString(48, 35).Trim();
                    data.LastName = lineData.SafeSubString(83, 35).Trim();
                    data.LoanType = lineData.SafeSubString(208, 1);
                    data.FirstDisbursement = DateTime.Parse(lineData.SafeSubString(198, 10));
                    data.EffectiveDate = DateTime.Parse(lineData.SafeSubString(21, 10));
                    //The payoff amount does not have a decimal place in the flat file, divide by 100 to add it.
                    data.PayoffAmount = Decimal.Parse(lineData.SafeSubString(256, 8)) / 100;
                    data.LoanSequence = GetLoanSequence(data);
                    data.AccountNumber = GetAccountNumber(data);
                    data.ManifestNumber = lineData.SafeSubString(6, 15);
                    data.NsldsId = lineData.SafeSubString(118, 17);
                    data.OriginatorId = lineData.SafeSubString(135, 13);
                    RI.FastPath("TX3ZITS26" + data.Ssn);
                    if (RI.ScreenCode == "TSX28" || RI.ScreenCode == "TSX29")
                        data.ShouldTarget = true;
                    else
                        data.ShouldTarget = false;
                    if (!data.Ssn.IsNullOrEmpty())
                        Post.BorData.Add(data);
                }
            }
            return true;
        }

        private bool DirectFile(StreamReader sr)
        {
            while (!sr.EndOfStream)
            {
                List<string> lineData = sr.ReadLine().SplitAndRemoveQuotes(",");
                BorrowerData data = new BorrowerData();
                data.Ssn = lineData[0];
                data.PayoffAmount = Decimal.Parse(lineData[1]);
                data.EffectiveDate = DateTime.Parse(lineData[2]);
                data.LoanSequence = int.Parse(lineData[3]);
                data.OriginatorId = lineData[4];
            }
            return true;
        }

        /// <summary>
        /// Gets the loan sequence number from ITS26
        /// </summary>
        private int GetLoanSequence(BorrowerData data)
        {
            RI.FastPath("TX3ZITS26" + data.Ssn);
            if (RI.ScreenCode == "TSX28")
                return GetSequence(data);
            else if (RI.ScreenCode == "TSX29")
                return RI.GetText(7, 35, 4).ToInt();
            else if (RI.ScreenCode == "T1X07")
                return 0;
            else
                return int.Parse(RI.GetText(7, 35, 4));
        }

        /// <summary>
        /// Gets the borrowers account number from ITX1J
        /// </summary>
        private string GetAccountNumber(BorrowerData data)
        {
            RI.FastPath("TX3ZITX1JB" + data.Ssn);
            if (RI.ScreenCode == "TXX1K")
                return "";
            else
                return RI.GetText(3, 34, 12).Replace(" ", "");
        }

        /// <summary>
        /// Loops through all loans on TS26 to find the loan sequence number that matches the 1st disb date and the loan type
        /// </summary>
        private int GetSequence(BorrowerData data)
        {
            List<PaymentTypes> types = Types.Where(p => p.TivaFileLoanType == data.LoanType).ToList();
            foreach (PaymentTypes type in types) //Loops through each Tiva loan type
            {
                int row = 8;
                while (RI.MessageCode != "90007")
                {
                    if (!RI.CheckForText(row, 3, " ") && (RI.CheckForText(row, 19, type.CompassLoanType) && RI.GetText(row, 5, 8).ToDate() == data.FirstDisbursement))
                        return int.Parse(RI.GetText(row, 14, 4));
                    if (row == 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 8;
                    }
                    row++;
                }
            }

            LoanSequence ls = new LoanSequence(data);
            ls.ShowDialog();
            return ls.LoanSequenceNumber;
        }

        private void RemoveACH()
        {
            foreach (BorrowerData current in Post.BorData)
            {
                RI.FastPath("TX3ZITS7O" + current.AccountNumber);
                if (RI.ScreenCode == "TSX7K")
                {
                    if (!RI.CheckForText(10, 18, "A"))
                        continue;
                    RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                    RI.PutText(10, 18, "D", ReflectionInterface.Key.Enter);
                    RI.PutText(10, 57, "E", ReflectionInterface.Key.Enter);
                }
                
                if (!RI.MessageCode.IsIn("02526", "03512"))
                {
                    string message = string.Format("There was an error cancelling the ACH for borrower: {0}", current.AccountNumber);
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    ErrorsFound = true;
                }
            }
        }

        /// <summary>
        /// Adds a PCONS arc to each borrower account in the file.
        /// </summary>
        private void AddArcs()
        {
            bool arcError = false;
            foreach (BorrowerData bor in Post.BorData)
            {
                string comment = string.Format("Consolidation payment received from {0}", Post.PaymentSource.PaymentSource);
                if (!RI.Atd22AllLoans(bor.Ssn, "PCONS", comment, "", ScriptId, false))
                {
                    LogRun.AddNotification(string.Format("Error adding PCONS arc for borrower {0}", bor.AccountNumber), NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    arcError = true;
                }
            }
            if (arcError)
                Dialog.Error.Ok("There were errors found while adding arcs. Please view the errors in Process Logger", "Errors Found");
        }

        /// <summary>
        /// Creates the batch
        /// </summary>
        /// <returns>The new batch number</returns>
        private string CreateBatch()
        {
            if (Recovery.RecoveryValue.IsPopulated())
            {
                RI.FastPath("TX3ZDTS1G" + Recovery.RecoveryValue);
                if (RI.MessageCode == "01023")
                    RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.MessageCode != "01006")
                    LogRun.AddNotification(string.Format("There was an error deleting the batch: {0}", Recovery.RecoveryValue), NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }

            RI.FastPath("TX3ZATS1G");
            if (RI.ScreenCode == "T1X03")
                RI.PutText(10, 32, "", ReflectionInterface.Key.Enter, true);
            RI.PutText(6, 51, Post.IsCash ? "1" : "2");
            RI.PutText(10, 28, Post.BorData.Count().ToString());
            RI.PutText(11, 28, Post.PaymentAmount.ToString());
            RI.PutText(12, 28, "70");
            RI.PutText(15, 28, UserId, ReflectionInterface.Key.Enter);
            string batch = RI.GetText(6, 18, 14);
            Recovery.RecoveryValue = batch;
            return batch;
        }

        /// <summary>
        /// Adds borrower information to the batch
        /// </summary>
        /// <param name="batchNumber"></param>
        private void AddDataToBatch(string batchNumber)
        {
            RI.FastPath("TX3ZATS1D" + batchNumber);
            int row = 8;
            foreach (BorrowerData bor in Post.BorData)
            {
                RI.PutText(row, 5, bor.Ssn);
                RI.PutText(row, 17, bor.PayoffAmount.ToString());
                RI.PutText(row, 30, bor.EffectiveDate.ToString("MMddyy"));
                RI.PutText(row, 48, Post.PaymentSource.InstitutionId);
                RI.PutText(row, 59, "2");
                RI.Hit(ReflectionInterface.Key.Enter);
                CheckForErrors(row, bor);
                if (RI.CheckForText(24, 13, "SET2"))
                    RI.Hit(ReflectionInterface.Key.F2);
                if (bor.ShouldTarget)
                {
                    RI.PutText(22, 17, (row - 7).ToString(), ReflectionInterface.Key.F6, true);
                    if (RI.ScreenCode == "TSX1N")
                        SelectLoanSequence(bor);
                }
                row++;
                if (row == 20)
                {
                    if (RI.CheckForText(24, 13, "SET1"))
                        RI.Hit(ReflectionInterface.Key.F2);
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 8;
                }
            }
        }

        /// <summary>
        /// Check for errors in targeting the borrower
        /// </summary>
        /// <param name="row"></param>
        /// <param name="bor"></param>
        private void CheckForErrors(int row, BorrowerData bor)
        {
            string message = "";
            if (RI.MessageCode == "30008")
            {
                FixError(row, bor, "Not Duplicate");
                return;
            }
            else if (RI.MessageCode == "04404")
            {
                FixError(row, bor, "Loans Deconverted");
                message = string.Format("Loans deconverted for borrower: {0}, Loan Seq: {1}, Payment Amount {2:#.00}", bor.AccountNumber, bor.LoanSequence, bor.PayoffAmount);
            }
            else if (RI.MessageCode == "05097")
            {
                bor.LastName = "Unknown";
                FixError(row, bor, "Loans not on Compass");
                message = string.Format("Loans not on Compass for borrower: {0}, Loan Seq: {1}, Payment Amount {2:#.00}", bor.AccountNumber, bor.LoanSequence, bor.PayoffAmount);
            }
            else
                return;
            LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
            ErrorsFound = true;
        }

        /// <summary>
        /// Fix any errors found in targeting
        /// </summary>
        /// <param name="row"></param>
        /// <param name="bor"></param>
        /// <param name="message"></param>
        private void FixError(int row, BorrowerData bor, string message)
        {
            if (RI.CheckForText(24, 13, "SET2"))
                RI.Hit(ReflectionInterface.Key.F2);
            RI.PutText(22, 17, RI.GetText(row, 2, 2), ReflectionInterface.Key.F4, true);
            RI.PutText(11, 17, bor.LastName);
            string errorMessage = string.Format("{0}/{1}", message, UserId);
            RI.PutText(19, 2, errorMessage, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F12);
            RI.Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Target each loan.
        /// </summary>
        /// <param name="bor"></param>
        private void SelectLoanSequence(BorrowerData bor)
        {
            bool Updated = false;
            int row = 12;
            while (RI.MessageCode != "90007")
            {
                string seq = RI.GetText(row, 50, 2);
                if (!seq.IsNullOrEmpty())
                {
                    if (bor.LoanSequence == int.Parse(seq))
                    {
                        RI.PutText(row, 3, "X", ReflectionInterface.Key.Enter);
                        if (RI.MessageCode != "01004")
                        {
                            string message = string.Format("There was an error targeting for borrower {0}; Loan Sequence {1}; Amount ${2:#.00}", bor.AccountNumber, bor.LoanSequence, bor.PayoffAmount);
                            LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                            ErrorsFound = true;
                        }
                        Updated = true;
                        RI.Hit(ReflectionInterface.Key.F12);
                        return;
                    }
                }
                row++;

                if (row == 22)
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 12;
                }
            }

            if (!Updated)
            {
                LoanSequence ls = new LoanSequence(bor);
                ls.ShowDialog();
                bor.LoanSequence = ls.LoanSequenceNumber;
                RI.Hit(ReflectionInterface.Key.Enter);
                SelectLoanSequence(bor);
            }
        }

        /// <summary>
        /// Verifies the batch numbers
        /// </summary>
        /// <param name="batchNumber"></param>
        private void VerifyBatch(string batchNumber)
        {
            RI.FastPath("TX3ZCTS1R" + batchNumber);
            RI.Hit(ReflectionInterface.Key.F10);
            if (RI.MessageCode == "30006")
                Dialog.Error.Ok("Batch totals do not match, please proceed manually", "Batch Error");
            else
                RI.Hit(ReflectionInterface.Key.PrintScreen);
        }
    }
}