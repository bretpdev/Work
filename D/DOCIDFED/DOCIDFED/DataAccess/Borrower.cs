using Uheaa.Common.WinForms;

namespace DOCIDFED
{
    public class Borrower
    {

        public string AccountIdentifier { get; set; }
        public string Ssn { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool IsFederal { get; set; }
        public bool IsOneLink { get; set; }
        public bool CompassOnly { get; set; }
        public string RecipientId { get; set; }
        public bool RecordAdded { get; set; }
    }
}