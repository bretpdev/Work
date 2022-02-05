using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace NSLDSCONSO
{
    public class BorrowerUnderlyingLoanFunding
    {
        [CsvRequired]
        public string UnderlyingLoanId { get; set; }
        [CsvRequired]
        public string LoanType { get; set; }
        [CsvHeaderName("AMOUNT TYPE P-INITIAL PAYOFF A-ADJUSTMENT(UNDER/OVER PMT) D-NEW DISBURSEMENT"), CsvRequired]
        public string AmountType { get; set; }
        [CsvHeaderName("AdjustmentAmount"), CsvRequired]
        public decimal TotalAmount { get; set; }
        public decimal RebateAmount { get; set; }
        [CsvHeaderName("DISBURSEMENT/ADJUSTMENT DATE"), CsvRequired]
        public DateTime DisbursementDate { get; set; }
        [CsvRequired]
        public int DisbursementNumber { get; set; }
        [CsvRequired]
        public int DisbursementSequenceNumber { get; set; }
        [CsvHeaderName("EFFECTIVE DATE"), CsvRequired]
        public DateTime DateFunded { get; set; }
        [CsvRequired]
        public string LoanHolderId { get; set; }
        public string Comments { get; set; }
    }
}
