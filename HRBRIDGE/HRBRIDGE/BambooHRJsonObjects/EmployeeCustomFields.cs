using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class EmployeeCustomFields
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("hireDate")]
        public DateTime HireDate { get; set; }
        [JsonProperty("ethnicity")]
        public string Ethnicity { get; set; }
        [JsonProperty("employeeNumber")]
        public string EmployeeNumber { get; set; }
        [JsonProperty("eeo")]
        public string EEOJobCategory { get; set; }
        [JsonProperty("customClearance")]
        public string Clearance { get; set; }
        [JsonProperty("dateOfBirth")]
        public string Birthdate { get; set; }
        [JsonProperty("customEmployeeLevel")]
        public string EmployeeLevel { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("customSCACode")]
        public string SCACode { get; set; }
        [JsonProperty("customVacationCategory")]
        public string VacationCategory { get; set; }
        [JsonProperty("customDepartmentID")]
        public string DepartmentId { get; set; }


    }
}
