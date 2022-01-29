namespace INCIDENTRP
{
	partial class AccessControlIncidentType : BaseIncidentType
	{
		private AccessControlIncident _accessControlIncident;

		public AccessControlIncidentType(AccessControlIncident accessControlIncident)
		{
			InitializeComponent();
			_accessControlIncident = accessControlIncident;

			if (_accessControlIncident.ImproperAccessWasGranted)
                chkGranted.Checked = true;
			if (_accessControlIncident.SystemAccessWasNotTerminatedOrModified)
                chkSystem.Checked = true;
			if (_accessControlIncident.PhysicalAccessWasNotTerminatedOrModified)
                chkPhysical.Checked = true;
		}

		private void chkGranted_CheckedChanged(object sender, System.EventArgs e)
		{
			_accessControlIncident.ImproperAccessWasGranted = chkGranted.Checked;
			if (chkGranted.Checked)
			{
				chkPhysical.Checked = false;
				chkSystem.Checked = false;
			}
		}

		private void chkSystem_CheckedChanged(object sender, System.EventArgs e)
		{
			_accessControlIncident.SystemAccessWasNotTerminatedOrModified = chkSystem.Checked;
			if (chkSystem.Checked)
			{
				chkGranted.Checked = false;
				chkPhysical.Checked = false;
			}
		}

		private void chkPhysical_CheckedChanged(object sender, System.EventArgs e)
		{
			_accessControlIncident.PhysicalAccessWasNotTerminatedOrModified = chkPhysical.Checked;
			if (chkPhysical.Checked)
			{
				chkGranted.Checked = false;
				chkSystem.Checked = false;
			}
		}
	}
}
