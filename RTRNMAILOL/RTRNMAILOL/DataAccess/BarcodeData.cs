using System;

namespace RTRNMAILOL
{
    public class BarcodeData
    {
        public int BarcodeDataId { get; set; }
        public string AccountIdentifier { get; set; }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string LetterId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Comment { get; set; }
        //Will contain the Ssn of the person the letter to the recipient is in regards to
        public string BorrowerSsn { get; set; }
        public bool IsReference { get; set; }
    }
}