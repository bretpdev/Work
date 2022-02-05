using System.Collections.Generic;
using Uheaa.Common.Scripts;

namespace FINALREV
{
    public class SchoolLetterData
    {
        public int BorrowerRecordId { get; set; }
        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Email { get; set; }
        public List<string> Schools { get; set; }
        public string CostCenterCode { get; set; }

        public static void Populate(DataAccess da, SchoolLetterData letterData)
        {
            SystemBorrowerDemographics demos = da.GetDemos(letterData.Ssn);
            letterData.FirstName = demos.FirstName;
            letterData.MI = demos.MiddleIntial;
            letterData.LastName = demos.LastName;
            letterData.Address1 = demos.Address1;
            letterData.Address2 = demos.Address2;
            letterData.City = demos.City;
            letterData.State = demos.State;
            letterData.Zip = demos.ZipCode;
            letterData.Country = demos.Country;
            letterData.Phone = demos.PrimaryPhone;
            letterData.AltPhone = demos.AlternatePhone;
            letterData.Email = demos.EmailAddress;
        }
    }
}