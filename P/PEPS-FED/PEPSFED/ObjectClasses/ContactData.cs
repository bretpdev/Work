using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEPSFED
{
    public class ContactData :ObjectBase
    {
        public override long RecordId { get; set; }
        public override string RecordType { get; set; }
        public override string OpeId { get; set; }
        public string ChangeIndicator { get; set; }
        public string ContType { get; set; }
        public string ContStreet1 { get; set; }
        public string ContStreet2 { get; set; }
        public string ContCity { get; set; }
        public string ContState { get; set; }
        public string Zip { get; set; }
        public string ContProvince { get; set; }
        public string ContCountry { get; set; }
        public string ContSaluataion { get; set; }
        public string ContFirstName { get; set; }
        public string ContMI { get; set; }
        public string ContLastName { get; set; }
        public string ContSuffix { get; set; }
        public string ContAreaCode { get; set; }
        public string ContExchange { get; set; }
        public string ContExt { get; set; }
        public string ContExt2 { get; set; }
        public string ContForeignPhone { get; set; }
        public string ContFax { get; set; }
        public string ContInternetAdd { get; set; }
        public string ContEffectDte { get; set; }
        public string ContEndDte { get; set; }
        public string SchoolsJobTitle { get; set; }
        public string ContSysId { get; set; }
        public string Filler { get; set; }

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

        public void FormatProperties()
        {
            ContType = ContType.Trim();
            ContFirstName = RemoveSpecialCharacters(ContFirstName.Trim());
            ContLastName = RemoveSpecialCharacters(ContLastName.Trim());
            ContAreaCode = ContAreaCode.Trim();
            ContExchange = ContExchange.Trim();
            ContExt = ContExt.Trim();
            ContExt2 = ContExt2.Trim();
            ContForeignPhone = ContForeignPhone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(".", "").Replace("/", "").Trim();
            ContFax = ContFax.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(".", "").Replace("/", "").Trim();
        }
    }
}
