using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    abstract class BatchProcessorBase
    {
        protected General G { get; private set; }
        protected DataAccess DA { get; private set; }
        protected bool SkipTaskClose { get; private set; }

        public BatchProcessorBase(General g, bool skipTaskClose)
        {
            G = g;
            DA = g.DA;
            this.SkipTaskClose = skipTaskClose;
        }

        public void Process(QueueInfo queueData, List<PendingDemos> recordsToProcess)
        {
            foreach (var record in recordsToProcess)
            {
                try
                {
                    ProcessApplicablePath(queueData, record);
                    if (!record.HasAddress && !record.HasHomePhone)
                    {
                        var cth = new CompassTaskHelper(G, this.SkipTaskClose);
                        cth.CloseOrReassignTask(queueData, record, "Unable to close task.");
                    }
#if !DEBUG
                    G.DA.UpdateProcessed(record.ProcessQueueId);
#endif
                }
                catch (Exception ex)
                {
                    G.LogRun.AddNotification("Error processing ProcessQueueId #" + record.ProcessQueueId, Uheaa.Common.ProcessLogger.NotificationType.ErrorReport, Uheaa.Common.ProcessLogger.NotificationSeverityType.Critical, ex);
                }
            }
        }

        protected abstract void ProcessApplicablePath(QueueInfo data, PendingDemos task);
    }
}