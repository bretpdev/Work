namespace CSLSLTRFED
{
    public class LetterData
    {
        public int LoanServicingLettersId { get; set; }
        public string LetterType { get; set; }
        public string LetterOptions { get; set; }
        public string LetterChoices { get; set; }
        public string LetterId { get; set; }
        public string LetterName { get; set; }
        public string StoredProcedureName { get; set; }
        public string Arc { get; set; }
        public bool DischargeAmount { get; set; }
        public bool SchoolName { get; set; }
        public bool LastDateAttendance { get; set; }
        public bool SchoolClosureDate { get; set; }
        public bool DefForbType { get; set; }
        public bool DefForbEndDate { get; set; }
        public bool LoanTermEndDate { get; set; }
        public bool SchoolYear { get; set; }
        public bool AdditionalReason { get; set; }
        public bool DeathLetter { get; set; }
    }
}