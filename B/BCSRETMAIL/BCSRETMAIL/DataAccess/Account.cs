using Uheaa.Common.DataAccess;

namespace BCSRETMAIL
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string IsValidAddress { get; set; }
        public string AddressValidityDate { get; set; }
        public BarcodeInfo.SystemType System { get; set; }
        public DataAccessHelper.Region Region { get; set; }
    }
}