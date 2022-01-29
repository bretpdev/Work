using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDRRPTFED
{
    public class BdRecordData : IAppAndAward
    {
        public string AwardId { get; set; }
        public int ApplicationId { get; set; }
        public string EApplicationId { get; set; }
        public string FormattedAppId { get; set; }
        public DateTime? BDApplicationReceivedDate { get; set; }
        public string BDRepaymentPlanRequestCode { get; set; }
        public bool? BDLoansAtOtherServicer { get; set; }
        public int? BDSpouseId { get; set; }
        public bool? BDJointRepaymentPlan { get; set; }
        public int? BDTaxYear { get; set; }
        public int? BDFilingStatusCode { get; set; }
        public int? BDAgi { get; set; }
        public bool? BDAgiReflectsCurrentIncome { get; set; }
        public int? BDTaxableIncome { get; set; }
        public bool? BDSupportingDocRequired {
            get
            {
                if (Program.Legacy)
                    return BDAppSupportingDocRequired ?? false | SpouseSupDocsReq ?? false;
                else
                    return BDAppSupportingDocRequired ?? false;

            }
        } 
        public bool? BDAppSupportingDocRequired { get; set; }
        public DateTime? BDAppSupportingDocRecDate { get; set; }
        public string CurrentDefForbOpt { get; set; }
        public string PublicServiceIndicator { get; set; }
        public int? ReducedPaymentForb { get; set; }
        public int? NumberOfChildren { get; set; }
        public int? NumberOfDependents { get; set; }
        public int? MaritalStatus { get; set; }
        public bool? SeperatedFromSpouse { get; set; }
        public bool? AccessToSpouseIncome { get; set; }
        public bool? SpousesTaxesFiled { get; set; }
        public string SpouseTaxYear { get; set; }
        public int? SpouseFilingStatusId { get; set; }
        public int? SpouseAgi { get; set; }
        public bool? SpouseAgiReflectsIncome { get; set; }
        public bool? SpouseSupDocsReq { get; set; }
        public DateTime? SpouseSupDocRecDate { get; set; }
        public int? SpouseIncome { get; set; }
        public bool? BorrowerSelectedLowestPlan { get; set; }
        public bool? TaxesFiledFlag { get; set; }
        public bool? TaxableIncomeIndicator
        {
            get
            {
                if (((BDAgi == null || BDAgi.HasValue && BDAgi.Value == 0) && (BDTaxableIncome == null || (BDTaxableIncome.HasValue && BDTaxableIncome.Value == 0))) // Borrower no income
                    && ((SpouseAgi == null || SpouseAgi.HasValue && SpouseAgi.Value == 0) && (SpouseIncome == null || SpouseIncome.HasValue && SpouseIncome.Value == 0))) // Spouse no income
                    return false;
                else
                    return true;
            }
        }
        public string LoanSeq { get; set; }
    }
}
