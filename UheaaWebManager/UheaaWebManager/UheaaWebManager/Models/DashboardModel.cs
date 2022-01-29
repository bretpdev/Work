using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class DashboardModel
    {
        public int ApiTokenCount { get; set; }
        public int ExpiredApiTokenCount { get; set; }
        public int RoleCount { get; set; }
        public int ExpiredRoleCount { get; set; }
        public int WebAppCount { get; set; }
        public int ExpiredWebAppCount { get; set; }
    }
}