using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUEDTECNG
{
    class MonthlyPayment
    {
        public decimal Amount { get; set; }
        public decimal BalanceAtRepayment { get; set; }
        public int RpsTerm { get; set; }
        public decimal GetInstallmentAmount(decimal borrowerMonthlyInterest)
        {
            var installmentAmount = Math.Min(Amount, borrowerMonthlyInterest + 5);
            return installmentAmount;
        }
    }
}
