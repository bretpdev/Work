using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Q;

namespace ACDCAccess
{
	partial class BusinessUnitRoot : UserControl
	{
		private readonly BusinessUnit _businessUnit;
		private readonly DataAccess _dataAccess;
		private readonly bool _testMode;

		public BusinessUnitRoot()
		{
			InitializeComponent();
		}

		public BusinessUnitRoot(bool testMode, BusinessUnit businessUnit)
		{
			InitializeComponent();
			_businessUnit = businessUnit;
			_dataAccess = new DataAccess(testMode);
			pnlApplications.Size = new Size(pnlApplications.Width, 0);
			pnlApplications.MinimumSize = new Size(pnlApplications.Width, 0);
			pnlApplications.MaximumSize = new Size(pnlApplications.Width, 0);
			this.Size = new Size(this.Width, 0);
			this.MinimumSize = new Size(this.Width, 0);
			this.MaximumSize = new Size(this.Width, 0);
			lblBusinessUnit.Text = _businessUnit.Name;
			_testMode = testMode;
			pnlApplications.Visible = false;
		}

		private void btnExpand_Click(object sender, EventArgs e)
		{
			if (btnExpand.Text == "-")
			{
				pnlApplications.Controls.Clear();
				pnlApplications.Visible = false;
				btnExpand.Text = "+";
			}
			else
			{
				FillApplicationsPanel();
				pnlApplications.Visible = true;
				btnExpand.Text = "-";
			}
		}

		private void FillApplicationsPanel()
		{
			pnlApplications.Controls.Clear();
			IEnumerable<string> applications = _dataAccess.GetApplications();
			foreach (string application in applications)
			{
				pnlApplications.Controls.Add(new ApplicationSubRoot(_testMode, _businessUnit, application));
			}
		}
	}//class
}//namespace
