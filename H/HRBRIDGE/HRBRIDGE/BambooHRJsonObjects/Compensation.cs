using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class Compensation
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("employees")]
        public Dictionary<string, CompensationRecordUpdated> Employees { get; set; }
    }

    public class CompensationRecordUpdated
    {
        [JsonProperty("lastChanged")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("rows")]
        public List<CompensationRecord> Rows { get; set; }
    }

    public class CompensationRecord
    {
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("rate")]
        public string Rate { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("exempt")]
        public string Exempt { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("paidPer")]
        public string PaidPer { get; set; }
        [JsonProperty("paySchedule")]
        public string PaySchedule { get; set; }
        [JsonProperty("overtimeRate")]
        public string OvertimeRate { get; set; }

    }
}
