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
    public class LoanInformation
    {
        [LoanAttribute("LOAN SEQUENCE: ", 1)]
        [DbName("Loan Seq")]
        public string LoanSequence { get; set; }
        [LoanAttribute("LOAN PROGRAM: ", 2)]
        [DbName("Loan Program")]
        public string LoanProgram { get; set; }
        [LoanAttribute("FIRST DISBURSEMENT DATE: ", 3, @"MM/dd/yyyy")]
        [DbName("1st Disb Date")]
        public string FirstDisbursementDate { get; set; }
        [LoanAttribute("PRINCIPAL BALANCE AT TRANSFER: ", 5)]
        [DbName("Principal Balance at Transfer")]
        public decimal PrincipalBalanceAtTransfer { get; set; }
        [LoanAttribute("CURRENT PRINCIPAL BALANCE: ", 7)]
        [DbName("Current Principal Balance")]
        public decimal CurrentPrincipalBalance { get; set; }


        public DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");//These are needed for the data table but will not be used.
            dt.Columns.Add("2");
            int mod = 1;
            List<object> obj = new List<object>();
            foreach (PropertyInfo pi in this.GetType().GetProperties().OrderBy(r => r.GetAttributeValue<LoanAttribute, int?>(o => o.Order, null)))
            {
                string title = pi.GetAttributeValue<LoanAttribute, string>(p => p.Title, ""); //The title will be what the borrowers see's as the header
                string format = pi.GetAttributeValue<LoanAttribute, string>(p => p.Format, "");//Some properties have a special format
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

            if (mod % 2 == 0) //handle odd number of fields
            {
                dt.Rows.Add(obj.ToArray());
            }
            return dt;
        }
    }
}
