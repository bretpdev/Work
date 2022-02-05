using System;
using System.Collections.Generic;

namespace CLSCHLLNFD
{
    public class BorrowerPrintRecord
    {
        public BorrowerPrintRecord()
        {
        }

        public DateTime AddedAt { get; set; }
        public string BorrowerSsn { get; set; }
        public string AccountNumber { get; set; }
        public List<string> SchoolCodes { get; set; }
        public List<int> ClosedSchoolIds { get; set; }
        public List<int> AllLoans { get; set; } 
        public List<int> DischargedLoans { get; set; }
    }
}