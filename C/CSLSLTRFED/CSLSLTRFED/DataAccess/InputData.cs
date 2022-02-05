using System;
using System.Collections.Generic;

namespace CSLSLTRFED
{
    public class InputData
    {
        public string AccountNumber { get; set; }
        public int LoanServicingLettersId { get; set; }
        public string LetterType { get; set; }
        public string LetterOption { get; set; }
        public List<string> DenialReasons { get; set; }
        public List<string> UpdatedDenialReasons { get; set; }
        public string AmountForDischarge { get; set; }
        public string SchoolName { get; set; }
        public DateTime LastDateOfAttendance { get; set; }
        public DateTime SchoolClosureDate { get; set; }
        public string DefermentForbearanceType { get; set; }
        public DateTime DefForbEndDate { get; set; }
        public DateTime LoanTermEndDate { get; set; }
        public string LowDirectoryBegin { get; set; }
        public string LowDirectoryEnd { get; set; }
        public string BorrowerSsn { get; set; }

        public InputData()
        {
            DenialReasons = new List<string>();
            UpdatedDenialReasons = new List<string>();
        }
    }
}