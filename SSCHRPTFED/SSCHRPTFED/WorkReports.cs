using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;

namespace SSCHRPTFED
{
    public class WorkReports
    {
        private ReflectionInterface RI { get; set; }
        private ProcessLogData ProcessLogs { get; set; }
        private EndOfJobReport Eoj { get; set; }
        private RecoveryLog Recovery { get; set; }
        private bool FirstEmail { get; set; }
        public WorkReports(ReflectionInterface ri, ProcessLogData processLogs, RecoveryLog recovery, EndOfJobReport eoj)
        {
            RI = ri;
            ProcessLogs = processLogs;
            Recovery = recovery;
            Eoj = eoj;
            FirstEmail = false;
        }

        public void Process(SchoolInfo sInfo)
        {
            string schoolFile = string.Format("{0}{1}*", sInfo.SchoolCode, sInfo.BranchCode);
            List<string> filesToProcess = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, schoolFile).ToList();

            if (filesToProcess.Count < 1)
            {
                string message = string.Format("The following file was missing: {0}.  The script will continue to the next school.", schoolFile);
                SendSchoolReports.MissingFiles.Add(schoolFile);

                ProcessLogger.AddNotification(ProcessLogs.ProcessLogId, message, NotificationType.NoFile, NotificationSeverityType.Warning);

                Eoj.Counts[SendSchoolReports.EOJ_MISSING_FILES].Increment();
                return;
            }

            foreach (string file in filesToProcess)
            {
                string toAddress = string.Empty;

                switch (sInfo.Recipient.ToUpper())
                {
                    case "B":
                        toAddress = string.Format("{0};{1}", sInfo.EmailAddressSchool, sInfo.EmailAddress3rdParty);
                        break;
                    case "S":
                        toAddress = sInfo.EmailAddressSchool;
                        break;
                    case "T":
                        toAddress = sInfo.EmailAddress3rdParty;
                        break;
                }

                string subject = string.Format("[Secure] School Information from CornerStone for {0}", sInfo.SchoolName);
                string body = string.Format("Attached is the CornerStone borrower information for {0}", sInfo.SchoolName);

                if (!FirstEmail)
                {
                    ScriptBase.SendMail("sshelp@utahsbr.edu", "noreply@mycornerstoneloan.org", "Send School Reports is Starting: " + subject, body, "", file, ScriptBase.EmailImportance.Normal, true);
                    FirstEmail = true;
                }

                try
                {
                    ScriptBase.SendMail(toAddress, "noreply@mycornerstoneloan.org", subject, body, "", file, ScriptBase.EmailImportance.Normal, true);
                    Eoj.Counts[SendSchoolReports.EOJ_EMAILS_SENT].Increment();
                }
                catch (Exception ex)
                {
                    string message = SchoolInfo.GenerateErrorNotificationString(sInfo);
                    ProcessLogger.AddNotification(ProcessLogs.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ProcessLogs.ExecutingAssembly, ex);
                    Eoj.Counts[SendSchoolReports.EOJ_ERROR_REPORT].Increment();
                }

                Eoj[SendSchoolReports.TotalNumberOfFiles].Increment();
                Recovery.RecoveryValue = string.Format("{0},{1},{2}", sInfo.SchoolCode, sInfo.BranchCode, file);
                SendSchoolReports.DeleteFile(file);
                Recovery.RecoveryValue = string.Format("{0},{1}", sInfo.SchoolCode, sInfo.BranchCode);
            }
        }
    }
}
