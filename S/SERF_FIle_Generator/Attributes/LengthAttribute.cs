using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public class LengthAttribute : Attribute
    {
        public int Length { get; set; }
        public int DecimalLength { get; set; }
        public LengthAttribute(int length, int decimalLength = 0)
        {
            Length = length;
            DecimalLength = decimalLength;
        }
    }
}
