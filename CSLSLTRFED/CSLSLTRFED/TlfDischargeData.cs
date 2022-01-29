using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSLSLTRFED
{
    class TlfDischargeData
    {
        public int LoanSeq { get; set; }
        public string LoanProgramId { get; set; }
        public string FirstDisbDate { get; set; }
        public string CurrentBalance { get; set; }
        public string InterestRate { get; set; }
    }

    class DischargeData
    {
        public int LoanSeq { get; set; }
        public string FirstDisbDate { get; set; }
        public string RepaymentStart { get; set; }
        public string BeginBalance { get; set; }
    }
}
