namespace ACURINTR
{
	/// <summary>
	/// Projection class for the DEMS_DAT_RejectActions table in BSYS.
	/// </summary>
	public class RejectAction
	{
		public string RejectReason { get; set; }
		public string ActionCodeAddress { get; set; }
		public string ActionCodePhone { get; set; }
		public string ActionCodeEmail { get; set; }

		/// <summary>
		/// Container for constants representing valid values for RejectReason.
		/// </summary>
		/// <remarks>
		/// The values of these constants must exactly match the distinct values
		/// of the RejectReason column from the DEMS_DAT_RejectActions table in BSYS.
		/// </remarks>
		public class RejectReasons
		{
			public const string ADDRESS_INVALID_IN_HISTORY = "Address invalid in history";
			public const string ADDRESS_SAME_AS_ON_FILE = "Address same as on file";
			public const string DEMOGRAPHICS_IS_INVALID = "demographics is invalid";
			public const string FORWARDING_ADDRESS_DOES_NOT_BELONG_TO_UHEAA_BORROWER = "Forwarding address does not belong to UHEAA borrower";
			public const string NO_PENDING_PDEM_RECORDS_TO_REVIEW = "No pending PDEM records to review";
			public const string NONE = "None";
			public const string PHONE_NUMBER_BLOCKED_BY_PARAMETER_CARD = "Phone number blocked by parameter card";
			public const string PHONE_NUMBER_INVALID_IN_HISTORY = "Phone number invalid in history";
			public const string PHONE_NUMBER_SAME_AS_ON_FILE = "Phone number same as on file";
			public const string RETURN_ADDRESS_WITHOUT_STREET = "Return address without street";
			public const string RETURN_FOREIGN_DEMOGRAPHICS = "Return foreign demographics";
			public const string RETURN_INCOMPLETE_ADDRESS = "Return incomplete address";
			public const string RETURN_INCOMPLETE_PHONE_NUMBER = "Return incomplete phone number";
			public const string RETURN_INVALID_ADDRESS = "Return invalid address";
			public const string RETURN_INVALID_PHONE_NUMBER = "Return invalid phone number";
		}

		/// <summary>
		/// Container for constants representing valid values for Source.
		/// </summary>
		/// <remarks>
		/// The values of these constants must exactly match the distinct values
		/// of the DemographicsSource column from the DEMS_DAT_RejectActions table in BSYS.
		/// </remarks>
		public class Sources
		{
			public const string ACCURINT = "Accurint";
			public const string AUTOPAY = "Autopay";
			public const string COMPASS_PENDING_PDEM = "COMPASS Pending PDEM";
			public const string DUDE = "DUDE";
			public const string EMAIL = "Email";
			public const string ONELINK_PENDING_PDEM = "OneLINK Pending PDEM";
			public const string POST_OFFICE = "Post Office";
			public const string SKIPCHNG_PENDING_PDEM = "SKIPCHNG Pending PDEM";
			public const string COMPASS_ACCURINT = "Accurint";
		}
	}
}
