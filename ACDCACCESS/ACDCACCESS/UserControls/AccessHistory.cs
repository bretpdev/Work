using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ACDCAccess
{
	partial class AccessHistory : BaseMainTabUserControl
	{
		private readonly DataAccess _dataAccess;

		public AccessHistory()
			: base()
		{
			InitializeComponent();
		}

		public AccessHistory(bool testMode)
			: base(testMode)
		{
			InitializeComponent();
			_dataAccess = new DataAccess(testMode);
			List<User> users = _dataAccess.GetUsersWithKeysAndCurrentEmployees().ToList();
			users.Insert(0, new User() { Name = DataAccess.COMBOBOX_DEFAULT_SELECTION });
			cmbUsers.DataSource = users;
			cmbUsers.DisplayMember = "Name";
			cmbUsers.ValueMember = "SqlUserId";
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			pnlHistory.Controls.Clear();
			if ((int)cmbUsers.SelectedValue != 0)
			{
				IEnumerable<FullAccessRecord> fars = _dataAccess.GetUsersFullAccessRecordHistory(cmbUsers.SelectedValue.ToString());
				bool evenRow = true;
				foreach (FullAccessRecord far in fars)
				{
					pnlHistory.Controls.Add(new AccessHistoryDetail(far));
					//alternate colors
					evenRow = !evenRow;
					if (evenRow) { pnlHistory.Controls[pnlHistory.Controls.Count - 1].BackColor = Color.WhiteSmoke; }
				}
			}
		}
	}//class
}//namespace
