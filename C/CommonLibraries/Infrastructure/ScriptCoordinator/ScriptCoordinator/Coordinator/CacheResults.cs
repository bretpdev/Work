using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace ScriptCoordinator
{
    public class CacheResults
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
        public CacheResults(bool successful, string message)
        {
            Successful = successful;
            Message = message;
        }
        public static CacheResults Success()
        {
            return new CacheResults(true, null);
        }
        public static CacheResults CacheFailure(ProcessLogData pld, string networkLocation, string localLocation, Tuple<string, Exception> failedFile)
        {
            return CacheFailure(pld, networkLocation, localLocation, new Tuple<string, Exception>[] { failedFile });
        }
        public static CacheResults CacheFailure(ProcessLogData pld, string networkLocation, string localLocation, IEnumerable<Tuple<string, Exception>> failedFiles)
        {
            List<string> lines = new List<string>();
            lines.Add("The following scripts could not be cached.  Please close all sessions and try again.  PL#" + pld.ProcessLogId);
            lines.Add(string.Format("Attempted to copy files from ({0}) to ({1}). ", networkLocation, localLocation));
            foreach (var failure in failedFiles)
            {
                string message = string.Format("Error attempting to copy file ({2}) from ({0}) to ({1}).", networkLocation, localLocation, failure.Item1);
                ProcessLogger.AddNotification(pld.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, pld.ExecutingAssembly, failure.Item2);
                lines.Add(failure.Item1);
            }
            return new CacheResults(false, string.Join(Environment.NewLine, lines));
        }
    }
}
