namespace TRDPRTYRES
{
    public class BorReferenceInfo
    {
        public string Ssn { get; set; }
        public string ReferenceId { get; set; }
        public string RefFirstName { get; set; }
        public string RefLastName { get; set; }
        public string RefMI { get; set; }
        public string SourceCode { get; set; }
        public string RelationshipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string HomePhoneExt { get; set; }
        public string OtherPhone { get; set; }
        public string OtherPhoneExt { get; set; }
        public string Email { get; set; }
        public string Foreign { get; set; }
        public string ForeignExt { get; set; }
        public string NewReferenceID { get; set; }
        public string AdditionalComments { get; set; }
        public bool RefIsAuthed { get; set; }
        public bool BorHasValidAddr { get; set; }

        public BorReferenceInfo()
        {
            Ssn = string.Empty;
            RefFirstName = string.Empty;
            RefLastName = string.Empty;
            RefMI = string.Empty;
            SourceCode = string.Empty;
            RelationshipCode = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            HomePhone = string.Empty;
            HomePhoneExt = string.Empty;
            OtherPhone = string.Empty;
            OtherPhoneExt = string.Empty;
            Email = string.Empty;
            Foreign = string.Empty;
            ForeignExt = string.Empty;
            NewReferenceID = string.Empty;
            AdditionalComments = string.Empty;
        }
    }
}