using System;
using System.Collections.Generic;

namespace CLSCHLLNFD
{
    public class ProcessingData
    {
        public int SchoolClosureDataId { get; set; }
        public string BorrowerSsn { get; set; }
        public string AccountNumber { get; set; }
        public string StudentSsn { get; set; }
        public int LoanSeq { get; set; }
        public List<int> LoanSeqs { get; set; }
        public int DisbursementSeq { get; set; }
        public double DischargeAmount { get; set; }
        public DateTime DischargeDate { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }

        public ProcessingData()
        {
            LoanSeqs = new List<int>();
        }
    }
}