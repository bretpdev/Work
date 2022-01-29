using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class AcpBankruptcyInfo
    {
        public string OfficiallyFiled { get; set; }
        public string AttorneyInformation { get; set; }
        public string CourtInformation { get; set; }
        public string Chapter { get; set; }
        public string FilingDate { get; set; }
        public string DocketNumber { get; set; }

        public bool EndIndicator { get; set; }
        public string EndIdentifier { get; set; }
    }
}
