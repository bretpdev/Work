namespace INCIDENTRP
{
	public class DataEntryIncident : IncidentBase
	{
		public bool IncorrectInformationWasAdded { get; set; }
		public bool InformationWasIncorrectlyChanged { get; set; }
		public bool InformationWasIncorrectlyDeleted { get; set; }


		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveDataEntryIncident(this, ticketNumber);
		}
	}
}
