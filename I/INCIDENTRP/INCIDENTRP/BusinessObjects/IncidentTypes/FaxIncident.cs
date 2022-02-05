namespace INCIDENTRP
{
	public class FaxIncident : IncidentBase
	{
		public string FaxNumber { get; set; }
		public bool IncorrectDocumentsWereFaxed { get; set; }
		public bool FaxNumberWasIncorrect { get; set; }
		public string Recipient { get; set; }

		public override bool IsComplete()
		{
			if (string.IsNullOrEmpty(FaxNumber))
                return false; 
			if (string.IsNullOrEmpty(Recipient))
                return false; 
			return true;
		}

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveFaxIncident(this, ticketNumber);
		}
	}
}
