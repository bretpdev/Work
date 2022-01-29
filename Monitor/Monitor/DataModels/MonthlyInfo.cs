using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    class MonthlyInfo
    {
        public decimal OldMonthlyPayment { get; set; }
        public decimal BeginningBalance { get; set; }
        public bool MultipleRepaymentStartDates { get; set; }
    }
}
