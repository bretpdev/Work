using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class JobCodeSSMS
    {
        public int EmployeeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? JobCodeEffectiveDate { get; set; }
        public string JobCode { get; set; }
        public string UOfUJobTitle { get; set; }
    }
}
