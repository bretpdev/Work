using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    class UserDirectoryRecord
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
        [JsonProperty("linked")]
        public LinkedRecord Linked { get; set; }
        [JsonProperty("users")]
        public List<UserRecord> Users { get; set; }
    }
}
