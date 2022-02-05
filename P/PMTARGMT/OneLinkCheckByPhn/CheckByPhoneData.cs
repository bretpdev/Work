using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace OneLinkCheckByPhn
{
    public class CheckByPhoneData
    {

        public string SSNOrAcctNum { get; set; }
        public string SSN { get; set; }
        public string Routing { get; set; }
        public string VerRouting { get; set; }
        public string CheckingAcctNum { get; set; }
        public string VerCheckingAcctNum { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentEffectiveDate { get; set; }
        public bool Checking { get; set; }
        public bool Savings { get; set; }
        public bool Inbound { get; set; }
        public bool Outbound { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CheckByPhoneData(string tSSNOrAcctNum)
        {
            SSNOrAcctNum = tSSNOrAcctNum;
            SSN = string.Empty;
            Routing = string.Empty;
            VerRouting = string.Empty;
            CheckingAcctNum = string.Empty;
            VerCheckingAcctNum = string.Empty;
            PaymentAmount = string.Empty;
            PaymentEffectiveDate = DateTime.Today.ToString("MM/dd/yyyy");
            Checking = false;
            Savings = false;
        }

        /// <summary>
        /// Returns a string representing the account type ("Checking" or "Savings")
        /// </summary>
        /// <returns></returns>
        public string CalculatedAccountType()
        {
            return (Checking ? "Checking" : "Savings");
        }

    }
}
