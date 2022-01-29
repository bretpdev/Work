using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class LinkedRecord
    {
        [JsonProperty("custom_fields")]
        public List<CustomFieldListItem> CustomFields { get; set; }

        [JsonProperty("custom_field_values")]
        public List<LinkedFields> CustomFieldValues { get; set; }
    }

    public class LinkedFields
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("links")]
        public Link Links { get; set; }
    }

    public class Link
    {
        [JsonProperty("custom_field")]
        public LinkCustomField LinkCustomField { get; set; }
    }

    public class LinkCustomField
    {
        //The ID of the CustomField Type Record
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class LinkValueList
    {
        [JsonProperty("custom_field_values")]
        public List<string> CustomFieldValues { get; set; }
    }

}
