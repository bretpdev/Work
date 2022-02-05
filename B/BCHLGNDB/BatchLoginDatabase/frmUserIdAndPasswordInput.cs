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
	public partial class frmUserIdAndPasswordInput : Form
	{

		List<UserIdsAndPasswords> _selectedIds;
		IList<UserIdsAndPasswords> _userIds;
		Results _userSelection;

		public frmUserIdAndPasswordInput(IList<UserIdsAndPasswords> userIds, List<UserIdsAndPasswords> selectedIds, Results userSelection)
		{
			InitializeComponent();
			_userIds = userIds;
			clbUserIds.DataSource = userIds.Select(p =>p.UserNameId).ToList();
			_selectedIds = selectedIds;
			_userSelection = userSelection;
		}

		private void btnChangeId_Click(object sender, EventArgs e)
		{
			
			foreach (var item in clbUserIds.CheckedItems)
			{
				UserIdsAndPasswords ids = new UserIdsAndPasswords();
				ids.UserNameId = item.ToString();
				ids.Notes = _userIds.Where(p => p.UserNameId.Contains(ids.UserNameId)).Select(q => q.Notes).Single();
				ids.DecryptedPassword = _userIds.Where(p => p.UserNameId.Contains(ids.UserNameId)).Select(q => q.DecryptedPassword).Single();
				_selectedIds.Add(ids);
			}

			_userSelection.Action = 1;
			DialogResult = DialogResult.OK;
		}

		private void btnCreateNewId_Click(object sender, EventArgs e)
		{
			_userSelection.Action = 2;
			DialogResult = DialogResult.OK;
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			foreach (var item in clbUserIds.CheckedItems)
			{
				UserIdsAndPasswords ids = new UserIdsAndPasswords();
				ids.UserNameId = item.ToString();
				ids.DecryptedPassword = _userIds.Where(p => p.UserNameId.Contains(ids.UserNameId)).Select(q => q.DecryptedPassword).Single();
				_selectedIds.Add(ids);
			}

			_userSelection.Action = 3;
			DialogResult = DialogResult.OK;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void clbUserIds_SelectedIndexChanged(object sender, EventArgs e)
		{
			txtNotes.Text = _userIds.Where(p => p.UserNameId.Contains(clbUserIds.SelectedValue.ToString())).Select(q => q.Notes).Single();
		}
	}
}
