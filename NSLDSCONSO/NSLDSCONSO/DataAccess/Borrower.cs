using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace NSLDSCONSO
{
    public class Borrower
    {
        public string Ssn { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FileName { get; set; }

        public List<BorrowerConsolidationLoan> ConsolidationLoans { get; set; }
        public List<BorrowerUnderlyingLoan> UnderlyingLoans { get; set; }
        public List<BorrowerUnderlyingLoanFunding> UnderlyingLoanFunding { get; set; }
    }
}
