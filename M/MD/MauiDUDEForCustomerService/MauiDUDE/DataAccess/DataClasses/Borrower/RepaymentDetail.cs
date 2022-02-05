using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class RepaymentDetail
    {
        public string Term { get; }
        public decimal Amount { get; set; }
        public string BeginDate { get; }

        public RepaymentDetail(string term, decimal amount, string beginDate)
        {
            Term = term;
            Amount = amount;
            BeginDate = beginDate;
        }
    }
}
