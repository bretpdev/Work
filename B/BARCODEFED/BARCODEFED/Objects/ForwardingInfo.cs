using System;

namespace BARCODEFED
{
	public class ForwardingInfo
	{
        public int ReturnMailId { get; set; }
        public string RecipientId { get; set; }
		public string Ssn { get; set; }
		public string LetterId { get; set; }
		public DateTime CreateDate { get; set; }
		//public BarcodeScanner.PersonType? PersonType { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
        public string PersonType { get; set; }
        //Will contain the Ssn of the person the letter to the recipient is in regards to
        public string BorrowerSsn { get; set; }
        public DateTime ReceivedDate { get; set; }

        //Provide a no-parameter constructor so a DataContext can use this as a projection class.
        public ForwardingInfo() { }

        //Code can use this constructor to copy properties from a BarcodeInfo object.
        public ForwardingInfo(BarcodeInfo barcodeInfo)
        {
            RecipientId = barcodeInfo.RecipientId;
            LetterId = barcodeInfo.LetterId;
            CreateDate = barcodeInfo.CreateDate;
            ReceivedDate = barcodeInfo.ReceivedDate;
            PersonType = BarcodeScanner.PersonTypeToString(barcodeInfo.PersonType);
        }
	}
}