using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDRRPTFED
{
    public class BfRecordData : IAppAndAward
    {
        public string AwardId { get; set; }
        public int ApplicationId { get; set; }
        public string EApplicationId { get; set; }
        public string FormattedAppId { get; set; }
        public DateTime? BFDisclosureDate { get; set; }
        public string LoanSeq { get; set; }
    }
}
