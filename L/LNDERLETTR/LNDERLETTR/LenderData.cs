using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace LNDERLETTR
{
    public class LenderData
    {
        [DbName("IF_DOE_LDR")]
        public int LenderId { get; set; }
        [DbName("IM_LDR_FUL")]
        public string LenderName { get; set; }
        [DbName("IX_LDR_STR_ADR_1")]
        public string Address1 { get; set; }
        [DbName("IX_LDR_STR_ADR_2")]
        public string Address2 { get; set; }
        [DbName("IM_LDR_CT")]
        public string City { get; set; }
        [DbName("IC_LDR_DOM_ST")]
        public string State { get; set; }
        [DbName("IF_LDR_ZIP_CDE")]
        public string Zip { get; set; }
        [DbName("IM_LDR_FGN_CNY")]
        public string Country { get; set; }
        public List<BorrowerData> UnprintedBorrowers { get; set; }
        public List<BorrowerData> AllBorrowers { get; set; }
    }
}