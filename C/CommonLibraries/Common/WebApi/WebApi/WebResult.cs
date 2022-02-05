using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.WebApi
{
    public class WebResult
    {
        public bool DatabaseConnectionSuccessful { get; set; }
        public bool DatabaseCallSuccessful { get; set; }
        public string ExceptionText { get; set; }
    }
}
