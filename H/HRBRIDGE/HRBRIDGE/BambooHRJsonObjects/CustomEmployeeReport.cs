using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class CustomEmployeeReport
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("employees")]
        public List<CustomEmployeeReportRecord> Employees { get; set; }
    }

    public class CustomEmployeeReportRecord
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("fullName2")]
        public string FullName { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }
        [JsonProperty("workEmail")]
        public string WorkEmail { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("division")]
        public string Division { get; set; }
        //This is parsed to a string because it doesn't seem like there is a strong guarantee that hire date for
        //inactive employees is a valid null/datetime value
        [JsonProperty("hireDate")] 
        public string HireDate { get; set; }
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
        [JsonProperty("customSCACode")]
        public string SCACode { get; set; }
        [JsonProperty("customVacationCategory")]
        public string VacationCategory { get; set; }
        [JsonProperty("customDepartmentID")]
        public string DepartmentId { get; set; }
        [JsonProperty("4047")]
        public string EffectiveDate { get; set; }
        [JsonProperty("91")]
        public string ReportsTo { get; set; }
    }
}
