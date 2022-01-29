namespace INCIDENTRP
{
	public class RegularMailDeliveryIncident : IncidentBase
	{
		//Valid values for Problem, according to the LST_RegularMailDeliveryProblem table:
		public const string INCORRECT_ADDRESS = "Incorrect Address";
		public const string INCORRECT_CONTENTS = "Incorrect Contents";

		public string Problem { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string TrackingNumber { get; set; }

		public override bool IsComplete()
		{
			if (string.IsNullOrEmpty(Address1))
                return false; 
			if (string.IsNullOrEmpty(City))
                return false; 
			if (string.IsNullOrEmpty(State))
                return false; 
			if (string.IsNullOrEmpty(Zip))
                return false; 
			return true;
		}

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			dataAccess.SaveRegularMailDeliveryIncident(this, ticketNumber);
		}
	}
}
