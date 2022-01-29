using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    public class PayStubs
    {
        public enum Frequency
        {
            BiWeekly = 26,
            SemiMonthly = 24,
            Weekly = 52,
            Monthly = 12,
            Yearly = 1
        }

        [DbName("employer_name")]
        public string EmployerName { get; set; }
        public decimal Ftw { get; set; }
        public decimal Gross { get; set; }
        public decimal Bonus { get; set; }
        public decimal Overtime { get; set; }
        [DbName("total_pre_tax")]
        public decimal TotalPreTax { get; set; }
        [DbName("adoi_paystub_frequency_id")]
        public Frequency PayFrequency { get; set; }

        #region Calculated Fields
        public decimal AdjustedPay
        {
            get
            {
                if (Ftw > 0)
                    return Ftw - Bonus - Overtime;
                if (Gross > 0)
                    return Gross - Bonus - Overtime - TotalPreTax;
                return 0;
            }
        }

        public decimal AnnualTaxableGross
        {
            get
            {
                return AdjustedPay * (int)PayFrequency;
            }
        }

        public decimal OneTimeAdditions
        {
            get
            {
                return Bonus + Overtime;
            }
        }

        public decimal EmployerBorrowerAltIncome
        {
            get
            {
                return AnnualTaxableGross + OneTimeAdditions;
            }
        }
        #endregion
    }
}