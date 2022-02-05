using System;

namespace BATCHESP
{
    public class Ts2hPendingDisbursement 
    {
        public int Ts2hPendingDisbursementId { get; set; }
        public string BorrowerSSN { get; set; }
        public int LoanSequence { get; set; }
        public int DisbSequence { get; set; }
        public string DisbType { get; set; }
        public DateTime? DisbursementDate { get; set; }
    }
}