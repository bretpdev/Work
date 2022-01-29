using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTHIST
{
    public class TransactionData
    {
        public string PmtType { get; set; }//PaymentType
        public string RevType { get; set; }//ReversalType
        public decimal PmtAmt { get; set; }//PaymentAmount
        public decimal OV { get; set; }
        public decimal PrincCol { get; set; }
        public decimal IntCol { get; set; }
        public decimal Legal { get; set; }
        public decimal Other { get; set; }
        public decimal CC { get; set; }

    }
}
