using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHRPT.Models
{
    public class RecipientWithSchools
    {
        public Recipient Recipient { get; set; }
        public List<SchoolRecipient> Schools { get; set; }
        public List<School> AllSchools { get; set; }
        public List<ReportType> ReportTypes { get; set; }
    }
}