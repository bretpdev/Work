using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ONTIMEPYBB
{
    class BorrowerData
    {
        public string SSN { get; set; }
        public int LoanSequence { get; set; }
        public StatusCodes.CorrectedStatus CorrectedStatus { get; set; }
        //public string PaymentCounter { get; set; }
        public string BBType { get; set; } = "";
        public string PCVPayments { get; set; }
        public string DisqualDate { get; set; }
        public string DisqualReason { get; set; }
        public string Comment { get; set; }
    }
}

