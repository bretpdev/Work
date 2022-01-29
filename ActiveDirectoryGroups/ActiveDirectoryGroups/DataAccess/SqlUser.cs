using System;

namespace ActiveDirectoryGroups
{
    public class SqlUser : IEquatable<SqlUser>
    {
        public int ID { get; set; }
        public string WindowsUserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Title { get; set; }
        public string PrimaryExtension { get; set; }
        public string SecondaryExtension { get; set; }
        public int AesAccountNumberId { get; set; }
        public int BusinessUnit { get; set; }
        public int Role { get; set; }
        public string Status { get; set; }
        private string legalName;
        public string LegalName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
            set
            {
                legalName = value;
            }
        }

        public SqlUser()
        {
            ID = 0;
            WindowsUserName = "";
            FirstName = "";
            MiddleInitial = "";
            LastName = "";
            EmailAddress = "";
            Title = "";
            PrimaryExtension = "";
            SecondaryExtension = "";
            AesAccountNumberId = 0;
            Role = 0;
            Status = "";
            LegalName = "";
        }

        public SqlUser(int id, string windowsUserName, string firstName, string middleInitial, string lastName, string email, string title, string primaryExt, string secondaryExt, int aesAccountNumberId, int role, int businessUnit, string status, string legalName)
        {
            ID = id;
            WindowsUserName = windowsUserName;
            FirstName = firstName;
            MiddleInitial = middleInitial;
            LastName = lastName;
            EmailAddress = email;
            Title = title;
            PrimaryExtension = primaryExt;
            SecondaryExtension = secondaryExt;
            AesAccountNumberId = aesAccountNumberId;
            Role = role;
            BusinessUnit = businessUnit;
            Status = status;
            LegalName = legalName;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public static bool operator ==(SqlUser a, SqlUser b)
        {
            if (Object.ReferenceEquals(a, b)) return true;
            if ((Object)a == null || (Object)b == null) return false;
            return a.ID == b.ID;
        }

        public static bool operator !=(SqlUser a, SqlUser b)
        {
            return !(a == b);
        }

        public bool Equals(SqlUser other)
        {
            if (ID == 0 || other.ID == 0)
                return false;
            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            SqlUser other = obj as SqlUser;
            if (other == null) return false;
            return base.Equals(obj) && this.ID == other.ID;
        }

        public override int GetHashCode()
        {
            if (WindowsUserName == null) return "".GetHashCode();
            return ID.GetHashCode();
        }
    }
}
