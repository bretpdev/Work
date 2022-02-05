using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace CPINTRTLPD
{
    public class Logger
    {
        ProcessLogData pld;
        Action<string> processMessage;
        Action<string> processError;
        public Logger(ProcessLogData pld, Action<string> processMessage, Action<string> processError)
        {
            this.pld = pld;
            this.processMessage = processMessage;
            this.processError = processError;
        }

        public void Log(string message, params object[] parameters)
        {
            message = string.Format(message, parameters);
            ProcessLogger.AddNotification(pld.ProcessLogId, message, NotificationType.EndOfJob, NotificationSeverityType.Informational);
            processMessage(message);
        }

        public void Error(string message, params object[] parameters)
        {
            message = string.Format(message, parameters);
            ProcessLogger.AddNotification(pld.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            processError(message);
        }
    }
}
