using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class Meta
    {
        [JsonProperty("next")]
        public string Next { get; set; }
    }
}
