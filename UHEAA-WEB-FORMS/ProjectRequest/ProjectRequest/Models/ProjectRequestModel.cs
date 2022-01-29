using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectRequest.Models
{
    public class ProjectRequest
    {
        public string ProjectName { get; set; }
        public string SubmittedBy { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public string ProjectSummary { get; set; }
        public string BusinessNeed { get; set; }
        public string Benefits { get; set; }
        public string ImplementationApproach { get; set; }

    }
}