using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace TILPCROST
{
    public class TilpCreditPost : ScriptBase
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }

        public TilpCreditPost(ReflectionInterface ri)
            : base(ri, "TILPCROST", DataAccessHelper.Region.Uheaa)
        {
            LogRun = RI.LogRun ?? new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, false, true);
            DA = new DataAccess(LogRun);
        }

        public override void Main()
        {
            //This process needs to post a batch so the amount that the user is expecting needs to be the same amount ready
            //to process in the database. This will make sure the user verifies the amounts that are loaded into the table with
            //any records that were previously in the table that need to be processed.
            List<TilpRecord> records = DA.GetAccounts();
            bool hadFile = LoadFile(records);
            if (hadFile || !hadFile && records?.Count() > 0)
            {
                records = DA.GetAccounts();
                double fileAmount = records.Sum(p => p.PrincipalAmount);
                string message = "";
                if (hadFile)
                    message = $"There are {records.Count} records available to process in the amount of {fileAmount}. Is this correct?";
                else
                    message = $"There was not a file found to process but there are {records.Count()} records available to process in the database in the amount of {fileAmount}. Is this correct?";
                if (Dialog.Info.YesNo(message, "TILP Teaching Credit Posting Script"))
                {
                    ProcessRecords(records);
                    PostFile(records);
                    foreach (TilpRecord record in records)
                        DA.SetProcessed(record.AccountsId);
                    Dialog.Info.Ok("Processing Complete!");
                }
                else
                    Dialog.Info.Ok("Process canceled, no records processed.");
            }

            LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
        }

        /// <summary>
        /// Loads the file. If a file is not available, checks for accounts that are ready to process, could be accounts in recovery.
        /// </summary>
        private bool LoadFile(List<TilpRecord> records)
        {
            bool foundFile = false;
            List<string> potentialFiles = new List<string>()
            { //The file could be named one of two names
                "TILP Teaching Credit Entry.txt",
                "TILPTeachingCredit.R2.txt"
            };
            foreach (string file in potentialFiles)
            {
                string path = $"{EnterpriseFileSystem.FtpFolder}{file}";
                if (File.Exists(path))
                {
                    foundFile = true;
                    using (StreamReader sr = new StreamR(path))
                    {
                        string header = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            List<string> lineData = sr.ReadLine().SplitAndRemoveQuotes(",").ToList();
                            TilpRecord record = new TilpRecord
                            {
                                AccountNumber = lineData[0],
                                TransactionType = lineData[1],
                                LoanSequence = lineData[2].ToInt(),
                                PrincipalAmount = lineData[3].ToDouble(),
                                TransactionDate = lineData[4].ToDate(),
                                LastName = lineData[5]
                            };
                            DA.InsertRecord(record);
                        }
                    }
                    Repeater.TryRepeatedly(() => FS.Delete(path));
                    //If the first file was found, don't process the second file
                    break;
                }
            }

            if (!foundFile && records?.Count() == 0)
            {
                string message = "The script could not find a data file to process. Verify the file is in the FTP folder and try again";
                Dialog.Error.Ok(message, "Missing File");
                LogRun.AddNotification(message, NotificationType.NoFile, NotificationSeverityType.Critical);
            }
            return foundFile;
        }

        /// <summary>
        /// Writes off the interest for each record then calls the method to write off late fees
        /// </summary>
        private void ProcessRecords(List<TilpRecord> records)
        {
            foreach (TilpRecord record in records)
            {
                RI.FastPath($"TX3ZATS3Q{record.AccountNumber}{DateTime.Now:MMddyy}");
                if (RI.ScreenCode == "TSX3S")
                {
                    int row = 7;
                    while (record.LoanSequence != RI.GetText(row, 20, 4).ToInt() && RI.MessageCode != "90007")
                    { //If the loan sequence was not found, increase the row, check for next screen and move on
                        row++;
                        if (RI.GetText(row, 4, 4).IsNullOrEmpty())
                        {
                            row = 7;
                            RI.Hit(ReflectionInterface.Key.F8);
                        }
                    }
                    RI.PutText(22, 19, RI.GetText(row, 3, 2), ReflectionInterface.Key.Enter);
                }
                if (!RI.CheckForText(1, 68, "TSX3O"))
                {
                    string message = $"There was an error writing off interest for borrower: {record.AccountNumber}, loan sequence: {record.LoanSequence}, amount: {record.PrincipalAmount}.";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    break;
                }
                if (record.PrincipalAmount >= RI.GetText(12, 32, 11).ToDouble() && RI.GetText(13, 32, 11).ToDouble() > 0)
                {
                    RI.PutText(8, 42, "X");
                    RI.PutText(9, 45, "O");
                    RI.PutText(12, 48, "0.00");
                    RI.PutText(13, 49, RI.GetText(13, 32, 11));
                    RI.PutText(18, 2, "Wrote off outstanding interest for TILP Teaching Credit borrower", ReflectionInterface.Key.F11);
                    WriteOffLateFee(record);
                }
                else
                {
                    string message = "";
                    if (record.PrincipalAmount != RI.GetText(12, 32, 11).ToDouble())
                        message = $"The principal amount does not match the amount from the file for borrower: {record.AccountNumber}, loan sequence: {record.LoanSequence}, amount: {record.PrincipalAmount}";
                    else if (RI.GetText(13, 32, 11).ToDouble() == 0)
                        message = $"The interest amount is $0 and can not be written off for borrower: {record.AccountNumber}, loan sequence: {record.LoanSequence}, amount: {record.PrincipalAmount}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
            }
        }

        /// <summary>
        /// Do a write off for the borrower of all late fees
        /// </summary>
        private void WriteOffLateFee(TilpRecord record)
        {
            RI.FastPath($"TX3ZCTS89{record.Ssn}");
            RI.PutText(10, 36, "W");
            RI.PutText(11, 36, "010101");
            RI.PutText(12, 36, DateTime.Now.ToString("MMddyy"));
            RI.PutText(13, 36, "S");
            RI.PutText(17, 36, "X", ReflectionInterface.Key.F10);
            bool found = true;
            if (RI.MessageCode != "03726")
            {
                int row = 12;
                while (RI.GetText(row, 18, 2).ToInt() != record.LoanSequence && RI.MessageCode != "90007")
                {
                    row++;
                    if (RI.GetText(row, 3, 2).IsNullOrEmpty())
                    {
                        row = 12;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
                if (row == 12 && RI.MessageCode == "90007")
                    found = false;
                else
                {
                    RI.PutText(row, 3, "X");
                    RI.Hit(ReflectionInterface.Key.F6, 2);
                }
                if (RI.MessageCode != "03347" || !found)
                {
                    string message = $"There was an error writing off the late fees for borrower: {record.AccountNumber}, loan sequence: {record.LoanSequence}, amount: {record.PrincipalAmount}.";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
            }
        }

        /// <summary>
        /// Add each record to the batch, target the record and verify totals
        /// </summary>
        private void PostFile(List<TilpRecord> records)
        {
            RI.FastPath("TX3ZATS1G");
            if (RI.ScreenCode == "T1X03")
            {
                RI.Hit(ReflectionInterface.Key.EndKey);
                RI.Hit(ReflectionInterface.Key.Enter);
            }
            RI.PutText(6, 51, "3"); // Batch code
            RI.PutText(10, 28, records.Count().ToString()); //Total number of records to process in batch
            RI.PutText(11, 28, records.Sum(p => p.PrincipalAmount).ToString("##0.00")); //Total amount to add to batch
            RI.PutText(12, 28, "54");
            RI.PutText(15, 28, UserId, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.PrintScreen);
            string batchNo = RI.GetText(6, 18, 14);

            RI.FastPath($"TX3ZATS1J{batchNo}");
            int row = 8;
            foreach (TilpRecord record in records)
            {
                RI.PutText(row, 5, record.AccountNumber);
                RI.PutText(row, 17, record.PrincipalAmount.ToString("##0.00"));
                RI.PutText(row, 28, record.TransactionDate.ToString("MMddyy"));
                RI.PutText(row, 41, record.TransactionType.Substring(record.TransactionType.Length - 2, 2), ReflectionInterface.Key.Enter);
                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.MessageCode == "30008")
                {
                    //Clear out the duplicate transaction error
                    RI.Hit(ReflectionInterface.Key.F2);
                    RI.PutText(22, 17, RI.GetText(row, 2, 2), ReflectionInterface.Key.F4);
                    RI.PutText(11, 17, record.LastName);
                    RI.PutText(19, 2, $"not duplication / {UserId}", ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F12);
                    RI.Hit(ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F2);
                }
                //Target the transaction
                RI.PutText(22, 17, RI.GetText(row, 2, 2), ReflectionInterface.Key.F6);
                if (!ProcessTargeting(record.LoanSequence, record.PrincipalAmount))
                {
                    string message = $"Error targeting loan for borrower: {record.AccountNumber}, loan sequence: {record.LoanSequence}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
                RI.Hit(ReflectionInterface.Key.F12);
                row++;
                if (row == 20) //Go to the next screen if the screen if full
                {
                    if (RI.CheckForText(24, 13, "SET1"))
                        RI.Hit(ReflectionInterface.Key.F2);
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 8;
                }
            }
            //Verify the batch totals
            RI.FastPath($"TX3ZCTS1R{batchNo}");
            if (RI.GetText(11, 28, 13).ToDouble() == RI.GetText(10, 67, 13).ToDouble())
            {
                RI.Hit(ReflectionInterface.Key.F10);
                RI.Hit(ReflectionInterface.Key.PrintScreen); //Print the screen
            }
        }

        /// <summary>
        /// Targets the loan by sequence number in the batch.
        /// </summary>
        private bool ProcessTargeting(int loanSequence, double principalAmount)
        {
            if (RI.ScreenCode != "TSX1N")
                return false;
            int row = 12;
            while (RI.MessageCode != "90007")
            {
                if (RI.GetText(row, 50, 2).ToInt() == loanSequence)
                {
                    RI.PutText(row, 3, "X");
                    RI.PutText(row, 6, principalAmount.ToString());
                    break;
                }
                row++;
                if (!RI.CheckForText(row, 3, "_"))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 12;
                }
            }
            RI.Hit(ReflectionInterface.Key.Enter);
            return RI.MessageCode == "01004";
        }
    }
}