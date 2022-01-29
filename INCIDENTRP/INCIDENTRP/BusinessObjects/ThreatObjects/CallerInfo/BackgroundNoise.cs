namespace INCIDENTRP
{
	public class BackgroundNoise : IsEmptyBase
	{
		public bool Airplanes { get; set; }
		public bool Animals { get; set; }
		public bool Conversation { get; set; }
		public bool Crowd { get; set; }
		public bool FactoryMachines { get; set; }
		public bool Music { get; set; }
		public bool OfficeMachines { get; set; }
		public bool Party { get; set; }
		public bool PublicAddressSystem { get; set; }
		public bool SchoolBell { get; set; }
		public bool StreetTraffic { get; set; }
		public bool Trains { get; set; }
		public bool Other { get; set; }
		public string OtherDescription { get; set; }

		public static BackgroundNoise Load(DataAccess dataAccess, long ticketNumber)
		{
			return dataAccess.LoadBackgroundNoise(ticketNumber);
		}

		public void Save(DataAccess dataAccess, long ticketNumber)
		{
			if (IsEmpty())
			 dataAccess.DeleteBackgroundNoise(ticketNumber); 
			else
		 dataAccess.SaveBackgroundNoise(this, ticketNumber); 
		}
	}
}
