using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDRRPTFED
{
    public class BeRecordData : IAppAndAward
    {
        public string AwardId { get; set; }
        public int ApplicationId { get; set; }
        public string EApplicationId { get; set; }
        public string FormattedAppId { get; set; }
        public string BERepaymentTypeProgram { get; set; }
        public bool? BERequestedByBorrower { get; set; }
        public string BERepaymentPlanTypeStatus { get; set; }
        public DateTime? BERepaymentPlanTypeStausDate { get; set; }
        public int? BEFamilySize { get; set; }
        public int? TotalIncome { get; set; }
        public string LoanSeq { get; set; }
        public bool? NotSame { get; set; }
    }
}
