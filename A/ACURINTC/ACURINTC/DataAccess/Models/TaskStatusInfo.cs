using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    public class TaskStatusInfo
    {
        public bool TaskIsClosed { get; set; }
        public List<string> ArcsCreated { get; set; }
        public TaskStatusInfo()
        {
            ArcsCreated = new List<string>();
        }
    }
}
