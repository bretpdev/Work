using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMAILBATCH
{
    public class EmailCampaigns
    {
        public int EmailCampaignId { get; set; }
        public string SourceFile { get; set; }
        public bool ProcessAllFiles { get; set; }
        public bool OKIfMissing { get; set; }
        public bool OKIfEmpty { get; set; }
    }
}
