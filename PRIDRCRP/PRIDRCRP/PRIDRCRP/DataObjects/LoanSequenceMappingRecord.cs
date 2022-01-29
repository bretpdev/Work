using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    public class LoanSequenceMappingRecord
    {
        public string Ssn { get; set; }

        public DateTime? DisbursementDate { get; set; }

        public decimal? DisbursementAmount { get; set; }
        public decimal? InterestRate { get; set; }

        public DateTime? GuaranteeDate { get; set; }
        public int? LoanNum { get; set; }
        public int? LoanSequence { get; set; }
    }
}
