using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;

namespace KEYIDENCHN
{
    public class AccountInfo
    {
        KeyIdentifierChange script;
        ReflectionInterface ri;
        public AccountInfo(KeyIdentifierChange script, ReflectionInterface ri)
        {
            this.script = script;
            this.ri = ri;
        }
        public string AccountNumber { get; set; }
        public string SSN { get; set; }
        public void LoadAccount()
        {
            AccountNumber = ri.GetText(6, 10, 13).Replace(" ", "");
            SSN = ri.GetText(4, 16, 12).Replace(" ", "");
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string NameOnRecord
        {
            get { return string.Join(" ", new string[] {FirstName, MiddleName, LastName, Suffix}.Where(o => !string.IsNullOrEmpty(o)).ToArray()); }
        }
        public string DOB { get; set; }
        public void LoadDemographics()
        {
            FirstName = Get(4, 34, 13);
            MiddleName = Get(4, 53, 13);
            LastName = Get(4, 6, 23);
            Suffix = Get(4, 72, 4);
            DOB = ri.GetText(20, 6, 10).Replace(" ", "/");
        }

        private string Get(int row, int col, int length)
        {
            string get = ri.GetText(row, col, length);
            get = get.Replace("_", "");
            get = get.Replace(" ", "");
            get = get.Trim();
            return get;
        }
    }

}
