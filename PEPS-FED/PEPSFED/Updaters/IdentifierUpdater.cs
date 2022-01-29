using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;
using System;

namespace PEPSFED
{
    class IdentifierUpdater 
    {
        private DataAccess DA { get; set; }
        public IdentifierUpdater(DataAccess da)
        {
            DA = da;
        }

        public void UpdateSystem(ReflectionInterface ri, IdentiferData data)
        {
            Console.WriteLine("About to process SCHIDS_ID: {0}", data.RecordId);
            ri.FastPath(string.Format("TX3Z/CTX0Y{0};000", data.OpeId));
            if (ri.CheckForText(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
            {
                Program.PLR.AddNotification(string.Format("Unable to update Identifier Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateIdentifierProcessed(data.RecordId);
                return;
            }
            ri.Hit(Key.F10);
            ri.PutText(16, 57, data.FdslpIdCurrent, Key.Enter);
            if (!ri.CheckForText(23, 2, "01005", "01004", "01003"))
            {
                Program.PLR.AddNotification(string.Format("Unable to update Identifier Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateIdentifierProcessed(data.RecordId);
                return;
            }

            DA.UpdateIdentifierProcessed(data.RecordId);
        }
    }
}
