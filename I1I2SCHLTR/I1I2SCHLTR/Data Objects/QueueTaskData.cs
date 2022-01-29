using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I1I2SCHLTR
{
    public class QueueTaskData
    {
        public int QueueTaskDataId { get; set; }
        public string SSN { get; set; }
        public string Queue { get; set; }
        public int RunDateId { get; set; }
        public string SubQueue
        {
            get
            {
                return Queue == "I1" ? "SC" : "01";
            }
        }
    }
}
