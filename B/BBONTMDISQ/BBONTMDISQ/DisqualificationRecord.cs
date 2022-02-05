using System;

namespace BBONTMDISQ
{
    public class DisqualificationRecord
    {
        public string Ssn { get; set; }
        public string LoanSequence { get; set; }
        public DateTime DisqualificationDate { get; set; }
    }
}