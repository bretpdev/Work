using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLoggerRC;

namespace RCCALLHIST
{
    public class Processor
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public Processor(ProcessLogRun logRun)
        {
            DA = new DataAccess(logRun);
            LogRun = logRun;
        }

        public Program.Status Process()
        {
            var outboundStatus = ProcessOutbound();
            var inboundStatus = ProcessInbound();
            return inboundStatus == Program.Status.Success && outboundStatus == Program.Status.Success ? Program.Status.Success : Program.Status.Failure;
        }

        public Program.Status ProcessOutbound()
        {
            var calls = DA.GetOutboundCalls();
            if(calls == null || calls.Count == 0)
            {
                LogRun.AddNotification("No outbound calls found.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return Program.Status.Success;
            }
            var success = DA.InsertOutboundCalls(calls);
            return success ? Program.Status.Success : Program.Status.Failure;
        }

        public Program.Status ProcessInbound()
        {
            var calls = DA.GetInboundCalls();
            if (calls == null || calls.Count == 0)
            {
                LogRun.AddNotification("No inbound calls found.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return Program.Status.Success;
            }
            var success = DA.InsertInboundCalls(calls);
            return success ? Program.Status.Success : Program.Status.Failure;
        }

    }
}
