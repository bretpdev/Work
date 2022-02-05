using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace THRDPAURES
{
    public class ReferencesDemographics : DemographicsBase
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInital { get; set; }
        public string RelationshipCode { get; set; }
        //NOTE to programmer be careful when changing these names we are using reflection to locate the address types values.
        public AddressDemographics Legal { get; set; }
        public AddressDemographics Billing { get; set; }
        public AddressDemographics Disbursement { get; set; }
        public ReferencePhone HomePhones { get; set; }
        public ReferencePhone AlternatePhones { get; set; }
        public ReferencePhone MobilePhones { get; set; }
        public ReferencePhone DayTimePhones { get; set; }
        public string EmailAddress { get; set; }
        public string Id { get; set; }

        /// <summary>
        /// Use this constructor if the reference does not exist on the system.
        /// </summary>
        /// <param name="referenceFirstName">References First Name.</param>
        /// <param name="referenceLastName">References Last Name.</param>
        public ReferencesDemographics(string referenceFirstName, string referenceLastName)
        {
            FirstName = referenceFirstName;
            LastName = referenceLastName;
            //These will get set later on.
            MiddleInital = "";
            EmailAddress = "";
        }


        /// <summary>
        /// Use this constructor only if the reference demographic screen is being displayed.
        /// </summary>
        /// <param name="ri">Current Reflection Interface object.</param>
        public ReferencesDemographics(ReflectionInterface ri)
            : base(ri)
        {
            Id = GetTextRemoveUnderscore(3, 12, 11).Replace(" ", "");
            LastName = GetTextRemoveUnderscore(4, 6, 22);
            FirstName = GetTextRemoveUnderscore(4, 34, 12);
            MiddleInital = GetTextRemoveUnderscore(4, 53, 1);
            RelationshipCode = GetTextRemoveUnderscore(8, 15, 2);
            ri.Hit(ReflectionInterface.Key.F6);
            ri.Hit(ReflectionInterface.Key.F6);
            Legal = GetAddressInformation(ri, "L");
            Billing = GetAddressInformation(ri, "B");
            Disbursement = GetAddressInformation(ri, "D");
            ri.Hit(ReflectionInterface.Key.F6);
            HomePhones = GetPhoneData(ri, "H");
            AlternatePhones = GetPhoneData(ri, "A");
            MobilePhones = GetPhoneData(ri, "M");
            DayTimePhones = GetPhoneData(ri, "W");
            EmailAddress = GetEmailAddress(ri);
        }

        /// <summary>
        /// From TX1j will access email screen and return the reference's email address if it is valid.
        /// </summary>
        /// <param name="ri">Current Reflection Interface object.</param>
        /// <returns>Reference's email address if valid otherwise return null.</returns>
        private string GetEmailAddress(ReflectionInterface ri)
        {
            ri.Hit(ReflectionInterface.Key.F2);
            ri.Hit(ReflectionInterface.Key.F10);

            if (ri.CheckForText(12, 14, "N"))
                return null;

            StringBuilder email = new StringBuilder();

            for (int row = 14; row < 19; row++)
                email.Append(GetTextRemoveUnderscore(row, 10, 59));

            return email.ToString();

        }

        /// <summary>
        /// From TX1J gets the References phone and foreign phone for a given type.
        /// </summary>
        /// <param name="ri">Current Reflection Interface object.</param>
        /// <param name="phoneType">Phone type to Check.</param>
        /// <returns>Object with phone and foreign phone for the given type, returns null if phone data does not exist.</returns>
        private ReferencePhone GetPhoneData(ReflectionInterface ri, string phoneType)
        {
            ri.PutText(16, 14, phoneType, ReflectionInterface.Key.Enter);

            if (ri.MessageCode.IsIn("01105", "01103"))
                return null;

            return new ReferencePhone()
            {
                Phone = GetTextRemoveUnderscore(17, 14, 3) + GetTextRemoveUnderscore(17, 23, 3) + GetTextRemoveUnderscore(17, 31, 4),
                ForeignPhone = GetTextRemoveUnderscore(18, 15, 3) + GetTextRemoveUnderscore(18, 24, 5) + GetTextRemoveUnderscore(18, 36, 11),
                DomesticPhoneValid = ri.CheckForText(17, 54, "Y"),
                ForeignPhoneValid = ri.CheckForText(17, 54, "Y"),
                Consent = GetTextRemoveUnderscore(16, 30, 1),
                Mbl = GetTextRemoveUnderscore(16, 20, 1),
                LastVerifiedDate = DateTime.Parse(GetTextRemoveUnderscore(16, 45, 8).Replace(" ", "/")).ToString("MM/dd/yyyy"),
                SourceCode = GetTextRemoveUnderscore(19, 14, 2),
                ForeignPhoneExtension = GetTextRemoveUnderscore(18, 53, 5),
                PhoneExtension = GetTextRemoveUnderscore(17, 40, 5)
            };
        }

        /// <summary>
        /// From TX1J gets the references address information for a given address type.
        /// </summary>
        /// <param name="ri">Current Reflection Interface object</param>
        /// <param name="addressType">Address type to check.</param>
        /// <returns>Returns object with address information for the given type, return null if no address.</returns>
        private AddressDemographics GetAddressInformation(ReflectionInterface ri, string addressType)
        {
            ri.PutText(10, 13, addressType, ReflectionInterface.Key.Enter);
            if (!ri.CheckForText(11, 55, "Y"))
                return null;

            return new AddressDemographics()
            {
                Street1 = GetTextRemoveUnderscore(11, 10, 28),
                Street2 = GetTextRemoveUnderscore(12, 10, 28),
                Street3 = GetTextRemoveUnderscore(13, 10, 28),
                ForeignState = GetTextRemoveUnderscore(12, 52, 14),
                ForeignCode = GetTextRemoveUnderscore(12, 77, 2),
                City = GetTextRemoveUnderscore(14, 8, 19),
                State = GetTextRemoveUnderscore(14, 32, 2),
                Zip = GetTextRemoveUnderscore(14, 40, 5)
            };
        }

        /// <summary>
        /// Looks though the Address Properties to determine if we need to add them to the selection.
        /// </summary>
        /// <param name="referneceDemos">Object to check.</param>
        /// <returns>Dictionary with the address code and description for the possible address types.</returns>
        public static Dictionary<string, string> GetAddressTypes(ReferencesDemographics referneceDemos)
        {
            Dictionary<string, string> addressTypeCodes = new Dictionary<string, string>();
            foreach (PropertyInfo pi in referneceDemos.GetType().GetProperties().Where(p => p.PropertyType == typeof(AddressDemographics)))
            {
                if (pi.GetValue(referneceDemos, null) != null)
                    addressTypeCodes.Add(pi.Name.Substring(0, 1), pi.Name);
            }

            //We always want to have this type available.
            if (addressTypeCodes.Count == 0)
                addressTypeCodes.Add("L", "Legal");

            return addressTypeCodes;
        }
    }
}
