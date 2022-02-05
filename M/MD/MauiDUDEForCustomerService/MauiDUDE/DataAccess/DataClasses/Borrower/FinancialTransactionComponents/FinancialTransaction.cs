using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class FinancialTransaction
    {
        public string ReversalReason { get; set; }
        public string EffectiveDate { get; set; }
        public string PostedDate { get; set; }
        public string TransactionType { get; set; }
        public decimal AppliedPrincipal { get; set; }
        public decimal AppliedInterest { get; set; }
        public decimal AppliedLateFee { get; set; }
        public decimal TransactionAmount { get; set; }
        public int LoanSeqNum { get; set; }
    }
}
