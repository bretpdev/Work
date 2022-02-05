using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class ApplicationRoot : UserControl
	{
		private readonly DataAccess _dataAccess;
		private readonly bool _testMode;

		public ApplicationRoot()
		{
			InitializeComponent();
		}

		public ApplicationRoot(bool testMode, string application)
		{
			InitializeComponent();
			_dataAccess = new DataAccess(testMode);
			_testMode = testMode;
			pnlAccessKey.Size = new Size(pnlAccessKey.Width, 0);
			pnlAccessKey.MinimumSize = new Size(pnlAccessKey.Width, 0);
			pnlAccessKey.MaximumSize = new Size(pnlAccessKey.Width, 0);
			this.Size = new Size(this.Width, 0);
			this.MinimumSize = new Size(this.Width, 0);
			this.MaximumSize = new Size(this.Width, 0);
			lblApplication.Text = application;
			pnlAccessKey.Visible = false;
		}

		private void btnExpand_Click(object sender, EventArgs e)
		{
			if (btnExpand.Text == "-")
			{
				pnlAccessKey.Controls.Clear();
				pnlAccessKey.Visible = false;
				btnExpand.Text = "+";
			}
			else
			{
				FillAccessKeyPanel();
				pnlAccessKey.Visible = true;
				btnExpand.Text = "-";
			}
		}

		private void FillAccessKeyPanel()
		{
			pnlAccessKey.Controls.Clear();
			IEnumerable<Key> keys = _dataAccess.GatherExistingKeysForApplication(lblApplication.Text);
			foreach (Key key in keys)
			{
				pnlAccessKey.Controls.Add(new AccessKeySubRoot(_testMode, key.Name, key.Application, key.Description));
			}
		}
	}//class
}//namespace
