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
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace DPALETTERS.Forms
{
    public partial class DPACancellation : Form
    {
        public CancellationResponse Response { get; set; } = null;

        public DPACancellation(string ssn = null)
        {
            InitializeComponent();
            if(ssn != null)
            {
                accountIdentifierTextBoxBorrower.Text = ssn;
            }
            if(DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev)
            {
                radioButtonDPACANP.Enabled = true;
                radioButtonDPACANP.Visible = true;
            }
        }

        private bool ValidateInput()
        {
            if (!AccountIdentifierTextBox.ValidateInput(accountIdentifierTextBoxBorrower.Text))
            {
                Dialog.Warning.Ok("The value provided for borrower account identifier is not a valid account number or ssn", "Bad Input");
                return false;
            }
            if(accountIdentifierTextBoxCosigner.Text.Length > 0 && !AccountIdentifierTextBox.ValidateInput(accountIdentifierTextBoxCosigner.Text))
            {
                Dialog.Warning.Ok("The value provided for cosigner account identifier is not a valid account number or ssn", "Bad Input");
                return false;
            }
            if(!radioButtonBankAccountClosed.Checked && !radioButtonBorrowersRequest.Checked && !radioButtonConsolidated.Checked && !radioButtonNSF.Checked && !radioButtonOtherReason.Checked && !radioButtonPIF.Checked && !radioButtonRehabilitated.Checked && !radioButtonStoppedPayments.Checked && !radioButtonDPACANP.Checked)
            {
                Dialog.Warning.Ok("You must select a cancelation reason.", "Bad Input");
                return false;
            }
            if(radioButtonOtherReason.Checked && textBoxOtherReason.Text.Length == 0)
            {
                Dialog.Warning.Ok("You must add a comment to use the Other Reason selection.", "Bad Input");
                return false;
            }

            SetResponse();
            return true;
        }

        public void SetResponse()
        {
            Response = new CancellationResponse()
            {
                BorrowerAccountIdentifier = accountIdentifierTextBoxBorrower.Text,
                CosignerAccountIdentifier = accountIdentifierTextBoxCosigner.Text.Length == 0 ? null : accountIdentifierTextBoxCosigner.Text,
                OtherComment = textBoxOtherReason.Text
            };

            if(radioButtonBankAccountClosed.Checked)
                Response.Reason = CancellationResponse.CancellationReason.BankClosed;
            if(radioButtonBorrowersRequest.Checked)
                Response.Reason = CancellationResponse.CancellationReason.BorrowerRequest;
            if (radioButtonConsolidated.Checked)
                Response.Reason = CancellationResponse.CancellationReason.Consolidation;
            if (radioButtonNSF.Checked)
                Response.Reason = CancellationResponse.CancellationReason.NSF;
            if (radioButtonOtherReason.Checked)
                Response.Reason = CancellationResponse.CancellationReason.OtherReason;
            if(radioButtonPIF.Checked)
                Response.Reason = CancellationResponse.CancellationReason.PIF;
            if (radioButtonRehabilitated.Checked)
                Response.Reason = CancellationResponse.CancellationReason.Rehabilitation;
            if (radioButtonStoppedPayments.Checked)
                Response.Reason = CancellationResponse.CancellationReason.StoppedPayment;
            if(radioButtonDPACANP.Checked)
                Response.Reason = CancellationResponse.CancellationReason.ManualDPACANP;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(ValidateInput())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void radioButtonOtherReason_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButtonOtherReason.Checked)
            {
                textBoxOtherReason.Enabled = true;
            }
            else
            {
                textBoxOtherReason.Text = "";
                textBoxOtherReason.Enabled = false;
            }
        }
    }
}
