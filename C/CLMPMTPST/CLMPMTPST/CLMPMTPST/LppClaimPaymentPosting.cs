using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Excel = Microsoft.Office.Interop.Excel;


namespace CLMPMTPST
{
    public class LppClaimPaymentPosting : ScriptBase
    {
        public ProcessLogRun LogRun { get; set; }
        public static new string ScriptId { get; set; } = "CLMPMTPST";
        private const string SAS_FILE = "ULWR10.LWR10R2*";
        private const string ERROR_FILE = @"T:\clmtargerr.txt";

        public LppClaimPaymentPosting(ReflectionInterface ri)
            : base(ri, ScriptId, DataAccessHelper.Region.Uheaa)
        {
            //HACK: I was testing something the other day trying to figure out why my process logs didn't have an end time and realized that
            // ScriptBase always registers a process log and then it registers another in the starter program.cs. I changed it to this so that it will create a logrun 
            // using the current ProcessLogData.ProcessingLogId in the ScriptBase class. When run from the session, it doesn't set the LogRun to the RI.LogRun so it will
            // be null when you do the LogRun = RI.LogRun ?? and will register it again. If you use this overload, it doesn't register a 2nd one when run from a session.
            // Then, when you do the LogRun.LogEnd() in your Main, it closes the one created by ScriptBase and you can call it in your Program.cs as well so they all end.
            // LogRun = RI.LogRun ?? new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, false, false);
            LogRun = RI.LogRun ?? new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, false, false);

        }

        public override void Main()
        {
            if (Dialog.Info.YesNo("You are running the LPP Claim Payment Post script. Do you wish to continue?"))
            {
                DeleteErrorFile(); // Remove error file if there is one left over from the previous run
                bool result = PostPayment();
                if (!result) // Failed to read file or get user input
                    return;

                CleanUp();
            }
            LogRun.LogEnd();
            return;
        }

        /// <summary>
        /// Driver method that: (1) Reads payment amounts from the SAS file,
        /// (2) Prompts user to indicate total and type, (3) Creates batch, 
        /// (4) Inputs batch payments into session, (5) Generates an error report 
        /// if it hits errors, (6) Prompts user whether post was success or not.
        /// 
        /// Returns false if file reading or user input hit snags.
        /// 
        /// </summary>
        /// <returns></returns>
        private bool PostPayment()
        {
            List<Payment> payments = GetPaymentsFromSasFile();
            if (payments == null)
                return false;

            // Prompt the user for the payment total and type.
            PaymentTotal total = GetPaymentTotalFromUser(Math.Round(payments.Select(p => p.PaymentAmount).Sum(), 2));
            if (total == null)
            {
                LogRun.LogEnd();
                return false;
            }

            // Create a batch on the system.
            string userId = RI.UserId;
            string batchNumber = CreateBatch(total, payments.Count, userId);
            if (string.IsNullOrEmpty(batchNumber))
            {
                LogRun.LogEnd();
                return false;
            }

            // Enter the payments.
            EnterPayments(batchNumber, payments, userId);
            if (File.Exists(ERROR_FILE))
                new ReportGenerator(LogRun).Generate(ERROR_FILE);

            // Check whether the batch is balanced.
            BalancePrompt balancePrompt = CheckBatchBalance(batchNumber);
            Dialog.Info.Ok(balancePrompt.Message, balancePrompt.Caption);

            return true;
        }

        /// <summary>
        /// Cleans up files by removing the SAS file and the error file.
        /// </summary>
        /// <returns></returns>
        private void CleanUp()
        {
            File.Delete(SAS_FILE); // TO DO: Do we want this to only be deleted if it was successful?
            DeleteErrorFile();
        }

        /// <summary>
        /// Deletes the error file if it exists.
        /// </summary>
        private void DeleteErrorFile()
        {
            if (File.Exists(ERROR_FILE))
                File.Delete(ERROR_FILE);
        }

        /// <summary>
        /// Driver method to determine if SAS file exists and if payments
        /// are parsed successfully from the file. Returns the payments if
        /// so, else returns null.
        /// </summary>
        /// <returns></returns>
        private List<Payment> GetPaymentsFromSasFile()
        {
            // Determine if SAS file is present
            FileSearchResult searchResult = FindSasFile(SAS_FILE);
            if (!searchResult.FileFound)
                return null;

            // Read payments from file
            List<Payment> payments = new List<Payment>();
            FileReader fileReader = new FileReader(LogRun);
            payments = fileReader.ReadFile(searchResult.FileName);

            return payments;
        }

        /// <summary>
        /// Checks whether the payment amount provided by the user
        /// matches the total amount in the file. Provides feedback
        /// for the user accordingly.
        /// </summary>
        /// <param name="batchNumber"></param>
        /// <returns></returns>
        private BalancePrompt CheckBatchBalance(string batchNumber)
        {
            BalancePrompt balancePrompt = new BalancePrompt();

            RI.FastPath($"TX3Z/CTS1R{batchNumber}");
            if (RI.GetText(11, 30, 10) == RI.GetText(10, 69, 10))
            {
                RI.Hit(Key.F10);
                if (File.Exists(ERROR_FILE))
                {
                    balancePrompt.Caption = "Batch Complete - Target Errors";
                    balancePrompt.Message = "Batch entry is complete and the batch is in balance.  However, the script was not able to target one or more transactions.  Print the target error spreadsheet and review those transactions.";
                }
                else
                {
                    balancePrompt.Caption = "Batch Complete";
                    balancePrompt.Message = "Batch entry is complete and the batch is in balance.  There were no target errors.";
                }
            }
            else if (File.Exists(ERROR_FILE))
            {
                balancePrompt.Caption = "Unable to Target Errors";
                balancePrompt.Message = "The batch does not balance.  Print the target error spreadsheet and correct the errors manually.";
            }
            else
            {
                balancePrompt.Caption = "Batch out of Balance";
                balancePrompt.Message = "The batch does not balance.  Correct the errors manually.";
            }
            return balancePrompt;
        }

        /// <summary>
        /// Creates the batch in the session. Returns the batch number.
        /// </summary>
        /// <param name="total"></param>
        /// <param name="numberOfPayments"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string CreateBatch(PaymentTotal total, int numberOfPayments, string userId)
        {
            RI.FastPath("TX3Z/ATS1G;");
            if (RI.CheckForText(23, 2, "01473"))
                RI.Hit(Key.EndKey);
            RI.PutText(6, 51, (total.Type == PaymentType.Cash ? "1" : "2"));
            RI.PutText(10, 28, numberOfPayments.ToString());
            RI.PutText(11, 28, total.Amount.ToString());
            RI.PutText(12, 28, "30");
            RI.PutText(15, 28, userId);
            RI.Hit(Key.Enter);

            if (RI.MessageCode != "01004")
            {
                Dialog.Error.Ok("Error encountered trying to add batch to session. Please try again or add the batch manually");
                LogRun.AddNotification($"Error adding batch. Expected session message 01004 but encountered: {RI.Message}", NotificationType.ErrorReport, NotificationSeverityType.Warning); 
                return null;
            }

            return RI.GetText(6, 18, 14);
        }

        /// <summary>
        /// Inputs the payments into the session. Adds any errors
        /// to the error file (used later to generate a report).
        /// </summary>
        /// <param name="batchNumber"></param>
        /// <param name="payments"></param>
        /// <param name="userId"></param>
        private void EnterPayments(string batchNumber, List<Payment> payments, string userId)
        {
            RI.FastPath($"TX3Z/ATS1D{batchNumber}");
            int row = 8;
            foreach (Payment payment in payments)
            {
                // Enter the payment.
                RI.PutText(row, 5, payment.Ssn);
                RI.PutText(row, 17, payment.PaymentAmount.ToString());
                RI.PutText(row, 30, payment.EffectiveDate.ToString(Payment.DATE_FORMAT));
                RI.PutText(row, 48, payment.GuarantorCode);
                RI.PutText(row, 59, "3", Key.Enter);

                // Clear duplicate transactions error.
                if (RI.CheckForText(23, 2, "30008"))
                {
                    if (RI.CheckForText(24, 13, "SET2"))
                        RI.Hit(Key.F2);
                    RI.PutText(row, 17, RI.GetText(row, 2, 2), Key.F4);
                    RI.PutText(11, 17, payment.LastName);
                    RI.PutText(19, 2, $"not duplicate / {userId}");
                    RI.Hit(Key.F12);
                    RI.Hit(Key.Enter);
                    RI.Hit(Key.F2);
                }

                // Target transactions.
                if (RI.CheckForText(24, 13, "SET2"))
                    RI.Hit(Key.F2);
                RI.PutText(22, 17, RI.GetText(row, 2, 2), Key.F6);

                if (RI.MessageCode == "01020") // TODO: Waiting to hear back from BA on how to handle this situation. In the meantime, I have put the placeholder code below
                {
                    if (!Dialog.Error.YesNo($"The transaction for item {RI.GetText(row, 2, 2)} was not found. {Environment.NewLine}{Environment.NewLine}Do you wish to manually fix the record and have the script continue?"))
                        return;
                }

                foreach (int loanSequence in payment.LoanSequences)
                {
                    // Make sure we're on the first page.
                    while (RI.CheckForText(9, 76, "-"))
                        RI.Hit(Key.F7);

                    // Find the loan sequence.
                    int sequenceRow = 12;
                    while (int.Parse(RI.GetText(sequenceRow, 50, 2)) != loanSequence && !RI.CheckForText(23, 2, "90007"))
                    {
                        sequenceRow++;
                        if (RI.CheckForText(sequenceRow, 50, "  "))
                        {
                            RI.Hit(Key.F8);
                            sequenceRow = 12;
                        }
                    }

                    // Mark the disbursement and check for errors.
                    if (!RI.CheckForText(23, 2, "90007"))
                        RI.PutText(sequenceRow, 3, "X", Key.Enter);
                    if (!RI.CheckForText(23, 2, "01004", "01005"))
                    {
                        try
                        {
                            using (StreamWriter errorWriter = new StreamWriter(ERROR_FILE, true))
                            {
                                errorWriter.WriteCommaDelimitedLine(payment.Ssn, payment.PaymentAmount.ToString(), payment.EffectiveDate.ToString(Payment.DATE_FORMAT), loanSequence.ToString(), payment.GuarantorCode);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogRun.AddNotification("Error encountered while trying to write out the error file.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                        }
                    }
                }

                //Return to the payment entry screen and prep for the next payment.
                RI.Hit(Key.F12);
                row++;
                if (row > 19)
                {
                    RI.Hit(Key.Enter);
                    if (RI.CheckForText(24, 13, "SET1"))
                        RI.Hit(Key.F2);
                    RI.Hit(Key.F8);
                    row = 8;
                }
            }
        }

        /// <summary>
        /// Determines if SAS file is out on the FTP folder. Returns a FileSearchResult object
        /// that indicates whether the file was found. If found, the FileName field is populated.
        /// Else, the Error field is populated (specifying whether the file was missing, old, 
        /// empty, or if there were multiple files).
        /// </summary>
        /// <returns></returns>
        private FileSearchResult FindSasFile(string sasFile)
        {
            string fileMissing = "The Claim Payment file is missing. Please contact Systems Support for assistance.";
            string fileMultiple = "Multiple files exist. Please review the old file.";
            string fileOld = "This file is too old. Please contact Systems Support for assistance.";
            string fileEmpty = "The Claim Payment file is empty. Please contact Systems Support for assistance.";

            string[] foundFiles = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, SAS_FILE, SearchOption.TopDirectoryOnly);
            FileSearchResult result = null;

            if (foundFiles.Length == 0)
                result = new FileSearchResult(false, fileMissing);
            if (foundFiles.Length > 1)
                result = new FileSearchResult(false, fileMultiple);

            if (result == null) // If file is not missing or there are not multiple
            {
                FileInfo sasInfo = new FileInfo(foundFiles[0]);
                if (sasInfo.CreationTime.Date.AddDays(5) < DateTime.Now.Date)
                    result = new FileSearchResult(false, fileOld);
                if (sasInfo.Length == 0)
                    result = new FileSearchResult(false, fileEmpty);
            }

            if (result != null)
            {
                IssueNotification(result.Error);
                LogRun.LogEnd();
                return result;
            }

            return new FileSearchResult(true, null, foundFiles[0]);
        }

        /// <summary>
        /// Prompts user for payment type and payment amount.
        /// </summary>
        /// <param name="fileTotal"></param>
        /// <returns></returns>
        private PaymentTotal GetPaymentTotalFromUser(double fileTotal)
        {
            PaymentTotal total = new PaymentTotal();
            while (total?.Amount != fileTotal)
            {
                using (PaymentDialog dialog = new PaymentDialog(total))
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        LogRun.AddNotification("User ended script run by cancelling payment total form", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                        return null;
                    }

                    if (total?.Amount != fileTotal)
                        Dialog.Error.Ok($"The total amount entered does not match the totals in the posting file. {Environment.NewLine}{Environment.NewLine}Either update the amount in the form to match the file, or hit the Cancel button, update the posting file, and then re-run the script.", "Amounts do not match");
                }
            }
            return total;
        }

        /// <summary>
        /// Logs issue and notifies user with a prompt.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="ex"></param>
        private void IssueNotification(string errorMessage, Exception ex = null)
        {
            LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex); // Log issue
            Dialog.Error.Ok(errorMessage, ScriptId); // Notify user of issue
        }
    }
}
