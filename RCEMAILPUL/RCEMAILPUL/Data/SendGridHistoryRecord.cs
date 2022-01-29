using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCEMAILPUL
{
    class SendGridHistoryRecord
    {
        public string FromEmail { get; set; }
        public string MsgId { get; set; }
        public string Subject { get; set; }
        public string ToEmail{ get; set; }
        public string Status { get; set; }
        public int OpensCount { get; set; }
        public int ClicksCount { get; set; }
        public DateTime LastEventTime { get; set; }
    }
}
