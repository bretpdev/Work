using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class AcpResponses
    {
        public AcpBankruptcyInfo Bankruptcy { get; set; }
        public AcpDeceasedInfo Deceased { get; set; }
        public AcpEntryInfo EntryInfo { get; set; }
        public AcpQueueInfo Queue { get; set; }
        public AcpSelectionResult Selection { get; set; }
    }
}
