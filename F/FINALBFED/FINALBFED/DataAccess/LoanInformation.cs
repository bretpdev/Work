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

namespace FINALBFED
{
    public class LoanInformation
    {
        [LoanDetailAttribute("LOAN TYPE: ", 1)]
        public string LoanProgram { get; set; }
        [LoanDetailAttribute("DISBURSMENT DATE: ", 2)]
        public string DisbDate { get; set; }
        [LoanDetailAttribute("ORIGINAL PRINCIPAL BALANCE: ", 3)]
        public decimal OrigPrincipalBalance { get; set; }
        [LoanDetailAttribute("OUTSTANDING INTERST DUE: ", 5)]
        public decimal OutstandingInterestDue { get; set; }
        [LoanDetailAttribute("DATE LAST PAYMENT RECEIVED: ", 7, @"MM/dd/yyyy")]
        public string DateLastPaymentReceived { get; set; }
        [LoanDetailAttribute("PRINCIPAL PAID SINCE LAST STATEMENT: ", 9)]
        public decimal PrincipalPaidSinceLastStatement { get; set; }
        [LoanDetailAttribute("INTEREST PAID SINCE LAST STATEMENT: ", 11)]
        public decimal InterestPaidSinceLastStatement { get; set; }
        [LoanDetailAttribute("FEES PAID SINCE LAST STATEMENT: ", 13)]
        public decimal FeesPaidSinceLastPayment { get; set; }
        [LoanDetailAttribute("TOTAL OF PMTS RCVD SINCE LAST STATEMENT: ", 15)]
        public decimal TotalPaidSinceLastStatement { get; set; }
        [LoanDetailAttribute("CURRENT PRINCIPAL BALANCE: ", 17)]
        public decimal CurrentPrincipalBalance { get; set; }
        [LoanDetailAttribute("INTEREST RATE: ", 4)]
        public double InterestRate { get; set; }
        [LoanDetailAttribute("AMOUNT PAST DUE: ", 6)]
        public decimal AmountPastDue { get; set; }
        [LoanDetailAttribute("OUTSTANDING LATE FEES TO DATE: ", 8)]
        public decimal OutstandingLateFees { get; set; }
        [LoanDetailAttribute("TOTAL PRINCIPAL PAID TO DATE: ", 10)]
        public decimal TotalPrincipalPaid { get; set; }
        [LoanDetailAttribute("TOTAL INTEREST PAID TO DATE: ", 12)]
        public decimal TotalInterestPaid { get; set; }
        [LoanDetailAttribute("TOTAL FEES PAID TO DATE: ", 14)]
        public decimal TotalFeesPaid { get; set; }
        [LoanDetailAttribute("AGGREGATE AMOUNT PAID: ", 16)]
        public decimal AggregateAmountPaid
        {
            get
            {
                return TotalPrincipalPaid + TotalInterestPaid + TotalFeesPaid;
            }
        }
        [LoanDetailAttribute("LOAN PAYOFF: ", 18)]
        public decimal LoanPayoff { get; set; }

        public DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");//These are needed for the data table but will not be used.
            dt.Columns.Add("2");
            int mod = 1;
            List<object> obj = new List<object>();
            foreach (PropertyInfo pi in this.GetType().GetProperties().OrderBy(r => r.GetAttributeValue<LoanDetailAttribute, int?>(o => o.Order, null)))
            {
                string title = pi.GetAttributeValue<LoanDetailAttribute, string>(p => p.Title, ""); //The title will be what the borrowers see's as the header
                string format = pi.GetAttributeValue<LoanDetailAttribute, string>(p => p.Format, "");//Some properties have a special format
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
            if (mod % 2 == 0)
            {
                dt.Rows.Add(obj.ToArray());
            }
            return dt;
        }

        public LoanInformation()
        {
            DateLastPaymentReceived = "";
        }
    }
}
