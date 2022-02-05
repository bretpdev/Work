using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYOFF
{
    public class CoborrowerLoan
    {
        public string CoborrowerSSN { get; set; }
        public int LoanSequence { get; set; }
        public string CurrentPrincipal { get; set; }
        public string LoanProgram { get; set; }
    }
}
