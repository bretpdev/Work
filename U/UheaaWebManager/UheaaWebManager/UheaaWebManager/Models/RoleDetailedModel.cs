using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class RoleDetailedModel
    {
        public int? RoleId { get; set; }
        public string ActiveDirectoryRoleName { get; set; }
        public string Notes { get; set; }
        public int RoleTokenCount { get; set; }
        public DateTime? InactivatedAt { get; set; }
        public List<ControllerAccessModel> ControllerAccess { get; set; }

        public List<string> AvailableGroupNames { get; set; }
    }
}