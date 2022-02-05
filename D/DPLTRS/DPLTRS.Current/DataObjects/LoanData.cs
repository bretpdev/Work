using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DPLTRS.Current.Current;

namespace DPLTRS.Current.DataObjects
{
    public class LoanData
    {
        public LC05Sta status { get; set; } = LC05Sta.None;
        public string dueDate { get; set; } = "";
        public string dueDay { get; set; } = "";
        public string billedAmt { get; set; } = "";
        public List<LoanDataRow> loanDataRows { get; set; } = new List<LoanDataRow>();
    }
}
