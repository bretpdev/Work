namespace INCIDENTRP
{
	partial class DataEntryIncidentType : BaseIncidentType
	{
		private DataEntryIncident _dataEntryIncident;

		public DataEntryIncidentType(DataEntryIncident dataEntryIncident)
		{
			InitializeComponent();
			_dataEntryIncident = dataEntryIncident;

			if (_dataEntryIncident.IncorrectInformationWasAdded)
                chkAdded.Checked = true;
			if (_dataEntryIncident.InformationWasIncorrectlyDeleted)
                chkDeleted.Checked = true;
			if (_dataEntryIncident.InformationWasIncorrectlyChanged)
                chkChanged.Checked = true;
		}

		private void chkAdded_CheckedChanged(object sender, System.EventArgs e)
		{
			_dataEntryIncident.IncorrectInformationWasAdded = chkAdded.Checked;
			if (chkAdded.Checked)
			{
				chkChanged.Checked = false;
				chkDeleted.Checked = false;
			}
		}

		private void chkDeleted_CheckedChanged(object sender, System.EventArgs e)
		{
			_dataEntryIncident.InformationWasIncorrectlyDeleted = chkDeleted.Checked;
			if (chkDeleted.Checked)
			{
				chkAdded.Checked = false;
				chkChanged.Checked = false;
			}
		}

		private void chkChanged_CheckedChanged(object sender, System.EventArgs e)
		{
			_dataEntryIncident.InformationWasIncorrectlyChanged = chkChanged.Checked;
			if (chkChanged.Checked)
			{
				chkAdded.Checked = false;
				chkDeleted.Checked = false;
			}
		}
	}//class
}//namespace
