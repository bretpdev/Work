using System;
using System.Windows.Forms;
using Uheaa.Common;

namespace CONPMTPST
{
    public partial class LoanSequence : Form
    {
        public int LoanSequenceNumber { get; set; }

        public LoanSequence(BorrowerData bor)
        {
            InitializeComponent();

            BorrowerName.Text = bor.FirstName + " " + bor.LastName;
            PayoffAmount.Text = string.Format("{0:C}", bor.PayoffAmount);
            DisbursementDate.Text = bor.FirstDisbursement.ToShortDateString();
            LoanType.Text = bor.LoanType;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            LoanSequenceNumber = LoanSeqManual.Text.ToInt();
            if (LoanSequenceNumber > 0)
                this.DialogResult = DialogResult.OK;
        }

        private void LoanSeqManual_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                Ok_Click(sender, e);
        }

        private void LoanSequence_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LoanSeqManual.Text.IsNullOrEmpty())
                e.Cancel = true;
        }
    }
}