using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace ACDCAccess
{
	partial class AccessKeySubSubRoot : UserControl
	{
		private readonly string _application;
		private readonly BusinessUnit _businessUnit;
		private readonly DataAccess _dataAccess;
		private readonly bool _testMode;

		public AccessKeySubSubRoot()
		{
			InitializeComponent();
		}

		public AccessKeySubSubRoot(bool testMode, BusinessUnit businessUnit, string application, string accessKey, string keyDescription)
		{
			InitializeComponent();
			_application = application;
			_businessUnit = businessUnit;
			_dataAccess = new DataAccess(testMode);
			_testMode = testMode;
			lblAccessKey.Text = accessKey;
			lblKeyDescription.Size = new Size(lblKeyDescription.Width, 0);
			lblKeyDescription.Text = keyDescription;
			pnlUsers.Size = new Size(pnlUsers.Width, 0);
			pnlUsers.MinimumSize = new Size(pnlUsers.Width, 0);
			pnlUsers.MaximumSize = new Size(pnlUsers.Width, 0);
			this.Size = new Size(this.Width, 0);
			this.MinimumSize = new Size(this.Width, 0);
			this.MaximumSize = new Size(this.Width, 0);
			pnlUsers.Visible = false;
			//populate users combo box
			List<User> users = _dataAccess.GetCurrentEmployees().ToList();
			users.Insert(0, new User() { Name = DataAccess.COMBOBOX_DEFAULT_SELECTION });
			cmbUsers.DataSource = users;
			cmbUsers.DisplayMember = "Name";
			cmbUsers.ValueMember = "SqlUserId";
		}

		private void btnAddAccess_Click(object sender, EventArgs e)
		{
			if ((int)cmbUsers.SelectedValue == 0)
			{
				MessageBox.Show("You must select a user to add access to", "User Selection Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				UserAccessKey newKey = new UserAccessKey();
				newKey.UserID = (int)cmbUsers.SelectedValue;
				newKey.BusinessUnit = _businessUnit;
				newKey.Application = _application;
				newKey.Name = lblAccessKey.Text;
				try
				{
					_dataAccess.AddUserAccess(newKey, AccessUI.SqlUserId);
					MessageBox.Show("The defined access has been added.  To see it an updated list re-expand your list.", "Access Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (UserKeyAssignmentAlreadyExistsException ex)
				{
					MessageBox.Show(ex.Message, "Access Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btnExpand_Click(object sender, EventArgs e)
		{
			if (btnExpand.Text == "-")
			{
				pnlUsers.Controls.Clear();
				pnlUsers.Visible = false;
				btnExpand.Text = "+";
			}
			else
			{
				FillSystemsPanel();
				pnlUsers.Visible = true;
				btnExpand.Text = "-";
			}
		}

		private void FillSystemsPanel()
		{
			pnlUsers.Controls.Clear();
			IEnumerable<UserAccessKey> keys = _dataAccess.GetUsersWithSpecifiedAccess(lblAccessKey.Text, _application, _businessUnit.ID);
			foreach (UserAccessKey key in keys)
			{
				pnlUsers.Controls.Add(new UserDetailDisplay(_testMode, key.ID, key.LegalName));
			}
		}
	}//class
}//namespace
