using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskAudits
{
    public class Audit
    {
        [Browsable(false)]
        public int? AuditId { get; set; }
        [Browsable(true)]
        public string Auditor { get; set; }
        [Browsable(true)]
        public string Auditee { get; set; }
        [Browsable(false)]
        public bool? Passed { get; set; }
        [Browsable(true)]
        public string Result => !Passed.HasValue ? "" : Passed.Value ? "Pass" : "Fail";
        [Browsable(false)]
        public int? CommonFailReasonId { get; set; }
        [Browsable(false)]
        public string CommonFailReasonDescription { get; set; }
        [Browsable(false)]
        public int? CustomFailReasonId { get; set; }
        [Browsable(false)]
        public string CustomFailReasonDescription { get; set; }
        [Browsable(true), DisplayName("Fail Reason")]
        public string  FailReason => (!string.IsNullOrWhiteSpace(CommonFailReasonDescription) && !CommonFailReasonDescription.StartsWith("Other")) ? CommonFailReasonDescription : CustomFailReasonDescription;
        [Browsable(true), DisplayName("Audit Date")]
        public DateTime AuditDate { get; set; }
        [Browsable(false)]
        public DateTime CreatedAt { get; set; }
    }
}
