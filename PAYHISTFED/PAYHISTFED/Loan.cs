using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAYHISTFED
{
    public class Loan
    {
        public string LoanProgram { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string AwardId { get; set;}
        public string AwardIdSeq { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();

        
    }
}
