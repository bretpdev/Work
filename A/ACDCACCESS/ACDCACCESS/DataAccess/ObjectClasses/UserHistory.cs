using System;

namespace ACDCAccess
{
    public class UserHistory
    {
        public int EventCode { get; set; }
        public string AccountAffected { get; set; }
        public string ActionTaken { get; set; }
        public string GroupName { get; set; }
        public string ChangedBy { get; set; }
        public string DateTimeChanged { get; set; }
    }
}
