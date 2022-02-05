namespace DEMUPDTFED
{
	class RejectReason
	{
		//The following values need to exactly match the RejectReason column in CSYS.DemographicUpdateRejectReason.
		public const string BlockedAddress = "Blocked Address";
		public const string BlockedPhone = "Blocked Phone";
		public const string InHistoryMoreThan1Year = "In History More than 1 year";
		public const string InHistoryValid = "In History Valid";
		public const string InHistoryWithin1Year = "In History Within 1 Year";
		public const string IncomingForeignDemos = "Incoming Foreign Demos";
		public const string IncompleteAddress = "Incomplete Address";
		public const string IncompleteEmail = "Incomplete Email";
		public const string IncompletePhone = "Incomplete Phone";
		public const string InvalidAddress = "Invalid Address";
		public const string InvalidPhone = "Invalid Phone";
		public const string Locate = "Locate";
		public const string Match = "Match";
		public const string NoMatchRecentUpdate = "No Match, Recent Update";
		public const string NotInHistory = "Not in History";
	}
}