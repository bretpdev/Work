using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BatchLoginDatabase
{
	public partial class frmAdd : Form
	{
		IList<UserIdsAndPasswords> _userIdsFromDb;
		List<UserIdsAndPasswords> _updatedUserIdsAndPasswords;

		public frmAdd(List<UserIdsAndPasswords> updatedUserIdsAndPasswords, IList<UserIdsAndPasswords> userIdsFromDb)
		{
			InitializeComponent();
			userIdsAndPasswordsBindingSource.DataSource = updatedUserIdsAndPasswords;
			_userIdsFromDb = userIdsFromDb;
			_updatedUserIdsAndPasswords = updatedUserIdsAndPasswords;
			if (!string.IsNullOrEmpty(lsbidsToCHange.Text)) { btnSaveAll.Visible = true; }
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			bool allDataIsValid = true;

			if (txtPassword.Text != txtConfirmPassword.Text)
			{
				allDataIsValid = false;
				MessageBox.Show("The New and Confirm password fields do no match for the following IDs, please re-enter this information");
			}

			foreach (UserIdsAndPasswords item in _userIdsFromDb.Where(p => p.UserNameId.ToUpper().Contains(txtUserId.Text.ToString().ToUpper())))
			{
				allDataIsValid = false;
				MessageBox.Show("The User ID you have requested to add already exists in the active table");
			}

			if (string.IsNullOrEmpty(txtNotes.Text))
			{
				allDataIsValid = false;
				MessageBox.Show("The Notes is a required field.  Please enter this information");
			}

			if (allDataIsValid)
			{
				UserIdsAndPasswords lData = new UserIdsAndPasswords() { UserNameId = txtUserId.Text, NewPassword = txtPassword.Text, Notes = txtNotes.Text };
				_updatedUserIdsAndPasswords.Add(lData);
				DialogResult = DialogResult.OK;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnSaveAll_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Yes;
		}
	}
}
