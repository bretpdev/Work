using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace SOALTRFED
{
    public class AddressInfo
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityStateZip { get; set; }
        public string ForeignState { get; set; }
        public string Country { get; set; }
        public bool HasValidAddress { get; set; }

        public AddressInfo()
        {
            Name = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            CityStateZip = string.Empty;
            ForeignState = string.Empty;
            Country = string.Empty;
        }
        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", Address1, Environment.NewLine, Address2, Address2.IsPopulated() ? Environment.NewLine : "",
                        CityStateZip, Environment.NewLine, ForeignState, ForeignState.IsPopulated() ? Environment.NewLine : "", Country);
        }
    }
}
