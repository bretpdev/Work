using System.Windows.Forms;

namespace CLMPMTPST
{
	partial class PaymentDialog : Form
	{
		private PaymentTotal _total;

		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required for the Windows Forms Designer, but it doesn't work with the script.
		/// </summary>
		public PaymentDialog()
		{
			InitializeComponent();
		}

		public PaymentDialog(PaymentTotal total)
		{
			InitializeComponent();
			UpdateWireLabelVisibility();
			_total = total;
			totalBindingSource.DataSource = _total;
		}

		private void BtnOk_Click(object sender, System.EventArgs e)
		{
			if (!radCash.Checked && !radWire.Checked)
			{
				MessageBox.Show("You must select Cash or Wire to continue.", "Consolidation Payment Posting", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			_total.Type = (radCash.Checked ? PaymentType.Cash : PaymentType.Wire);
			this.DialogResult = DialogResult.OK;
		}

		private void Cash_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateWireLabelVisibility();
		}

		private void Wire_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateWireLabelVisibility();
		}

		private void UpdateWireLabelVisibility()
		{
			lblWire.Visible = radWire.Checked;
		}
	}//class
}//namespace
