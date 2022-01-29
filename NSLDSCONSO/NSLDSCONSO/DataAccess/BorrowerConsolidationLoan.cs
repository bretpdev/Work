using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace NSLDSCONSO
{ 
    public class BorrowerConsolidationLoan
    {
        [CsvHeaderName("LOAN ID"), CsvRequired]
        public string NewLoanId { get; set; }
        [CsvRequired]
        public decimal GrossAmount { get; set; }
        [CsvRequired]
        public decimal InterestRate { get; set; }
        [CsvRequired]
        public decimal RebateAmount { get; set; }
        public decimal? NewGrossAmountSubsidized { get; set; }
        public decimal? NewGrossAmountUnsubsidized { get; set; }
        public decimal? NewInterestRate { get; set; }
        public decimal? NewRebateSubsidized { get; set; }
        public decimal? NewRebateUnsubsidized { get; set; }
    }
}
