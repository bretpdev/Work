using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.DocumentProcessing;
using System.IO;
using System.Diagnostics;

namespace ACHREVFED
{
    public class AchReviewFed
    {
        ReflectionInterface ri { get; set; }
        ProcessLogRun PLR { get; set; }
        string DialerFile { get; set; }
        DataAccess DA { get; set; }
        readonly string ScriptId;
        public AchReviewFed(string scriptId, ProcessLogRun plr, ReflectionInterface ri, DataAccess da)
        {

            this.ScriptId = scriptId;
            this.PLR = plr;
            this.DialerFile = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("UNWO20.NWO20R4.{0}.txt", Guid.NewGuid().ToBase64String()));
            this.DA = da;
            this.ri = ri;
        }

        public bool Process(bool skipWorkAdd = false)
        {
            if (skipWorkAdd)
                Console.WriteLine("Skipping work add step.");
            else
                DA.AddNewWork();
            List<ACHData> accountsToUpdate = DA.GetAccounts();
            CleanNullValues(accountsToUpdate);
            Console.WriteLine("Found {0} accounts to process", accountsToUpdate.Count);

            foreach (ACHData account in accountsToUpdate)
            {
                if(account.IsCoBorrower)
                {
                    SendLetter(account.AccountNumber, account.Ssn, account.IsCoBorrower, account.BorrowerSsn);
                    DA.MarkAccountProcessed(account.AccountNumber);
                }
                else
                {
                    if (ProcessCTS7O(ri, account.AccountNumber))
                    {
                        SendLetter(account.AccountNumber, account.Ssn, account.IsCoBorrower, account.BorrowerSsn);
                        AddToDialerFile(account);
                        DA.MarkAccountProcessed(account.AccountNumber);
                    }
                }
            }

            if (File.Exists(DialerFile))
            {
                File.Copy(DialerFile, Path.Combine(EnterpriseFileSystem.FtpFolder, Path.GetFileName(DialerFile)));
                File.Copy(DialerFile, Path.Combine(EnterpriseFileSystem.GetPath("Archive"), Path.GetFileName(DialerFile)));
                Repeater.TryRepeatedly(() => File.Delete(DialerFile));
            }
            ri.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();
            return true;
        }

        private void CleanNullValues(List<ACHData> accountsToUpdate)
        {
            foreach (ACHData record in accountsToUpdate)
            {
                foreach (PropertyInfo pi in record.GetType().GetProperties())
                {
                    if (pi.PropertyType == typeof(string))
                    {
                        try
                        {
                            string origValue = (string)pi.GetValue(record, null);
                            string value = origValue;
                            value = value.Trim();
                            if (string.IsNullOrEmpty(value))
                                value = "";
                            if (value != origValue)
                                pi.SetValue(record, value, null);
                        }
                        catch (Exception)
                        {
                            continue; //getter or setter threw an error, probably a property we weren't supposed to access.
                        }
                    }
                }
            }
        }

        public void AddToDialerFile(ACHData account)
        {
            using (StreamWriter sw = new StreamWriter(DialerFile, true))
            {
                if (account.HomePhone.IsPopulated() || account.WorkPhone.IsPopulated() || account.AltPhone.IsPopulated()) //If any phone number is valid, add to dialer file
                    sw.WriteLine(account.ToString());
            }
        }

        private void SendLetter(string account, string ssn, bool isCoBorrower, string borrowerSsn)
        {
            try
            {
                AddressInfo addr = AddressInfo.GetBorrowersAddress(DA, account, isCoBorrower);
                bool onEcorr = DA.CheckOnEcorr(account);
                if (!isCoBorrower && (addr.HasValidAddress || onEcorr))
                {
                    string letterData = CreateLetterData(addr, ssn, addr.AccountNumber);
                    EcorrProcessing.AddRecordToPrintProcessing(ScriptId, "APDNYFED", letterData, addr.AccountNumber, "MA4481");
                }
                else if (isCoBorrower && (addr.HasValidAddress || onEcorr))
                {
                    string borrowerAccountNumber = DA.GetAccountNumberFromSsn(borrowerSsn);
                    string letterData = CreateLetterData(addr, ssn, borrowerAccountNumber);
                    EcorrProcessing.AddCoBwrRecordToPrintProcessing(ScriptId, "APDNYFED", letterData, addr.AccountNumber, "MA4481", borrowerSsn);
                }
            }
            catch (Exception ex)
            {
                LogError(string.Format("The Letter APDNYFED could not be generated for the follow borrower {0}", account), ex);
            }
        }

        private string CreateDataFile(AddressInfo addr, string ssn)
        {
            string keyLine = DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string file = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("{0}_{1}.txt", addr.AccountNumber, Guid.NewGuid().ToBase64String()));
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine("KeyLine, AccountNumber, Name, Address1, Address2, City, State, Zip, Country, DenialReason1, DenialReason2, DenialReason3, DenialReason4, DenialReason5");

                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},You've had multiple insufficient funds.,,,,",
                    keyLine, addr.AccountNumber, addr.Name, addr.Address1, addr.Address2, addr.City, addr.State, addr.Zip, addr.Country));
            }

            return file;
        }

        private string CreateLetterData(AddressInfo addr, string ssn, string accountNumber)
        {
            string keyLine = DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);

            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},You've had multiple insufficient funds.,,,,",
                keyLine, accountNumber, addr.Name, addr.Address1, addr.Address2, addr.City, addr.State, addr.Zip, addr.Country);
        }

        /// <summary>
        /// Removed a borrowers ACH
        /// </summary>
        /// <param name="ri">Reflection Interface object</param>
        /// <param name="accountNumber">Borrowers Account number</param>
        public bool ProcessCTS7O(IReflectionInterface ri, string accountNumber)
        {
            ri.FastPath("TX3Z/CTS7O");
            ri.PutText(8, 42, accountNumber, ReflectionInterface.Key.Enter);
            if (ri.ScreenCode != "TSX7K")
            {
                LogError(string.Format("Unknown screen found for account {0} expected TSX7K but found {1}", accountNumber, ri.ScreenCode));
                return false;
            }

            ri.PutText(10, 18, "D", ReflectionInterface.Key.Enter);
            ri.PutText(10, 57, "N", ReflectionInterface.Key.Enter);

            if (ri.MessageCode != "02526")
            {
                LogError(string.Format("Unable to remove ACH from the following account: {0}; Session Message: {1}", accountNumber, ri.Message));
                return false;
            }

            return true;
        }

        void LogError(string message, Exception ex = null)
        {
            PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
        }
    }
}
