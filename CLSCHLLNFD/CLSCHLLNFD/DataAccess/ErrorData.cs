using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSCHLLNFD
{
    public class ErrorData
    {
        public string BorrowerSsn { get; set; }
        public string AccountNumber { get; set; }
        public int LoanSeq { get; set; }
        public int DisbursementSeq { get; set; }
        public string Arc { get; set; }
        public string ErrorMessage { get; set; }
        public string SessionMessage { get; set; }
        public int SchoolClosureDataId { get; set; }
        public int ArcAddProcessingId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
