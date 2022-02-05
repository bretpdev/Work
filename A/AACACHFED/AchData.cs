using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AACACHFED
{
    public class AchData
    {
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string AbaRoutingNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountType { get; set; }
        public string MonthlyInstallment { get; set; }
        public DateTime DueDate { get; set; }
        public string DayDue { get; set; }
        public string AwardType { get; set; }
        public string LoanType { get; set; }
        public DateTime FirstDisbDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ZipPlus4 { get; set; }
        public string Email { get; set; }
        private const string eftSource = "PPD";
        public string EftSource 
        {
            get
            {
                return eftSource;
            }
        }
        public string LoanProgram { get; set; }
        public string LoanSeq { get; set; }
        public bool HasNoGracePeriod { get; set; }
        public bool IsMissingData { get; set; }


        public AchData(string fileLine)
        {
            string[] parsedData = fileLine.Split(',');

            Ssn = parsedData[0];
            AbaRoutingNumber = parsedData[1];
            BankAccountNumber = parsedData[2];
            BankAccountType = parsedData[3];
            MonthlyInstallment = parsedData[4];
            if (!string.IsNullOrEmpty(parsedData[6])) 
            { 
                DueDate = Convert.ToDateTime(parsedData[6]); 
            }
            else
            {
                IsMissingData = true;
                return;
            }
            AwardType = parsedData[8];
            LoanType = parsedData[9];
            if (!string.IsNullOrEmpty(parsedData[10]))
            {
                FirstDisbDate = Convert.ToDateTime(parsedData[10]);
            }
            else
            {
                IsMissingData = true;
                return;
            }

            FirstName = parsedData[11];
            LastName = parsedData[12];
            Address1 = parsedData[13];
            Address2 = parsedData[14];
            City = parsedData[15];
            State = parsedData[16];
            Zip = parsedData[17];
            ZipPlus4 = parsedData[18];
            Email = parsedData[19];

            //check for missing data
            IsMissingData = false;
            if (string.IsNullOrEmpty(Ssn) || string.IsNullOrEmpty(AbaRoutingNumber) || string.IsNullOrEmpty(BankAccountNumber) || DueDate == null || string.IsNullOrEmpty(AwardType)
                || string.IsNullOrEmpty(LoanType) ||  string.IsNullOrEmpty(BankAccountType) || string.IsNullOrEmpty(MonthlyInstallment))
            { 
                IsMissingData = true;
                return;
            }
            
            //get day due from date due
            DayDue = Convert.ToString(DueDate.Day);

            //set LoanProgram
            if (AwardType == "5" && LoanType == "D") { LoanProgram = "PLUSGB"; }
            else if (AwardType == "1" && LoanType == "G") { LoanProgram = "STFFRD"; }
            else if (AwardType == "2" && LoanType == "G") { LoanProgram = "UNSTFD"; }
            else if (AwardType == "3" && LoanType == "P") { LoanProgram = "PLUS"; }
            else if (AwardType == "4" && LoanType == "C") { LoanProgram = "UNCNS and/or SUBCNS"; }
            else if (AwardType == "4" && LoanType == "Z") { LoanProgram = "SUBSPC and/or UNSPC"; }
            else if (AwardType == "6" && LoanType == "S") { LoanProgram = "SLS"; }
            else if (AwardType == "7" && LoanType == "F") { LoanProgram = "FISL"; }
            else if (AwardType == "U" && LoanType == "G") { LoanProgram = "DLUNST"; }
            else if (AwardType == "S" && LoanType == "G") { LoanProgram = "DLSTFD"; }
            else if (AwardType == "U" && LoanType == "C") { LoanProgram = "DLUCNS"; }
            else if (AwardType == "S" && LoanType == "C") { LoanProgram = "DLSCNS"; }
            else if (AwardType == "U" && LoanType == "Z") { LoanProgram = "DLUSPL"; }
            else if (AwardType == "S" && LoanType == "Z") { LoanProgram = "DLSSPL"; }
            else if (AwardType == "P" && LoanType == "P") { LoanProgram = "DLPLUS"; }
            else if (AwardType == "P" && LoanType == "D") { LoanProgram = "DLPLGB"; }
            else if (AwardType == "H" && LoanType == "T") { LoanProgram = "TEACH"; }
			else if (AwardType == "P" && LoanType == "C") { LoanProgram = "DLPCNS"; }
			else if (AwardType == "P" && LoanType == "Z") { LoanProgram = "DLUSPL"; }

            //set no grace period indicator
			string[] gracelessLoans = { "DLPLUS", "DLPLGB", "DLSSPL", "DLUSPL", "DLUCNS", "DLSCNS", "PLUS", "PLUSGB", "UNCNS", "SUBCNS", "SUBSPC", "UNSPC", "DLPCNS", "DLUSPL" };
			HasNoGracePeriod = gracelessLoans.Contains(LoanProgram);

        }
    }
}
