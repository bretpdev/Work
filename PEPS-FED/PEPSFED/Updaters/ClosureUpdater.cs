using System;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace PEPSFED
{
    public class ClosureUpdater 
    {
        private DataAccess DA { get; set; }
        public ClosureUpdater(DataAccess da)
        {
            DA = da;
        }

        public void UpdateSystem(ReflectionInterface ri, ClosureData data)
        {
            Console.WriteLine("About to process CLOSURE_ID: {0}", data.RecordId);
            DateTime? closureDate = data.ParseDate(data.ClosureDtCurrent, data);
            if (closureDate.HasValue)
            {
                ri.FastPath(string.Format("TX3Z/CTX0Y{0};000", data.OpeId));
                if (ri.CheckForText(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
                {
                    Program.PLR.AddNotification(string.Format("Unable to update Closure Info for the following peps line: {0}; Session Message:{1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateClosureProcessed(data.RecordId);
                    return;
                }

                ri.Hit(Key.F10);
                ri.PutText(13, 19, "C"); //CURRENT STATUS
                if (closureDate.Value != default(DateTime))
                    ri.PutText(14, 27, closureDate.Value.ToString("MMddyy"));

                ri.Hit(Key.Enter);
                if (!ri.CheckForText(23, 2, "01005", "01004", "01003"))
                    Program.PLR.AddNotification(string.Format("Unable to update Closure Info for the following peps line: {0}; Session Message:{1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            DA.UpdateClosureProcessed(data.RecordId);
        }
    }
}
