using System;
using Uheaa.Common.DocumentProcessing;

namespace PAYARRREHB
{
    public class PaymentArrangement
    {
        public string SSN { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public double Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public int DueDay { get; set; }
        public bool isCoborrower { get; set; }
        public ArrangementType Type { get; set; }
    }
}
