
namespace CLMPMTPST
{
    public class SasFileError
    {
		public int LineNumber { get; set; }
		public string Ssn { get; set; }
		public string LastName { get; set; }
		public string ExceptionMessage { get; set; }

		public SasFileError(int lineNumber, string ssn, string lastName, string exceptionMessage)
		{
			LineNumber = lineNumber;
			Ssn = ssn;
			LastName = lastName;
			ExceptionMessage = exceptionMessage;
		}
	}
}
