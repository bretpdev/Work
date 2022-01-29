using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    public class RejectionResults
    {
        public RejectReason Reason { get; set; }
        public string ReasonText { get; set; }
        public string ActionCode { get; set; }
        public string Comment { get; set; }
    }
}
