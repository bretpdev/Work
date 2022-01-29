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
	public partial class frmDelete : Form
	{
		List<UserIdsAndPasswords> _selectedIds;
		List<UserIdsAndPasswords> _idsToDelete;
		public frmDelete(List<UserIdsAndPasswords> selectedIds, List<UserIdsAndPasswords> idsToDelete)
		{
			InitializeComponent();
			userIdsAndPasswordsBindingSource.DataSource = selectedIds;
			userIdsAndPasswordsBindingSource1.DataSource = idsToDelete;
			_selectedIds = selectedIds;
			_idsToDelete = idsToDelete;
			if (!string.IsNullOrEmpty(lboIdsToDelete.Text)) { btnSaveAll.Visible = true; }
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			
			if (txtCurrentPassword.Text != _selectedIds.Where(p => p.UserNameId == lsbSelectedIds.Text).Select(q => q.DecryptedPassword).Single())
			{
				MessageBox.Show("The Current Password you have entered in one or more instance(s) is not correct.  Please re-enter the current password in order to delete the selected User ID.");
				return;
			}

			if (MessageBox.Show(string.Format("Are you sure you want to delete User Id: {0}?", lsbSelectedIds.Text), "DELETE", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				UserIdsAndPasswords lData = new UserIdsAndPasswords() { UserNameId = lsbSelectedIds.Text };
				_idsToDelete.Add(lData);

				foreach (UserIdsAndPasswords item in _selectedIds.Where(p => p.UserNameId.Contains(lsbSelectedIds.Text)))
				{
					_selectedIds.Remove(item);
					break;
				}

				if (_selectedIds.Count < 1)
				{
					btnSaveAll.Visible = true;
					btnAdd.Visible = false;
				}

				DialogResult = DialogResult.OK;
			}
			else
			{
				foreach (UserIdsAndPasswords item in _selectedIds.Where(p => p.UserNameId.Contains(lsbSelectedIds.Text)))
				{
					_selectedIds.Remove(item);
					break;
				}
				
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
