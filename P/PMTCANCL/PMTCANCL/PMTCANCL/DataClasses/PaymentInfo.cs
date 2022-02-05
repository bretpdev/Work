using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTCANCL
{
    public class PaymentInfo
    {
        public string Conf { get; set; }
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string Borrower { get; set; }
        public string PayType { get; set; }
        public string PayAmt { get; set; }
        public string PaySource { get; set; }
        public DateTime? PayCreated { get; set; }
        public DateTime? PayEffective { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public bool Deleted { get; set; }
        public bool BackResult { get; set; } = false;
    }
}
