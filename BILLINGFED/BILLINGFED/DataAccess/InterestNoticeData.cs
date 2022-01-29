using System.Collections.Generic;
using System.Data;
using Uheaa.Common;

namespace BILLINGFED
{
    public class InterestNoticeData
    {
        public int BorrowerCounter { get; set; }
        public string ACSKeyLine { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleInt { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string LoanProgram { get; set; }
        public int LoanSequence { get; set; }
        public string FirstDisbDate { get; set; }
        public string BalanceAtTimeOfBill { get; set; }
        public string PriorInterest { get; set; }
        public string TotalInterestDue { get; set; }

        public List<string> GetAddressInfo()
        {
            return new List<string>()
            {
                FirstName + " " +  LastName,
                Address1,
                Address2,
                City + " " + State + "  " + ZipCode,
                Country
            };
        }
		
        public static DataTable GetLoanDetail(List<InterestNoticeData> borrowerData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Disbursement Date");
            dt.Columns.Add("Loan Program");
            dt.Columns.Add("Interest Accrued Since Last Statement");
            dt.Columns.Add("Interest Accrued to Date");
            dt.Columns.Add("Current Balance");

            foreach (InterestNoticeData data in borrowerData)
            {
                List<object> obj = new List<object>()
                {
                    data.FirstDisbDate,
                    data.LoanProgram,
                    data.PriorInterest,
                    data.TotalInterestDue,
                    data.BalanceAtTimeOfBill
                };

                dt.Rows.Add(obj.ToArray());  
            }
            return dt;
        }

        public static InterestNoticeData Parse(string line, bool isEndorser)
        {
            List<string> splitLine = line.SplitAndRemoveQuotes(",");

            return new InterestNoticeData()
            {
                BorrowerCounter = splitLine[0].ToInt(),
                ACSKeyLine = splitLine[1],
                AccountNumber = isEndorser ? splitLine[19] : splitLine[2],
                FirstName = splitLine[3].Replace(",",""),
                MiddleInt = splitLine[4].Replace(",", ""),
                LastName = splitLine[5].Replace(",", ""),
                Address1 = splitLine[6].Replace(",", ""),
                Address2 = splitLine[7].Replace(",", ""),
                City = splitLine[8].Replace(",", ""),
                State = splitLine[9],
                ZipCode = splitLine[10].Replace(",", ""),
                Country = splitLine[11].Replace(",", ""),
                LoanProgram = splitLine[12],
                LoanSequence = splitLine[13].ToInt(),
                FirstDisbDate = splitLine[14],
                BalanceAtTimeOfBill = string.Format("$ {0:0.00}", splitLine[15].ToDecimal()),
                PriorInterest = string.Format("$ {0:0.00}", splitLine[16].ToDecimal()),
                TotalInterestDue = string.Format("$ {0:0.00}", splitLine[17].ToDecimal()) 
            };
        }

    }
}