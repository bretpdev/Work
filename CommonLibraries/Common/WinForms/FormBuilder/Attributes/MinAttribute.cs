using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// Specifies the minimum value for integer properties
    /// </summary>
    public class MinAttribute : Attribute
    {
        public int Min { get; set; }
        public MinAttribute(int min)
        {
            Min = min;
        }
    }
}
