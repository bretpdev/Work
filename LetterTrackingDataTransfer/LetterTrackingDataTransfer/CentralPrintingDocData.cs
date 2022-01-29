namespace LetterTrackingDataTransfer
{
    public class CentralPrintingDocData
    {
        public int DocSeqNo { get; set; }
        public string ID { get; set; }
        public decimal Pages { get; set; }
        public string Instructions { get; set; }
        public bool Duplex { get; set; }
        public string UHEAACostCenter { get; set; }
        public string Path { get; set; }
        public bool ResendMail { get; set; }
    }
}