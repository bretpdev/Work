using System;

namespace BTCHLTRSFD
{
    public class DatabaseData
    {
        public int BatchLettersFedId { get; set; }
        public string LetterId { get; set; }
        public string SasFilePattern { get; set; }
        public string StateFieldCodeName { get; set; }
        public string AccountNumberFieldName { get; set; }
        public int AccountNumberIndex { get; set; }
        public string CostcenterFieldCodeName { get; set; }
        public bool OkIfMissing { get; set; }
        public bool ProcessAllFiles { get; set; }
        public string Arc { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Active { get; set; }
        public int BorrowerSsnIndex { get; set; }
        public bool DoNotProcessEcorr { get; set; }
    }
}
