using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFACDFED
{
    public class BorrowerInfo
    {
        public UnprocessedLetter Letter { get; set; }
        public List<LoanInfo> Loans { get; set; }

        public BorrowerInfo(UnprocessedLetter letter, List<LoanInfo> loans)
        {
            this.Letter = letter;
            this.Loans = loans;
        }
    }
}
