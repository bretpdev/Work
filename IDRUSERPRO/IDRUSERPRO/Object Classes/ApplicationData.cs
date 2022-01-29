using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    public class ApplicationData
    {
        public int? ApplicationId { get; set; }
        public string EApplicationId { get; set; }
        public string AwardId { get; set; }
        public int? SpouseId { get; set; }
        public int? ApplicationSourceId { get; set; }
        public int? RepaymentPlanStatusId { get; set; }
        public DateTime? DisclosureDate { get; set; }
        public DateTime? RepaymentPlanDateEntered { get; set; }
        public int? FilingStatusId { get; set; }
        public int? FamilySize { get; set; }
        public int? TaxYear { get; set; }
        public bool? LoansAtOtherServicers { get; set; }
        public bool? JointRepaymentPlan { get; set; }
        public decimal? Agi { get; set; }
        [DbName("manually_submitted_income_indicator")]
        public bool? ManuallySubmittedIncomeIndicator { get; set; }
        public decimal? ManuallySubmittedIncome { get; set; }
        public bool? RequestedByBorrower { get; set; }
        public int? BorrowerEligibilityId { get; set; }
        public int? RepaymentPlanReason { get; set; }
        public int? RepaymentPlanTypeRequested { get; set; }
        public string DueDateRequested { get; set; }
        public string State { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public bool? TaxesFiledFlag { get; set; }
        public bool Active { get; set; }
        public bool? RepaymentTypeProcessedNotSame { get; set; }
        public int? NumberOfChildren { get; set; }
        public int? NumberOfDependents { get; set; }
        public decimal? TotalIncome { get; set; }
        public int? DefForbId { get; set; }
        public string GradeLevel { get; set; } 
        public string PublicServiceEmployment//This is needed for a file but will always be null
        {
            get
            {
                return null;
            }
            set
            {
                
            }

        }
        public decimal? RPF { get; set; }
        public int? MaritalStatusId { get; set; }
        public bool? IncludeSpouseInFamilySize { get; set; }
        [DbName("income_source_id")]
        public IncomeSources? BorrowerIncomeSource { get; set; }
        [DbName("agi_reflects_current_income")]
        public bool? BorrowerAgiReflectsCurrentIncome { get; set; }
        [DbName("taxable_income")]
        public bool? BorrowerIncomeTaxable { get; set; }
        [DbName("supporting_documentation_required")]
        public bool BorrowerSupportingDocumentationRequired { get; set; }
        [DbName("supporting_documentation_received_date")]
        public DateTime? BorrowerSupportingDocumentationReceivedDate { get; set; }
        [DbIgnore]
        public List<string> MissingDocData { get; set; }
        [DbIgnore]
        public List<PayStubs> BorrowerStubs { get; set; }
        [DbIgnore]
        public List<PayStubs> SpouseStubs { get; set; }
        [DbIgnore]
        public IncomeSource IncomeSource { get; set; }


        public ApplicationData()
        {
            Active = true;
            MissingDocData = new List<string>();
        }

    }
}