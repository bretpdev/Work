using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPAFEDPS
{
    class SasFileData
    {
        public string AccountNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RunDate { get; set; }
        public string MobileIndicator { get; set; }
        public bool HasArcToUpdate { get; set; }
    }
}
