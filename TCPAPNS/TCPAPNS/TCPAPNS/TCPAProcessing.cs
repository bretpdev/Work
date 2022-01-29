using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace TCPAPNS
{
    public class TCPAProcessing
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private readonly string[] CompassFiles = new string[] { "ULWS62.LWS62R2" };
        private readonly string[] OneLinkFiles = new string[] { "ULWS62.LWS62R3", "ULWS62.LWS62R4" };

        private UpdateCompass Compass { get; set; }
        private UpdateOnelink OneLink { get; set; }

        public TCPAProcessing(ProcessLogRun logRun, bool oneLink)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun.LDA, oneLink);
            if (oneLink)
                OneLink = new UpdateOnelink(logRun, DA);
            else
                Compass = new UpdateCompass(logRun, DA);
        }

        public void Process()
        {
            ReflectionInterface ri = new ReflectionInterface();
            var loginInfo = BatchProcessingLoginHelper.Login(LogRun, ri, Program.ScriptId, "BatchUheaa");
            if (loginInfo == null)
            {
                string message = "Error logging into session";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                WriteLine(message);
                return;
            }
            WriteLine($"Logging into session using User Id: {loginInfo.UserName}");

            List<FileProcessingRecord> records = DA.GetUnprocessedRecords();
            WriteLine($"There are {records.Count} records to process.");

            foreach (var record in records)
                UpdateStatus(ri, record);

            ri.CloseSession();
        }

        public void UpdateStatus(ReflectionInterface ri, FileProcessingRecord record)
        {
            string sourceFile = record.SourceFile.Substring(record.SourceFile.IndexOf("ULWS62"), 14).ToUpper();
            if (record.AccountNumber.Contains("RF@") && OneLinkFiles.Any(p => p.ToUpper().StartsWith(sourceFile)))
            {
                if (!OneLink.UpdateReference(ri, record))
                    LogRun.AddNotification($"Unable to update phone number in Onelink reference, account: {record.AccountNumber} phone: {record.PhoneNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            else
            {
                if (CompassFiles.Any(p => p.ToUpper().StartsWith(sourceFile)))
                    if (!Compass.Update(ri, record))
                        LogRun.AddNotification($"Unable to update phone number in Compass, account: {record.AccountNumber} phone: {record.PhoneNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                if (OneLinkFiles.Any(p => p.ToUpper().StartsWith(sourceFile)) && !record.AccountNumber.Contains("P"))
                    if (!OneLink.Update(ri, record))
                        LogRun.AddNotification($"Unable to update phone number in Onelink, account: {record.AccountNumber} phone: {record.PhoneNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }
    }
}