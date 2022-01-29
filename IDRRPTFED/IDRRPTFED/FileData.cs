using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDRRPTFED
{
    class FileData
    {
        public string AwardId { get; set; }
        public int ApplicationId { get; set; }
        public string FormattedAppId { get; set; }
        public string ABPersonRole { get; set; }
        public string ABSpouseSsn { get; set; }
        public DateTime? ABSpouseDob { get; set; }
        public string ABSpouseFirstName { get; set; }
        public string ABSpouseLastName { get; set; }
        public string ABSpouseMiddleName { get; set; }
        public bool? ABPersonsSsnIndicator { get; set; }
        public string ABDriversLicense { get; set; }
        public string ABDriversLicenseSt { get; set; }
        public DateTime? BDApplicationReceivedDate { get; set; }
        public string BDRepaymentPlanRequestCode { get; set; }
        public bool? BDLoansAtOtherServicer { get; set; }
        public int? BDSpouseId { get; set; }
        public bool? BDJointRepaymentPlan { get; set; }
        public int? BDFamilySize { get; set; }
        public int? BDTaxYear { get; set; }
        public int? BDFilingStatusCode { get; set; }
        public int? BDAgi { get; set; }
        public bool? BDAgiReflectsCurrentIncome { get; set; }
        public bool? BDManuallySubmittedIncomeIndicator { get; set; }
        public int? BDManuallySubmittedIncome { get; set; }
        public bool? BDSupportingDocRequired { get; set; }
        public DateTime? BDSupportingDocRecDate { get; set; }
        public string BERepaymentTypeProgram { get; set; }
        public bool? BERequestedByBorrower { get; set; }
        public string BERepaymentPlanTypeStatus { get; set; }
        public DateTime? BERepaymentPlanTypeStausDate { get; set; }
        public DateTime? BFRepaymentPlanDateEntered { get; set; }
    }
}
