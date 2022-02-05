using System;

namespace CONPMTPST
{
    public class BorrowerData
    {
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime FirstDisbursement;
        public DateTime EffectiveDate { get; set; }
        public decimal PayoffAmount { get; set; }
        public string LoanType { get; set; }
        public int LoanSequence { get; set; }
        public string ManifestNumber { get; set; }
        public string NsldsId { get; set; }
        public string OriginatorId { get; set; }
        public string Servicer { get; set; }
        public bool ShouldTarget { get; set; }
    }
}