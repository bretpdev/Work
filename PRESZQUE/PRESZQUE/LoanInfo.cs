using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRESZQUE
{
    public class LoanInfo
    {
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string Seq { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string LoanType { get; set; }
        public bool IsFixedRate { get; set; }
        public bool IsUnsubsidized { get { return new string[] { "UNSTFD", "PLUS", "PLUSGB", "SLS", "UNCNS", "UNSPC" }.Contains(LoanType); } }
        public bool IsSubsidized { get { return new string[] { "STFFRD", "SUBCNS", "SUBSPC" }.Contains(LoanType); } }

        public bool IsRehabbedLoan { get; set; }
        public bool AddedEffectivePriorToConversion { get; set; }
        public bool InterestRatePriorToConversionChanged { get; set; }
        public bool OutOfSchoolDateChanged { get; set; }
        public bool DefermentAdded { get; set; }
        public bool ForbearanceAdded { get; set; }
        public string CloseReason { get; set; }
    }
}
