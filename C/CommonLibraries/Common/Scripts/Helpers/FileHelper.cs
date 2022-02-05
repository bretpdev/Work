using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Uheaa.Common.ProcessLogger;
using System.Threading;

namespace Uheaa.Common.Scripts
{
    public static class FileHelper
    {
        /// <summary>
        /// Deletes an empty file. Adds a Notification to the ProcessLogger if an error occured while deleting.
        /// </summary>
        public static void DeleteEmptyFile(string filePath, int processLogId, Assembly assembly, int retryCount = 5)
        {
            var results = Repeater.TryRepeatedly(() => FS.Delete(filePath), retryCount, 5000, increaseMillisecondDelay: true);
            if (File.Exists(filePath))
            {
                string deleteMessage = string.Format("The file {0} was empty but could not be deleted.", filePath);
                ProcessLogger.ProcessLogger.AddNotification(processLogId, deleteMessage, NotificationType.EmptyFile,
                    NotificationSeverityType.Warning, assembly, results.CaughtExceptions.LastOrDefault());
            }
            else
            {
                string message = string.Format("The file {0} was empty and has been deleted. A copy of the file can be found in Archive.", filePath);
                ProcessLogger.ProcessLogger.AddNotification(processLogId, message, NotificationType.EmptyFile, NotificationSeverityType.Informational);
            }          
        }

        /// <summary>
        /// Deletes a file. Adds a notification to the ProcessLogger if an error occured while deleting.
        /// </summary>
        public static void DeleteFile(string filePath, int processLogId, Assembly assembly, int retryCount = 5)
        {
            var results = Repeater.TryRepeatedly(() => FS.Delete(filePath), retryCount, 5000, increaseMillisecondDelay: true);
            if (File.Exists(filePath))
            {
                string deleteMessage = string.Format("The file {0} could not be deleted.", filePath);
                ProcessLogger.ProcessLogger.AddNotification(processLogId, deleteMessage, NotificationType.EmptyFile, 
                    NotificationSeverityType.Warning, assembly, results.CaughtExceptions.LastOrDefault());
            }
        }
    }
}
