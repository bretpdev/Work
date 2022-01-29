using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class CopyableDataGridView : DataGridView
    {
        public CopyableDataGridView()
        {
            this.MouseClick += CopyableDataGridView_MouseClick;
            GenerateMenu();
        }

        ContextMenu menu;
        private void GenerateMenu()
        {
            menu = new ContextMenu();
            MenuItem copy = new MenuItem("Copy");
            copy.Click += Copy_Click;
            menu.MenuItems.Add(copy);
            MenuItem copyHeaders = new MenuItem("Copy with Headers");
            copyHeaders.Click += CopyHeaders_Click;
            menu.MenuItems.Add(copyHeaders);
        }

        void CopyableDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                menu.Show(this, new Point(e.X, e.Y));
        }
        void Copy_Click(object sender, EventArgs e)
        {
            Copy(DataGridViewClipboardCopyMode.EnableWithoutHeaderText);
        }
        void CopyHeaders_Click(object sender, EventArgs e)
        {
            Copy(DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText);
        }

        private void Copy(DataGridViewClipboardCopyMode mode)
        {
            DataGridViewClipboardCopyMode oldMode = this.ClipboardCopyMode;
            this.ClipboardCopyMode = mode;
            Clipboard.SetDataObject(this.GetClipboardContent());
            this.ClipboardCopyMode = oldMode;
        }

        public override DataObject GetClipboardContent()
        {
            //Temporarily disable row headers to remove the empty column on the left when copying.
            //Doesn't seem like an ideal solution, but there aren't any graphical glitches because of it.
            bool oldValue = this.RowHeadersVisible;
            this.RowHeadersVisible = false;
            var result = base.GetClipboardContent();
            this.RowHeadersVisible = oldValue;
            return result;
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
