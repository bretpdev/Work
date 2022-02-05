using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMPMTHIST
{
    public class Transaction
    {

        public string EffectiveDate { get; set; }
        public string Type { get; set; }
        public string PostedDate { get; set; }
        public string RejectReason { get; set; }
        public string ReversalReason { get; set; }
        public double BeginningPrincipalBalance { get; set; }
        public double AppliedPrincipalAmount { get; set; }
        public double AppliedInterestAmount { get; set; }
        public double AppliedLateFeeAmount { get; set; }
        public double RemainingPrincipalBalance { get; set; }
        public double FinancialActAmount { get; set; }

        public void SumTransactions(Transaction transactionToBeAdded)
        {
            BeginningPrincipalBalance += transactionToBeAdded.BeginningPrincipalBalance;
            AppliedPrincipalAmount += transactionToBeAdded.AppliedPrincipalAmount;
            AppliedInterestAmount += transactionToBeAdded.AppliedInterestAmount;
            AppliedLateFeeAmount += transactionToBeAdded.AppliedLateFeeAmount;
            RemainingPrincipalBalance += transactionToBeAdded.RemainingPrincipalBalance;
            FinancialActAmount += transactionToBeAdded.FinancialActAmount;
        }

    }
}
