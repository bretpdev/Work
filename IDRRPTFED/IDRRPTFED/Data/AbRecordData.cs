using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDRRPTFED
{
    public class AbRecordData : IAppAndAward
    {
        public int ApplicationId { get; set; }
        public string AwardId { get; set; }
        public string ABPersonRole { get; set; }
        public string ABSpouseSsn { get; set; }
        public DateTime? ABSpouseDob { get; set; }
        public string ABSpouseFirstName { get; set; }
        public string ABSpouseLastName { get; set; }
        public string ABSpouseMiddleName { get; set; }
        public bool? ABPersonsSsnIndicator { get; set; }
        public string ABDriversLicense { get; set; }
        public string ABDriversLicenseSt { get; set; }
        public string LoanSeq { get; set; }
    }
}
