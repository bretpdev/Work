using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRPQASSIGN
{
    public sealed class MyListBox : CheckedListBox
    {
        public MyListBox()
        {
            
        }
        public override int ItemHeight
        {
            get
            {
                return Font.Height + 3;
            }
        }
    }
}
