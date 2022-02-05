using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace ACHSETUPFD.DataClasses
{
    public class EndorserRecord
    {
        [DbName("BF_SSN")]
        public string BF_SSN { get; set; }

        [DbName("LF_EDS")]
        public string LF_EDS { get; set; }

        [DbName("LN_SEQ")]
        public int LN_SEQ { get; set; }

        [DbName("DM_PRS_1")]
        public string DM_PRS_1 { get; set; }

        [DbName("DM_PRS_LST")]
        public string DM_PRS_LST { get; set; }

        [DbName("DX_STR_ADR_1")]
        public string DX_STR_ADR_1 { get; set; }

        [DbName("DX_STR_ADR_2")]
        public string DX_STR_ADR_2 { get; set; }

        [DbName("DX_STR_ADR_3")]
        public string DX_STR_ADR_3 { get; set; }

        [DbName("DM_CT")]
        public string DM_CT { get; set; }

        [DbName("DC_DOM_ST")]
        public string DC_DOM_ST { get; set; }

        [DbName("DF_ZIP_CDE")]
        public string DF_ZIP_CDE { get; set; }

        [DbName("DM_FGN_CNY")]
        public string DM_FGN_CNY { get; set; }

        [DbName("DM_FGN_ST")]
        public string DM_FGN_ST { get; set; }
    }
}
