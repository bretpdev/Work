namespace INCIDENTRP
{
	partial class AgencyDataInvolvedOption : BaseDataInvolvedOption
	{
		private AgencyDataInvolved _agencyDataInvolved;

		public AgencyDataInvolvedOption(AgencyDataInvolved agencyDataInvolved)
		{
			InitializeComponent();
			_agencyDataInvolved = agencyDataInvolved;
			agencyDataInvolvedBindingSource.DataSource = _agencyDataInvolved;

			if (_agencyDataInvolved.OtherInformationWasReleased)
                chkOther.Checked = true;
		}

		private void chkOther_CheckedChanged(object sender, System.EventArgs e)
		{
			_agencyDataInvolved.OtherInformationWasReleased = chkOther.Checked;
			if (!chkOther.Checked)
                txtOther.Clear();
			txtOther.Enabled = chkOther.Checked;
		}
	}
}
