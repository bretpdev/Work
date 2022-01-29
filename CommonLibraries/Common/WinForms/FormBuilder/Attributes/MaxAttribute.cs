using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// Specifies the maximum value for integer properties
    /// </summary>
    public class MaxAttribute : Attribute
    {
        public int Max { get; set; }
        public MaxAttribute(int max)
        {
            Max = max;
        }
    }
}
