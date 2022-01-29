using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class JobInfoSSMS
    {
        public int EmployeeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string JobTitle { get; set; }
        public string ReportsTo { get; set; }
    }
}
