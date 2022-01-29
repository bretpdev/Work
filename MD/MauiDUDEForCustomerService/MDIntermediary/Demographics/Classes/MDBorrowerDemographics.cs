//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MDIntermediary
//{
//    public abstract class MDBorrowerDemographics
//    {
//        //Previously in BorrowerDemographics, removing inheritance
//        public string SSN { get; set; }
//        public string AccountNumber { get; set; }

//        //Previously in PersonDemographics
//        public string FName { get; set; }
//        public string MI { get; set; }
//        public string LName { get; set; }

//        //Previously in Q Demographics
//        public string Addr1 { get; set; }
//        public string Addr2 { get; set; }
//        /// <summary>
//        /// Address Line 3
//        /// This was added for COMPASS compatability. Staff have been instructed to not use this line so, PA should have to code for it..
//        /// </summary>
//        public string Addr3 { get; set; }
//        public string City { get; set; }
//        public string State { get; set; }
//        public string Zip { get; set; }
//        public string ForeignState { get; set; }
//        public string Country { get; set; }
//        public string Phone { get; set; }
//        public string AltPhone { get; set; }
//        public string Email { get; set; }

//        /// <summary>
//        /// Applies the following abbreviations to the address lines 1, 2 and 3
//        /// Replaces "STREET" with "ST"
//        /// Replace "AVENUE" with "AVE"
//        /// Replace "ROAD" with "RD"
//        /// Replace "LANE" with "LN"
//        /// Replace "DRIVE" with "DR"
//        /// Replace "HIGHWAY" with "HWY"
//        /// Replace "FLOOR" with "FL"
//        /// Replace "P O BOX" with "PO BOX"
//        /// Replace "P O BX" with "PO BOX"
//        /// Replace "-" with " "
//        /// </summary>
//        /// <remarks></remarks>
//        public void ApplyStandardAbbreviationsToAllAddressLines()
//        {
//            if (Addr1 != null)
//            {
//                Addr1 = ApplyStandardAbbreviations(Addr1);
//            }
//            if (Addr2 != null)
//            {
//                Addr2 = ApplyStandardAbbreviations(Addr2);
//            }
//            if (Addr3 != null)
//            {
//                Addr3 = ApplyStandardAbbreviations(Addr3);
//            }
//        }

//        /// <summary>
//        /// Applies the following abbreviations to the address part
//        /// Replaces "STREET" with "ST"
//        /// Replace "AVENUE" with "AVE"
//        /// Replace "ROAD" with "RD"
//        /// Replace "LANE" with "LN"
//        /// Replace "DRIVE" with "DR"
//        /// Replace "HIGHWAY" with "HWY"
//        /// Replace "FLOOR" with "FL"
//        /// Replace "P O BOX" with "PO BOX"
//        /// Replace "P O BX" with "PO BOX"
//        /// Replace "-" with " "
//        /// </summary>
//        /// <param name="addressPart">String to apply standard abbreviations to.</param>
//        /// <returns></returns>
//        /// <remarks></remarks>
//        public string ApplyStandardAbbreviations(string addressPart)
//        {
//            addressPart = addressPart.Replace("STREET", "ST");
//            addressPart = addressPart.Replace("AVENUE", "AVE");
//            addressPart = addressPart.Replace("ROAD", "RD");
//            addressPart = addressPart.Replace("LANE", "LN");
//            addressPart = addressPart.Replace("DRIVE", "DR");
//            addressPart = addressPart.Replace("HIGHWAY", "HWY");
//            addressPart = addressPart.Replace("FLOOR", "FL");
//            addressPart = addressPart.Replace("P O BOX", "PO BOX");
//            addressPart = addressPart.Replace("P O BX", "PO BOX");
//            addressPart = addressPart.Replace("-", " ");
//            return addressPart;
//        }

//        //MDBorrowerDemographics
//        private string _fpText;
//        private Font _printFont;

//        public bool EcorrCorrespondence { get; set; }
//        public bool EcorrBilling { get; set; }
//        public bool EcorrTax { get; set; }
//        public int DefaultLetterFormat { get; set; }
//        public string UpdatedAlternateFormat { get; set; }
//        public int UpdatedAlternateFormatId { get; set; }
//        public bool IsForeignAddress { get; set; }

//        //Demographic Data

//        /// <summary>
//        /// Borrower's account number.
//        /// </summary>
//        public string CLAccNum { get; set; }

//        /// <summary>
//        /// The COMPASS demographics information for the borrower.
//        /// </summary>
//        public MDBorrowerDemographics CompassDemos { get; set; }

//        /// <summary>
//        /// Indicator as to whether the borrower was found on the system the instance is designated for
//        /// </summary>
//        public bool FoundOnSystem { get; set; }

//        /// <summary>
//        /// Borrower's first name.
//        /// </summary>
//        public string FirstName { get; set; }

//        /// <summary>
//        /// Borrower's last name.
//        /// </summary>
//        public string LastName { get; set; }

//        /// <summary>
//        /// Borrower's full name (first and last names combined).
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// Borrower's date of birth.
//        /// </summary>
//        public string DOB { get; set; }

//        //Home Phone
//        public string HomePhoneNum { get; set; }
//        public string HomePhoneExt { get; set; }
//        public string HomePhoneForeignCountry { get; set; }
//        public string HomePhoneForeignCity { get; set; }
//        public string HomePhoneForeignLocalNumber { get; set; }
//        public string HomePhoneMBL { get; set; }
//        public string HomePhoneConsent { get; set; }
//        public string HomePhoneValidityIndicator { get; set; }
//        public string HomePhoneVerificationDate { get; set; }

//        //Other Phone
//        public string OtherPhoneNum { get; set; }
//        public string OtherPhoneExt { get; set; }
//        public string OtherPhoneForeignCountry { get; set; }
//        public string OtherPhoneForeignCity { get; set; }
//        public string OtherPhoneForeignLocalNumber { get; set; }
//        public string OtherPhoneMBL { get; set; }
//        public string OtherPhoneConsent { get; set; }
//        public string OtherPhoneValidityIndicator { get; set; }
//        public string OtherPhoneVerificationDate { get; set; }

//        //Other Phone 2
//        public string OtherPhone2Num { get; set; }
//        public string OtherPhone2Ext { get; set; }
//        public string OtherPhone2ForeignCountry { get; set; }
//        public string OtherPhone2ForeignCity { get; set; }
//        public string OtherPhone2ForeignLocalNumber { get; set; }
//        public string OtherPhone2MBL { get; set; }
//        public string OtherPhone2Consent { get; set; }
//        public string OtherPhone2ValidityIndicator { get; set; }
//        public string OtherPhone2VerificationDate { get; set; }

//        //Other Phone 3
//        public string OtherPhone3Num { get; set; }
//        public string OtherPhone3Ext { get; set; }
//        public string OtherPhone3ForeignCountry { get; set; }
//        public string OtherPhone3ForeignCity { get; set; }
//        public string OtherPhone3ForeignLocalNumber { get; set; }
//        public string OtherPhone3MBL { get; set; }
//        public string OtherPhone3Consent { get; set; }
//        public string OtherPhone3ValidityIndicator { get; set; }
//        public string OtherPhone3VerificationDate { get; set; }

//        /// <summary>
//        /// Indicator as whether the demographics have been verified.
//        /// </summary>
//        public bool DemographicsVerified { get; set; }

//        /// <summary>
//        /// Indicator as to whether a PO box should be allowed for the borrower.
//        /// </summary>
//        public bool POBoxAllowed { get; set; }

//        //Valid indicators and data collected from the system (SP stands for system provided)

//        /// <summary>
//        /// Address verified date gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPAddrVerDt { get; set; }

//        /// <summary>
//        /// Address validity indicator gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPAddrInd { get; set; }

//        /// <summary>
//        /// Email verified date gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPEmailVerDt { get; set; }

//        /// <summary>
//        /// Email validity indicator gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPEmailInd { get; set; }

//        /// <summary>
//        /// Address valididty indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPAddrVal { get; set; }

//        /// <summary>
//        /// Address verified indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPAddrVer { get; set; }

//        /// <summary>
//        /// Phone validity indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPPhoneVal { get; set; }

//        /// <summary>
//        /// Phone verified indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPPhoneNumVer { get; set; }

//        /// <summary>
//        /// Phone MBL indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPPhoneMBL { get; set; }

//        /// <summary>
//        /// Phone consent indicator provided by the user (UP stand for user provided)
//        /// </summary>
//        public string UPPhoneConsent { get; set; }

//        /// <summary>
//        /// Other or alternate phone valididty indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPOtherVal { get; set; }

//        /// <summary>
//        /// Other or alternate phone verified indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPOtherVer { get; set; }

//        /// <summary>
//        /// Phone MBL indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPOtherMBL { get; set; }

//        /// <summary>
//        /// Phone consent indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPOtherConsent { get; set; }

//        /// <summary>
//        /// Other or alternate phone #2 validity indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPOther2Val { get; set; }

//        /// <summary>
//        /// Other or alternate phone #2 verified indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPOther2Ver { get; set; }

//        /// <summary>
//        /// Phone MBL indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPOther2MBL { get; set; }

//        /// <summary>
//        /// Phone consent indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPOther2Consent { get; set; }

//        /// <summary>
//        /// Email validity indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPEmailVal { get; set; }

//        /// <summary>
//        /// Email verified indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public bool UPEmailVer { get; set; }

//        //Demographic Fields updated for COMPASS only

//        /// <summary>
//        /// Indicator to track whether the Address was saved to the system (FOR COMPASS SYSTEM ONLY)
//        /// </summary>
//        public static bool AddressSaved { get; set; }

//        /// <summary>
//        /// Indicator to track whether the Phone was saved to the system (FOR COMPASS SYSTEM ONLY)
//        /// </summary>
//        public static bool PhoneSaved { get; set; }

//        /// <summary>
//        /// Indicator to track whether the Other Phone was saved to the system (FOR COMPASS SYSTEM ONLY)
//        /// </summary>
//        public static bool OtherSaved { get; set; }

//        /// <summary>
//        /// Indicator to track whether the Other Phone #2 was saved to the system (FOR COMPASS SYSTEM ONLY)
//        /// </summary>
//        public static bool Other2Saved { get; set; }

//        /// <summary>
//        /// Indicator to track whether the Email was saved to the system (FOR COMPASS SYSTEM ONLY)
//        /// </summary>
//        public static bool EmailSaved { get; set; }

//        /// <summary>
//        /// Indicator to track whether the other Email was saved to the system (FOR COMPASS SYSTEM ONLY)
//        /// </summary>
//        public static bool OtherEmailSaved { get; set; }

//        /// <summary>
//        /// Indicator to track whether the other 2 Email was saved to the system (FOR COMPASS SYSTEM ONLY)
//        /// </summary>
//        public static bool OtherEmail2Saved { get; set; }

//        /// <summary>
//        /// Alternate or other email address
//        /// </summary>
//        public string OtherEmail { get; set; }

//        /// <summary>
//        /// Alternate or other e-mail verified date gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPOtEmailVerDt { get; set; }

//        /// <summary>
//        /// Alternate or other email validity indicator gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPOtEmailInd { get; set; }

//        /// <summary>
//        /// Work or other email 2 address
//        /// </summary>
//        public string OtherEmail2 { get; set; }

//        /// <summary>
//        /// Work or other email 2 verified date gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPOt2EmailVerDt { get; set; }

//        /// <summary>
//        /// Work or other email 2 validity indicator gathered originally from the system (SP stands for system provided)
//        /// </summary>
//        public string SPOt2EmailInd { get; set; }

//        /// <summary>
//        /// Work or other email 2 address validity indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPEmailOther2Val { get; set; }

//        /// <summary>
//        /// Alternate or other email address validity indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPEmailOtherVal { get; set; }

//        /// <summary>
//        /// Alternate or other email address verified indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPEMailOtherVer { get; set; }

//        /// <summary>
//        /// Work or other email 2 address verified indicator provided by the user (UP stands for user provided)
//        /// </summary>
//        public string UPEMailOther2Ver { get; set; }
//    }
//}
