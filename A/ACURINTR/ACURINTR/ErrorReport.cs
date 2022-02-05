using System;
using System.Data;
using System.IO;
using System.Text;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace ACURINTR
{
    class ErrorReport
    {
        private readonly string QUEUE_NAME;
        private int processLogId;
        /// <summary>
        /// Creates an object to facilitate reporting errors in the ACURINTR script.
        /// </summary>
        public ErrorReport(string queueName, int processLogId)
        {
            QUEUE_NAME = queueName.ToUpper();
            this.processLogId = processLogId;
        }

        /// <summary>
        /// Adds a record to the error file.
        /// Records are automatically sorted by error reason and account number
        /// before being written back to disk.
        /// </summary>
        /// <param name="task">The QueueTask object being processed when the error occurred.</param>
        /// <param name="errorReason">The reason for adding this record, as stated in the script spec.</param>
        public virtual void AddRecord(QueueTask task, string errorReason)
        {
            ProcessLogger.AddNotification(
                processLogId,
                string.Format("AccountNumber: {0}; QueueName: {1}; CapturedDemographics: {2}; ErrorReason: {3};", task.Demographics.AccountNumber, QUEUE_NAME, task.OriginalDemographicsText, errorReason),
                NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly());
        }
    }
}
