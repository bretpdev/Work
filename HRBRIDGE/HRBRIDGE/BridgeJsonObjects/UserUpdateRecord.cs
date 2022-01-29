using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class UserPostUpdateRecord
    {
        [JsonProperty("users")]
        public List<UserAddRecord> Users { get; set; }

    }

    public class UserPutUpdateRecord
    {
        [JsonProperty("user")]
        public UserUpdateRecord User { get; set; }

    }

    public class UserAddRecord
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }
        [JsonProperty("hris_id")]
        public string HrisId { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("job_title")]
        public string JobTitle { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
    }

    public class UserUpdateRecord
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }
        [JsonProperty("hris_id")]
        public string HrisId { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("job_title")]
        public string JobTitle { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("manager_id")]
        public string ManagerId { get; set; }
        [JsonProperty("custom_field_values")]
        public List<CustomField> CustomFields { get; set; }
    }
}
