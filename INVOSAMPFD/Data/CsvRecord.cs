using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace INVOSAMPFD
{
    class CsvRecord
    {
        public string BorrowerSsn { get; set; }
        public string PerformanceCategoryBilled { get; set; }
        public string PerformanceCategoryLoan { get; set; }
        public int LoanSequence { get; set; }
        public string PrincipalBalance { get; set; }
        public string InterestBalance { get; set; }
        public string TotalPrincipalAndInterest { get; set; }
        public string MaxDaysDelinquent { get; set; }
        public string PifDate { get; set; }
        [CsvLineNumber]
        public int LineNumber { get; set; }
        [CsvLineContent]
        public string LineContent { get; set; }
    }
}
