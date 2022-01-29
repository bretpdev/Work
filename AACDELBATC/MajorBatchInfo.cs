using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AACDELBATC
{
    public class MajorBatchInfo
    {
        public string MajorBatchToDelete { get; set; }
        public List<MinorBatchInfo> MinorBatchesToDelete { get; set; }

        public MajorBatchInfo()
        {
            MinorBatchesToDelete = new List<MinorBatchInfo>();
        }
    }
}
