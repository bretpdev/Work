using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class Modeling : BaseMainTabUserControl
	{
		private readonly DataAccess _dataAccess;

		public Modeling()
			: base()
		{
			InitializeComponent();
		}

		public Modeling(bool testMode)
			: base(testMode)
		{
			InitializeComponent();
			_dataAccess = new DataAccess(testMode);
			List<User> users = _dataAccess.GetCurrentEmployees().ToList();
			users.Insert(0, new User() { Name = DataAccess.COMBOBOX_DEFAULT_SELECTION });
			cmbUserToChangeInModeling.DataSource = users;
			cmbUserToChangeInModeling.ValueMember = "SqlUserId";
			cmbUserToChangeInModeling.DisplayMember = "Name";
			users = _dataAccess.GetUsersWithKeys().ToList();
			users.Insert(0, new User() { Name = DataAccess.COMBOBOX_DEFAULT_SELECTION });
			cmbUserToModelAfter.DataSource = users;
			cmbUserToModelAfter.ValueMember = "SqlUserId";
			cmbUserToModelAfter.DisplayMember = "Name";
		}

		private void btnPerformModeling_Click(object sender, EventArgs e)
		{
			if (dgvChangesToBeMade.RowCount == 0)
			{
				MessageBox.Show("You must select users that result in changes.  Please try again.");
			}
			else
			{
				IEnumerable<ModelAfterKey> keyChanges = dgvChangesToBeMade.DataSource as IEnumerable<ModelAfterKey>;
				Modeler.DoModeling(_testMode, keyChanges, (int)cmbUserToChangeInModeling.SelectedValue, AccessUI.SqlUserId);
				MessageBox.Show("Modeling Complete", "Modeling Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
				cmbUserToChangeInModeling.SelectedIndex = 0;
				cmbUserToModelAfter.SelectedIndex = 0;
			}
		}

		private void cmbUserToModelAfter_SelectedIndexChanged(object sender, EventArgs e)
		{
			dgvModelsAccess.DataSource = null;
			dgvChangesToBeMade.DataSource = null;
			User selectedUser = cmbUserToModelAfter.SelectedItem as User;
			if (selectedUser != null && selectedUser.SqlUserId != 0)
			{
				IEnumerable<UserAccessKey> userAccessKeys = _dataAccess.GatherExistingKeysForSystemUserCombo("NO_SYSTEM", (int)cmbUserToModelAfter.SelectedValue);
				dgvModelsAccess.DataSource = userAccessKeys;
				//if user to change is already populated then change detail accordingly
				if (cmbUserToChangeInModeling.SelectedValue != null && (int)cmbUserToChangeInModeling.SelectedValue != 0)
				{
					IEnumerable<ModelAfterKey> userAccessKeys2 = _dataAccess.FindKeyDifferencesBetweenUsers((int)cmbUserToModelAfter.SelectedValue, (int)cmbUserToChangeInModeling.SelectedValue);
					dgvChangesToBeMade.DataSource = userAccessKeys2;
				}
			}//if
		}

		private void cmbUserChangedInModeling_SelectedIndexChanged(object sender, EventArgs e)
		{
			dgvChangesToBeMade.DataSource = null;
			User modelUser = cmbUserToModelAfter.SelectedItem as User;
			User selectedUser = cmbUserToChangeInModeling.SelectedItem as User;
			if (modelUser != null && modelUser.SqlUserId != 0 && selectedUser != null && selectedUser.SqlUserId != 0)
			{
				IEnumerable<ModelAfterKey> userAccessKeys = _dataAccess.FindKeyDifferencesBetweenUsers((int)cmbUserToModelAfter.SelectedValue, (int)cmbUserToChangeInModeling.SelectedValue);
				dgvChangesToBeMade.DataSource = userAccessKeys;
			}
		}
	}//class
}//namespace
