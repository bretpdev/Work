using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMTHISTFED
{
    public class Transaction:IComparable<Transaction>
    {

        public string EffectiveDate { get; set; }
        public string Type { get; set; }
        public string PostedDate { get; set; }
        public string ReversalReason { get; set; }
        public double BeginningPrincipalBalance { get; set; }
        public double AppliedPrincipalAmount { get; set; }
        public double AppliedInterestAmount { get; set; }
        public double AppliedLateFeeAmount { get; set; }
        public double FinancialActAmount { get; set; }
        public double InterestRebateAmount { get; set; }

        public void SumTransactions(Transaction transactionToBeAdded)
        {
            BeginningPrincipalBalance = BeginningPrincipalBalance + transactionToBeAdded.BeginningPrincipalBalance;
            AppliedPrincipalAmount = AppliedPrincipalAmount + transactionToBeAdded.AppliedPrincipalAmount;
            AppliedInterestAmount = AppliedInterestAmount + transactionToBeAdded.AppliedInterestAmount;
            AppliedLateFeeAmount = AppliedLateFeeAmount + transactionToBeAdded.AppliedLateFeeAmount;
            FinancialActAmount = FinancialActAmount + transactionToBeAdded.FinancialActAmount;
            InterestRebateAmount = InterestRebateAmount + transactionToBeAdded.InterestRebateAmount;
        }

        public int CompareTo(Transaction transaction)
        {
            DateTime thisEffDate = Convert.ToDateTime(this.EffectiveDate);
            DateTime tranEffDate = Convert.ToDateTime(transaction.EffectiveDate);

            return thisEffDate.CompareTo(tranEffDate);
        }
    }
}
