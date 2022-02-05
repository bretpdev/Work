namespace CentralizedPrintingProcess
{
    public class FaxRecord : PropsToStringBase
    {
        public int SeqNum { get; set; }
        public string BusinessUnit { get; set; }
        public string LetterID { get; set; }
        public string FaxNumber { get; set; }
        public string AccountNumber { get; set; }
        public string CommentsAddedTo { get; set; }
        public string RightFaxHandle { get; set; }
    }
}