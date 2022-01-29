namespace INCIDENTRP
{
	public class DisposalOrDestructionIncident : IncidentBase
	{
		public bool ElectronicMediaRecordsWereDestroyedInError { get; set; }
		public bool ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod { get; set; }
		public bool MicrofilmWithRecordsWasDestroyedInError { get; set; }
		public bool MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod { get; set; }
		public bool PaperRecordsWereDestroyedInError { get; set; }
		public bool PaperRecordsWereDestroyedUsingIncorrectMethod { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveDisposalOrDestructionIncident(this, ticketNumber);
		}
	}
}
