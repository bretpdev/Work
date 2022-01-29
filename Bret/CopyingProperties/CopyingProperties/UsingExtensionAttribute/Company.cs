using System.Collections.Generic;

namespace CopyingProperties.ExtensionSameName
{
    public class Company
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Program.JobType Job { get; set; }
        public List<Employee> Employees { get; set; }

        public Company(string cName, string address, string city, string state, string zip, Program.JobType job)
        {
            CompanyName = cName;
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Job = job;
            Employees = new List<Employee>();
        }
    }
}