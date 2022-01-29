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
	public partial class frmChange : Form
	{
		List<UserIdsAndPasswords> _selectedIds;
		List<UserIdsAndPasswords> _changedIds;
		public frmChange(List<UserIdsAndPasswords> selectedIds, List<UserIdsAndPasswords> changedPasswords)
		{
			InitializeComponent();
			_selectedIds = selectedIds;
			_changedIds = changedPasswords;
			userIdsAndPasswordsBindingSource.DataSource = _selectedIds;
			userIdsAndPasswordsBindingSource1.DataSource = _changedIds;
			if (!string.IsNullOrEmpty(lboChangedIds.Text)) { btnSaveAll.Visible = true; }
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			bool allDataIsValid = true;
			string temp = _selectedIds.Where(p => p.UserNameId == lboSelectedIds.Text).Select(q => q.DecryptedPassword).Single();

			if (txtCurrentPassword.Text != _selectedIds.Where(p => p.UserNameId == lboSelectedIds.Text).Select(q => q.DecryptedPassword).Single())
			{
				allDataIsValid = false;
				MessageBox.Show("The Current Password you have entered in one or more instance(s) is not correct.  Please try again.");
			}
			if (txtNewPassword.Text != txtConfirmPassword.Text)
			{
				allDataIsValid = false;
				MessageBox.Show("The New Password and Confirm Password fields do not match.  Please re-enter this information.");
			}
			if (_selectedIds.Where(p => p.UserNameId == lboSelectedIds.Text).Select(q => q.DecryptedPassword).SingleOrDefault() == txtNewPassword.Text)
			{
				allDataIsValid = false;
				MessageBox.Show("The New Password is the same as the Current Password.  Please re-enter this information.");
			}
			if (string.IsNullOrEmpty(txtNotes.Text))
			{
				allDataIsValid = false;
				MessageBox.Show("The Notes is a required field.  Please enter this information.");
			}
			if (allDataIsValid)
			{
				UserIdsAndPasswords iData = new UserIdsAndPasswords() { UserNameId = lboSelectedIds.Text, DecryptedPassword = _selectedIds.Where(p => p.UserNameId == lboSelectedIds.SelectedItem.ToString()).Select(q => q.DecryptedPassword).ToString(), CurrentPassword = txtCurrentPassword.Text, ConfirmPassword = txtConfirmPassword.Text, NewPassword = txtNewPassword.Text, Notes = txtNotes.Text };
				_changedIds.Add(iData);

				foreach (UserIdsAndPasswords item in _selectedIds.Where(p => p.UserNameId.Contains(lboSelectedIds.Text)))
				{
					_selectedIds.Remove(item);
					break;
				}
				DialogResult = DialogResult.OK;
			}
		}

		private void btnSaveAll_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Yes;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
