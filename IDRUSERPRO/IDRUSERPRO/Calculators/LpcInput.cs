using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    /// <summary>
    /// Information necessary for the Lowest Plan Calculator to begin working.
    /// </summary>
    public class LpcInput
    {
        const string Hawaii = "HI";
        const string Alaska = "AK";
        public string AccountNumber { get; set; }
        public string StateCode { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RepaymentPlans RepaymentPlan { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MaritalStatuses MaritalStatus { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public FilingStatuses FilingStatus { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfDependents { get; set; }
        public decimal BorrowerAgiFromTaxes { get; set; }
        public decimal BorrowerAltIncome { get; set; }
        public decimal SpouseAgiFromTaxes { get; set; }
        public decimal SpouseAltIncome { get; set; }
        public IEnumerable<ExternalLoan> ExternalLoans { get; set; } = new List<ExternalLoan>();
        public PovertyGuideline PovertyGuideline { get; set; }
        public IEnumerable<IncomePercentageFactor> IncomePercentageFactors { get; set; }

        #region Calculated Fields
        [JsonIgnore]
        public int FamilySize
        {
            get
            {
                int size = 1;
                if (IncludeSpouseInFamilySize)
                    size++;
                size += NumberOfChildren;
                size += NumberOfDependents;
                return size;
            }
        }
        [JsonIgnore]
        public bool IncludeSpouseInFamilySize
        {
            get
            {
                if (MaritalStatus != MaritalStatuses.Single)
                {
                    if (RepaymentPlan != RepaymentPlans.REPAYE)
                        return true;
                    else if (MaritalStatus == MaritalStatuses.Married)
                        return true;
                }
                return false;
            }
        }
        [JsonIgnore]
        public decimal FinalDeductionAmount
        {
            get
            {
                var guideline = PovertyGuideline;
                if (guideline == null)
                    return 0;
                var size = FamilySize - 1;
                var income = guideline.ContinentalIncome;
                var increment = guideline.ContinentalIncrement;
                if (StateCode == Hawaii)
                {
                    income = guideline.HawaiiIncome;
                    increment = guideline.HawaiiIncrement;
                }
                if (StateCode == Alaska)
                {
                    income = guideline.AlaskaIncome;
                    increment = guideline.AlaskaIncrement;
                }
                return (increment * size) + income;
            }
        }
        [JsonIgnore]
        public decimal AGI
        {
            get
            {
                var borrowerAgi = BorrowerAgiFromTaxes + BorrowerAltIncome;
                var spouseAgi = SpouseAgiFromTaxes + SpouseAltIncome;
                if (FilingStatus == FilingStatuses.MarriedFilingJointly)
                {
                    return borrowerAgi + spouseAgi;
                }
                else if (FilingStatus == FilingStatuses.MarriedFilingSeparately)
                {
                    if (RepaymentPlan == RepaymentPlans.REPAYE)
                        return borrowerAgi + spouseAgi;
                    else
                        return borrowerAgi;
                }
                return borrowerAgi;
            }
        }

        #endregion
    }
}
