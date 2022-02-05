using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace BILLINGFED
{
    public class Bill
    {
        public string AccountNumber { get; set; }
        public string CoBorrowerAccountNumber { get; set; }
        public List<BillData> BillLevelData { get; set; }
        public string DaysPastDue { get; set; }
        public List<string> LineData { get; set; }
        private DataAccess da;

        public Bill(string accountNumber, List<string> lineData, DataAccess da, string coBorAcct)
        {
            AccountNumber = accountNumber;
            BillLevelData = new List<BillData>();
            LineData = lineData;
            CoBorrowerAccountNumber = coBorAcct.IsPopulated() ? coBorAcct : accountNumber;
            this.da = da;
        }

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public void LoadBillData(Dictionary<string, int> header, string line)
        {
            List<BillData> items = da.GetBillData();
            DaysPastDue = line.SplitAndRemoveQuotes(",")[header["LN_DLQ_MAX"]];
            string endorserAcctNumber = line.SplitAndRemoveQuotes(",")[header["LF_EDS"]];
            BillLevelData.Add(new BillData() { XCoord = 470f, YCoord = 607f, Value = DateTime.Now.ToString("MM/dd/yyyy"), FontTypeId = 1, FontSize = 10, EnumValue = 2, IsBold = false, FontType = "TimesNewRoman" });

            foreach (BillData item in items)
            {
                string value = "";
                bool isAddress = false;
                foreach (string field in item.SASFieldName.SplitAndRemoveQuotes("|"))
                {
                    if (field == "DX_STR_ADR_1" && !isAddress)
                        isAddress = true;
                    if (line.SplitAndRemoveQuotes(",")[header[field]].IsPopulated())
                        value += line.SplitAndRemoveQuotes(",")[header[field]] + (isAddress ? Environment.NewLine : " ");
                }

                if (isAddress)
                    isAddress = false;

                BillData data = Bill.DeepCopy<BillData>(item);
                data.Value = value;

                BillLevelData.Add(data);
            }
        }
    }
}
