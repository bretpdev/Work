using System;


namespace BBREININSC
{
    public class ReinstatementRecord
    {
        public string BF_SSN { get; set; }
        public int LN_SEQ { get; set; }
        public DateTime LD_BIL_DU { get; set; }
        public string RecordType { get; set; }
        public int RecordId { get; set; }
        //public bool ResetBenefits { get; set; }
        public bool CleanupBills { get; set; }
        public bool UpdateTSDS { get; set; }
        public string EarlyDate { get; set; }
        public string LateDate { get; set; }
    }

   
}
