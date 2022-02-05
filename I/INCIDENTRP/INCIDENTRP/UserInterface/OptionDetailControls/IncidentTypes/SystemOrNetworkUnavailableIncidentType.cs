namespace INCIDENTRP
{
	partial class SystemOrNetworkUnavailableIncidentType : BaseIncidentType
	{
		private SystemOrNetworkUnavailableIncident _systemIncident;

		public SystemOrNetworkUnavailableIncidentType(SystemOrNetworkUnavailableIncident systemIncident)
		{
			InitializeComponent();
			_systemIncident = systemIncident;

			if (_systemIncident.DenialOrDisruptionOfService)
                chkDisruption.Checked = true;
			if (_systemIncident.UnableToLogIntoAccount)
                chkLogin.Checked = true;
		}

		private void chkDisruption_CheckedChanged(object sender, System.EventArgs e)
		{
			_systemIncident.DenialOrDisruptionOfService = chkDisruption.Checked;
			if (chkDisruption.Checked)
                chkLogin.Checked = false;
		}

		private void chkLogin_CheckedChanged(object sender, System.EventArgs e)
		{
			_systemIncident.UnableToLogIntoAccount = chkLogin.Checked;
			if (chkLogin.Checked)
                chkDisruption.Checked = false;
		}
	}
}
