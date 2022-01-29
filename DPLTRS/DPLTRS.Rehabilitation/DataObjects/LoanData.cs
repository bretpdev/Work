using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLTRS.Rehabilitation.DataObjects
{
    public class LoanData
    {
        public string DueDay { get; set; }
        public double ARate { get; set; } = 0;
        public double ExpPay { get; set; } = 0;
        public double CollCostProj { get; set; } = 0;
        public double ABal { get; set; } = 0;
        public List<LoanDataRow> loanDataRows { get; set; } = new List<LoanDataRow>();
        public LoanDataRow loanDataTotals { get; set; } = new LoanDataRow();
        public double RHPay { get; set; } = 0;
    }
}
