using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace LENDERLTRS
{
    public class Hours
    {
        [DbName("Hours1")]
        public string Open { get; set; }
        [DbName("Hours2")]
        public string Close { get; set; }
    }
}
