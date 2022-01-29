using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLQBuilder
{
    public class QueueRecord
    {
        public string TargetId { get; set; }
        public string QueueName { get; set; }
        public string InstitutionId { get; set; }
        public string InstitutionType { get; set; }
        public DateTime? DateDue { get; set; }
        public TimeSpan? TimeDue { get; set; }
        public string Comment { get; set; }
        public int QueueId { get; set; }
        public string Filename { get; set; } 
    }
}
