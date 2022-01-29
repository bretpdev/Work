namespace INCIDENTRP
{
	partial class TelephoneIncidentType : BaseIncidentType
	{
		private TelephoneIncident _telephoneIncident;

		public TelephoneIncidentType(TelephoneIncident telephoneIncident)
		{
			InitializeComponent();
			_telephoneIncident = telephoneIncident;
			telephoneIncidentBindingSource.DataSource = telephoneIncident;

			if (_telephoneIncident.RevealedInformationOnVoicemail)
                chkVoicemail.Checked = true;
			if (_telephoneIncident.RevealedInformationToUnauthorizedIndividual)
                chkUnauthorizedIndividual.Checked = true;
		}

		private void chkVoicemail_CheckedChanged(object sender, System.EventArgs e)
		{
			_telephoneIncident.RevealedInformationOnVoicemail = chkVoicemail.Checked;
			if (chkVoicemail.Checked)
                chkUnauthorizedIndividual.Checked = false;
		}

		private void chkUnauthorizedIndividual_CheckedChanged(object sender, System.EventArgs e)
		{
			_telephoneIncident.RevealedInformationToUnauthorizedIndividual = chkUnauthorizedIndividual.Checked;
			if (chkUnauthorizedIndividual.Checked)
                chkVoicemail.Checked = false; 
		}
	}
}
