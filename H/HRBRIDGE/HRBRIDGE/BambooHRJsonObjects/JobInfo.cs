using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class JobInfo
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("employees")]
        public Dictionary<string, JobInfoRecordUpdated> Employees { get; set; }
    }

    public class JobInfoRecordUpdated
    {
        [JsonProperty("lastChanged")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("rows")]
        public List<JobInfoRecord> Rows { get; set; }
    }

    public class JobInfoRecord
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("division")]
        public string Division { get; set; }
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }
        [JsonProperty("reportsTo")]
        public string ReportsTo { get; set; }

    }

}
