using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRQTSKLLR
{
    public class EnrollmentTask
    {
        public int? ProcessingQueueId { get; set; }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string QueueName { get; set; }
        public DateTime? ArcAddedAt { get; set; }
    }
}
