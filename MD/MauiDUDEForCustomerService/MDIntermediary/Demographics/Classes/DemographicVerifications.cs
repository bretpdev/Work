using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace MDIntermediary
{
    public class DemographicVerifications
    {
        public VerificationSelection Address { get; set; }
        public VerificationSelection HomePhone { get; set; }
        public bool HomePhoneConsent { get; set; }
        public VerificationSelection OtherPhone { get; set; }
        public bool OtherPhoneConsent { get; set; }
        public VerificationSelection OtherPhone2 { get; set; }
        public bool OtherPhone2Consent { get; set; }
        public VerificationSelection Email { get; set; }
        public VerificationSelection OtherEmail { get; set; }
        public VerificationSelection OtherEmail2 { get; set; }

        private VerificationSelection[] AllSelections
        {
            get
            {
                return new VerificationSelection[] { Address, HomePhone, OtherPhone, OtherPhone2, Email, OtherEmail, OtherEmail2 };
            }
        }
        /// <summary>
        /// True if any of the VerificationSelection properties on this class are ValidWithChangeAndInvalidateFirst
        /// </summary>
        public bool HasAnyInvalidateFirstSelections
        {
            get
            {
                return AllSelections.Contains(VerificationSelection.ValidWithChangeAndInvalidateFirst);
            }
        }

        public bool AnyChanges
        {
            get
            {
                return AllSelections.Any(o => o != VerificationSelection.NoChange);
            }
        }

        public void PersistChanges(MDBorrowerDemographics demographics, bool activityCodeIsAM)
        {
            Func<bool, string> yn = new Func<bool, string>(b => b ? "Y" : "N");
            demographics.DemographicsVerified = AnyChanges || activityCodeIsAM;
            demographics.UPAddrVal = Address != VerificationSelection.InvalidNoChange;
            demographics.UPAddrVer = Address.IsValid();

            demographics.UPPhoneVal = HomePhone != VerificationSelection.InvalidNoChange;
            demographics.UPPhoneNumVer = HomePhone.IsValid();
            demographics.UPPhoneConsent = yn(HomePhoneConsent);

            demographics.UPOtherVal = OtherPhone != VerificationSelection.InvalidNoChange;
            demographics.UPOtherVer = OtherPhone.IsValid();
            demographics.UPOtherConsent = yn(OtherPhoneConsent);

            demographics.UPOther2Val = OtherPhone2 != VerificationSelection.InvalidNoChange;
            demographics.UPOther2Ver = OtherPhone2.IsValid();
            demographics.UPOther2Consent = yn(OtherPhone2Consent);

            demographics.UPEmailVal = Email != VerificationSelection.InvalidNoChange;
            demographics.UPEmailVer = Email.IsValid();

            demographics.UPEmailOtherVal = yn(OtherEmail != VerificationSelection.InvalidNoChange);
            demographics.UPEMailOtherVer = yn(OtherEmail.IsValid());
            demographics.UPEmailOther2Val = yn(OtherEmail2 != VerificationSelection.InvalidNoChange);
            demographics.UPEMailOther2Ver = yn(OtherEmail2.IsValid());

        }
    }
}