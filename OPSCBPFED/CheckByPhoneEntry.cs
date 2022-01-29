using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common.WinForms;
using Q;

namespace OPSCBPFED
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

            radTotalAmountDue.Text = string.Format("{0} ({1})", radTotalAmountDue.Text, data.AppendToTotalAmountDue);
            radScheduleAmountDue.Text = string.Format("{0} ({1})", radScheduleAmountDue.Text, data.AppendToMonthlyAmountDue);
            dtpEffectiveDate.MinDate = DateTime.Today;
            dtpEffectiveDate.MaxDate = DateTime.Today.AddMonths(1);

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
            if (radScheduleAmountDue.Checked)
                txtPaymentAmount.Text = Data.AppendToMonthlyAmountDue.Replace("$", "");
        }

        private void radEmail_CheckedChanged(object sender, EventArgs e)
        {
            txtEmailAddress.Enabled = radEmail.Checked;
        }

        private void radOtherPaymentAmount_CheckedChanged(object sender, EventArgs e)
        {
            txtPaymentAmount.Enabled = radOtherPaymentAmount.Checked;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (radTotalAmountDue.Checked)
                Data.PayOpt = OPSEntry.PaymentOption.TotalAmountDue;
            else if (radScheduleAmountDue.Checked)
                Data.PayOpt = OPSEntry.PaymentOption.MonthlyAmountDue;
            else if (radOtherPaymentAmount.Checked)
                Data.PayOpt = OPSEntry.PaymentOption.OtherPaymentAmount;
            else
            {
                MessageBox.Show("You must provide a payment option.", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.DialogResult = DialogResult.None;
            }

            //confirmation option
            if (radLetter.Checked)
                Data.ConfOpt = OPSEntry.ConfirmationOptions.Letter;
            else if (radNone.Checked)
                Data.ConfOpt = OPSEntry.ConfirmationOptions.None;
            else if (radEmail.Checked)
                Data.ConfOpt = OPSEntry.ConfirmationOptions.Email;
            else
            {
                MessageBox.Show("You must select a confirmation option.", "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
            Data.EffectiveDate = dtpEffectiveDate.Value.ToString("MM/dd/yyyy");

            if (this.DialogResult != DialogResult.None)
            {
                OPSEntry.ValidityCheckResult result = Data.ValidDataCheck(); //do general data checks

                if (result == OPSEntry.ValidityCheckResult.Valid)
                    this.DialogResult = DialogResult.OK;
                else if (result == OPSEntry.ValidityCheckResult.InvalidSetFocusToEffectiveDate)
                {
                    dtpEffectiveDate.Focus();
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