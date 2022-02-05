using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public static class ComboBoxExtensions
    {
        static Dictionary<ComboBox, bool> isSet = new Dictionary<ComboBox, bool>();
        public static void SetInitialIndex(this ComboBox cb, int index)
        {
            EventHandler visibleChangedHandler = null;
            visibleChangedHandler = delegate
            {
                if (cb.Visible)
                {
                    cb.SelectedIndex = index;
                    cb.VisibleChanged -= visibleChangedHandler; // Only do this once.
                }
            };
            cb.VisibleChanged += visibleChangedHandler;
        }
    }
}
