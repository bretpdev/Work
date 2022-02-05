using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class Allocation
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("employees")]
        public Dictionary<string, AllocationRecordUpdated> Employees { get; set; }
    }

    public class AllocationRecordUpdated
    {
        [JsonProperty("lastChanged")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("rows")]
        public List<AllocationRecord> Rows { get; set; }
    }

    public class AllocationRecord
    {
        [JsonProperty("customBusinessUnit")]
        public string BusinessUnit { get; set; }
        [JsonProperty("customCostCenter")]
        public string CostCenter { get; set; }
        [JsonProperty("customAccount")]
        public string Account { get; set; }
        [JsonProperty("customFTE")]
        public string FTE { get; set; }
        [JsonProperty("customAllocationEffectiveDate")]
        public DateTime? AllocationEffectiveDate { get; set; }
        [JsonProperty("customSquareFootage")]
        public int SquareFootage { get; set; }
    }
}
