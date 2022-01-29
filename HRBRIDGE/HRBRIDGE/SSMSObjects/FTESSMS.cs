using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    class FTESSMS
    {
        public int EmployeeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? FTEEffectiveDate { get; set; }
        public string FTE { get; set; }
        public string Notes { get; set; }
    }
}
