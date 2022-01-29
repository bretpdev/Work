using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLDSCONSO
{
    public class MappedBorrowerUnderlyingLoan
    {
        public int BorrowerUnderlyingLoanId { get; set; }
        public string AwardId { get; set; }
        public string NsldsLabel { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? DateFunded { get; set; }
        public string LoanType { get; set; }
    }
}
