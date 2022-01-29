using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace THRDPAURES
{
    public class ReferenceToSystem
    {
        private ReflectionInterface RI { get; set; }
        public ReferenceToSystem(ReflectionInterface ri)
        {
            RI = ri;
        }

        /// <summary>
        /// Adds the reference to Compass.
        /// </summary>
        /// <param name="refDemos">Reference demographic gathered from AddModifyReferenceDemos form.</param>
        /// <param name="borrowerSsn">Borrowers ssn.</param>
        /// <param name="isApproved">indicates if third party auth or POA was approved.</param>
        /// <returns>REFERENCE ID</returns>
        public string AddTheReference(ReferencesDemographics refDemos, string expirationDate, string borrowerSsn, bool isApproved)
        {
            RI.FastPath("TX3Z/ATX1JR");
            AddNameAndAuthorization(refDemos, expirationDate, borrowerSsn, isApproved);

            AddAddress(refDemos.Legal, "L");
            AddPhone(refDemos.HomePhones, "H");
            //Commit what we have entered so far
            RI.Hit(ReflectionInterface.Key.Enter);

            if (RI.MessageCode == "01079")
                RI.Hit(ReflectionInterface.Key.Enter);
            //Screen changes to Change mode when a successful update happens
            if (!RI.CheckForText(1, 4, "C"))
            {
                MessageBox.Show(string.Format("Unable to add the reference.  Error Message: {0}", RI.Message));
                return null;
            }

            AddOptionalAddressAndPhone(refDemos);
            AddEmailAddress(refDemos);

            //REFERENCE ID
            return RI.GetText(5, 13, 11).Replace(" ", "");
        }

        /// <summary>
        /// Updates an existing reference.
        /// </summary>
        /// <param name="refDemos">Reference Information.</param>
        /// <param name="expirationDate">POA expiration date.</param>
        /// <param name="borrowerSsn">Borrowers Ssn.</param>
        /// <param name="isApproved">Approved Indicator.</param>
        /// <returns>reference id</returns>
        public string UpdateTheReference(ReferencesDemographics refDemos, string expirationDate, string borrowerSsn, bool isApproved)
        {
            RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);

            if (isApproved)
            {
                RI.PutText(8, 33, DateTime.Now.ToString("MMddyy"));
                RI.PutText(8, 49, "Y");
                RI.PutText(8, 71, expirationDate);
            }
            else
                RI.PutText(8, 49, "N");

            RI.PutText(8, 15, refDemos.RelationshipCode.Length < 2 ? "0" + refDemos.RelationshipCode: refDemos.RelationshipCode);
            RI.Hit(ReflectionInterface.Key.Enter);

            RI.Hit(ReflectionInterface.Key.F6);
            AddAddress(refDemos.Legal);
            RI.Hit(ReflectionInterface.Key.Enter);
            AddAddress(refDemos.Disbursement, "D");
            RI.Hit(ReflectionInterface.Key.Enter);
            AddAddress(refDemos.Billing, "B");
            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            AddPhone(refDemos.HomePhones);
            RI.Hit(ReflectionInterface.Key.Enter);
            AddPhone(refDemos.MobilePhones, "M");
            RI.Hit(ReflectionInterface.Key.Enter);
            AddPhone(refDemos.AlternatePhones, "A");
            RI.Hit(ReflectionInterface.Key.Enter);
            AddPhone(refDemos.DayTimePhones, "W");
            RI.Hit(ReflectionInterface.Key.Enter);
            AddEmailAddress(refDemos);
            return RI.GetText(5, 13, 11).Replace(" ", "");
        }

        /// <summary>
        /// Adds the references email address.
        /// </summary>
        /// <param name="refDemos">Reference Information.</param>
        private void AddEmailAddress(ReferencesDemographics refDemos)
        {
            RI.Hit(ReflectionInterface.Key.F2);
            RI.Hit(ReflectionInterface.Key.F10);

            if (refDemos.EmailAddress.Length == 0)
                return;

            RI.PutText(9, 20, "05");
            RI.PutText(11, 17, DateTime.Now.ToString("MMddyy"));
            RI.PutText(12, 14, "Y");

            if (refDemos.EmailAddress.Length < 59)
                RI.PutText(14, 10, refDemos.EmailAddress, ReflectionInterface.Key.EndKey);
            else
            {
                int row = 14;
                for (int i = 0; i > refDemos.EmailAddress.Length; i += 59)
                {
                    if (i > 59)
                        RI.PutText(row, 10, refDemos.EmailAddress.Substring(i, 59));
                    else
                        RI.PutText(row, 10, refDemos.EmailAddress.Substring(i, (((row - 14) * 59) - refDemos.EmailAddress.Length)));//Take the starting row and minus that from the current row to get the number of 59 character sets we have used and get the length of the original email address

                    row++;
                }
            }

            RI.Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Adds Billing and Disbursement addresses as well as all alternate phones if they exist.
        /// </summary>
        /// <param name="refDemos">Reference demographic gathered from AddModifyReferenceDemos form.</param>
        private void AddOptionalAddressAndPhone(ReferencesDemographics refDemos)
        {
            //Now that we are in change mode we need to move though the screen with F6
            RI.Hit(ReflectionInterface.Key.F6);
            RI.Hit(ReflectionInterface.Key.F6);
            AddAddress(refDemos.Billing, "B");
            RI.Hit(ReflectionInterface.Key.Enter);
            AddAddress(refDemos.Disbursement, "D");
            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            AddPhone(refDemos.AlternatePhones, "A");
            RI.Hit(ReflectionInterface.Key.Enter);
            AddPhone(refDemos.MobilePhones, "M");
            RI.Hit(ReflectionInterface.Key.Enter);
            AddPhone(refDemos.DayTimePhones, "W");
            RI.Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Adds the references name and authorization to Compass. 
        /// </summary>
        /// <param name="refDemos">Reference demographic gathered from AddModifyReferenceDemos form.</param>
        /// <param name="borrowerSsn">Borrowers ssn.</param>
        /// <param name="isApproved">indicates if third party auth or POQ was approved.</param>
        private void AddNameAndAuthorization(ReferencesDemographics refDemos, string expirationDate, string borrowerSsn, bool isApproved)
        {
            RI.PutText(4, 6, refDemos.LastName);
            RI.PutText(4, 34, refDemos.FirstName);
            RI.PutText(4, 53, refDemos.MiddleInital);
            RI.PutText(7, 11, borrowerSsn);

            if (isApproved)
            {
                RI.PutText(8, 33, DateTime.Now.ToString("MMddyy"));
                RI.PutText(8, 49, "Y");
                RI.PutText(8, 71, expirationDate);
            }

            RI.PutText(8, 15, refDemos.RelationshipCode);
        }

        /// <summary>
        /// Adds the references phone to Compass.
        /// </summary>
        /// <param name="phone">References phone information.</param>
        /// <param name="type">Phone type to update.</param>
        private void AddPhone(ReferencePhone phone, string type = "")
        {
            if (phone == null)
                return;
            else if (RemovePhoneMask(phone.Phone).IsNullOrEmpty() && RemovePhoneMask(phone.ForeignPhone).IsNullOrEmpty())
                return;

            if (!type.IsNullOrEmpty())
                RI.PutText(16, 14, type);
            RI.PutText(16, 20, phone.Mbl);
            RI.PutText(16, 30, phone.Consent);
            RI.PutText(16, 45, DateTime.Parse(phone.LastVerifiedDate).ToString("MMddyy"));

            if (!RemovePhoneMask(phone.Phone).IsNullOrEmpty())
            {
                RI.PutText(17, 14, phone.Phone.Substring(1, 3));
                RI.PutText(17, 23, phone.Phone.Substring(6, 3));
                RI.PutText(17, 31, phone.Phone.Substring(10, 4));
                RI.PutText(17, 40, phone.PhoneExtension);
                RI.PutText(18, 15, "", ReflectionInterface.Key.EndKey);
                RI.PutText(18, 24, "", ReflectionInterface.Key.EndKey);
                RI.PutText(18, 36, "", ReflectionInterface.Key.EndKey);
                RI.PutText(18, 53, "", ReflectionInterface.Key.EndKey);
            }
            else
            {
                RI.PutText(18, 15, phone.ForeignPhone.Substring(0, 3));
                RI.PutText(18, 24, phone.ForeignPhone.Substring(3, 5));
                RI.PutText(18, 36, phone.ForeignPhone.Substring(8, phone.ForeignPhone.Length - 9));
                RI.PutText(18, 53, phone.ForeignPhoneExtension);
                RI.PutText(17, 14, "", ReflectionInterface.Key.EndKey);
                RI.PutText(17, 23, "", ReflectionInterface.Key.EndKey);
                RI.PutText(17, 31, "", ReflectionInterface.Key.EndKey);
                RI.PutText(17, 40, "", ReflectionInterface.Key.EndKey);
            }

            RI.PutText(17, 54, phone.DomesticPhoneValid.Value ? "Y" : "N");
            RI.PutText(19, 14, phone.SourceCode);
            RI.PutText(16, 78, "A");
        }

        private string RemovePhoneMask(string phone)
        {
            return phone.Replace("", "(", ")", "-", " ");
        }

        /// <summary>
        /// Adds the references address to Compass.
        /// </summary>
        /// <param name="demos">References address information.</param>
        /// <param name="type">Address Type to add.</param>
        private void AddAddress(AddressDemographics demos, string type = "")
        {
            if (demos == null)
                return;

            if (!type.IsNullOrEmpty())
                RI.PutText(10, 13, type, ReflectionInterface.Key.EndKey);
            RI.PutText(11, 10, demos.Street1, ReflectionInterface.Key.EndKey);
            RI.PutText(12, 10, demos.Street2, ReflectionInterface.Key.EndKey);
            RI.PutText(13, 10, demos.Street3, ReflectionInterface.Key.EndKey);
            RI.PutText(11, 55, "Y");
            RI.PutText(12, 52, demos.ForeignState, ReflectionInterface.Key.EndKey);
            RI.PutText(12, 77, demos.ForeignCode, ReflectionInterface.Key.EndKey);
            if (demos.ForeignCode.IsNullOrEmpty())
                RI.PutText(13, 52, "", ReflectionInterface.Key.EndKey);
            RI.PutText(14, 8, demos.City, ReflectionInterface.Key.EndKey);
            RI.PutText(14, 32, demos.State, ReflectionInterface.Key.EndKey);
            RI.PutText(14, 40, demos.Zip, ReflectionInterface.Key.EndKey);
            RI.PutText(10, 67, "A");
            RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"));
        }
    }
}
