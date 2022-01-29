using System.Windows.Forms;
using Q;

namespace RefereneceTelephoneActivityContact
{
	public partial class ReferenceCall : FormBase
	{
		private bool _testMode;

		public ReferenceCall(bool testMode, ContactDetail contactDetail)
		{
			InitializeComponent();

			_testMode = testMode;
			cmbContactResult.Items.Clear();
			cmbContactResult.Items.AddRange(DataAccess.GetContactResults(_testMode).ToArray());
			contactDetailBindingSource.DataSource = contactDetail;
		}

		private void cmbContactResult_TextChanged(object sender, System.EventArgs e)
		{
			if (DataAccess.GetContactResultsForReferences(_testMode).Contains(cmbContactResult.Text))
			{
				//Enable the Reference Result combo box and fill it with applicable options.
				lblReferenceResult.Enabled = true;
				cmbReferenceResult.Enabled = true;
				cmbReferenceResult.Items.Clear();
				cmbReferenceResult.Items.AddRange(DataAccess.GetReferenceResults(_testMode, cmbContactResult.Text).ToArray());
			}
			else
			{
				//Clear and disable the Reference Result combo box.
				cmbReferenceResult.Text = "";
				lblReferenceResult.Enabled = false;
				cmbReferenceResult.Enabled = false;
			}
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			//Make sure the user selected a Reference Result if needed.
			if (cmbReferenceResult.Enabled && cmbReferenceResult.Text.Length == 0)
			{
				string message = string.Format("A Reference Result is required when the Contact Result is \"{0}.\"", cmbContactResult.Text);
				MessageBox.Show(message, "Reference Result Required", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			else
			{
				this.DialogResult = DialogResult.OK;
			}
		}
	}//class
}//namespace
