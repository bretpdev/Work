using System;

namespace TILPCROST
{
    public class TilpRecord
    {
        public int AccountsId { get; set; }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string TransactionType { get; set; }
        public int LoanSequence { get; set; }
        public double PrincipalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string LastName { get; set; }
    }
}