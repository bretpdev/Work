using System;

namespace PRECONADJ
{
    public class LineData
    {
        public string PrincipalBalance { get; set; }
        public string AdditionalDisbursement { get; set; }
        public string InterestRate { get; set; }
        public DateTime PaymentDates { get; set; }
        public string PaymentAmount { get; set; }
        public string Cap { get; set; }
        public string PrincipalApplied { get; set; }
        public string InterestApplied { get; set; }
    }
}