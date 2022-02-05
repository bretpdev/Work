using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class CheckButton : Button
    {
        public CheckButton()
        {
            cb = new TransparentCheckBox();
            cb.Dock = DockStyle.Left;
            cb.BackColor = Color.Transparent;
            cb.CheckAlign = ContentAlignment.MiddleRight;
            //cb.BackColor = Color.Black;
            
            this.Controls.Add(cb);
            this.Click += (o, ea) => IsChecked = !IsChecked;
            this.SizeChanged += (o, ea) => SetWidth();
            this.FontChanged += (o, ea) => SetWidth();
            SetWidth();
        }
        TransparentCheckBox cb;
        public bool IsChecked
        {
            get { return cb.Checked; }
            set { cb.Checked = value; }
        }

        public void SetWidth()
        {
            cb.Width = (int)(20 * (this.Font.Size / 8.25));
        }

        private class TransparentCheckBox : CheckBox
        {
            protected override void WndProc(ref Message m)
            {
                const int WM_NCHITTEST = 0x0084;
                const int HTTRANSPARENT = (-1);

                if (m.Msg == WM_NCHITTEST)
                {
                    m.Result = (IntPtr)HTTRANSPARENT;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
        }
    }
}
