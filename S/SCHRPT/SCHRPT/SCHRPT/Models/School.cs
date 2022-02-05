using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCHRPT.Models
{
    public class School
    {
        public int? SchoolId { get; set; }
        public string Name { get; set; }
        [Display(Name = "School Code")]
        public string SchoolCode { get; set; }
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        public int RecipientCount { get; set; }
    }
}