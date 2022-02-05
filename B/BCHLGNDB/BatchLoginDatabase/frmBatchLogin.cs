using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BatchLoginDatabase
{
	public partial class frmBatchLogin : Form
	{
		public frmBatchLogin(LoginCredentais lData ,bool testMode)
		{
			InitializeComponent();
			loginCredentaisBindingSource.DataSource = lData;
			if (testMode) { this.Text = "Login (TEST)"; }	
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
