using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace LNDERLETTR
{
    public class CoverData
    {
        [DbName ("UHEAACostCenter")]
        public string  CostCenter { get; set; }
        [DbName ("Unit")]
        public string BusUnit { get; set; }
    }
}
