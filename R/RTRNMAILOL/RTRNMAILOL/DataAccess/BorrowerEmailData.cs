namespace RTRNMAILOL
{
    public class BorrowerEmailData
    {
        public string AccountNumber { get; set; }
        public string BorSsn { get; set; }
        public string EndSsn { get; set; }
        public string EmailData { get; set; }
        public int Priority { get; set; } = -1;
        public string State { get; set; }
    }
}