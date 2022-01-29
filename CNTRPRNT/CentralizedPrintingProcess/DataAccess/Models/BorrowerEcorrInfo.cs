namespace CentralizedPrintingProcess
{
    public class BorrowerEcorrInfo
    {
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public bool OptedIntoEcorrLetters { get; set; }
        public string EmailAddress { get; set; }
        public bool EmailAddressIsValid { get; set; }
    }
}