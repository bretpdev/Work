using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace VERFORBUH
{
    public partial class ProcessingSelection : Form
    {
        public bool IsCollectionSuspense { get; set; }
        public ProcessingSelection()
        {
            InitializeComponent();
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (!Verb.Checked && !Collection.Checked)
            {
                Dialog.Error.Ok("You must selection an option.");
                return;
            }

            IsCollectionSuspense = Collection.Checked;
            DialogResult = DialogResult.OK;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (Verb.Focused)
                {
                    Collection.Focus();
                    return true;
                }
                if (Cancel.Focused)
                {
                    Verb.Focus();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
