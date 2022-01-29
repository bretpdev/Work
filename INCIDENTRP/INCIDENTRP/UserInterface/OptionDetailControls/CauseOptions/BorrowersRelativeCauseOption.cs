namespace INCIDENTRP
{
	partial class BorrowersRelativeCauseOption : BaseCauseOption
	{
		public BorrowersRelativeCauseOption(Incident incident)
		{
			InitializeComponent();
			incidentBindingSource.DataSource = incident;
		}

		private void chkYes_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkYes.Checked)
                chkNo.Checked = false; 
		}

		private void chkNo_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkNo.Checked)
                chkYes.Checked = false;
		}
	}
}
