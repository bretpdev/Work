using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMREFCOM
{
    class SSNAndRefundAmtCombo
    {

        public string SSN { get; set; }
        public string RefundAmount { get; set; }

        public SSNAndRefundAmtCombo(string ssn, string refundAmount)
        {
            SSN = ssn;
            RefundAmount = refundAmount;
        }

    }
}
