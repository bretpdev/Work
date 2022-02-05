using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS008
{
    public class DbData
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string Description { get; set; }
        public string Arc { get; set; }
        public string ArcMessageText { get; set; }
        public string OriginalCommentText { get; set; }
    }
}
