using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.DataAccess
{
    public class ArcAddResults
    {
        public int ArcAddProcessingId { get; set; }
        public bool ArcAdded { get; set; }
        public List<string> Errors { get; set; }
        public Exception Ex { get; set; }

        public ArcAddResults()
        {
            Errors = new List<string>();
        }
    }
}
