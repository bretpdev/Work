using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class EmploymentStatusSSMS
    {
        public int EmployeeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime Date { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentStatusComment { get; set; }
        public string TerminationReason { get; set; }
        public string TerminationType { get; set; }
        public string ElligableForRehire { get; set; }
    }
}
