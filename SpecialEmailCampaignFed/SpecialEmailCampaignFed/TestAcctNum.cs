using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SpecialEmailCampaignFed
{
	public partial class TestAcctNum : Form
	{
		BorrowerDetails _acctNum;
		public TestAcctNum(BorrowerDetails acctNum)
		{
			_acctNum = acctNum;
			InitializeComponent();
			borrowerDetailsBindingSource.DataSource = _acctNum;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
