using System;
using System.Collections.Generic;
using Uheaa.Common.Scripts;

namespace LSLETTERSU
{
    public class InputData
    {
        public string AccountNumber { get; set; }
        public SystemBorrowerDemographics Demos { get; set; }
        public string AcsKeyLine { get; set; }
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
        public string BeginYear { get; set; }
        public string EndYear { get; set; }
        public string BorrowerSsn { get; set; }
        public List<LetterData> Letters { get; set; }
        public Exception EX { get; set; }
        public string ErrorMessage { get; set; }

        public InputData()
        {
            DenialReasons = new List<string>();
            UpdatedDenialReasons = new List<string>();
            Letters = new List<LetterData>();
        }
    }
}