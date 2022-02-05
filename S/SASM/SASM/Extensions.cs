using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SASM
{
    public static class Extensions
    {
        public static void Prepend(this ToolStripItemCollection col, ToolStripItem value)
        {
            col.Insert(0, value);
        }
    }
}
