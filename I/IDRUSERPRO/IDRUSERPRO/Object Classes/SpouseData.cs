using System;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    public class SpouseData
    {
        [DbName("spouse_id")]
        public int? SpouseId { get; set; }
        [DbName("person_role_id")]
        public int PersonRole { get; set; } = 4;
        public string Ssn { get; set; }
        [DbName("birth_date")]
        public DateTime? BirthDate { get; set; }
        [DbName("first_name")]
        public string FirstName { get; set; }
        [DbName("last_name")]
        public string LastName { get; set; }
        [DbName("middle_name")]
        public string MiddleName { get; set; }
        [DbName("state_code")]
        public string StateCode { get; set; }
        [DbName("separated_from_spouse")]
        public bool? SeparatedFromSpouse { get; set; }
        [DbName("access_spouse_income_info")]
        public bool? AccessSpouseIncome { get; set; }
        [DbName("spouse_taxes_filed_flag")]
        public bool? TaxesFiled { get; set; }
        [DbName("spouse_tax_year")]
        public int? TaxYear { get; set; }
        [DbName("spouse_filing_status_id")]
        public int? FilingStatusId { get; set; }
        [DbName("spouse_AGI")]
        public decimal? SpouseAgi { get; set; }
        [DbName("spouse_AGI_relects_current_income")]
        public bool? AgiReflectsIncome { get; set; }
        [DbName("spouse_support_docs_required")]
        public bool? SupportingDocsReq { get; set; }
        [DbName("spouse_support_docs_recvd_date")]
        public DateTime? SupportingDocsRecDate { get; set; }
        [DbName("spouse_alt_submitted_income")]
        public decimal? AltIncome { get; set; }
        [DbName("spouse_Loans_Same_Region")]
        public bool? LoansInSameRegion { get; set; }
        [DbName("spouse_taxable_income")]
        public bool? TaxableIncome { get; set; }
        [DbName("spouse_income_source_id")]
        public IncomeSources? IncomeSourceId { get; set; }
    }
}