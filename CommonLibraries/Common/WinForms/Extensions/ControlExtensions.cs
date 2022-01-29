using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public static class ControlExtensions
    {
        public static Form ParentForm(this Control c)
        {
            Control parent = c;
            while (!(parent is Form))
                parent = parent.Parent;
            return parent as Form;
        }

        public static List<Control> AllChildren(this Control c)
        {
            List<Control> results = new List<Control>();
            foreach (Control control in c.Controls)
            {
                results.Add(control);
                if (control.Controls.Count > 0)
                    results.AddRange(AllChildren(control));
            }
            return results; 
        }

        public static void MakeFontBold(this Control c, bool makeBold = true)
        {
            c.Font = new System.Drawing.Font(c.Font, makeBold ? FontStyle.Bold : FontStyle.Regular);
        }
        public static void MakeFontItalic(this Control c, bool makeItalic = true)
        {
            c.Font = new Font(c.Font, makeItalic ? FontStyle.Italic : FontStyle.Regular);
        }
        public static void MakeFontUnderlined(this Control c, bool makeUnderlined = true)
        {
            c.Font = new Font(c.Font, makeUnderlined ? FontStyle.Underline : FontStyle.Regular);
        }
        public static void MakeFontRegular(this Control c)
        {
            MakeFontBold(c, false);
        }
    }
}
