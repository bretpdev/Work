namespace OALETTERS
{
    public class LetterTypes
    {
        public int LetterTypeId { get; set; }
        public string LetterType { get; set; }
        public string Letter { get; set; }
        public bool IsCompany { get; set; }
        public bool HasPaymentSource { get; set; }
        public bool HasEffectiveDate { get; set; }
        public bool HasAccountNumber { get; set; }
    }
}