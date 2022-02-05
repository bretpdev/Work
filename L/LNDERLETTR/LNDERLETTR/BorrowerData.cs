using System;
using Uheaa.Common.DataAccess;

namespace LNDERLETTR
{
    public class BorrowerData
    {
        [DbName("DF_SPE_ACC_ID")]
        public string AccountNumber { get; set; }
        [DbName("DF_PRS_ID")]
        public string Ssn { get; set; }
        [DbName("DF_PRS_LST_4_SSN")]
        public string SsnLastFour { get; set; }
        [DbName("DM_PRS_1")]
        public string FirstName { get; set; }
        [DbName("DM_PRS_MID")]
        public string MiddleInitial { get; set; }
        [DbName("DM_PRS_LST")]
        public string LastName { get; set; }
        [DbName("DD_BRT")]
        public string Dob { get; set; }
        [DbName("DM_PRS_1_HST")]
        public string FirstNameHst { get; set; }
        [DbName("DM_PRS_MID_HST")]
        public string MiddleInitialHst { get; set; }
        [DbName("DM_PRS_LST_HST")]
        public string LastNameHst { get; set; }
        [DbName("DX_STR_ADR_1")]
        public string Address1 { get; set; }
        [DbName("DX_STR_ADR_2")]
        public string Address2 { get; set; }
        [DbName("DM_CT")]
        public string City { get; set; }
        [DbName("DC_DOM_ST")]
        public string State { get; set; }
        [DbName("DF_ZIP_CDE")]
        public string ZipCode { get; set; }
        [DbName("DM_FGN_CNY")]
        public string ForeignCountry { get; set; }
        [DbName("DM_FGN_ST")]
        public string ForeignState { get; set; }
        public string HomePhone { get; set; }
        public string AlternatePhone { get; set; }
        public string WorkPhone { get; set; }
        [DbName("DX_ADR_EML")]
        public string Email { get; set; }
        public int LettersId { get; set; }
        public int? ArcId { get; set; }
        public DateTime? QueId { get; set; }
    }
}