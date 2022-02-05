using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class AcpQueueInfo
    {
        public string Queue { get; set; }
        public string SubQueue { get; set; }
        public DataAccessHelper.Region QueueRegion { get; set; }
    }
}
