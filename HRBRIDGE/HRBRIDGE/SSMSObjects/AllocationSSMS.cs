using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    public class AllocationSSMS
    {
        public int EmployeeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string BusinessUnit { get; set; }
        public string CostCenter { get; set; }
        public string Account { get; set; }
        public string FTE { get; set; }
        public DateTime? AllocationEffectiveDate { get; set; }
        public int SquareFootage { get; set; }
    }
}
