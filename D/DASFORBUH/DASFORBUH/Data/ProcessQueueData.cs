using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASFORBUH
{
    public class ProcessQueueData
    {
        public int ProcessQueueId { get; set; }
        public DateTime? ForbearanceAddedAt { get; set; }
        public int? ArcAddProcessingId { get; set; }
        public string AccountNumber { get; set; }
        public string ActivityComment { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LD_DLQ_OCC { get; set; }
        public bool DelinquencyOverride { get; set; }
        
    }
}
