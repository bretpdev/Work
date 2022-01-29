using Uheaa.Common;

namespace SIRPTFED.Models
{
    public class ADUser
    {
        public ActiveDirectoryUser  userObj { get; set; }
        public string adAcctNm { get; set; }
        public string adRole { get; set; }
        public int adRoleId { get; set; }
        public bool AddUser { get; set; }
    }
}
