using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payments
{
    class MiscCommentAndQueueStuff
    {

        public string Comment2 { get; set; }
        public string Queue { get; set; }
        public string DateDue { get; set; }
        public string LP9OComment { get; set; }
        public string Comment { get; set; }
        public string ActCd { get; set; }
        public string FBSComment { get; set; }
        public string ActivityType { get; set; }
        public string ContactType { get; set; }

        public MiscCommentAndQueueStuff()
        {
            Comment2 = string.Empty;
            Queue = string.Empty;
            DateDue = string.Empty;
            LP9OComment = string.Empty;
            Comment = string.Empty;
            ActCd = string.Empty;
            FBSComment = string.Empty;
            ActivityType = string.Empty;
            ContactType = string.Empty;
        }
    }
}
