using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    public class DataGridViewAttribute : Attribute
    {
        public int Index { get; set; }
        public string DisplayName { get; set; }
        public bool Visible { get; set; }
        public string Format { get; set; }
        public DataGridViewAttribute(int index, string displayName, bool visible = true, string format = "")
        {
            Index = index;
            DisplayName = displayName;
            Visible = visible;
            Format = format;
        }
    }
}
