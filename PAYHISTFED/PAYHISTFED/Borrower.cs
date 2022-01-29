using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAYHISTFED
{
    public class Borrower
    {
        public int BorrowerNumber { get; set; }
        public string SSN { get; set; }
        public string Name { get; set; }
        public EA80Data Ea80Data { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();
       
    }
}
