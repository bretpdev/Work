using System;

namespace CNCRTRNSFR
{
    public class Borrower
    {
        public string Ssn { get; set; }
        public string AwardId { get; set; }
        public DateTime DefermentBeginDate { get; set; }
        public DateTime DefermentEndDate { get; set; }
        public double DefermentMonthCount { get; set; }
        public DateTime ForbearanceBeginDate { get; set; }
        public DateTime ForbearanceEndDate { get; set; }
        public double ForbearanceMonthCount { get; set; }
        public string SendingServicer { get; set; }
        public string ReceivingServicer { get; set; }
        public string DealId { get; set; }
        public DateTime SaleDate { get; set; }
    }
}