using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    public class LoanSequenceEligibility
    {
        public int LoanSequence { get; set; }
        public string EligibilityIndicator { get; set; }
        public string FutureEligibilityIndicator { get; set; }
        public string LoanProgram { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
