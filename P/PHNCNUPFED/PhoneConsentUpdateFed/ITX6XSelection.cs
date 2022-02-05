using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;

namespace PHNCNUPFED
{
    public class ITX6XSelection
    {
        public string SSN { get; set; }
        public string AccountNumber { get; set; }
        public bool HasAccountNumber
        {
            get
            {
                return AccountNumber.IsNumeric();
                
            }
        }
        public bool HasSSN { get { return !string.IsNullOrEmpty(SSN); } }
        public ITX6XSelection(string ssn, string accountNumber)
        {
            SSN = ssn;
            AccountNumber = accountNumber;
        }
    }
}
