using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace PIFLTR
{
    public class QueueData
    {
        [DbName("ProcessQueueId")]
        public int QueueId { get; set; }
        public string Queue { get; set; }
        public string SubQueue { get; set; }
        public string TaskControlNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        
        [DbName("RequestArc")]
        public string ActionResponse { get; set; }
     
        public DateTime TaskRequestedDate { get; set; }
        public bool IsSuccessful { get; set; }

        public override string ToString()
        {
            return string.Format("QueueId:{0}; Queue:{1}; SubQueue:{2}; TaskControlNumber:{3}; AccountNumber:{4}; Ssn:{5}; ActionResponse:{6}; TaskRequestedDate:{7}",
                QueueId, Queue, SubQueue, TaskControlNumber, AccountNumber, Ssn, ActionResponse, TaskRequestedDate);
        }

    }
}
