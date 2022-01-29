using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace ACDCAccess
{
	partial class AddAndRemoveAccessUserBased : BaseMainTabUserControl
	{
		private readonly DataAccess _dataAccess;

		public AddAndRemoveAccessUserBased()
			: base()
		{
			InitializeComponent();
		}

		public AddAndRemoveAccessUserBased(bool testMode)
			: base(testMode)
		{
			InitializeComponent();
			_dataAccess = new DataAccess(testMode);
			//load combo boxes
			List<string> applications = _dataAccess.GetApplications().ToList();
			applications.Insert(0, DataAccess.COMBOBOX_DEFAULT_SELECTION);
			cmbApplication.DataSource = applications;
			List<User> users = _dataAccess.GetUsersWithKeysAndCurrentEmployees().ToList();
			users.Insert(0, new User() { Name = DataAccess.COMBOBOX_DEFAULT_SELECTION });
			cmbUsers.DataSource = users;
			cmbUsers.DisplayMember = "Name";
			cmbUsers.ValueMember = "SqlUserId";
			List<BusinessUnit> businessUnits = DataAccess.GetBusinessUnits(_testMode).ToList();
			businessUnits.Insert(0, new BusinessUnit() { Name = DataAccess.COMBOBOX_DEFAULT_SELECTION });
			cmbBusinessUnit.DataSource = businessUnits;
			cmbBusinessUnit.DisplayMember = "Name";
			cmbBusinessUnit.ValueMember = "ID";
		}

		private void GatherAddAndRemoveContents()
		{
			if (cmbApplication.SelectedItem != null && cmbUsers.SelectedValue != null &&
				cmbApplication.SelectedItem.ToString() != DataAccess.COMBOBOX_DEFAULT_SELECTION && (int)cmbUsers.SelectedValue != 0)
			{
				//user and system are selected
				pnlPossibleKeysToAdd.Controls.Clear();
				pnlPossibleKeysToRemove.Controls.Clear();
				//add controls for keys to add
				IEnumerable<Key> keysToAdd = _dataAccess.GatherExistingKeysForApplication(cmbApplication.SelectedItem.ToString());
				bool evenRow = true;
				foreach (Key key in keysToAdd)
				{
					pnlPossibleKeysToAdd.Controls.Add(new UserKeyInfoForAddingAccess(key));
					//alternate colors
					evenRow = !evenRow;
					if (evenRow) { pnlPossibleKeysToAdd.Controls[pnlPossibleKeysToAdd.Controls.Count - 1].BackColor = Color.WhiteSmoke; }
				}
				//add controls for keys to remove
				IEnumerable<UserAccessKey> keysToRemove = _dataAccess.GatherExistingKeysForSystemUserCombo(cmbApplication.SelectedItem.ToString(), (int)cmbUsers.SelectedValue);
				evenRow = true;
				foreach (UserAccessKey key in keysToRemove)
				{
					pnlPossibleKeysToRemove.Controls.Add(new UserKeyInfoForRemovingAccess(key));
					//alternate colors
					evenRow = !evenRow;
					if (evenRow) { pnlPossibleKeysToRemove.Controls[pnlPossibleKeysToRemove.Controls.Count - 1].BackColor = Color.WhiteSmoke; }
				}
			}
			else if (cmbApplication.SelectedItem.ToString() != DataAccess.COMBOBOX_DEFAULT_SELECTION && (int)cmbUsers.SelectedValue == 0)
			{
				//only systems is selected
				pnlPossibleKeysToAdd.Controls.Clear();
				pnlPossibleKeysToRemove.Controls.Clear();
				//add controls for keys to add
				IEnumerable<Key> keysToAdd = _dataAccess.GatherExistingKeysForApplication(cmbApplication.SelectedItem.ToString());
				bool evenRow = true;
				foreach (Key key in keysToAdd)
				{
					pnlPossibleKeysToAdd.Controls.Add(new UserKeyInfoForAddingAccess(key));
					//alternate colors
					evenRow = !evenRow;
					if (evenRow) { pnlPossibleKeysToAdd.Controls[pnlPossibleKeysToAdd.Controls.Count - 1].BackColor = Color.WhiteSmoke; }
				}
			}
			else
			{
				//at least system isn't selected
				pnlPossibleKeysToAdd.Controls.Clear();
				pnlPossibleKeysToRemove.Controls.Clear();
			}
		}

		private void btnAddAndRemoveAccess_Click(object sender, EventArgs e)
		{
			foreach (Control c in pnlPossibleKeysToAdd.Controls)
			{
				UserKeyInfoForAddingAccess adder = c as UserKeyInfoForAddingAccess;
				if (adder != null && adder.Checked) //if selected to be added
				{
					UserAccessKey selectedKey = new UserAccessKey();
					Key k = adder.KeyData;
					selectedKey.Name = k.Name;
					selectedKey.Application = k.Application;
					selectedKey.Type = k.Type;
					selectedKey.Description = k.Description;
					selectedKey.BusinessUnit = (BusinessUnit)cmbBusinessUnit.SelectedItem;
					selectedKey.UserID = (int)cmbUsers.SelectedValue;
					try
					{
						_dataAccess.AddUserAccess(selectedKey, AccessUI.SqlUserId);
					}
					catch (UserKeyAssignmentAlreadyExistsException)
					{
						//do nothing, just don't choke
					}
				}
			}
			foreach (Control c in pnlPossibleKeysToRemove.Controls)
			{
				UserKeyInfoForRemovingAccess remover = c as UserKeyInfoForRemovingAccess;
				if (remover != null && remover.Checked)
				{
					_dataAccess.RemoveUserAccess(remover.KeyData.ID, AccessUI.SqlUserId);
				}
			}
			//clear panels
			cmbBusinessUnit.SelectedIndex = 0;
			cmbUsers.SelectedIndex = 0;
			cmbApplication.SelectedIndex = 0;
			pnlPossibleKeysToAdd.Controls.Clear();
			pnlPossibleKeysToRemove.Controls.Clear();
			MessageBox.Show("The user's access has been updated.", "Access Updated", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			GatherAddAndRemoveContents();
		}
	}//class
}//namespace
