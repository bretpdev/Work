using System;
using Uheaa.Common.ProcessLogger;

namespace ActiveDirectoryGroups
{
    public class ActiveDirectoryUser
		: IEquatable<ActiveDirectoryUser>
	{
        public int SqlUserID { get; set; }
        public string WindowsUserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
		public string EmailAddress { get; set; }
        public string Extension { get; set; }
        public string Extension2 { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Role Role { get; set; }
        public bool Status { get; set; }
        public string AesUserID { get; set; }
        public string Title { get; set; }

		public static bool operator ==(ActiveDirectoryUser a, ActiveDirectoryUser b)
		{
			if (Object.ReferenceEquals(a, b)) { return true; }
			if ((Object)a == null || (Object)b == null) { return false; }
			return a.WindowsUserName == b.WindowsUserName;
		}

		public static bool operator !=(ActiveDirectoryUser a, ActiveDirectoryUser b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			ActiveDirectoryUser other = obj as ActiveDirectoryUser;
			if (other == null) { return false; }
            return base.Equals(obj) && this.WindowsUserName == other.WindowsUserName;
		}

		public bool Equals(ActiveDirectoryUser other)
		{
            if (WindowsUserName == null || other.WindowsUserName == null) { return false; }
            return WindowsUserName.Equals(other.WindowsUserName);
		}

		public override int GetHashCode()
		{
            if (WindowsUserName == null) { return "".GetHashCode(); }
            return WindowsUserName.GetHashCode();
		}
	}
}
