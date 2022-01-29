using System;
using System.Collections.Generic;

namespace SCHUPDATES
{
    public class SchoolData
    {
        public string SchoolCode { get; set; }
        public string MergedSchool { get; set; }
        public DateTime? MergedSchoolDate { get; set; }
        public List<string> LoanPgms { get; set; }
        public List<string> Guarantors { get; set; }
        public string TX10Approval { get; set; }
        public string TX13Approval { get; set; }
        public string TX13Reason { get; set; }
        public DateTime ApprovalDate { get; set; }

        public SchoolData()
        {
            LoanPgms = new List<string>();
            Guarantors = new List<string>();
        }
    }
}