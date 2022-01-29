using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class Alias
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("employees")]
        public List<AliasRecord> Employees { get; set; }
    }

    public class AliasRecord
    {
        [JsonProperty("id")]
        public int EmployeeId { get; set; }
        [JsonProperty("preferredName")]
        public string PreferredName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}
