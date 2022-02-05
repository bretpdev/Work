using System;

namespace PAYHISTLPP
{
    public class Payment
    {
        public DateTime TransactionEffectiveDate { get; set; }
        public DateTime PostEffectiveDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string TransactionType { get; set; }
        public string TransactionAmount { get; set; }
        public string Principal { get; set; }
        public string Interest { get; set; }
        public string Fees { get; set; }
        public string Balance { get; set; }
    }
}