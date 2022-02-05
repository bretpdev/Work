using System;
using System.Collections.Generic;

namespace WORKRAQUE
{
    public class Borrower
    {
        public string Ssn { get; set; }
        public List<string> BorrowerCode { get; set; }
        public List<string> CorrectedBorrowerCode { get; set; }
        public List<int> LoanSequence { get; set; }
        public DateTime? DisbursmentDate { get; set; }
        public string UniqueId { get; set; }
        public string LoanType { get; set; }
        public string LoanProgram { get; set; }

        public Borrower()
        {
            BorrowerCode = new List<string>();
            CorrectedBorrowerCode = new List<string>();
            LoanSequence = new List<int>();
        }
    }
}