using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    public class BorrowerRecord
    {
        public string Ssn { get; set; } = null;

        public decimal? InterestRate { get; set; }

        public DateTime? FirstPayDue { get; set; }

        public decimal? PaymentAmount { get; set; }

        public string RepayPlan { get; set; }

        public string Page { get; set; }
    }
}
