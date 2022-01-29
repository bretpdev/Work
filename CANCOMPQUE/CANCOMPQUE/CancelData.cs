using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CANCOMPQUE
{
    class CancelData
    {
        public string Queue { get; set; }
        public string SubQueue { get; set; }
        public string Message { get; set; }
        public string ARC { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SSN { get; set; }
    }
}
