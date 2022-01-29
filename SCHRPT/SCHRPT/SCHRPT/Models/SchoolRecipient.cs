using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHRPT
{
    public class SchoolRecipient
    {
        public int? SchoolRecipientId { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string BranchCode { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientCompanyName { get; set; }
        public int? ReportTypeId { get; set; }
        public string StoredProcedureName { get; set; }
    }
}