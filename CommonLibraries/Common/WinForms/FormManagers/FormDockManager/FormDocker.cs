using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public static class FormDocker
    {
        public static void Dock(Form parent, Form child, FormDockLocations location)
        {
            new FormDockManager(parent, child).Dock(location);
        }
    }
}
