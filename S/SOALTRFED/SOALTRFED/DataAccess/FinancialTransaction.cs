using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SOALTRFED
{
    public class FinancialTransaction
    {
        [FinancialAttribute("LOAN SEQUENCE: ", 1)]
        [DbName("Loan Seq")]
        public string LoanSequence { get; set; }
        [FinancialAttribute("TRANSACTION DATE: ", 2, @"MM/dd/yyyy")]
        [DbName("Transaction Date")]
        public string TransactionDate { get; set; }
        [FinancialAttribute("TRANSACTION TYPE: ", 3)]
        [DbName("Transaction Type")]
        public string TransactionType { get; set; }
        [FinancialAttribute("TRANSACTION AMOUNT: ", 5)]
        [DbName("Total Amount of Transaction")]
        public decimal TotalAmountOfTransaction { get; set; }
        [FinancialAttribute("APPLIED TO PRINCIPAL: ", 7)]
        [DbName("Amount to Principal")]
        public decimal AppliedToPrincipal { get; set; }
        [FinancialAttribute("APPLIED TO INTEREST: ", 9)]
        [DbName("Amount to Interest")]
        public decimal AppliedToInterest { get; set; }

        public DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");//These are needed for the data table but will not be used.
            dt.Columns.Add("2");
            int mod = 1;
            List<object> obj = new List<object>();
            foreach (PropertyInfo pi in this.GetType().GetProperties().OrderBy(r => r.GetAttributeValue<FinancialAttribute, int?>(o => o.Order, null)))
            {
                string title = pi.GetAttributeValue<FinancialAttribute, string>(p => p.Title, ""); //The title will be what the borrowers see's as the header
                string format = pi.GetAttributeValue<FinancialAttribute, string>(p => p.Format, "");//Some properties have a special format
                var val = pi.GetValue(this, null);
                if (val.GetType() == typeof(decimal))//Format decimal value
                    obj.Add(title + (val.ToString().Length < 3 ? "$ " + val.ToString() + ".00" : "$ " + val.ToString()));
                else if (val.GetType() == typeof(double))//Format percents
                    obj.Add(title + val.ToString().PadRight(5, '0') + "%");
                else
                {
                    if (!format.IsNullOrEmpty())
                    {
                        if (val == null || val.ToString().ToDateNullable().Value.Year == 1900)//Exclude bad dates
                            obj.Add(title);
                        else
                            obj.Add(title + DateTime.Parse(val.ToString()).ToString(format));
                    }
                    else
                        obj.Add(title + val.ToString());//no formating needed
                }
                if (mod % 2 == 0)
                {
                    dt.Rows.Add(obj.ToArray());
                    obj = new List<object>();
                }
                mod++;
            }

            if(mod % 2 == 0) //handle odd number of rows
            {
                dt.Rows.Add(obj.ToArray());
            }

            return dt;
        }
    }
}
