using System;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class UserDetailDisplay : UserControl
	{
		private readonly DataAccess _dataAccess;
		private readonly long _keyId;
		private readonly bool _testMode;

		public UserDetailDisplay()
		{
			InitializeComponent();
		}

		public UserDetailDisplay(bool testMode, long keyId, string legalName)
		{
			InitializeComponent();
			_dataAccess = new DataAccess(testMode);
			_keyId = keyId;
			_testMode = testMode;
			lblName.Text = legalName;
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			_dataAccess.RemoveUserAccess(_keyId, AccessUI.SqlUserId);
			this.Enabled = false;
		}
	}
}
