namespace INCIDENTRP
{
	public class UnauthorizedSystemAccessIncident : IncidentBase
	{
		public bool SuspiciousEntryInSystemOrNetworkLog { get; set; }
		public bool SystemAccountDiscrepancy { get; set; }
		public bool UnauthorizedUseOfUserCredentials { get; set; }
		public bool UnexplainedNewUserAccount { get; set; }
		public bool UnusualTimeOfUsage { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveUnauthorizedSystemAccessIncident(this, ticketNumber);
		}
	}
}
