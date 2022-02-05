using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace OPSCBPFED
{
    public partial class Confirmation : Form
    {
        /// <summary>
        /// Default constructor (Do Not Use).
        /// </summary>
        public Confirmation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Confirmation(OPSEntry data)
        {
            InitializeComponent();
            oPSEntryBindingSource.DataSource = data;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (chkReminder1.Checked && chkReminder2.Checked && chkReminder3.Checked)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please notify borrower of all reminders (then check them) and recap the summary information.", "Notify Borrower Of reminders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
            }
        }
    }
}
