using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSHRCPTFED
{
    public class AccountIdentifiers
    {
        public const string NoSsn = "000000000";
        public const string NoAcountNumber = NoSsn + "0";
        public static AccountIdentifiers None()
        {
            return new AccountIdentifiers() { Ssn = NoSsn, AccountNumber = NoAcountNumber };
        }
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
    }
}