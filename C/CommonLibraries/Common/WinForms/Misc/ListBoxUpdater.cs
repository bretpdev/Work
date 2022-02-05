using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// This class calls BeginUpdate on all ListBoxes on the form, then calls EndUpdate when disposed.
    /// It can be used manually, but is most effective in a using statement.
    /// BeginUpdate and EndUpdate are used for visually smoother updates.
    /// </summary>
    public class ListBoxUpdater : IDisposable
    {
        private Form form;
        public ListBoxUpdater(Form form)
        {
            this.form = form;
            BeginUpdate();
        }
        public void Dispose()
        {
            EndUpdate();
        }
        public void BeginUpdate()
        {
            foreach (var listbox in AllListBoxes)
                listbox.BeginUpdate();

        }
        public void EndUpdate()
        {
            foreach (var listbox in AllListBoxes)
                listbox.EndUpdate();
        }
        private IEnumerable<ListBox> AllListBoxes { get { return form.Controls.Cast<Control>().Recurse(o => o.Controls.Cast<Control>()).Filter<ListBox>(); } }
    }
}
