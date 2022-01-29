namespace INCIDENTRP
{
	partial class ViolationOfAcceptableUseIncidentType : BaseIncidentType
	{
		private ViolationOfAcceptableUseIncident _violationIncident;

		public ViolationOfAcceptableUseIncidentType(ViolationOfAcceptableUseIncident violationIncident)
		{
			InitializeComponent();
			_violationIncident = violationIncident;

			if (_violationIncident.AccessKeycardWasShared)
                chkKeycard.Checked = true; 
			if (_violationIncident.MisuseOfSystemResourcesByValidUser)
                chkSystemResources.Checked = true; 
			if (_violationIncident.UserSystemCredentialsWereShared)
                chkSystemCredentials.Checked = true; 
		}

		private void chkKeycard_CheckedChanged(object sender, System.EventArgs e)
		{
			_violationIncident.AccessKeycardWasShared = chkKeycard.Checked;
			if (chkKeycard.Checked)
			{
				chkSystemCredentials.Checked = false;
				chkSystemResources.Checked = false;
			}
		}

		private void chkSystemResources_CheckedChanged(object sender, System.EventArgs e)
		{
			_violationIncident.MisuseOfSystemResourcesByValidUser = chkSystemResources.Checked;
			if (chkSystemResources.Checked)
			{
				chkKeycard.Checked = false;
				chkSystemCredentials.Checked = false;
			}
		}

		private void chkSystemCredentials_CheckedChanged(object sender, System.EventArgs e)
		{
			_violationIncident.UserSystemCredentialsWereShared = chkSystemCredentials.Checked;
			if (chkSystemCredentials.Checked)
			{
				chkKeycard.Checked = false;
				chkSystemResources.Checked = false;
			}
		}
	}
}
