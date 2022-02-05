using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class ArrowlessNumericUpDown : NumericUpDown
    {
        public ArrowlessNumericUpDown()
        {
            bool isSet = false;
            NW nw = null;
            this.BackColorChanged += (o, ea) => Refresh();
            if (!isSet)
            {
                foreach (Control c in this.Controls)
                {
                    if (!(c is TextBox))
                    {
                        // prevent flicker
                        typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, c, new object[] { true });
                        c.Paint += (sender, e) =>
                        {
                            var g = e.Graphics;

                            if (this.Enabled)
                                g.Clear(this.BackColor);
                            else
                                g.Clear(SystemColors.Control);
                        };

                        nw = new NW(c.Handle);
                        isSet = true;
                    }
                }
            }
        }

        class NW : NativeWindow
        {

            public NW(IntPtr hwnd)
            {
                AssignHandle(hwnd);
            }
            const int WM_NCHITTEST = 0x84;
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_NCHITTEST)
                    return;

                base.WndProc(ref m);
            }
        }
    }
}
