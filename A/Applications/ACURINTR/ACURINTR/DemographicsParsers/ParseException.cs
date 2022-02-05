using System;

namespace ACURINTR.DemographicsParsers
{
    class ParseException : Exception
    {
		public string AccountNumber { get; set; }
		public string DemographicsSource { get; set; }
		public string DemographicsText { get; set; }
		public string SystemSource { get; set; }

		public ParseException(string message, string accountNumber, string demographicsSource, string systemSource, string demographicsText)
            : base(message)
        {
            AccountNumber = accountNumber;
			DemographicsSource = demographicsSource;
            DemographicsText = demographicsText;
			SystemSource = systemSource;
        }

		public ParseException(string message, Exception innerException, string accountNumber, string demographicsSource, string systemSource, string demographicsText)
            : base(message, innerException)
        {
            AccountNumber = accountNumber;
			DemographicsSource = demographicsSource;
			DemographicsText = demographicsText;
			SystemSource = systemSource;
		}
    }
}
