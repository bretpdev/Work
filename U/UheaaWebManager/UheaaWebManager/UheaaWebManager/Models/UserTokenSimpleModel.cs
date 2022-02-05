using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class UserTokenSimpleModel
    {
        public int UserTokenId { get; set; }
        public string GeneratedTokenLast12 { get; set; }
        public string AssociatedWindowsUserName { get; set; }
        public string Notes { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}