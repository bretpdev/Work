using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    public class MonetaryHistoryRecord
    {
        public string Ssn { get; set; }
        public int? LoanNum { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? PostDate { get; set; }
        public string TransactionCode { get; set; }
        public string CancelCode { get; set; }
        public decimal? TransactionAmount { get; set; }
        public decimal? AppliedPrincipal { get; set; }
        public decimal? AppliedInterest { get; set; }
        public decimal? AppliedFees { get; set; }
        public decimal? PrincipalBalanceAfterTransaction { get; set; }
        public decimal? InterestBalanceAfterTransaction { get; set; }
        public decimal? FeesBalanceAfterTransaction { get; set; }
        public int? LoanSequence { get; set; }
    }
}
