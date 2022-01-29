using System;

namespace INCIDENTRP
{
	partial class FaxIncidentType : BaseIncidentType
	{
		private FaxIncident _faxIncident;

		public FaxIncidentType(FaxIncident faxIncident)
		{
			InitializeComponent();
			_faxIncident = faxIncident;
			faxIncidentBindingSource.DataSource = _faxIncident;

			if (_faxIncident.IncorrectDocumentsWereFaxed)
                chkIncorrectDocuments.Checked = true;
			if (_faxIncident.FaxNumberWasIncorrect)
                chkIncorrectFaxNumber.Checked = true;
		}

		private void chkIncorrectDocuments_CheckedChanged(object sender, EventArgs e)
		{
			_faxIncident.IncorrectDocumentsWereFaxed = chkIncorrectDocuments.Checked;
			if (chkIncorrectDocuments.Checked)
                chkIncorrectFaxNumber.Checked = false;
		}

		private void chkIncorrectFaxNumber_CheckedChanged(object sender, EventArgs e)
		{
			_faxIncident.FaxNumberWasIncorrect = chkIncorrectFaxNumber.Checked;
			if (chkIncorrectFaxNumber.Checked)
                chkIncorrectDocuments.Checked = false;
		}
	}
}
