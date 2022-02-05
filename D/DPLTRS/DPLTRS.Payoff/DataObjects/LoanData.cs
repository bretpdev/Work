using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLTRS.Payoff.DataObjects
{

    public class LoanData
    {
        public List<LoanDataRow> loanDataRows { get; set; } = new List<LoanDataRow>();
        public double Balance { get; set; } = 0;

    }

    public class LoanDataRow
    {
        public string UniqueId { get; set; }
        public double BalanceByLoan { get; set; }
    }
}
