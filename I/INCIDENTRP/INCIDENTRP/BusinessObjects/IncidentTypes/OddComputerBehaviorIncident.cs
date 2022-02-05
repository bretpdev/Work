namespace INCIDENTRP
{
	public class OddComputerBehaviorIncident : IncidentBase
	{
		public bool EmailPhishingOrHoax { get; set; }
		public bool DenialOfService { get; set; }
		public bool UnexplainedAttemptToWriteToSystemFiles { get; set; }
		public bool UnexplainedModificationOrDeletionOfDate { get; set; }
		public bool UnexplainedModificationToFileLengthOrDate { get; set; }
		public bool UnexplainedNewFilesOrUnfamiliarFileNames { get; set; }
		public bool Malware { get; set; }

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveOddComputerBehaviorIncident(this, ticketNumber);
		}
	}
}
