using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace PRIDRCRP
{
    public class DisbursementRecord
    {
        public DateTime? DisbursementDate { get; set; }

        public decimal? InterestRate { get; set; }

        public string LoanType { get; set; }

        public string DisbursementNumber { get; set; }

        public string LoanId { get; set; }

        public decimal? DisbursementAmount { get; set; }

        [DbName("CapInterest")]
        public decimal? CapitalizedInterest { get; set; }

        public decimal? RefundCancel { get; set; }

        [DbName("BorrPaidPrin")]
        public decimal? BorrowerPaidPrincipal { get; set; }

        [DbName("PrinOutstanding")]
        public decimal? PrincipalOutstanding { get; set; }

        [DbName("PaidInterest")]
        public decimal? InterestPaid { get; set; }
    }
}
