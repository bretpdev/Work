using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    public class RepaymentPlanChangesRecord
    {
        public string PlanType { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? OutstandingPrin { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? PmtAmt { get; set; }
    }
}
