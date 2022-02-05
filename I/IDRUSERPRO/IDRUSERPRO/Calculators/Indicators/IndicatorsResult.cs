using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace IDRUSERPRO
{
    public class IndicatorsResult
    {
        public List<LoanSequenceEligibility> Indicators { get; set; }

        public bool AllLoansWithNonZeroBalanceMatchLoanProgram(params string[] loanPrograms)
        {
            var nonZeroLoans = Indicators.Where(o => o.CurrentBalance > 0);
            if (!nonZeroLoans.Any())
                return false;
            return nonZeroLoans.All(o => o.LoanProgram.IsIn(loanPrograms));
        }
    }
}
