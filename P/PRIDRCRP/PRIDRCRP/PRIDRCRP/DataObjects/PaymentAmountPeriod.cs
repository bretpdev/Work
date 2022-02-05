using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    class PaymentAmountPeriod
    {
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? PaymentAmount { get; set; }
    }
}
