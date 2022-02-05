using System;
using System.Collections;

namespace PYOFFLTRFD
{
    public class PayoffData
    {
        public int SequenceNumber { get; set; }
        public string LoanProgram { get; set; }
        public string DateDisbursed { get; set; }
        public Decimal DailyInterest { get; set; }
        public Decimal CurrentPrincipal { get; set; }
        public Decimal PayoffInterest { get; set; }
        public Decimal LateFees { get; set; }
        public Decimal PayoffAmount { get; set; }
        public DateTime DateDate { get; set; }
    }
}