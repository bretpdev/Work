using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACHSETUPFD
{
    public class ARCTracker
    {

        public string RequestedDate { get; set; }
        public bool Nulled { get; set; }

        public ARCTracker(string tRequestedDate)
        {
            RequestedDate = tRequestedDate;
        }

    }
}
