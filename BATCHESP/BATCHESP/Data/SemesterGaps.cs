using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BATCHESP
{
    /// <summary>
    /// Information concerning deferment/forbearance date gaps during winter/summer school breaks.
    /// This info is pulled back from the TsayScrapedLoanInformation table.
    /// </summary>
    public class SemesterGaps
    {
        public int LoanSequence { get; set; }
        public string LoanProgramType { get; set; }
        public DateTime EndDate { get; set; }
        public string EndType {get; set;}
        public string EndSchool { get; set; }
        public DateTime BeginDate { get; set; }
        public string BeginType { get; set; }
        public string BeginSchool { get; set; }
    }
}
