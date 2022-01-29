using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DPALETTERS
{
    public class DrawAmountResponse
    {
        public DialogResult Result { get; set; }
        public decimal? DrawAmount { get; set; }
    }
}
