using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCRAINTUP
{
    class BorrowerLoanData
    {
        public int LoanSeq { get; set; }
        public DateTime DisbDate { get; set; }
        public string LoanStatus { get; set; }
        public string LoanProgram { get; set; }
        public decimal PaymentAmount { get; set; }
        public string ScheduleType { get; set; }
        //used to determine who to re-disclose to
        public bool InterestWasUpdated { get; set; }
    }
}
