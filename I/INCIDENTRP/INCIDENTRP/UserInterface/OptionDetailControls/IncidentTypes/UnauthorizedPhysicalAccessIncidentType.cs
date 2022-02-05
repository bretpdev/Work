namespace INCIDENTRP
{
	partial class UnauthorizedPhysicalAccessIncidentType : BaseIncidentType
	{
		private UnauthorizedPhysicalAccessIncident _physicalIncident;

		public UnauthorizedPhysicalAccessIncidentType(UnauthorizedPhysicalAccessIncident physicalIncident)
		{
			InitializeComponent();
			_physicalIncident = physicalIncident;

			if (_physicalIncident.AccessAccountingDiscrepancy)
                chkAccessAccounting.Checked = true; 
			if (_physicalIncident.AccessAccountingDiscrepancy)
                chkAccessAccounting.Checked = true;
			if (_physicalIncident.Piggybacking)
                chkPiggybacking.Checked = true;
			if (_physicalIncident.SuspiciousEntryInAccessLog)
                chkAccessLogs.Checked = true;
			if (_physicalIncident.SuspiciousEntryInVideoLog)
                chkVideoLogs.Checked = true; 
			if (_physicalIncident.UnauthorizedUseOfKeycard)
                chkUseOfKeycard.Checked = true;
			if (_physicalIncident.UnexplainedNewKeycard)
                chkNewKeycard.Checked = true;
			if (_physicalIncident.UnusualTimeOfUsage)
                chkTimeOfUsage.Checked = true;
		}

		private void chkAccessAccounting_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.AccessAccountingDiscrepancy = chkAccessAccounting.Checked;
			if (chkAccessAccounting.Checked)
			{
				chkAccessLogs.Checked = false;
				chkNewKeycard.Checked = false;
				chkPiggybacking.Checked = false;
				chkTimeOfUsage.Checked = false;
				chkTrespassing.Checked = false;
				chkUseOfKeycard.Checked = false;
				chkVideoLogs.Checked = false;
			}
		}

		private void chkTrespassing_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.BuildingBreakIn = chkTrespassing.Checked;
			if (chkTrespassing.Checked)
			{
				chkAccessAccounting.Checked = false;
				chkAccessLogs.Checked = false;
				chkNewKeycard.Checked = false;
				chkPiggybacking.Checked = false;
				chkTimeOfUsage.Checked = false;
				chkUseOfKeycard.Checked = false;
				chkVideoLogs.Checked = false;
			}
		}

		private void chkPiggybacking_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.Piggybacking = chkPiggybacking.Checked;
			if (chkPiggybacking.Checked)
			{
				chkAccessAccounting.Checked = false;
				chkAccessLogs.Checked = false;
				chkNewKeycard.Checked = false;
				chkTimeOfUsage.Checked = false;
				chkTrespassing.Checked = false;
				chkUseOfKeycard.Checked = false;
				chkVideoLogs.Checked = false;
			}
		}

		private void chkAccessLogs_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.SuspiciousEntryInAccessLog = chkAccessLogs.Checked;
			if (chkAccessLogs.Checked)
			{
				chkAccessAccounting.Checked = false;
				chkNewKeycard.Checked = false;
				chkPiggybacking.Checked = false;
				chkTimeOfUsage.Checked = false;
				chkTrespassing.Checked = false;
				chkUseOfKeycard.Checked = false;
				chkVideoLogs.Checked = false;
			}
		}

		private void chkVideoLogs_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.SuspiciousEntryInVideoLog = chkVideoLogs.Checked;
			if (chkVideoLogs.Checked)
			{
				chkAccessAccounting.Checked = false;
				chkAccessLogs.Checked = false;
				chkNewKeycard.Checked = false;
				chkPiggybacking.Checked = false;
				chkTimeOfUsage.Checked = false;
				chkTrespassing.Checked = false;
				chkUseOfKeycard.Checked = false;
			}
		}

		private void chkUseOfKeycard_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.UnauthorizedUseOfKeycard = chkUseOfKeycard.Checked;
			if (chkUseOfKeycard.Checked)
			{
				chkAccessAccounting.Checked = false;
				chkAccessLogs.Checked = false;
				chkNewKeycard.Checked = false;
				chkPiggybacking.Checked = false;
				chkTimeOfUsage.Checked = false;
				chkTrespassing.Checked = false;
				chkVideoLogs.Checked = false;
			}
		}

		private void chkNewKeycard_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.UnexplainedNewKeycard = chkNewKeycard.Checked;
			if (chkNewKeycard.Checked)
			{
				chkAccessAccounting.Checked = false;
				chkAccessLogs.Checked = false;
				chkPiggybacking.Checked = false;
				chkTimeOfUsage.Checked = false;
				chkTrespassing.Checked = false;
				chkUseOfKeycard.Checked = false;
				chkVideoLogs.Checked = false;
			}
		}

		private void chkTimeOfUsage_CheckedChanged(object sender, System.EventArgs e)
		{
			_physicalIncident.UnusualTimeOfUsage = chkTimeOfUsage.Checked;
			if (chkTimeOfUsage.Checked)
			{
				chkAccessAccounting.Checked = false;
				chkAccessLogs.Checked = false;
				chkNewKeycard.Checked = false;
				chkPiggybacking.Checked = false;
				chkTrespassing.Checked = false;
				chkUseOfKeycard.Checked = false;
				chkVideoLogs.Checked = false;
			}
		}
	}
}
