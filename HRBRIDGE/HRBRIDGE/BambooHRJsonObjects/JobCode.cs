using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    class JobCode
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("employees")]
        public Dictionary<string, JobCodeRecordUpdated> Employees { get; set; }
    }

    public class JobCodeRecordUpdated
    {
        [JsonProperty("lastChanged")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("rows")]
        public List<JobCodeRecord> Rows { get; set; }
    }

    public class JobCodeRecord
    {
        [JsonProperty("customJobCodeEffectiveDate")]
        public DateTime? JobCodeEffectiveDate { get; set; }
        [JsonProperty("customJobCode")]
        public string JobCode { get; set; }
        [JsonProperty("customUofUJobTitle")]
        public string UOfUJobTitle { get; set; }

    }
}
