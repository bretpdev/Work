using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCCLOSURES
{
    public class DataGridViewLabel : Attribute
    {
        public string Label { get; set; }
        public int Index { get; set; }

        public DataGridViewLabel(string label, int index)
        {
            Label = label;
            Index = index;
        }
    }
}
