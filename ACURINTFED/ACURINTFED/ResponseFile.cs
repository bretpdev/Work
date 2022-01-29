using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;

namespace ACURINTFED
{
    class ResponseFile
    {
        private const string RECOVERY_STEP_BACKUP = "Created backup";
        private const string RECOVERY_STEP_ARC = "Created ARC";
        private const string RECOVERY_RESPONSE = "Response file saved to T drive";
        private const string RESPONSE_FILE_PATTERN = "OutputAccurintFEDRequestFile_*";
        private readonly IEnumerable<string> DownloadedFiles;
        private readonly RecoveryLog Recovery;
        private ReflectionInterface RI { get; set; }
        private static string ScriptId { get; set; }
        private static ProcessLogRun logrun;

        private ResponseFile(ReflectionInterface ri, RecoveryLog recovery, IEnumerable<string> downloadedFiles, string scriptId, ProcessLogRun PLR)
        {
            DownloadedFiles = downloadedFiles;
            Recovery = recovery;
            RI = ri;
            ScriptId = scriptId;
            logrun = PLR;
            
        }

        #region Factory methods
        public static ResponseFile DownloadAll(ReflectionInterface ri, RecoveryLog recovery, Credentials ftpCredentials)
        {
            return DownloadAll(ri, recovery, ftpCredentials, logrun);
        }
        public static ResponseFile DownloadAll(ReflectionInterface ri, RecoveryLog recovery, Credentials ftpCredentials, ProcessLogRun PLR)
        {
            //Check that there's not already a response file.
            if (Directory.GetFiles(EnterpriseFileSystem.TempFolder, RESPONSE_FILE_PATTERN).Length > 0)
            {
                if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live)
                    throw new Exception("one or more response files already exist in T drive");
            }

            FtpHandler ftp = new FtpHandler(ftpCredentials);
            IEnumerable<string> outputFiles = ftp.DownloadFiles(RESPONSE_FILE_PATTERN, EnterpriseFileSystem.TempFolder);
            if (outputFiles.Count() > 0)
            {
                recovery.RecoveryValue = Accurint.RECOVERY_PHASE_DOWNLOADED;
                return new ResponseFile(ri, recovery, outputFiles, ScriptId, PLR);
            }
            else
                return null;
        }//DownloadAll()

        public static ResponseFile GetDownloaded(ReflectionInterface ri, RecoveryLog recovery)
        {
            return GetDownloaded(ri, recovery, logrun);
        }

        public static ResponseFile GetDownloaded(ReflectionInterface ri, RecoveryLog recovery, ProcessLogRun PLR)
        {
            IEnumerable<string> foundFiles = Directory.GetFiles(EnterpriseFileSystem.TempFolder, RESPONSE_FILE_PATTERN).Where(p => new FileInfo(p).LastWriteTime.DayOfYear == DateTime.Now.DayOfYear);

            if (foundFiles.Count() > 0)
                return new ResponseFile(ri, recovery, foundFiles, ScriptId, PLR);
            else
                return null;
        }//GetDownloaded()
        #endregion Factory methods

        public void Review(ErrorReport err, EndOfJobReport eoj, Credentials sessionCreds)
        {
            //Check for multiple response files.
            if (DownloadedFiles.Count() > 1)
                throw new Exception("one or more response files already exist in T drive");

            //Check that the file is not empty.
            string responseFile = DownloadedFiles.Single();
            if (string.IsNullOrEmpty(File.ReadAllText(responseFile)))
                throw new Exception("AccurintFEDResponseFile is empty");

            //Work with what we got.
            string decryptedFile = DecryptAndArchive(responseFile);
            CreateQueueTasks(decryptedFile, err, eoj, sessionCreds);

            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
                MessageBox.Show(string.Format("Please pull the decrypted file {0} file the T drive before it is deleted.", responseFile));

            //Clean up.
            File.Delete(responseFile);
            File.Delete(decryptedFile);
        }//Review()

        private void CreateQueueTasks(string decryptedFile, ErrorReport err, EndOfJobReport eoj, Credentials sessionCreds)
        {
            if (!RI.CheckForText(16, 2, "L"))
                RI.LogOut();

            if (!RI.Login(sessionCreds.UserName, sessionCreds.Password, DataAccessHelper.Region.CornerStone))
            {
                MessageBox.Show("Invalid Password, Please correct and try again.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new EndDLLException();
            }

            //Process the response file.
            using (StreamReader responseReader = new StreamReader(decryptedFile))
            {
                //read in the header row
                string responseLine = responseReader.ReadLine();
                int lastLineProcessed = 1;

                //Advance to the line we ended on if in recovery.
                if (!string.IsNullOrEmpty(Recovery.RecoveryValue) && Recovery.RecoveryValue.Split(',').Length >= 3 && Recovery.RecoveryValue.Split(',')[1] == RECOVERY_STEP_ARC)
                {
                    int recoveryLine = int.Parse(Recovery.RecoveryValue.Split(',')[2]);
                    for (; lastLineProcessed <= recoveryLine; lastLineProcessed++) { responseLine = responseReader.ReadLine(); }
                }//if
                else
                    responseLine = responseReader.ReadLine();

                //Process the remaining records.
                while (responseLine != null)
                {
                    eoj.Counts[Accurint.EOJ_RETRIEVED_FROM_ACCURINT].Increment();
                    List<string> responseFields = responseLine.SplitAndRemoveQuotes(",");
                    string ssn = responseFields[0];
                    string address1 = responseFields[9];
                    string city = responseFields[10];
                    string state = responseFields[11];
                    string zip = responseFields[12].Replace("-", "");
                    string phone = responseFields[13];

                    RI.FastPath("TX3Z/ITX1J");
                    RI.PutText(5, 16, "", true);
                    RI.PutText(6, 16, "", true);
                    RI.PutText(6, 20, "", true);
                    RI.PutText(6, 23, "", true);
                    RI.PutText(6, 16, ssn, ReflectionInterface.Key.Enter);

                    bool isBorrower = CheckTx1j(ssn, "B");
                    bool isEndorser = CheckTx1j(ssn, "E");

                    //We are assuming that all tasks are for borrower there will be very few endorser accounts
                    string borrowersSsn = ssn;
                    string endorserSsn = string.Empty;
                    string accountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
                    if (RI.CheckForText(1, 71, "TXX1R-02"))
                    {
                        borrowersSsn = RI.GetText(7, 11, 11).Replace(" ", "");
                        endorserSsn = ssn;
                    }

                    const string SOURCE = "Accurint";
                    if (isBorrower || isEndorser)
                    {
                        //Add a queue task for address information if an address was provided.
                        AddressComments(err, eoj, address1, city, state, zip, isBorrower, isEndorser, borrowersSsn, endorserSsn, accountNumber, SOURCE);

                        //Add a queue task for phone information if a phone was provided.
                        PhoneComments(err, eoj, phone, isBorrower, isEndorser, borrowersSsn, endorserSsn, accountNumber, SOURCE);

                        Recovery.RecoveryValue = string.Format("{0},{1},{2}", Accurint.RECOVERY_PHASE_PROCESSING, RECOVERY_STEP_ARC, lastLineProcessed);
                        lastLineProcessed++;
#if DEBUG
                        //if (MessageBox.Show("Would you like to process another task?", "New Task", MessageBoxButtons.YesNo) == DialogResult.No)
                        //    return;
#endif
                    }
                    responseLine = responseReader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Adds a TD22 ARC for borrower/Endorser account if the Address exists
        /// </summary>
        /// <param name="err">ErrorReport</param>
        /// <param name="eoj">EndOfJobReport</param>
        /// <param name="address1"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="isBorrower"></param>
        /// <param name="isEndorser"></param>
        /// <param name="borrowersSsn"></param>
        /// <param name="endorserSsn"></param>
        /// <param name="accountNumber"></param>
        /// <param name="SOURCE">Const string "Accurint"</param>
        private void AddressComments(ErrorReport err, EndOfJobReport eoj, string address1, string city, string state, string zip, bool isBorrower, bool isEndorser, string borrowersSsn, string endorserSsn, string accountNumber, string SOURCE)
        {
            if (address1.IsPopulated())
            {
                if (isBorrower)
                {
                    string comment = string.Format("{0},{1},,{2},{3},{4},,,,", SOURCE, address1, city, state, zip);
                    if (RI.Atd22ByBalance(borrowersSsn, "ACRTN", comment, "", ScriptId, false, false, false))
                        eoj.Counts[Accurint.EOJ_RA01_QUEUE_TASKS].Increment();
                    else
                    {
                        string errorMessage = string.Format("error creating LX/01 queue task for {0}", accountNumber);
                        logrun.AddNotification(string.Format("error creating LX/01 queue task for {0} {1} {2} {3} {4}", DataAccess.GetAccount(borrowersSsn), address1, city, state, zip), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        eoj.Counts[Accurint.EOJ_ERRORS].Increment();
                        ProcessLogger.AddNotification(RI.ProcessLogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    }
                }
                else if (isEndorser)
                {
                    string comment = string.Format("{0},{1},,{2},{3},{4},,,,", SOURCE, address1, city, state, zip);

                    List<int> loans = GetEndorserLoans(borrowersSsn, endorserSsn);

                    if (RI.Atd22ByLoan(borrowersSsn, "ACREN", comment, endorserSsn, loans, ScriptId, false))
                        eoj.Counts[Accurint.EOJ_RA01_QUEUE_TASKS].Increment();
                    else
                    {
                        string errorMessage = string.Format("error creating EX/01 queue task for {0}", accountNumber);
                        logrun.AddNotification(string.Format("error creating EX/01 queue task for {0} {1} {2} {3} {4}", DataAccess.GetAccount(borrowersSsn), address1, city, state, zip), NotificationType.ErrorReport, NotificationSeverityType.Informational);
                        //err.AddRecord(errorMessage, new ErrorDemographics(address1, city, state, zip));
                        eoj.Counts[Accurint.EOJ_ERRORS].Increment();
                        ProcessLogger.AddNotification(RI.ProcessLogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    }
                }
                else
                {
                    //Create an unknown type error if the contact type was not a borrower or endorser.
                    string message = string.Format("Expecting Borrower or Endorser but an unknown type was found for account {0}", accountNumber);
                    ProcessLogger.AddNotification(RI.ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
        }

        /// <summary>
        /// Adds a TD22 ARC for borrower/Endorser account if the Phone exists
        /// </summary>
        /// <param name="err">ErrorReport object</param>
        /// <param name="eoj">EndOfJobReport object</param>
        /// <param name="phone"></param>
        /// <param name="isBorrower"></param>
        /// <param name="isEndorser"></param>
        /// <param name="borrowersSsn"></param>
        /// <param name="endorserSsn"></param>
        /// <param name="accountNumber"></param>
        /// <param name="SOURCE">const string "Accurint"</param>
        private void PhoneComments(ErrorReport err, EndOfJobReport eoj, string phone, bool isBorrower, bool isEndorser, string borrowersSsn, string endorserSsn, string accountNumber, string SOURCE)
        {
            if (phone.IsPopulated())
            {
                if (isBorrower)
                {
                    string comment = string.Format("{0},,,,,,,{1},,", SOURCE, phone);
                    if (RI.Atd22ByBalance(borrowersSsn, "ACRTN", comment, "", ScriptId, false, false, false))
                        eoj.Counts[Accurint.EOJ_RA01_QUEUE_TASKS].Increment();
                    else
                    {
                        string errorMessage = string.Format("error creating LX/01 queue task for {0}", accountNumber);
                        logrun.AddNotification(string.Format("error creating LX/01 queue task for {0} {1}", DataAccess.GetAccount(borrowersSsn), phone), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        eoj.Counts[Accurint.EOJ_ERRORS].Increment();
                    }
                }
                else if (isEndorser)
                {
                    string comment = string.Format("{0},,,,,,,{1},,", SOURCE, phone);
                    List<int> loans = GetEndorserLoans(borrowersSsn, endorserSsn);

                    if (RI.Atd22ByLoan(borrowersSsn, "ACREN", comment, endorserSsn, loans, ScriptId, false))
                        eoj.Counts[Accurint.EOJ_RA01_QUEUE_TASKS].Increment();
                    else
                    {
                        string errorMessage = string.Format("error creating EX/01 queue task for {0}", accountNumber);
                        logrun.AddNotification(string.Format("error creating EX/01 queue task for {0} {1}", DataAccess.GetAccount(borrowersSsn), phone), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        eoj.Counts[Accurint.EOJ_ERRORS].Increment();
                    }
                }
                else
                {
                    //Create an unknown type error if the contact type was not a borrower or endorser.
                    string message = string.Format("Expecting Borrower or Endorser but an unknown type was found for account {0}", accountNumber);
                    ProcessLogger.AddNotification(RI.ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
        }

        private List<int> GetEndorserLoans(string borrowerSsn, string endorserSsn)
        {
            RI.FastPath("TX3Z/ITX1JB" + borrowerSsn);
            RI.Hit(Key.F2);
            RI.Hit(Key.F4);
            List<int> loans = new List<int>();

            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 21 || RI.GetText(row, 3, 1).IsNullOrEmpty())
                {
                    RI.Hit(Key.F8);
                    row = 9;
                    continue;
                }

                if (RI.CheckForText(row, 5, "E") && RI.CheckForText(row, 13, endorserSsn))
                    loans.Add(RI.GetText(row, 9, 3).ToInt());
            }

            return loans;
        }

        /// <summary>
        /// Verifes that the borrower and endorser accounts exist in TX1J
        /// </summary>
        /// <param name="ssn">Borrower/Endorser SSN</param>
        /// <param name="type">The type of account: Borrower/Endorser</param>
        /// <returns>True if the account is the correct type and exists in TX1J</returns>
        private bool CheckTx1j(string ssn, string type)
        {
            RI.FastPath("TX3Z/ITX1J");
            RI.PutText(5, 16, type, true);
            RI.PutText(6, 16, "", true);
            RI.PutText(6, 20, "", true);
            RI.PutText(6, 23, "", true);
            RI.PutText(6, 16, ssn, ReflectionInterface.Key.Enter);

            if (type.ToUpper() == "B")
                return RI.CheckForText(1, 71, "TXX1R-01");
            else if (type.ToUpper() == "E")
                return RI.CheckForText(1, 71, "TXX1R-02");
            else
                return false;
        }

        private string DecryptAndArchive(string encryptedFile)
        {
            //Decrypt the response file, convert the line endings to DOS format, and archive a copy.
            string decryptedFile = string.Format("{0}AccurintFEDResponseFile.txt", EnterpriseFileSystem.TempFolder);

            if (string.IsNullOrEmpty(Recovery.RecoveryValue) || Recovery.RecoveryValue == RECOVERY_RESPONSE)
            {
                FileEncryption.DecryptFile(encryptedFile, decryptedFile);
                CheckFile.Unix2DosNewline(decryptedFile);
                string archiveFile = string.Format("{0}AccurintFEDResponseFile_{1:MMddyyyyhhmmss}.txt", EnterpriseFileSystem.GetPath("Accurint Archive"), DateTime.Now);
                File.Copy(decryptedFile, archiveFile);
            }

            return decryptedFile;
        }//DecryptAndArchive()

        //For use with the error report, here's a private class with just the properties we care about.
        private class ErrorDemographics
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Phone { get; set; }

            public ErrorDemographics(string phoneNumber)
            {
                Street = "";
                City = "";
                State = "";
                Zip = "";
                Phone = phoneNumber;
            }

            public ErrorDemographics(string street, string city, string state, string zip)
            {
                Street = street;
                City = city;
                State = state;
                Zip = zip;
                Phone = "";
            }
        }
    }//class
}//namespace
