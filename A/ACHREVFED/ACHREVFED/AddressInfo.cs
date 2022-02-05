using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace ACHREVFED
{
    public class AddressInfo
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityStateZip { get; set; }
        public string ForeignState { get; set; }
        public string Country { get; set; }
        public string AccountNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool HasValidAddress { get; set; }

        public static AddressInfo GetBorrowersAddress(DataAccess DA, string account, bool isCoBorrower)
        {
            return DA.GetBorrowersAddress(account, isCoBorrower);
        }
    }
}
