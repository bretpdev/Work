using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    public class IncomePercentageFactor
    {
        public decimal Income { get; set; }
        public decimal Factor { get; set; }
        [DbName("start_date")]
        public DateTime StartDate { get; set; }
        [DbName("end_date")]
        public DateTime EndDate { get; set; }
        [DbName("married_or_head_of_household")]
        public bool Married { get; set; }
    }
}
