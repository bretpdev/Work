using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    public class PaymentHistoryRecord
    {
        public string Description { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? InterestPaid { get; set; }
        public decimal? PrincipalPaid { get; set; }
        public decimal? LcNsfPaid { get; set; }
        public decimal? AccruedInterest { get; set; }
        public decimal? LcNsfDue { get; set; }
        public decimal? PrincipalBalance { get; set; }

    }
}
