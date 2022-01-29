using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace SKIPACTS
{
    internal class SkipActivityProcess
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private ReflectionInterface RI { get; set; }
        private BatchProcessingHelper LoginInfo { get; set; }
        private string ScriptId { get; set; }
        private const string sasFilePattern = "ULWK07.LWK07R2.*";

        public SkipActivityProcess(ProcessLogRun logRun, string scriptId)
        {
            LogRun = logRun;
            ScriptId = scriptId;
            DA = new DataAccess(logRun);
            RI = new ReflectionInterface();
            LoginInfo = BatchProcessingLoginHelper.Login(logRun, RI, ScriptId, "BatchUheaa");
            Console.WriteLine($"Logging into session using user: {LoginInfo.LoginId}");
            RI.LogRun = LogRun;
        }

        internal int Process()
        {
            List<string> filesToProcess = GetFiles();
            if (!filesToProcess.Any())
            {
                string message = "No ULWK07 files were found. Contact Systems Support for assistance";
                LogRun.AddNotification(message, NotificationType.NoFile, NotificationSeverityType.Critical);
                Console.WriteLine(message);
                return 1;
            }

            List<SkipRecord> records = ParseFile(filesToProcess);

            //start a session to drop arcs in.  Required as this cares about arc order and AAP cant handle that currently
            
            foreach (SkipRecord record in records)
                AddArcs(record);

            RI.CloseSession();

            foreach (string file in filesToProcess)
                Repeater.TryRepeatedly(() => FS.Delete(file));

            return 0;
        }
        internal void AddArcs(SkipRecord record)
        {
            string accountNumber = DA.GetAccountNumberFromSsn(record.Ssn);
            string message;
            bool processed = false;
            if (accountNumber.IsPopulated())
            {
                processed = RI.Atd22AllLoans(accountNumber, record.ActivityCode, record.Comment, record.Ssn, ScriptId, false);

                if (!processed)
                {
                    message = $"Unable to add Arc for accountNumber: {accountNumber} activity code: {record.ActivityCode} activity response: {record.ActivityType} comment: {record.Comment}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine(message);
                }
                else if (record.ActivityType != "COMPL")
                { //Arc was added.  Need to mark with completion code
                    RI.FastPath("TX3Z/CTD2A" + record.Ssn);
                    RI.PutText(11, 65, record.ActivityCode);
                    RI.PutText(6, 60, "X");
                    string currentDate = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev ? new DateTime(2020, 06, 30).ToString("MMddyy") : DateTime.Now.ToString("MMddyy");
                    RI.PutText(21, 16, currentDate, ReflectionInterface.Key.Enter, true);
                    if (RI.CheckForText(23, 2, "01019"))
                    {
                        message = $"Unable to mark completed Arc for accountNumber: {accountNumber} activity code: {record.ActivityCode} activity response: {record.ActivityType} comment: {record.Comment} error message: {RI.Message}";
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        Console.WriteLine(message);
                    }
                    else
                    {
                        if (RI.CheckForText(1, 72, "TDX2C"))
                        {
                            RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
                            while (!RI.CheckForText(15, 2, "_") && !RI.CheckForText(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                                RI.Hit(ReflectionInterface.Key.F8);
                        }
                        if (RI.CheckForText(15, 2, "_"))
                        {
                            RI.PutText(15, 2, record.ActivityType, ReflectionInterface.Key.Enter);
                            if (RI.MessageCode != "01005")
                            {
                                message = $"Unable to mark completed Arc for accountNumber: {accountNumber} activity code: {record.ActivityCode} activity response: {record.ActivityType} comment: {record.Comment} error message: {RI.Message}";
                                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                Console.WriteLine(message);
                            }
                            else
                            {
                                message = $"Added arc successfully for accountNumber: {accountNumber} activity code: {record.ActivityCode} activity response: {record.ActivityType} comment: {record.Comment}";
                                Console.WriteLine(message);
                            }
                        }
                    }
                }
            }
            else
            {
                message = $"Account number not found for ssn {record.Ssn}.  Borrower not on system.";
                Console.WriteLine(message);
            }

        }

        internal List<SkipRecord> ParseFile(List<string> filesToProcess)
        {
            List<SkipRecord> records = new List<SkipRecord>();

            foreach (string file in filesToProcess)
            {
                Console.WriteLine($"Reading file {file}...");
                using (StreamReader sr = new StreamR(file))
                {
                    sr.ReadLine(); //remove header
                    while (!sr.EndOfStream)
                    {
                        List<string> currentLine = sr.ReadLine().SplitAndPreserveQuotes(",");
                        SkipRecord tempRecord = new SkipRecord
                        {
                            Ssn = currentLine[0],
                            ActivityCode = currentLine[1],
                            Comment = currentLine[2],
                            ActivityType = currentLine[3]
                        };
                        records.Add(tempRecord);
                    }
                }
            }
            return records;
        }

        private List<string> GetFiles()
        {
            return Directory.GetFiles(EnterpriseFileSystem.FtpFolder, sasFilePattern).ToList();
        }
    }
}