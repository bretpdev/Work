using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class EmployeeDirectory
    {
        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }
        [JsonProperty("employees")]
        public List<EmployeeDirectoryRecord> Employees { get; set; }
    }

    public class EmployeeDirectoryRecord
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("preferredName")]
        public string PreferredName { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }
        [JsonProperty("workPhone")]
        public string WorkPhone { get; set; }
        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }
        [JsonProperty("workEmail")]
        public string WorkEmail { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("division")]
        public string Division { get; set; }
        //[JsonProperty("linkedIn")]
        //public string LinkedIn { get; set; }
        [JsonProperty("workPhoneExtension")]
        public string WorkPhoneExtension { get; set; }
        [JsonProperty("supervisor")]
        public string Supervisor { get; set; }
        //[JsonProperty("photoUploaded")]
        //public bool PhotoUploaded { get; set; }
        //[JsonProperty("photoUrl")]
        //public string PhotoUrl { get; set; }
        //[JsonProperty("canUploadPhoto")]
        //public string CanUploadPhoto { get; set; }
    }

    public class Field
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
