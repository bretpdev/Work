using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// Specifies the order this property should appear in.
    /// </summary>
    public class OrdinalAttribute : Attribute
    {
        public int Ordinal { get; set; }
        public OrdinalAttribute(int ordinal)
        {
            Ordinal = ordinal;
        }
    }
}
