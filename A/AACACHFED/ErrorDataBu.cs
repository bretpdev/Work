using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AACACHFED
{
    class ErrorDataBu
    {
        public string SSN { get; set; }
        public string ABA { get; set; }
        public string Acct { get; set; }
        public string Type { get; set; }
        public string Amt { get; set; }
        public string Add_Amt { get; set; }
        public DateTime Due { get; set; }
        public string Source { get; set; }

        public ErrorDataBu(AchData data)
        {
            SSN = data.Ssn;
            ABA = data.AbaRoutingNumber;
            Acct = data.BankAccountNumber;
            Type = data.BankAccountType;
            Amt = data.MonthlyInstallment;
            Due = data.DueDate;
            Source = data.EftSource;
        }
    }
}
