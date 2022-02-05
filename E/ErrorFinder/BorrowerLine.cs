using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ErrorFinder
{
    public class BorrowerLine
    {
        public string SSN { get; set; }
        public string SeqNo { get; set; }
        public string ErrorCode { get; set; }
        public string MajorBatch { get; set; }
        public string MinorBatchNo { get; set; }
        [Browsable(false)]
        public string All = "All"; //this is a hack.  Deal with it
    }
}
