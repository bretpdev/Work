namespace INCIDENTRP
{
	public class AccessControlIncident : IncidentBase
	{
		public bool ImproperAccessWasGranted { get; set; }
		public bool SystemAccessWasNotTerminatedOrModified { get; set; }
		public bool PhysicalAccessWasNotTerminatedOrModified { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveAccessControlIncident(this, ticketNumber);
		}
	}
}
