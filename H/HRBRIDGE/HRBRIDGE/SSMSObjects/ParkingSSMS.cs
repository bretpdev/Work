using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class ParkingSSMS
    {
        public int EmployeeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Garage { get; set; }
        public string Type { get; set; }
        public string FobId { get; set; }
    }
}
