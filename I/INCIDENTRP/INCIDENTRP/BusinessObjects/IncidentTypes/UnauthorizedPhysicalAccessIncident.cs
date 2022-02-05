namespace INCIDENTRP
{
	public class UnauthorizedPhysicalAccessIncident : IncidentBase
	{
		public bool AccessAccountingDiscrepancy { get; set; }
		public bool BuildingBreakIn { get; set; }
		public bool Piggybacking { get; set; }
		public bool SuspiciousEntryInAccessLog { get; set; }
		public bool SuspiciousEntryInVideoLog { get; set; }
		public bool UnauthorizedUseOfKeycard { get; set; }
		public bool UnexplainedNewKeycard { get; set; }
		public bool UnusualTimeOfUsage { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveUnauthorizedPhysicalAccessIncident(this, ticketNumber);
		}
	}
}
