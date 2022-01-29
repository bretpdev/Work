using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// The given label will be used for the property instead of the property's name.
    /// </summary>
    public class LabelAttribute : Attribute
    {
        public string Label { get; set; }
        public LabelAttribute(string label)
        {
            Label = label;
        }
    }
}
