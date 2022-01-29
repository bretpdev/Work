using System.Windows.Forms;
using Q;

namespace DocIdCornerStone
{
	partial class Login : FormBase
	{
		public Login(SessionCredentials credentials)
		{
			InitializeComponent();
			sessionCredentialsBindingSource.DataSource = credentials;
		}

		private void txtUserId_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{ this.DialogResult = DialogResult.OK; }
			else if (e.KeyCode == Keys.Escape)
			{ this.DialogResult = DialogResult.Cancel; }
		}

		private void txtPassword_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{ this.DialogResult = DialogResult.OK; }
			else if (e.KeyCode == Keys.Escape)
			{ this.DialogResult = DialogResult.Cancel; }
		}
	}//class
}//namespace
