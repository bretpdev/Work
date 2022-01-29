using System.Collections.Generic;

namespace CLSCHLLNFD
{
    public class SchoolClosureData
    {
        public string BorrowerSsn { get; set; }
        public string StudentSsn { get; set; }
        public int LoanSeq { get; set; }
        public List<DisbursementData> DisbData { get; set; }
        public string SchoolCode { get; set; }
    }
}