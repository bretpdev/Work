using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace OLQBuilder
{
    public class OneLinkQueueBuilder
    {
        public string ScriptId = "OLQTSKBLDR";
        private DataAccess DA { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private ReflectionInterface RI { get; set; }

        public OneLinkQueueBuilder(ReflectionInterface ri, ProcessLogRun logRun)
        {
            DA = new DataAccess(logRun);
            LogRun = logRun;
            RI = ri;
        }

        public void Process()
        {

            FileLoader loader = new FileLoader(DA, LogRun);
            loader.LoadFiles();

            List<QueueRecord> records = DA.GetUnprocessedQueues();
            foreach(var record in records)
            {
                DA.UpdatingProcessingAttempts(record.QueueId);
                bool postTwice = record.Filename != null && record.Filename.ToUpper().StartsWith("ULWK19");
                if (!AddQueueTaskInLP9O(record, postTwice))
                {
                    //Moved Notification to inside AddQueueTask to improve logging
                }
                else
                {
                    bool success = DA.MarkQueueRecordProcessed(record.QueueId);
                    if(!success)
                    {
                        LogRun.AddNotification($"Failed to mark queue record processed, QueueId:{record.QueueId} TargetId:{record.TargetId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
            }
        }

        //This function differs from the Q.Common function in that it deals with the institution ID and type,
        //and posts comments twice for SAS file ULWK19.
        private bool AddQueueTaskInLP9O(QueueRecord task, bool postCommentTwice)
        {
            //Try up to three times to successfully access LP9O.
            for (int i = 0; i < 3; i++)
            {
                RI.FastPath(string.Format("LP9OA{0};;{1}", task.TargetId, task.QueueName));
                if (RI.CheckForText(22, 3, "44000")) { break; }
                if (i == 2) 
                {
                    LogRun.AddNotification($"Failed to add queue task for record(Failed to access LP90), QueueId:{task.QueueId} TargetId:{task.TargetId} SessionError:{RI.GetText(22, 3, 79)}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false; 
                }
                Thread.Sleep(5000);
            }

            //Enter the queue task info.
            RI.PutText(9, 25, task.InstitutionId);
            RI.PutText(9, 34, task.InstitutionType);
            if (task.DateDue != null) 
            {
                RI.PutText(11, 25, task.DateDue.Value.ToString("MMddyyyy")); 
            }
            if (task.TimeDue != null) 
            {
                RI.PutText(12, 25, task.TimeDue.Value.ToString("hhmm")); 
            }
            RI.PutText(16, 12, task.Comment);
            RI.Hit(Key.F6);
            Thread.Sleep(2000); //Go figure why this has to be here but it wouldn't work otherwise.
            if (postCommentTwice)
            {
                RI.PutText(16, 12, task.Comment);
                RI.Hit(Key.F6);
                Thread.Sleep(2000);
            }
            //Verify that the queue was updated.
            if (!RI.CheckForText(22, 3, "49000", "48003")) 
            {
                LogRun.AddNotification($"Failed to add queue task for record(Failed posting queue task), QueueId:{task.QueueId} TargetId:{task.TargetId} SessionError:{RI.GetText(22, 3, 79)}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false; 
            }
            return true;
        }
    }
}
