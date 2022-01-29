using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public class LinePosAttribute : Attribute
    {
        public int[] Positions { get; private set; }
        public LinePosAttribute(params int[] positions)
        {
            this.Positions = positions;
        }
    }
}
