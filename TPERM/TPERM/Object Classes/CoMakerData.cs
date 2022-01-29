using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPERM
{
    public class CoMakerData
    {
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Selected { get; set; }
        public List<int> LoanSeqs { get; set; }

        public CoMakerData()
        {
            LoanSeqs = new List<int>();
        }

        public string GetLoanSeq()
        {
            return string.Join(" ", LoanSeqs);
        }
    }
}
