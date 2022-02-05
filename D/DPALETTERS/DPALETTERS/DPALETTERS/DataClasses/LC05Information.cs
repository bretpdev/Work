using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPALETTERS
{
    public class LC05Information
    {
        //Provided at input to GetLC05Information
        public Borrower Borrower { get; set; }
        public Borrower Comaker { get; set; } = null;
        public DateTime? DrawDate { get; set; }
        public string DPInd { get; set; } = "";
        //Set during call to GetLC05Information
        public string LoanStatus { get; set; } = "";
        public string Aux { get; set; } = "";
        public decimal ExpectedPayment { get; set; } = 0;
        public DateTime? DueDate { get; set; }
        public DateTime LoanStatusDate { get; set; } = new DateTime(1900, 1, 1);
        public List<string> LoanList { get; set; } = new List<string>();
    }
}
