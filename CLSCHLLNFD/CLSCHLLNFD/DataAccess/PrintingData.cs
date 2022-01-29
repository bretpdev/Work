using System;

namespace CLSCHLLNFD
{
    public class PrintingData
    {
        public string BorrowerSsn { get; set; }
        public string AccountNumber { get; set; }
        public int LoanSeq { get; set; }
        public string SchoolCode { get; set; }
        public int SchoolClosureDataId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}