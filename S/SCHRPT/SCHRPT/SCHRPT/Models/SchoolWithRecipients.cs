using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHRPT.Models
{
    public class SchoolWithRecipients
    {
        public School School { get; set; }
        public List<SchoolRecipient> Recipients { get; set; }
        public List<Recipient> AllRecipients { get; set; }
        public List<ReportType> ReportTypes { get; set; }
    }
}