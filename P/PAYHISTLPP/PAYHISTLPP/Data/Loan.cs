using System;
using System.Collections.Generic;

namespace PAYHISTLPP
{
    public class Loan
    {
        public string LoanProgram { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string LoanId { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; } = DateTime.Now.ToString("MMddyyyy");
        public string LoanProgramType { get; set; }
        public DateTime GuarantyDate { get; set; }
        public string DealId { get; set; }
        public int LN_SEQ { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}