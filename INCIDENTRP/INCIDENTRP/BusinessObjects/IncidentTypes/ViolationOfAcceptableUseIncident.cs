namespace INCIDENTRP
{
	public class ViolationOfAcceptableUseIncident : IncidentBase
	{
		public bool AccessKeycardWasShared { get; set; }
		public bool MisuseOfSystemResourcesByValidUser { get; set; }
		public bool UserSystemCredentialsWereShared { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveViolationOfAcceptableUseIncident(this, ticketNumber);
		}
	}
}
