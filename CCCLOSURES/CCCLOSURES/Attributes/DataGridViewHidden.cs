using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCCLOSURES
{
    public class DataGridViewHidden : Attribute
    {
        public int Index { get; set; }
        public DataGridViewHidden(int index)
        {
            Index = index;
        }
    }
}
