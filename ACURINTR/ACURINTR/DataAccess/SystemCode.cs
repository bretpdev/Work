namespace ACURINTR
{
    /// <summary>
	/// Projection class for the DEMS_DAT_SystemCodes table in BSYS.
    /// </summary>
    public class SystemCode
    {
        public string Source { get; set; }
        public string LocateType { get; set; }
        public string OneLinkSourceCode { get; set; }
        public string CompassSourceCode { get; set; }
		public string ActivityType { get; set; }
		public string ContactType { get; set; }

        /// <summary>
        /// Container for constants representing valid values for Source.
        /// </summary>
        /// <remarks>
        /// The values of these constants must exactly match the distinct values
        /// of the Source column from the DEMS_DAT_SystemCodes table in BSYS.
        /// </remarks>
        public class Sources
        {
            public const string ACCURINT = "Accurint";
			public const string AUTOPAY = "Autopay";
			public const string COMPASS = "COMPASS";
			public const string COMPASS_EMAIL = "COMPASS Email";
			public const string CREDIT_BUREAU = "Credit Bureau";
			public const string DUDE = "DUDE";
            public const string NON_CREDIT_BUREAU = "Non CB";
			public const string ONELINK_EMAIL = "OneLINK Email";
			public const string POST_OFFICE = "Post Office";
        }
    }
}
