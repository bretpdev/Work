using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace IDRCONRPT
{
    public class Processor
    {
        public const string ScriptId = "IDRCONRPT";
        ProcessLogRun PLR;
        public Processor(ProcessLogRun plr)
        {
            PLR = plr;
        }
        public void Process()
        {
            var efs = new Efs();
            efs.EnsureDirectoriesExist();
            var filesToProcess = Directory.GetFiles(efs.SearchDirectory, efs.SearchPattern);
            string reviewPath = Path.Combine(efs.ReviewDirectory, Timestamp() + ".csv");
            bool anyRejects = false;
            var helper = new XPathHelper(PLR);
            foreach (var file in filesToProcess)
            {
                string contents = File.ReadAllText(file);
                var nav = helper.GetNavigatorFromContents(file, contents);
                if (nav == null)
                    continue;
                var outputPath = Path.Combine(efs.OutputDirectory, Path.GetFileName(file));
                if (File.Exists(outputPath))
                    outputPath = Path.Combine(efs.OutputDirectory, Path.GetFileNameWithoutExtension(file) + Timestamp() + Path.GetExtension(file));
                var rejects = helper.GetRejects(nav, outputPath);
                File.Move(file, outputPath);
                PLR.AddNotification(string.Format("Moved {0} to {1}", file, outputPath), NotificationType.EndOfJob, NotificationSeverityType.Informational);
                if (rejects.Any())
                {
                    anyRejects = true;
                    foreach (var reject in rejects)
                    {
                        if (!File.Exists(reviewPath))
                            File.AppendAllLines(reviewPath, new string[] { "SSN,FirstName,LastName,File" });
                        File.AppendAllLines(reviewPath, new string[] { string.Join(",", reject.Ssn, reject.FirstName, reject.LastName, reject.Path) });
                    }
                }
                PLR.AddNotification(string.Format("Successfully processed {0}.  {1} Borrowers Rejected", file, rejects.Count), NotificationType.EndOfJob, NotificationSeverityType.Informational);
            }
            if (anyRejects)
            {
                try
                {
                    EmailHelper.SendMailBatch(DataAccessHelper.TestMode, efs.ErrorReportEmail, "", "Consol Reject Report", "An error report file has been generated at " + reviewPath, "", "", "", EmailHelper.EmailImportance.High, false);
                }
                catch (Exception ex)
                {
                    PLR.AddNotification("Error sending report email.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }
            }
        }
        private string Timestamp()
        {
            return DateTime.Now.ToString("yyyyMMdd_hhmmss");
        }
    }
}
