using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Q;

namespace ACDCAccess
{
	partial class AccessKeySubRoot : UserControl
	{
		private readonly string _application;
		private readonly bool _testMode;

		public AccessKeySubRoot()
		{
			InitializeComponent();
		}

		public AccessKeySubRoot(bool testMode, string accessKey, string application, string keyDescription)
		{
			InitializeComponent();
			_application = application;
			_testMode = testMode;
			lblAccessKey.Text = accessKey;
			lblKeyDescription.Size = new Size(lblKeyDescription.Width, 0);
			lblKeyDescription.Text = keyDescription;
			pnlBusinessUnits.Size = new Size(pnlBusinessUnits.Width, 0);
			pnlBusinessUnits.MinimumSize = new Size(pnlBusinessUnits.Width, 0);
			pnlBusinessUnits.MaximumSize = new Size(pnlBusinessUnits.Width, 0);
			this.Size = new Size(this.Width, 0);
			this.MinimumSize = new Size(this.Width, 0);
			this.MaximumSize = new Size(this.Width, 0);
			pnlBusinessUnits.Visible = false;
		}

		private void btnExpand_Click(object sender, EventArgs e)
		{
			if (btnExpand.Text == "-")
			{
				pnlBusinessUnits.Controls.Clear();
				pnlBusinessUnits.Visible = false;
				btnExpand.Text = "+";
			}
			else
			{
				FillBusinessUnitsPanel();
				pnlBusinessUnits.Visible = true;
				btnExpand.Text = "-";
			}
		}

		private void FillBusinessUnitsPanel()
		{
			pnlBusinessUnits.Controls.Clear();
			IEnumerable<BusinessUnit> businessUnits = DataAccess.GetBusinessUnits(_testMode);
			foreach (BusinessUnit bu in businessUnits)
			{
				pnlBusinessUnits.Controls.Add(new BusinessUnitSubSubRoot(_testMode, bu, _application, lblAccessKey.Text));
			}
		}
	}//class
}//namespace
