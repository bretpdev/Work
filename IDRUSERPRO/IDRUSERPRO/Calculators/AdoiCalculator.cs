using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    /// <summary>
    /// Based on Borrower and Spouse Income/Paystubs, calculate Total Alt Incomes and Needed ADOIs
    /// </summary>
    public class AdoiCalculator
    {
        public AdoiIncomeInput BorrowerIncome { get; set; }
        public AdoiIncomeInput SpouseIncome { get; set; }

        public AdoiCalculator(AdoiIncomeInput borrowerIncome, AdoiIncomeInput spouseIncome)
        {
            BorrowerIncome = borrowerIncome;
            SpouseIncome = spouseIncome;
        }

        #region Calculated Fields
        /// <summary>
        /// The sum of all alternate income from the Borrower's Paystubs.
        /// </summary>
        [JsonIgnore]
        public decimal BorrowerAltIncome
        {
            get
            {
                var stubs = BorrowerIncome?.Paystubs;
                return SumPaystubs(stubs);
            }
        }
        /// <summary>
        /// The sum of all alternate income from the Spouse's Paystubs.
        /// </summary>
        [JsonIgnore]
        public decimal SpouseAltIncome
        {
            get
            {
                var stubs = SpouseIncome?.Paystubs;
                return SumPaystubs(stubs);
            }
        }
        /// <summary>
        /// The sum of all alternate income from the Borrower's Paystubs and Spouse's Paystubs combined.
        /// </summary>
        [JsonIgnore]
        public decimal TotalAltIncome
        {
            get
            {
                return SpouseAltIncome + BorrowerAltIncome;
            }
        }
        /// <summary>
        /// Based on given Borrower and Spouse Income Sources, determine which additional ADOIs are needed.
        /// Use this field to determine which additional calculation buttons should be enabled.
        /// </summary>
        [JsonIgnore]
        public NeededAdois NeededAdoi
        {
            get
            {
                bool borrowerAltTax = BorrowerIncome?.IncomeSource == IncomeSources.AltAdoi && BorrowerIncome?.TaxableIncome == true;
                bool spouseAltTax = SpouseIncome?.IncomeSource == IncomeSources.AltAdoi && SpouseIncome?.TaxableIncome == true;
                if (borrowerAltTax)
                {
                    if (spouseAltTax)
                        return NeededAdois.Both;
                    return NeededAdois.Borrower;
                }
                else
                {
                    if (spouseAltTax)
                        return NeededAdois.Spouse;
                    return NeededAdois.Neither;
                }
            }
        }
        #endregion

        public static decimal SumPaystubs(IEnumerable<PayStubs> stubs)
        {
            if (stubs == null || !stubs.Any())
                return 0;
            decimal total = 0;
            foreach (var employer in stubs.Select(o => o.EmployerName).Distinct())
            {
                decimal employerAgi = AverageEmployerPaystubs(stubs, employer);
                total += employerAgi;
            }
            return total;
        }

        public static decimal AverageEmployerPaystubs(IEnumerable<PayStubs> stubs, string employerName)
        {
            var employerStubs = stubs.Where(o => o.EmployerName == employerName);
            decimal employerAgi = employerStubs.Sum(o => o.AnnualTaxableGross);
            if (employerAgi != 0)
                employerAgi = employerAgi / employerStubs.Count();
            employerAgi += employerStubs.Sum(o => o.OneTimeAdditions);
            return employerAgi;
        }
    }
}
