using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class CustomFieldListItemRecord
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
        [JsonProperty("custom_fields")]
        public List<CustomFieldListItem> CustomFields { get; set; }
    }
        
    public class CustomFieldListItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class CustomField
    {
        [JsonProperty("id")]
        public string Id { get; set; } 
        [JsonProperty("custom_field_id")]
        public string CustomFieldId { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

    }
}
