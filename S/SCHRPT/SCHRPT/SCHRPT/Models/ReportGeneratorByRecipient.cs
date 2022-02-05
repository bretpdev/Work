using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHRPT.Models
{
    public class ReportGeneratorByRecipient
    {
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();
        public List<ReportType> Reports { get; set; } = new List<ReportType>();
        public int? SelectedRecipientId { get; set; }
    }
}