using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace PEPSFED
{
    public class OtherAddressData : ObjectBase
    {
        public override long RecordId { get; set; }
        public override string RecordType { get; set; }
        public override string OpeId { get; set; }
        public string ChangeIndicator { get; set; }
        public string AddressType { get; set; }
        public string Line1Adr { get; set; }
        public string Line2Adr { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; } = "";
        public string Zip { get; set; }
        public string ForeignProvinceName { get; set; }
        public string OtherAreaCode { get; set; }
        public string OtherExchange { get; set; }
        public string OtherExt { get; set; }
        public string OtherExt2 { get; set; }
        public string OtherForeignPhone { get; set; }
        public string OtherFax { get; set; }
        public string OtherInternetAdr { get; set; }
        public string FscLocName { get; set; }
        public string FscContFirstName { get; set; }
        public string FscContLastName { get; set; }
        public string Filler { get; set; }

        public void FormatProperties()
        {
            AddressType = AddressType.Trim();
            Line1Adr = RemoveSpecialCharacters(Line1Adr.Trim());
            Line2Adr = RemoveSpecialCharacters(Line2Adr.Trim());
            City = City.Trim();
            State = State.Trim();
            Country = Country.Trim();
            Zip = Zip.Trim();
            if (Zip.Length == 9 && Zip.SafeSubString(5, 4) == "0000")
                 Zip = Zip.SafeSubString(0, 5);
            ForeignProvinceName = ForeignProvinceName.Trim();
            OtherAreaCode = OtherAreaCode.Trim();
            OtherExchange = OtherExchange.Trim();
            OtherExt = OtherExt.Trim();
            OtherExt2 = OtherExt2.Trim();
            OtherForeignPhone = OtherForeignPhone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(".", "").Replace("/", "").Trim();
            OtherFax = OtherFax.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(".", "").Replace("/", "").Trim();
            FscContFirstName = FscContFirstName.Trim();
            FscContLastName = FscContLastName.Trim();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var info in GetType().GetProperties())
            {
                var value = info.GetValue(this, null);
                if (value == null)
                    value = "";
                sb.AppendLine(info.Name + ": " + value.ToString().Trim());
            }

            return sb.ToString();
        }
    }
}
