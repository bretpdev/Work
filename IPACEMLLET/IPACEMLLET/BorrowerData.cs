using System;

namespace IPACEMLLET
{
    public class BorrowerData
    {
        public string BorrowerSsn { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ForeignState { get; set; }
        public string Country { get; set; }
        public bool HasValidAddress { get; set; }
    }
}
