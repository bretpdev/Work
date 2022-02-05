using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public class OccursAttribute : Attribute
    {
        public int Times { get; set; }
        public OccursAttribute(int times)
        {
            Times = times;
        }
    }
}
