
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PEPSFED
{
	class AffiliationUpdater 
	{
        private DataAccess DA { get; set; }
        public AffiliationUpdater(DataAccess da)
		{
            DA = da;
		}

		public void UpdateSystem(AffiliationData data)
		{
            //Change of affiliation requires user review, so just send the record to the error report.
            Program.PLR.AddNotification(string.Format("Please review the following Affiliation Change.  {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            DA.UpdateAffiliationProcessed(data.RecordId);
        }
	}
}
