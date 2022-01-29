using System;

namespace ACDCAccess
{
    public class RoleHistory
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string AddedBy { get; set; }
        public string StartDate { get; set; }
        public string RemovedBy { get; set; }
        public string EndDate { get; set; }
    }
}
