using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public class ScriptAndServiceMenuItem : ToolStripMenuItem
    {
        public DataRow gsData { get; set; }

        public ScriptAndServiceMenuItem() : base()
        {

        }

        public ScriptAndServiceMenuItem(DataRow row, string text) : base(text)
        {
            gsData = row;
        }
    }
}
