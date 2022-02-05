using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLQBuilder
{
    public class QueueTableRecord
    {
        public string TargetId { get; set; }
        public string QueueName { get; set; }
        public string InstitutionId { get; set; }
        public string InstitutionType { get; set; }
        public DateTime? DateDue { get; set; }
        public TimeSpan? TimeDue { get; set; }
        public string Comment { get; set; }
        //public int QueueId { get; set; } //This needs to be at the bottom so the object can be automatically serialized to a data table without the value
        //public string Filename { get; set; } //We only want to set this on the call from get
    }
}
