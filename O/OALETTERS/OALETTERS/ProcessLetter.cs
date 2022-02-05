using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace OALETTERS
{
    public class ProcessLetter
    {
        public ProcessLogData LogData { get; set; }
        public DataAccess Da { get; set; }
        public string ScriptId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ReflectionInterface RI { get; set; }

        public ProcessLetter(ProcessLogData logData, string scriptId)
        {
            LogData = logData;
            ScriptId = scriptId;
            Da = new DataAccess();
        }

        /// <summary>
        /// Creates the LetterSelection form the prints and adds ARC
        /// </summary>
        public void Run()
        {
            try
            {
                int runCount = 0;
                while (runCount == 0)
                {
                    LetterSelection ls = new LetterSelection(ScriptId, LogData, Username, Password);
                    if (ls.ShowDialog() == DialogResult.Cancel)
                    {
                        runCount = 1;
                        continue;
                    }
                    Username = ls.UserName;
                    Password = ls.Password;
                    RI = new ReflectionInterface();
                    while (runCount == 0 && !RI.Login(ls.UserName, ls.Password))
                    {
                        Dialog.Error.Ok("Invalid username or password. Please fix and try again");
                        RI.LogOut();
                        if (ls.ShowDialog() == DialogResult.Cancel)
                        {
                            runCount = 1;
                            continue;
                        }
                    }
                    if (runCount == 1)
                        continue;
                    PrintLetter(ls);
                    AddArc(ls);
                }
                RI.LogOut();
                RI.CloseSession();
            }
            catch (Exception ex)
            {
                ProcessLogger.AddNotification(LogData.ProcessLogId, ex.ToString(), NotificationType.ErrorReport, NotificationSeverityType.Critical, LogData.ExecutingAssembly, ex);
            }
        }

        /// <summary>
        /// Creates the data file and calls the cost center printing twice to print the letter two times.
        /// </summary>
        /// <param name="ls"></param>
        private void PrintLetter(LetterSelection ls)
        {
            string dataFile = string.Format("{0}{1}{2}.txt", EnterpriseFileSystem.TempFolder, ScriptId, Guid.NewGuid().ToString());
            using (StreamWriter sw = new StreamWriter(dataFile, false))
            {
                sw.WriteLine("KeyLine, StaticCurrentDate, AccountNumber, FirstName, LastName, Address1, Address2, City, State, Zip, Country, EffectiveDate, RefundAmount, PaymentSource, CompanyName, MaskedSsn, CostCenter");
                string keyLine = DocumentProcessing.ACSKeyLine(ls.Bor.Ssn ?? "012345678", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                string costCenter = Da.GetCostCenter(ls.Letter.LetterType);
                //Use a NumberFormatInfo to set the CurrencySymbol to nothing. All the letters have the $ on them.
                NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
                nfi = (NumberFormatInfo)nfi.Clone();
                nfi.CurrencySymbol = "";
                string amt = string.Format(nfi, "{0:C}", ls.Bor.RefundAmount.ToDecimal());

                sw.WriteLine(string.Format("\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"{11}\", \"{12}\", \"{13}\", \"{14}\", \"{15}\", \"{16}\"", keyLine, DateTime.Now.Date.ToString().ToLongDateNoDOW(), ls.Bor.AccountNumber ?? "", ls.Bor.FirstName ?? "", ls.Bor.LastName ?? "", ls.IsCompany ? ls.CompanyData.Address1 : (ls.Bor.Address1 ?? ""), ls.IsCompany ? ls.CompanyData.Address2 : (ls.Bor.Address2 ?? ""), ls.IsCompany ? ls.CompanyData.City : (ls.Bor.City ?? ""), ls.IsCompany ? ls.CompanyData.State : (ls.Bor.State ?? ""), ls.IsCompany ? ls.CompanyData.Zip : (ls.Bor.Zip ?? ""), ls.IsCompany ? ls.CompanyData.Country : (ls.Bor.Country ?? ""), ls.Bor.EffectiveDate, amt, ls.Bor.PaymentSource, ls.CompanyData.FirstName ?? "", ls.IsCompany ? string.Format("XXX-XX-{0}", ls.Bor.Ssn.Substring(5, 4)) : "", costCenter));
            }
            //Print 2 copies
            DocumentProcessing.CostCenterPrinting(ls.Letter.LetterType, dataFile, "State", ScriptId, "AccountNumber", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, false, "CostCenter");
            DocumentProcessing.CostCenterPrinting(ls.Letter.LetterType, dataFile, "State", ScriptId, "AccountNumber", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, false, "CostCenter");
            Repeater.TryRepeatedly(() => File.Delete(dataFile));
        }

        /// <summary>
        /// Adds TC00 and LP50 comments.
        /// </summary>
        /// <param name="ls"></param>
        private void AddArc(LetterSelection ls)
        {
            string comment = GetMessage(ls);
            string message = "";
            RI.FastPath("TX3ZATC00");
            RI.PutText(6, 41, ls.Bor.AccountNumber);
            RI.PutText(19, 38, "4", ReflectionInterface.Key.Enter);
            if (RI.CheckForText(1, 74, "TCX14"))
            {
                message = string.Format("Borrower: {0} not found in the system. Cannot leave comment: {1} on borrower account in TC00", ls.Bor.AccountNumber, comment);
                ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            RI.PutText(12, 10, comment, ReflectionInterface.Key.Enter);
            if (RI.MessageCode != "01004")
            {
                message = string.Format("Error adding comment:\r\n\r\n{0}\r\n\r\nin TC00 for borrower: {1}", comment, ls.Bor.AccountNumber);
                ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            if (ls.Letter.LetterType.IsIn("OPALGPBORR", "OPACLAIMRF"))
            {
                if (!RI.AddCommentInLP50(ls.Bor.Ssn, "MS", "16", "FMISC", comment, ScriptId))
                {
                    message = string.Format("Error adding FMISC comment:\r\n\r\n{0}\r\n\r\nin LP50 for borrower: {1}", comment, ls.Bor.AccountNumber);
                    ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }

            if (message.IsPopulated())
                MessageBox.Show(message, "Error adding comment", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Gets the message to use for each letter when adding a comment on borrower account
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        private string GetMessage(LetterSelection ls)
        {
            string message = "";
            switch (ls.Letter.LetterType)
            {
                case "OPAPIFBORR":
                    message = string.Format("PROCESS REFUND TO BORROWER FOR {0} EFFECTIVE {1}-ACCOUNT IS PIF.", ls.Bor.RefundAmount, ls.Bor.EffectiveDate);
                    break;
                case "OPAPIFCONS":
                    message = string.Format("PROCESS REFUND TO BORROWER FOR {0} EFFECTIVE {1}-ACCOUNT IS PIF BY CONSOLIDATION.", ls.Bor.RefundAmount, ls.Bor.EffectiveDate);
                    break;
                case "OPAESTATER":
                    message = string.Format("PROCESS REFUND TO BORROWER’S ESTATE FOR {0}-THIS REFUND IS FOR ANY PMTS RCVD AFTER THE BORROWER PASSED AWAY.", ls.Bor.RefundAmount);
                    break;
                case "OPALGPBORR":
                    message = string.Format("PROCESS REFUND TO LGP FOR BORROWER PAYMENT EFFECTIVE {0} FOR {1} RECEIVED FROM {2} – BORROWER IS SERVICED ON LGP.", ls.Bor.RefundAmount, ls.Bor.EffectiveDate, ls.Bor.PaymentSource);
                    break;
                case "OPACLAIMRF":
                    message = string.Format("PROCESS REFUND TO LGP FOR CLAIM PAYMENT EFFECTIVE {0} FOR {1} – CLAIM HAS BEEN RECALLED.", ls.Bor.EffectiveDate, ls.Bor.RefundAmount);
                    break;
                case "OPACOMPANY":
                    message = string.Format("PROCESS REFUND TO {0} FOR {1} EFFECTIVE {2} – ACCOUNT IS PIF BY {0}.", ls.CompanyData.FirstName, ls.Bor.RefundAmount, ls.Bor.EffectiveDate);
                    break;
                default:
                    break;
            }
            return message;
        }
    }
}