namespace INCIDENTRP
{
	public class SystemOrNetworkUnavailableIncident : IncidentBase
	{
		public bool DenialOrDisruptionOfService { get; set; }
		public bool UnableToLogIntoAccount { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveSystemOrNetworkUnavailableIncident(this, ticketNumber);
		}
	}
}
