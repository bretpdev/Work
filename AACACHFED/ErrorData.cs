using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AACACHFED
{
    class ErrorData
    {
        public string SSN { get; set; }
        public string ABA_Routing_Number { get; set; }
        public string Bank_Account_Number { get; set; }
        public string Bank_Account_Type { get; set; }
        public string Monthly_Installment { get; set; }
        public string Additional_Amount { get; set; }
        public DateTime Due_Date { get; set; }
        public string Source_of_Application { get; set; }
        public string Award_Type { get; set; }
        public string Loan_Type { get; set; }
        public DateTime First_Disbursement_Date { get; set; }

        public ErrorData(AchData data)
        {
            SSN = data.Ssn;
            ABA_Routing_Number = data.AbaRoutingNumber;
            Bank_Account_Number = data.BankAccountNumber;
            Bank_Account_Type = data.BankAccountType;
            Monthly_Installment = data.MonthlyInstallment;
            Due_Date = data.DueDate;
            Source_of_Application = data.EftSource;
            Award_Type = data.AwardType;
            Loan_Type = data.LoanType;
            First_Disbursement_Date = data.FirstDisbDate;
        }
    }
}
