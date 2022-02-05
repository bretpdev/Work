using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Q;

namespace ACDCAccess
{
	partial class ApplicationSubRoot : UserControl
	{
		private readonly BusinessUnit _businessUnit;
		private readonly DataAccess _dataAccess;
		private readonly bool _testMode;

		public ApplicationSubRoot()
		{
			InitializeComponent();
		}

		public ApplicationSubRoot(bool testMode, BusinessUnit businessUnit, string application)
		{
			InitializeComponent();
			_businessUnit = businessUnit;
			_dataAccess = new DataAccess(testMode);
			_testMode = testMode;
			lblApplication.Text = application;
			pnlAccessKeys.Size = new Size(pnlAccessKeys.Width, 0);
			pnlAccessKeys.MinimumSize = new Size(pnlAccessKeys.Width, 0);
			pnlAccessKeys.MaximumSize = new Size(pnlAccessKeys.Width, 0);
			this.Size = new Size(this.Width, 0);
			this.MinimumSize = new Size(this.Width, 0);
			this.MaximumSize = new Size(this.Width, 0);
			pnlAccessKeys.Visible = false;
		}

		private void btnExpand_Click(object sender, EventArgs e)
		{
			if (btnExpand.Text == "-")
			{
				pnlAccessKeys.Controls.Clear();
				pnlAccessKeys.Visible = false;
				btnExpand.Text = "+";
			}
			else
			{
				FillAccessKeysPanel();
				pnlAccessKeys.Visible = true;
				btnExpand.Text = "-";
			}
		}

		private void FillAccessKeysPanel()
		{
			pnlAccessKeys.Controls.Clear();
			IEnumerable<Key> keys = _dataAccess.GatherExistingKeysForApplication(lblApplication.Text);
			foreach (Key key in keys)
			{
				pnlAccessKeys.Controls.Add(new AccessKeySubSubRoot(_testMode, _businessUnit, lblApplication.Text, key.Name, key.Description));
			}
		}
	}//class
}//namespace
