using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrsEstUpdate
{
    public class Estimates
    {
        public string RequestType { get; set; }
        public string RequestNumber { get; set; }
        public decimal EstimatedHours { get; set;  }
        public decimal? TestHours { get; set; }
        public decimal? AdditionalHrs { get; set; }
        public string ReasonForAdjustment { get; set; }
        public string AttachmentFileName { get; set; } = "";

        public decimal TotalHrs { get { return EstimatedHours +(AdditionalHrs ?? 0); } }

        //todo add note button
    }
}
