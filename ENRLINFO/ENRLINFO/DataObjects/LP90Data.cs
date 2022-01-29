using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLINFO
{
    public class LP90Data
    {
        public List<QueueTask> QueueTasks { get; set; }
        public bool OneQueue { get; set; }
        public bool DuplicateTimes { get; set; }
    }
}
