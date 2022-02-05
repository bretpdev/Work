using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace ECORRSLFED
{
    public class HeaderFooterData
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ForeignState { get; set; }
        public string Country { get; set; }
        public string AccountNumber { get; set; }
        public string BarcodeAccountNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool HasValidAddress { get; set; }

        /// <summary>
        /// Writes all properties to a comma delimited list for the printing file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (AccountNumber.Trim().IsNullOrEmpty() && BarcodeAccountNumber.Contains("P"))
                AccountNumber = BarcodeAccountNumber;
            return string.Join(",", Name, Address1, Address2, City, State, Zip, Country, ForeignState, AccountNumber, BarcodeAccountNumber);
        }

        public List<string> ToEcorrList()
        {
            if (!Name.IsNullOrEmpty())
            {
                return new List<string>()
                {
                    Name,
                    Address1,
                    Address2,
                    Country.IsPopulated() ? string.Format("{0} {1}", City, Zip) : string.Format("{0} {1} {2}", City, State, Zip),
                    ForeignState,
                    Country
                };
            }
            else
                return null;
        }
    }
}
