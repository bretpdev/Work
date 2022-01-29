using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class MaxLengthAttribute : Attribute
    {
        public int MaxLength { get; set; }
        public MaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
    }
}
