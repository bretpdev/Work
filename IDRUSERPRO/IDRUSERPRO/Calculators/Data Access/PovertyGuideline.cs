using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    public class PovertyGuideline
    {
        public int Year { get; set; }
        [DbName("continental_income")]
        public decimal ContinentalIncome { get; set; }
        [DbName("alaska_income")]
        public decimal AlaskaIncome { get; set; }
        [DbName("hawaii_income")]
        public decimal HawaiiIncome { get; set; }
        [DbName("continental_increment")]
        public decimal ContinentalIncrement { get; set; }
        [DbName("alaska_increment")]
        public decimal AlaskaIncrement { get; set; }
        [DbName("hawaii_increment")]
        public decimal HawaiiIncrement { get; set; }
    }
}
