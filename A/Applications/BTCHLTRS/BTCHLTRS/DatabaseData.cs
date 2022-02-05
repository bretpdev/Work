using System;

namespace BTCHLTRS
{
    public class DatabaseData
    {
        public int BatchLettersId { get; set; }
        public string LetterId { get; set; }
        public string SasFilePattern { get; set; }
        public string StateFieldCodeName { get; set; }
        public string AccountNumberFieldName { get; set; }
        public string CostcenterFieldCodeName { get; set; }
        public bool IsDuplex { get; set; }
        public bool OkIfMissing { get; set; }
        public bool ProcessAllFiles { get; set; }
        public string Arc { get; set; }
        public string Comment { get; set; }
        private DateTime? CreatedAt { get; set; }
        private string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Active { get; set; }
    }
}
