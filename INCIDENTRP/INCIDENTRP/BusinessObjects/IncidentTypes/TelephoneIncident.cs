using Uheaa.Common;

namespace INCIDENTRP
{
	public class TelephoneIncident : IncidentBase
	{
		public bool RevealedInformationOnVoicemail { get; set; }
		public bool RevealedInformationToUnauthorizedIndividual { get; set; }
		public string UnauthorizedIndividual { get; set; }

		public override bool IsComplete()
		{
			if (RevealedInformationToUnauthorizedIndividual)
                return UnauthorizedIndividual.IsPopulated();
			return true;
		}

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveTelephoneIncident(this, ticketNumber);
		}
	}
}