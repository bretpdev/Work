using System;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace ADDMOREFED
{
    public partial class PromptForAccountNumber : Form
    {
        public string AccountNumber { get; set; }
        public string ReferenceId { get; set; }

        public PromptForAccountNumber()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Starts reference add program if accountnumber/referenceId are valid
		/// </summary>
        private void btnContinue_Click(object sender, EventArgs e)
        {
            //verify all digits of the account number were entered
			if (accountNumberTextBox.Text != string.Empty && accountNumberTextBox.Text.Length != 10)
            {
                MessageBox.Show("Please input a valid Borrower Acct # in order for the Add or Modify Reference Script to continue.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //verify all digits of the reference ID were entered
			if (referenceIdTextBox.Text != string.Empty && referenceIdTextBox.Text.Length != 8)
            {
                MessageBox.Show("Please input a valid Reference ID in order for the Add or Modify Reference Script to continue.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			if (accountNumberTextBox.Text.Equals(string.Empty) && referenceIdTextBox.Text.Equals(string.Empty))
			{
				MessageBox.Show("No values provided for Borrower Acct # or Reference ID.  Please provide a valid Borrower Acct # or Reference ID to continue.", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
            //pass text box values to properties
            AccountNumber = accountNumberTextBox.Text;
			ReferenceId = referenceIdTextBox.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
