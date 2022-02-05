using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class FTE
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("employees")]
        public Dictionary<string, FTERecordUpdated> Employees { get; set; }
    }

    public class FTERecordUpdated
    {
        [JsonProperty("lastChanged")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("rows")]
        public List<FTERecord> Rows { get; set; }
    }

    public class FTERecord
    {
        [JsonProperty("customFTEEffectiveDate")]
        public DateTime? FTEEffectiveDate { get; set; }
        [JsonProperty("customFTE1")]
        public string FTE { get; set; }
        [JsonProperty("customNotes")]
        public string Notes { get; set; }
    }
}
