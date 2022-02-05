using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class EmploymentStatus
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("employees")]
        public Dictionary<string, EmploymentStatusRecordUpdated> Employees { get; set; }
    }

    public class EmploymentStatusRecordUpdated
    {
        [JsonProperty("lastChanged")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("rows")]
        public List<EmployementStatusRecord> Rows { get; set; }
    }

    public class EmployementStatusRecord
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("employmentStatus")]
        public string EmploymentStatus { get; set; }
        [JsonProperty("comment")]
        public string EmploymentStatusComment { get; set; }
        [JsonProperty("terminationReasonId")]
        public string TerminationReason { get; set; }
        [JsonProperty("terminationTypeId")]
        public string TerminationType { get; set; }
        [JsonProperty("terminationRehireId")]
        public string ElligableForRehire { get; set; }
    }
}
