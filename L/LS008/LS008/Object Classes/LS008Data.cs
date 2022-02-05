using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS008
{
    public class LS008Data
    {
        public string BorrowerName { get; set; }
        public string BwrSsn { get; set; }
        public string AccountNumber { get; set; }
        public string TaskControlNumber { get; set; }
        public string ActivitySeq
        {
            get
            {
                return TaskControlNumber.Substring(TaskControlNumber.Length - 5);
            }
        }
        public string CorrDocNum { get; set; }
        public bool HasNoLoans { get; set; }
        public List<DupDcns> DupDcns { get; set; }
        public List<int> LoanSeq { get; set; }
        public List<HistoryCommentData> HistoryComments { get; set; }

        public LS008Data ()
        {
            DupDcns = new List<DupDcns>();
            LoanSeq = new List<int>();
            HistoryComments = new List<HistoryCommentData>();
        }
    }
}
