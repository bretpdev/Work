using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace FUTRQUEFED
{
    public partial class QueueInfoEntry : FormBase
    {
        public QueueInfoEntry(QueueInfo queInfo)
        {
            InitializeComponent();
            queueInfoBindingSource.DataSource = queInfo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //validate data
            if (txtAccountNumber.TextLength == 0 || txtRecipientId.TextLength == 0 || txtArc.TextLength == 0)
            {
                MessageBox.Show("Required field is missing. Please update.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (!txtAccountNumber.Text.IsNumeric() || txtAccountNumber.Text.Length != 10)
            {
                MessageBox.Show("The account number has incorrect format.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if ((txtRecipientId.Text.Substring(0, 1).ToUpper() == "P" && txtRecipientId.Text.Length != 9) || !txtRecipientId.Text.Substring(1).IsNumeric())
            {
                MessageBox.Show("The recipient ID has incorrect format.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (txtRecipientId.Text.Substring(0, 1).ToUpper() != "P" && (txtRecipientId.Text.Length != 10 || !txtRecipientId.Text.IsNumeric()))
            {
                MessageBox.Show("The recipient ID has incorrect format.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (DateTime.Parse(dtpArcAddDate.Text) < DateTime.Today)
            {
                MessageBox.Show("The ARC add date has incorrect format.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
