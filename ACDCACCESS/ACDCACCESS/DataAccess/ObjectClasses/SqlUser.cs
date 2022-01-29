﻿namespace ACDCAccess
{
    public class SqlUser
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
        public string AesAccountNumber { get; set; }
        public int BusinessUnit { get; set; }
        public int Role { get; set; }
        public string Status { get; set; }
        public string LegalName { get; set; }
    }
}