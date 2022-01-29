using System;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class frmUpdateRole : Form
	{
		Role _role;
		DataAccess _da;

		public frmUpdateRole(DataAccess da, Role role)
		{
			InitializeComponent();
			_role = role;
			_da = da;

			lblRoleChange.Text = lblRoleChange.Text.Replace("@@@@", role.RoleName);
		}

		private void btnSubmitRole_Click(object sender, EventArgs e)
		{
			if (_da.UpdateRole(_role, txtRoleName.Text))
			{
				DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.Cancel;
			}
		}

		private void txtRoleName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btnSubmitRole_Click(sender, new EventArgs());
			}
		}
	}
}
