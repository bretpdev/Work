using System.Collections.Generic;
using System.Data;
using Uheaa.Common;

namespace BILLINGFED
{
    public class InterestStatementData
    {
        public int BorrowerCounter { get; set; }
        public string ACSKeyLine { get; set; }
        public string DF_SPE_ACC_ID { get; set; }
        public string CoBorrowerAccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string LoanProgram { get; set; }
        public string FirstDisbDate { get; set; }
        public string InterestRate { get; set; }
        public string OriginalPrincipal { get; set; }
        public string CurrentBalance { get; set; }
        public string LD_BIL_CRT { get; set; }
        public string LD_BIL_DU { get; set; }
        public string LD_FAT_EFF { get; set; }
        public string LA_FAT_CUR_PRI { get; set; }
        public string LA_FAT_NSI { get; set; }
        public string TAP { get; set; }
        public string LA_BIL_PAS_DU { get; set; }
        public string LA_CUR_INT_DU { get; set; }
        public string LA_BIL_DU_PRT { get; set; }

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

		
        public static DataTable GetLoanDetail(List<InterestStatementData> borrowerData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Loan Program");
            dt.Columns.Add("First Disbursed");
            dt.Columns.Add("Interest Rate");
            dt.Columns.Add("Original Principal");
            dt.Columns.Add("Current Balance");

            foreach (InterestStatementData data in borrowerData)
            {
                List<object> obj = new List<object>()
                {
                    data.LoanProgram,
                    data.FirstDisbDate,
                    data.InterestRate,
                    data.OriginalPrincipal,
                    data.CurrentBalance
                };

                dt.Rows.Add(obj.ToArray());
            }
            return dt;
        }

        public static InterestStatementData Parse(string line, bool isEndorser)
        {
            List<string> splitLine = line.SplitAndRemoveQuotes(",");

            return new InterestStatementData()
            {
                BorrowerCounter = splitLine[0].ToInt(),
                ACSKeyLine = splitLine[1],
                DF_SPE_ACC_ID = splitLine[2],
                CoBorrowerAccountNumber = splitLine[27].IsPopulated() ? splitLine[27] : splitLine[2],
                FirstName = splitLine[3].Replace(",", ""),
                LastName = splitLine[5].Replace(",", ""),
                Name = $"{splitLine[3].Replace(", ", "")} {splitLine[5].Replace(", ", "")}",
                Address1 = splitLine[6].Replace(",", ""),
                Address2 = splitLine[7].Replace(",", ""),
                City = splitLine[8].Replace(",", ""),
                State = splitLine[9].Replace(",", ""),
                ZipCode = splitLine[10].Replace(",", ""),
                Country = splitLine[11].Replace(",", ""),
                LoanProgram = splitLine[12],
                FirstDisbDate = splitLine[14],
                InterestRate = string.Format("{0:0.000} %", splitLine[15].ToDecimal()),
                OriginalPrincipal = string.Format("$ {0:0.00}", splitLine[16].ToDecimal()),
                CurrentBalance = string.Format("$ {0:0.00}", splitLine[17].ToDecimal()),
                LD_BIL_CRT = splitLine[19],
                LD_BIL_DU = splitLine[24],
                LD_FAT_EFF = splitLine[20],
                LA_FAT_CUR_PRI = string.Format("$ {0:0.00}", splitLine[37].ToDecimal()),
                LA_FAT_NSI = string.Format("$ {0:0.00}", splitLine[38].ToDecimal()),
                TAP = string.Format("$ {0:0.00}", splitLine[39].ToDecimal()),
                LA_BIL_PAS_DU =string.Format("$ {0:0.00}", splitLine[25].ToDecimal()),
                LA_CUR_INT_DU = string.Format("$ {0:0.00}", splitLine[35].ToDecimal()),
                LA_BIL_DU_PRT = string.Format("$ {0:0.00}", splitLine[26].ToDecimal())
            };
        }
    }
}