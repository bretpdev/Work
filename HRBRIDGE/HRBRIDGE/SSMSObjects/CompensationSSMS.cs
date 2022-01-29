using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class CompensationSSMS
    {
        public int EmployeeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public string Rate { get; set; }
        public string Type { get; set; }
        public string Exempt { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public string PaidPer { get; set; }
        public string PaySchedule { get; set; }
    }
}
