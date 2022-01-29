using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class CycleOption<T>
    {
        public string Label { get; set; }
        public Color LabelColor { get; set; }
        public bool BoldLabel { get; set; }
        public T Value { get; set; }
    }
}
