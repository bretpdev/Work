using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace NSLDSCONSO
{
    public class BorrowerUnderlyingLoan
    {
        public string NewLoanId { get; set; }
        [CsvRequired]
        public string UnderlyingLoanId { get; set; }
        public string NsldsLabel { get; set; }
        [CsvRequired]
        public string LoanType { get; set; }
        [CsvRequired, CsvHeaderName("FIRST DISBURSMENT DATE (UNDERLYING LOAN)")]
        public DateTime FirstDisbursement { get; set; }
        [CsvRequired]
        public string LoanStatus { get; set; }
        [CsvRequired]
        public string JointConsolidationIndicator { get; set; }
        [CsvHeaderName("UNDERLYING PARENT PLUS LOAN FLAG"), CsvRequired]
        public string ParentPlusLoanFlag { get; set; }
        [CsvRequired]
        public string ForcedIdrFlag { get; set; }
        public DateTime? IdrStartDate { get; set; }
        [CsvHeaderName("DAYS OF ECONOMIC HARDSHIP DEFERMENT USED")]
        public int EconomicHardshipDefermentDaysUsed { get; set; }
        [CsvRequired]
        public string LossOfSubsidyStatus { get; set; }
        public DateTime? LossOfSubsidyStatusDate { get; set; }
        public decimal InterestRate { get; set; }
        public decimal UnderlyingLoanBalance { get; set; }
    }
}
