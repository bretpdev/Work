using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AACDELBATC
{
    public class MinorBatchInfo
    {
        public string MinorBatch { get; set; }
        public List<string> SsnsInTheBatch { get; set; }

        public MinorBatchInfo()
        {
            SsnsInTheBatch = new List<string>();
        }
    }
}
