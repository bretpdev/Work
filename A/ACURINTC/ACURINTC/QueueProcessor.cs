using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACURINTC
{
    public class QueueProcessor
    {
        public ProcessLogRun LogRun { get; set; }
        public ReflectionInterface RI { get; set; }
        private string UserId { get; set; }
        private DataAccess DA { get; set; }
        private string ScriptId { get; set; }

        public QueueProcessor(ProcessLogRun logRun, string scriptId, DataAccess da, ReflectionInterface ri, string userId)
        {
            LogRun = logRun;
            UserId = userId;
            DA = da;
            ScriptId = scriptId;
            this.RI = ri;
        }

        public int Process(QueueInfo queueData, List<PendingDemos> recordsToProcess, bool skipTaskClose)
        {
            var g = new General(RI, UserId, ScriptId, LogRun, DA);
            try
            {
                switch (queueData.ProcessorId)
                {
                    case Processor.Compare:
                        CompareProcessor cp = new CompareProcessor(g, skipTaskClose);
                        cp.Process(queueData, recordsToProcess);
                        break;
                    case Processor.Pdem:
                        PdemProcessor pp = new PdemProcessor(g, skipTaskClose);
                        pp.Process(queueData, recordsToProcess);
                        break;
                    default:
                        {
                            string message = string.Format("Invalid or unsupported processor for Queue: {0}, Department: {1}", queueData.Queue, queueData.SubQueue);
                            LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            return 1;
                        }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("There was an error processing queue: {0}; Processor Class: {1}", queueData.Queue, queueData.ProcessorId.ToString());
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return 1;
            }
            return 0;
        }
    }
}