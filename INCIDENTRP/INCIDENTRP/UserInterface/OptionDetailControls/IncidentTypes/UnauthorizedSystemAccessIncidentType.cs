namespace INCIDENTRP
{
	partial class UnauthorizedSystemAccessIncidentType : BaseIncidentType
	{
		private UnauthorizedSystemAccessIncident _systemAccessIncident;

		public UnauthorizedSystemAccessIncidentType(UnauthorizedSystemAccessIncident systemAccessIncident)
		{
			InitializeComponent();
			_systemAccessIncident = systemAccessIncident;

			if (_systemAccessIncident.SuspiciousEntryInSystemOrNetworkLog)
                chkSystemLogs.Checked = true;
			if (_systemAccessIncident.SystemAccountDiscrepancy)
                chkAccounting.Checked = true;
			if (_systemAccessIncident.UnauthorizedUseOfUserCredentials)
                chkCredentials.Checked = true;
			if (_systemAccessIncident.UnusualTimeOfUsage)
                chkUnusualTime.Checked = true;
			if (_systemAccessIncident.UnexplainedNewUserAccount)
                chkNewAccounts.Checked = true;
		}

		private void chkSystemLogs_CheckedChanged(object sender, System.EventArgs e)
		{
			_systemAccessIncident.SuspiciousEntryInSystemOrNetworkLog = chkSystemLogs.Checked;
			if (chkSystemLogs.Checked)
			{
				chkAccounting.Checked = false;
				chkNewAccounts.Checked = false;
				chkCredentials.Checked = false;
				chkUnusualTime.Checked = false;
			}
		}

		private void chkAccounting_CheckedChanged(object sender, System.EventArgs e)
		{
			_systemAccessIncident.SystemAccountDiscrepancy = chkAccounting.Checked;
			if (chkAccounting.Checked)
			{
				chkNewAccounts.Checked = false;
				chkCredentials.Checked = false;
				chkSystemLogs.Checked = false;
				chkUnusualTime.Checked = false;
			}
		}

		private void chkCredentials_CheckedChanged(object sender, System.EventArgs e)
		{
			_systemAccessIncident.UnauthorizedUseOfUserCredentials = chkCredentials.Checked;
			if (chkCredentials.Checked)
			{
				chkAccounting.Checked = false;
				chkNewAccounts.Checked = false;
				chkSystemLogs.Checked = false;
				chkUnusualTime.Checked = false;
			}
		}

		private void chkUnusualTime_CheckedChanged(object sender, System.EventArgs e)
		{
			_systemAccessIncident.UnusualTimeOfUsage = chkUnusualTime.Checked;
			if (chkUnusualTime.Checked)
			{
				chkAccounting.Checked = false;
				chkNewAccounts.Checked = false;
				chkCredentials.Checked = false;
				chkSystemLogs.Checked = false;
			}
		}

		private void chkNewAccounts_CheckedChanged(object sender, System.EventArgs e)
		{
			_systemAccessIncident.UnexplainedNewUserAccount = chkNewAccounts.Checked;
			if (chkNewAccounts.Checked)
			{
				chkAccounting.Checked = false;
				chkCredentials.Checked = false;
				chkSystemLogs.Checked = false;
				chkUnusualTime.Checked = false;
			}
		}
	}
}
