using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    class UserRecord
    {
        [JsonProperty("id")]
        public string Id { get; set; }
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
        [JsonProperty("sortable_name")]
        public string SortableName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonProperty("deleted_at")]
        public DateTime? DeletedAt { get; set; }
        [JsonProperty("unsubscribed")]
        public string Unsubscribed { get; set; }
        [JsonProperty("welcomedAt")]
        public DateTime? WelcomedAt { get; set; }
        [JsonProperty("loggedInAt")]
        public DateTime? LoggedInAt { get; set; }
        [JsonProperty("passwordIsSet")]
        public bool PasswordIsSet { get; set; }
        [JsonProperty("hire_date")]
        public DateTime? HireDate { get; set; }
        [JsonProperty("is_manager")]
        public bool IsManager { get; set; }
        [JsonProperty("job_title")]
        public string JobTitle { get; set; }
        [JsonProperty("bio")]
        public string Bio { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("anonymized")]
        public string Anonymized { get; set; }
        [JsonProperty("domain_id")]
        public string DomainId { get; set; }
        [JsonProperty("birth_month")]
        public string BirthMonth { get; set; }
        [JsonProperty("birth_day_of_month")]
        public string BirthDayOfMonth { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("preferred_phone")]
        public string PreferredPhone { get; set; }
        [JsonProperty("links")]
        public LinkValueList Links { get; set; }
        [JsonProperty("manager_id")]
        public string ManagerId { get; set; }
    }
}
