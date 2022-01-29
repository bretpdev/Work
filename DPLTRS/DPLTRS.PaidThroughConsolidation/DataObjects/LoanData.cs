using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLTRS.DataObjects
{
    public class LoanData
    {
        public List<LoanDataStrings> LoanInfo { get; set; }
        public double Balance { get; set; }

        public LoanData(double balance, List<LoanDataStrings> loanInfo)
        {
            LoanInfo = loanInfo;
            Balance = balance;

        }
    }

    public class LoanDataStrings
    {
        public string LoanType { get; set; } = "";
        public string UniqueId { get; set; } = "";
        public DateTime? DisbDate { get; set; } = null;
        public double? Balance { get; set; } = null;
    }
}
