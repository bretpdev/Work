namespace INCIDENTRP
{
	public class ElectronicMailDeliveryIncident : IncidentBase
	{
		public bool EmailAddressWasDisclosed { get; set; }
		public bool FtpTransmissionWasSentToIncorrectDestination { get; set; }
		public bool IncorrectAttachmentContainedPii { get; set; }
		public bool EmailWasDeliveredToIncorrectAddress { get; set; }
		public string Detail { get; set; }

		public override bool IsComplete()
		{
			if (!EmailAddressWasDisclosed && !FtpTransmissionWasSentToIncorrectDestination && !IncorrectAttachmentContainedPii && !EmailWasDeliveredToIncorrectAddress)
                return false;
			if (string.IsNullOrEmpty(Detail))
                return false;
			return true;
		}

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveElectronicMailDeliveryIncident(this, ticketNumber);
		}
	}
}
