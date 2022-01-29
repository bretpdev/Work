using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLDSCONSO
{
    public class ReportBorrower
    {
        public int BorrowerId { get; set; }
        public string Ssn { get; set; }
        public bool HasReleasedLoans { get; set; }
    }
}
