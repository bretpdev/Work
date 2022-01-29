using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class EmployeeBridgeSSMS
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkEmail { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string JobTitle { get; set; }
        public string Supervisor { get; set; }
        public string Division { get; set; }
        public DateTime HireDate { get; set; }
        public string EmployeeNumber { get; set; }
        public string NewHire { get; set; }
        public string EmployeeLevel { get; set; }
        public string DepartmentId { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

