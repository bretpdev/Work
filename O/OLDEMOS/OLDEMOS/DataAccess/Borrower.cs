using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace OLDEMOS
{
    public class Borrower
    {
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName}{(MiddleInitial.IsPopulated() ? $" {MiddleInitial}" : "")} {LastName}";
            }
        }
        public string Ssn { get; set; }
        public string DateOfBirth { get; set; }

        #region Address
        public string Address1 { get; set; }
        public bool Address1Changed { get; set; }
        public string Address2 { get; set; }
        public bool Address2Changed { get; set; }
        public string City { get; set; }
        public bool CityChanged { get; set; }
        public string State { get; set; }
        public bool StateChanged { get; set; }
        public string Zip { get; set; }
        public bool ZipChanged { get; set; }
        public string Country { get; set; }
        public bool CountryChanged { get; set; }
        public bool AddressValid { get; set; }
        public bool AddressValidChanged { get; set; }
        public bool AddressForeign
        {
            get
            {
                return State == "FC";
            }
        }
        public bool AddressForeignChanged { get; set; }
        #endregion

        #region Primary Phone
        public string PrimaryPhone { get; set; }
        public bool PrimaryPhoneChanged { get; set; }
        public string PrimaryPhoneExt { get; set; }
        public bool PrimaryPhoneExtChanged { get; set; }
        public bool PrimaryPhoneConsent { get; set; }
        public bool PrimaryPhoneConsentChanged { get; set; }
        public bool PrimaryPhoneValid { get; set; }
        public bool PrimaryPhoneValidChanged { get; set; }
        #endregion

        #region Alternate Phone
        public string AlternatePhone { get; set; }
        public bool AlternatePhoneChanged { get; set; }
        public string AlternatePhoneExt { get; set; }
        public bool AlternatePhoneExtChanged { get; set; }
        public bool AlternatePhoneConsent { get; set; }
        public bool AlternatePhoneConsentChanged { get; set; }
        public bool AlternatePhoneValid { get; set; }
        public bool AlternatePhoneValidChanged { get; set; }
        #endregion

        #region Other Phone
        public string OtherPhone { get; set; }
        public bool OtherPhoneChanged { get; set; }
        public string OtherPhoneExt { get; set; }
        public bool OtherPhoneExtChanged { get; set; }
        public bool OtherPhoneConsent { get; set; }
        public bool OtherPhoneConsentChanged { get; set; }
        public bool OtherPhoneValid { get; set; }
        public bool OtherPhoneValidChanged { get; set; }
        #endregion

        #region Foreign Primary Phone
        public string ForeignPrimaryPhone { get; set; }
        public bool ForeignPrimaryPhoneChanged { get; set; }
        public string ForeignPrimaryPhoneExt { get; set; }
        public bool ForeignPrimaryPhoneExtChanged { get; set; }
        #endregion

        #region Foreign Alternate Phone
        public string ForeignAltPhone { get; set; }
        public bool ForeignAltPhoneChanged { get; set; }
        public string ForeignAltPhoneExt { get; set; }
        public bool ForeignAltPhoneExtChanged { get; set; }
        #endregion

        #region Foreign Other Phone
        public string ForeignOtherPhone { get; set; }
        public bool ForeignOtherPhoneChanged { get; set; }
        public string ForeignOtherPhoneExt { get; set; }
        public bool ForeignOtherPhoneExtChanged { get; set; }
        #endregion

        #region Email
        public string Email { get; set; }
        public bool EmailChanged { get; set; }
        public bool EmailValid { get; set; }
        public bool EmailValidChanged { get; set; }
        #endregion

        /// <summary>
        /// Important information about the borrower from the M1411 activity record. Displayed on the BrwInfo411
        /// </summary>
        public string Info411 { get; set; }


        public Borrower(ReflectionInterface ri, string accountIdentifier)
        {
            //access LP22
            if (accountIdentifier.Length == 10)
                ri.FastPath("LP22I;;;;L;;" + accountIdentifier);
            else if (accountIdentifier.Length == 9)
                ri.FastPath("LP22I" + accountIdentifier);
            else
                return;

            AccountNumber = ri.GetText(3, 60, 12).Replace(" ", "");
            FirstName = ri.GetText(4, 44, 12);
            MiddleInitial = ri.GetText(4, 60, 1);
            LastName = ri.GetText(4, 5, 35);
            Ssn = ri.GetText(3, 23, 9);
            DateOfBirth = ri.GetText(4, 72, 8);
            Address1 = ri.GetText(10, 9, 35);
            Address2 = ri.GetText(11, 9, 35);
            City = ri.GetText(12, 9, 35);
            State = ri.GetText(12, 52, 2);
            Zip = ri.GetText(12, 60, 5) + ri.GetText(12, 65, 4);
            Country = ri.GetText(11, 54, 25);
            AddressValid = ri.CheckForText(10, 57, "Y");
            PrimaryPhone = ri.GetText(13, 12, 10);
            PrimaryPhoneExt = ri.GetText(13, 27, 4);
            PrimaryPhoneConsent = ri.CheckForText(13, 69, "Y");
            PrimaryPhoneValid = ri.CheckForText(13, 38, "Y");
            AlternatePhone = ri.GetText(14, 12, 10);
            AlternatePhoneExt = ri.GetText(14, 27, 4);
            AlternatePhoneConsent = ri.CheckForText(14, 68, "Y");
            AlternatePhoneValid = ri.CheckForText(14, 38, "Y");
            OtherPhone = ri.GetText(15, 12, 10);
            OtherPhoneExt = ri.GetText(15, 27, 4);
            OtherPhoneConsent = ri.CheckForText(15, 68, "Y");
            OtherPhoneValid = ri.CheckForText(1, 1, "Y");
            ForeignPrimaryPhone = ri.GetText(16, 14, 17);
            ForeignPrimaryPhoneExt = ri.GetText(16, 36, 4);
            ForeignAltPhone = ri.GetText(16, 54, 17);
            ForeignAltPhoneExt = ri.GetText(16, 76, 4);
            ForeignOtherPhone = ri.GetText(17, 14, 17);
            ForeignOtherPhoneExt = ri.GetText(17, 36, 4);
            Email = ri.GetText(19, 9, 56);
            EmailValid = ri.CheckForText(18, 56, "Y");
        }
    }
}