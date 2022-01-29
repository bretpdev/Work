namespace SMBALWO
{
    public class BorrowerData
    {
        public int LoanWriteOffId { get; set; }
        public string AccountNumber { get; set; }
        public int LoanSequence { get; set; }
        public bool IsTilp { get; set; }

        public BorrowerData()
        {
        }        

        public override string ToString()
        {
            return $"Account Number: {AccountNumber}; Loan Sequence: {LoanSequence}";
        }
    }
}