using System;
using System.Windows.Forms;

namespace OPSCHKPHN
{
	public partial class CheckByPhoneEntry : Form
	{

		private OPSEntry Data;

		/// <summary>
		/// Default constructor (do not use)
		/// </summary>
		public CheckByPhoneEntry()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CheckByPhoneEntry(OPSEntry data)
		{
			InitializeComponent();
			//Called by DUDE
			radTotalAmountDue.Text = string.Format("{0} ({1})", radTotalAmountDue.Text, data.AppendToTotalAmountDue);
			radMonthlyAmountDue.Text = string.Format("{0} ({1})", radMonthlyAmountDue.Text, data.AppendToMonthlyAmountDue);
			Data = data;
			oPSEntryBindingSource.DataSource = Data;
		}

		private void radTotalAmountDue_CheckedChanged(object sender, EventArgs e)
		{
			if (radTotalAmountDue.Checked)
				txtPaymentAmount.Text = Data.AppendToTotalAmountDue.Replace("$", "");
		}

		private void radMonthlyAmountDue_CheckedChanged(object sender, EventArgs e)
		{
			if (radMonthlyAmountDue.Checked)
				txtPaymentAmount.Text = Data.AppendToMonthlyAmountDue.Replace("$", "");
		}

		private void radOtherPaymentAmount_CheckedChanged(object sender, EventArgs e)
		{
            txtPaymentAmount.Enabled = radOtherPaymentAmount.Checked;
        }

		private void btnContinue_Click(object sender, EventArgs e)
		{
			//payment options
			if (radTotalAmountDue.Checked)
				Data.PayOpt = OPSEntry.PaymentOption.TotalAmountDue;
			else if (radMonthlyAmountDue.Checked)
				Data.PayOpt = OPSEntry.PaymentOption.MonthlyAmountDue;
			else if (radOtherPaymentAmount.Checked)
				Data.PayOpt = OPSEntry.PaymentOption.OtherPaymentAmount;
			else
			{
				MessageBox.Show("You must provide a payment option.", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSSN.Focus();
                this.DialogResult = DialogResult.None;
            }

			//Account Type
			if (radChecking.Checked)
				Data.AcctType = OPSEntry.AccountType.Checking;
			else if (radSavings.Checked)
				Data.AcctType = OPSEntry.AccountType.Savings;
			else
			{
				MessageBox.Show("You must provide an account type.", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSSN.Focus();
                this.DialogResult = DialogResult.None;
            }

			//confirmation option
			if (radLetter.Checked)
				Data.ConfOpt = OPSEntry.ConfirmationOptions.Letter;
			else if (radNone.Checked)
				Data.ConfOpt = OPSEntry.ConfirmationOptions.None;
			else
			{
				MessageBox.Show("You must select a confirmation option.", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSSN.Focus();
                this.DialogResult = DialogResult.None;
			}

            if (this.DialogResult != DialogResult.None)
            {

                OPSEntry.ValidityCheckResult result = Data.ValidDataCheck(); //do general data checks

                if (result == OPSEntry.ValidityCheckResult.Valid)
                    this.DialogResult = DialogResult.OK;
                else if (result == OPSEntry.ValidityCheckResult.InvalidSetFocusToEffectiveDate)
                {
                    txtEffectiveDate.Focus();
                    txtEffectiveDate.Clear();
                    this.DialogResult = DialogResult.None;
                }
                else if (result == OPSEntry.ValidityCheckResult.InvalidSetFocusToSSN)
                {
                    txtSSN.Focus();
                    this.DialogResult = DialogResult.None;
                }
            }
		}
	}
}
