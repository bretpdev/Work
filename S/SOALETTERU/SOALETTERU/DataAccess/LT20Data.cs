using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOALETTERU
{
    public class LT20Data
    {
        public string DF_SPE_ACC_ID { get; set; }
        public string RM_APL_PGM_PRC { get; set; }
        public DateTime RT_RUN_SRT_DTS_PRC { get; set; }
        public int RN_SEQ_LTR_CRT_PRC { get; set; }
        public List<int> RN_SEQ_REC_PRC { get; set; }
        public string RM_DSC_LTR_PRC { get; set; }
        public string RF_SBJ_PRC { get; set; }
        public bool InvalidLoanStatus { get; set; }
        public string EndorsersSsn { get; set; }
        public string EndorsersAccountNumber { get; set; }
        public string DataFile { get; set; }
        public bool OnEcorr { get; set; }
        public string EmailAddress { get; set; }
        public string Hours1 { get; set; }
        public string Hours2 { get; set; }
        public string Recipient { get; set; }
        public bool IsCoborrower { get; set; }
        public string CoborrowerSSN { get; set; }
        public string BorrowerSSN { get; set; }
        public string LetterAccountNumber { get; set; }
        public DateTime? PrintedAt { get; set; }
        public DateTime? EcorrDocumentCreatedAt { get; set; }
    }
}
