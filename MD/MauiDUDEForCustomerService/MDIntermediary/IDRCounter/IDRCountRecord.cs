using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class IDRCountRecord
    {
        public int LoanForgivenessProgram { get; set; }
        public int LoanSequence { get; set; }
        public string LoanProgram { get; set; }
        public string ActiveRepaymentProgram { get; set; }
        public string PreConversionPayCount { get; set; }
        public string PostConversionPayCount { get; set; }
        public string OverridePayCount { get; set; }
        public string TotalPayCount { get; set; }
        public string PreConversionMonthCount { get; set; }
        public string PostConversionMonthCount { get; set; }
        public string ExpectedEligibilityDate { get; set; }
    }
}
