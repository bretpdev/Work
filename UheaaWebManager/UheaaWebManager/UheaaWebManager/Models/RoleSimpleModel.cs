using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class RoleSimpleModel
    {
        public int RoleId { get; set; }
        public string ActiveDirectoryRoleName { get; set; }
        public string Notes { get; set; }
        public int RoleTokenCount { get; set; }
        public int RoleControllerCount { get; set; }
    }
}