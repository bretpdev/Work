using System.ComponentModel;

namespace ActiveDirectoryGroups
{
    public class SystemUsers
    {
        [Description("SQL User ID")]
        public string SqlUserID { get; set; }
        [Description("Windows User Name")]
        public string WindowsUserName { get; set; }
        [Description("First Name")]
        public string FirstName { get; set; }
        [Description("Middle Initial")]
        public string MiddleInitial { get; set; }
        [Description("Last Name")]
        public string LastName { get; set; }
        [Description("Email Address")]
        public string EmailAddress { get; set; }
        [Description("Extension")]
        public string Extension { get; set; }
        [Description("Extension 2")]
        public string Extension2 { get; set; }
        [Description("Business Unit")]
        public string BusinessUnit { get; set; }
        [Description("Role")]
        public string Role { get; set; }
        [Description("Status")]
        public string Status { get; set; }
        [Description("Title")]
        public string Title { get; set; }
        [Description("AesUserId")]
        public string AesUserId { get; set; }
    }

    public class BaseUser
    {
        public int SqlUserID { get; set; }
        public string WindowsUserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string Extension { get; set; }
        public string Extension2 { get; set; }
        public string BusinessUnit { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string AesUserId { get; set; }
    }
}
