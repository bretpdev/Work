namespace CSLSLTRFED
{
    public class DischargeData
    {
        public int LoanSeq { get; set; }
        public string LoanProgramId { get; set; }
        public string FirstDisbDate { get; set; }
        public string CurrentBalance { get; set; }
        public string InterestRate { get; set; }
        public string RepaymentStart { get; set; }
        public string BeginBalance { get; set; }
        public bool IsTLF { get; set; }
    }
}