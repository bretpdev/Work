using System;
using System.Windows.Forms;


namespace ACHSETUPFD
{
	partial class PaymentDueDate : Form
	{
		public PaymentDueDate(NextPaymentDueDate payment)
		{
			InitializeComponent();
			nextPaymentDueDateBindingSource.DataSource = payment;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			try
			{
				DateTime checkIfDate = DateTime.Parse(txtDueDate.Text);
			}
			catch (Exception)
			{
				MessageBox.Show("Please provide a valid date", "Invalid Date Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			this.DialogResult = DialogResult.OK;
		}
	}
}
