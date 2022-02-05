using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class SearchBorrower
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AccountIdentifier { get; set; }

        public void FormatForSearch()
        {
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (pi.Name == "StateCode" || pi.Name == "MiddleInitial" || pi.Name == "DOB" || pi.Name == "AccountIdentifier") continue;
                string value = (string)pi.GetValue(this, null);
                if (value.IsNullOrEmpty()) continue;
                if (value.Contains("*"))
                    value = value.Replace("*", "%");
                else
                    value = "%" + value + "%";
                pi.SetValue(this, value, null);
            }
        }
    }
}
