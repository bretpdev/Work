using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPERM
{
    public class TaskData
    {
        public string TaskControlNumber { get; set; }
        public string ActivitySeq { get; set; }
        public string CorrDocNum { get; set; }

        public TaskData(string taskControlNumber)
        {
            this.TaskControlNumber = taskControlNumber;
            this.ActivitySeq = taskControlNumber.Substring(taskControlNumber.Length - 5);
        }
    }
}
