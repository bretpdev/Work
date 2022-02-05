using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;

namespace I1I2FEDREV
{
    public class BorrowerInfo
    {
        public string Ssn { get; set; }
        public string Name { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string AddressStatus { get; set; }
        private string PhoneStatus { get; set; }
        public bool HasValidPhone
        {
            get
            {
                return PhoneStatus == "VALID";
            }
        }

        /// <summary>
        /// Populates a BorrowerInfo Object. NOTE you must be on TS24
        /// </summary>
        /// <param name="ri"></param>
        /// <returns></returns>
        public static BorrowerInfo Populate(ReflectionInterface ri)
        {
            //we need to be on ITS24 in order for this method to work.
            if (!ri.CheckForText(1, 76, "TSX25"))
                return null;

            return new BorrowerInfo()
            {
                Ssn = ri.GetText(4, 16, 11).Replace(" ", ""),
                Name = ri.GetText(4, 37, 30),
                Street1 = ri.GetText(8, 13, 33),
                Street2 = ri.GetText(9, 13, 33),
                City = ri.GetText(11, 13, 22),
                State = ri.GetText(11, 36, 2),
                Zip = ri.GetText(11, 40, 20),
                Country = ri.GetText(9, 54, 23),
                AddressStatus = ri.GetText(7, 33, 12),
                PhoneStatus = ri.GetText(13, 71, 9)
            };
        }
    }
}
