using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS008
{
    public class DupDcns
    {
        public string TaskControlNumber { get; set; }
        public string ActivitySeq
        {
            get
            {
                return TaskControlNumber.Substring(TaskControlNumber.Length - 5);
            }
        }
        public string Dcn { get; set; }
        public bool Selected { get; set; }
    }
}
