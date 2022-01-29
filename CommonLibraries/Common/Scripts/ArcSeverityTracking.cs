using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace Uheaa.Common.Scripts
{
        public class ArcSeverityTracking
        {
            public string ErrorCode { get; set; }
            public int ProcessingAttempts { get; set; }
            public NotificationSeverityType NotificationSeverityTypeId { get; set; }
            public int RequeueHours { get; set; }
        }

}
