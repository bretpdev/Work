using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;

namespace THRDPAURES
{
    public class BorrowerData : DemographicsBase
    {
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public string BorrowersFirstName { get; set; }
        public string BorrowersLastName { get; set; }
        public string ReferenceFirstName { get; set; }
        public string ReferenceLastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string ForeignState { get; set; }
        public string ForeignCountry { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool HasValidAddress { get; set; }
        public bool IsPowerOfAttorney { get; set; }
        public string ExpirationDate { get; set; }
        public string Mbl { get; set; }
        public string Consent { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string SourceCode { get; set; }
        public string ForeignPhone { get; set; }
        public string ForeignExt { get; set; }

        /// <summary>
        /// Only use this constructor if you are on TX1J
        /// </summary>
        /// <param name="ri"></param>
        public BorrowerData(ReflectionInterface ri, bool hasValidAddress)
            : base(ri)
        {
            HasValidAddress = hasValidAddress;
            Ssn = GetTextRemoveUnderscore(3, 12, 11).Replace(" ","");
            AccountNumber = GetTextRemoveUnderscore(3, 34, 12).Replace(" ", "");
            BorrowersFirstName = GetTextRemoveUnderscore(4, 34, 13);
            BorrowersLastName = GetTextRemoveUnderscore(4, 6, 23);

            if (HasValidAddress)
            {
                Street1 = GetTextRemoveUnderscore(11, 10, 29);
                Street2 = GetTextRemoveUnderscore(12, 10, 29);
                ForeignState = GetTextRemoveUnderscore(12, 52, 14);
                ForeignCountry = GetTextRemoveUnderscore(13, 52, 25);
                City = GetTextRemoveUnderscore(14, 8, 20);
                State = GetTextRemoveUnderscore(14, 32, 2);
                Zip = GetTextRemoveUnderscore(14, 40, 5);
            }

            if (ri.CheckForText(17, 54, "Y"))
            {
                Mbl = ri.GetText(16, 20, 1);
                Consent = ri.GetText(16, 30, 1);
                Phone = ri.GetText(17, 14, 3) + ri.GetText(17, 23, 3) + ri.GetText(17, 31, 4);
                PhoneExt = GetTextRemoveUnderscore(17, 40, 5);
                ForeignPhone = GetTextRemoveUnderscore(18, 15, 3) + GetTextRemoveUnderscore(18, 24, 5) + GetTextRemoveUnderscore(18, 36, 11);
                ForeignExt = GetTextRemoveUnderscore(18, 53, 5);
            }
        }
    }
}
