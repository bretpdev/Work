using System;
using System.Windows.Forms;

namespace BARCODEFED
{
	partial class PersonTypeDialog : Form
	{
		private readonly BarcodeInfo _barcodeInfo;

		public PersonTypeDialog(BarcodeInfo barcodeInfo)
		{
			InitializeComponent();
			_barcodeInfo = barcodeInfo;
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			if (radBorrower.Checked)
			{ _barcodeInfo.PersonType = BarcodeScanner.PersonType.Borrower; }
			else if (radEndorser.Checked)
			{ _barcodeInfo.PersonType = BarcodeScanner.PersonType.Endorser; }
			else if (radReference.Checked)
			{ _barcodeInfo.PersonType = BarcodeScanner.PersonType.Reference; }
			DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}