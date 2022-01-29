using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALIGNPNOTE
{
    public class BorrowerData
    {
        public bool PNote_Found { get; set; }
        public int Loan_Sequence { get; set; }
        public DateTime? Signed_Date { get; set; }
        public DateTime Disbursement_Date { get; set; }
        public string SSN { get; set; }
    }

    public class NoLoans
    {
        public string NoLoansFound { get; set; }
    }
}
