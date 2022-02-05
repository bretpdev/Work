namespace SOPRMCRD
{
    class BorrowerRecord
    {
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string RecordType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public bool IsBorrower { get; set; }
    }
}