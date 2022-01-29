namespace INCIDENTRP
{
	partial class ScansProbesIncidentType : BaseIncidentType
	{
		private ScanOrProbeIncident _scanOrProbeIncident;

		public ScansProbesIncidentType(ScanOrProbeIncident scanOrProbeIncident)
		{
			InitializeComponent();
			_scanOrProbeIncident = scanOrProbeIncident;

			if (_scanOrProbeIncident.UnauthorizedProgramOrSnifferDevice)
                chkSniffer.Checked = true;
			if (_scanOrProbeIncident.PrioritySystemAlarmOrIndicationFromIds)
                chkAlarm.Checked = true;
			if (_scanOrProbeIncident.UnauthorizedPortScan)
                chkPortScan.Checked = true;
			if (_scanOrProbeIncident.UnauthroizedVulnerabilityScan)
                chkVulnerabilityScan.Checked = true;
		}

		private void chkSniffer_CheckedChanged(object sender, System.EventArgs e)
		{
			_scanOrProbeIncident.UnauthorizedProgramOrSnifferDevice = chkSniffer.Checked;
			if (chkSniffer.Checked)
			{
				chkAlarm.Checked = false;
				chkPortScan.Checked = false;
				chkVulnerabilityScan.Checked = false;
			}
		}

		private void chkAlarm_CheckedChanged(object sender, System.EventArgs e)
		{
			_scanOrProbeIncident.PrioritySystemAlarmOrIndicationFromIds = chkAlarm.Checked;
			if (chkAlarm.Checked)
			{
				chkPortScan.Checked = false;
				chkSniffer.Checked = false;
				chkVulnerabilityScan.Checked = false;
			}
		}

		private void chkPortScan_CheckedChanged(object sender, System.EventArgs e)
		{
			_scanOrProbeIncident.UnauthorizedPortScan = chkPortScan.Checked;
			if (chkPortScan.Checked)
			{
				chkAlarm.Checked = false;
				chkSniffer.Checked = false;
				chkVulnerabilityScan.Checked = false;
			}
		}

		private void chkVulnerabilityScan_CheckedChanged(object sender, System.EventArgs e)
		{
			_scanOrProbeIncident.UnauthroizedVulnerabilityScan = chkVulnerabilityScan.Checked;
			if (chkVulnerabilityScan.Checked)
			{
				chkAlarm.Checked = false;
				chkPortScan.Checked = false;
				chkSniffer.Checked = false;
			}
		}
	}
}
