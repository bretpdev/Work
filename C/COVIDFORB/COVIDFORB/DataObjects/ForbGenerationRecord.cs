using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COVIDFORB
{
    public class ForbGenerationRecord
    {
        public string AccountNumber { get; set; }
        public DateTime DelinquencyOccurence { get; set; }
        public bool ComakerEligibility { get; set; }
    }
}
