namespace INCIDENTRP
{
	partial class OddComputerBehaviorIncidentType : BaseIncidentType
	{
		private OddComputerBehaviorIncident _computerIncident;

		public OddComputerBehaviorIncidentType(OddComputerBehaviorIncident computerIncident)
		{
			InitializeComponent();
			_computerIncident = computerIncident;

			if (_computerIncident.EmailPhishingOrHoax)
                chkPhishing.Checked = true;
			if (_computerIncident.DenialOfService)
                chkDenialOfService.Checked = true; 
			if (_computerIncident.UnexplainedAttemptToWriteToSystemFiles)
                chkSystemFiles.Checked = true; 
			if (_computerIncident.UnexplainedModificationOrDeletionOfDate)
                chkDate.Checked = true; 
			if (_computerIncident.UnexplainedModificationToFileLengthOrDate)
                chkFileLength.Checked = true; 
			if (_computerIncident.UnexplainedNewFilesOrUnfamiliarFileNames)
                chkNewFiles.Checked = true; 
			if (_computerIncident.Malware)
                chkMalware.Checked = true;
		}

		private void chkPhishing_CheckedChanged(object sender, System.EventArgs e)
		{
			_computerIncident.EmailPhishingOrHoax = chkPhishing.Checked;
			if (chkPhishing.Checked)
			{
				chkDate.Checked = false;
				chkDenialOfService.Checked = false;
				chkFileLength.Checked = false;
				chkMalware.Checked = false;
				chkNewFiles.Checked = false;
				chkSystemFiles.Checked = false;
			}
		}

		private void chkDenialOfService_CheckedChanged(object sender, System.EventArgs e)
		{
			_computerIncident.DenialOfService = chkDenialOfService.Checked;
			if (chkDenialOfService.Checked)
			{
				chkDate.Checked = false;
				chkFileLength.Checked = false;
				chkMalware.Checked = false;
				chkNewFiles.Checked = false;
				chkPhishing.Checked = false;
				chkSystemFiles.Checked = false;
			}
		}

		private void chkSystemFiles_CheckedChanged(object sender, System.EventArgs e)
		{
			_computerIncident.UnexplainedAttemptToWriteToSystemFiles = chkSystemFiles.Checked;
			if (chkSystemFiles.Checked)
			{
				chkDate.Checked = false;
				chkDenialOfService.Checked = false;
				chkFileLength.Checked = false;
				chkMalware.Checked = false;
				chkNewFiles.Checked = false;
				chkPhishing.Checked = false;
			}
		}

		private void chkDate_CheckedChanged(object sender, System.EventArgs e)
		{
			_computerIncident.UnexplainedModificationOrDeletionOfDate = chkDate.Checked;
			if (chkDate.Checked)
			{
				chkDenialOfService.Checked = false;
				chkFileLength.Checked = false;
				chkMalware.Checked = false;
				chkNewFiles.Checked = false;
				chkPhishing.Checked = false;
				chkSystemFiles.Checked = false;
			}
		}

		private void chkFileLength_CheckedChanged(object sender, System.EventArgs e)
		{
			_computerIncident.UnexplainedModificationToFileLengthOrDate = chkFileLength.Checked;
			if (chkFileLength.Checked)
			{
				chkDate.Checked = false;
				chkDenialOfService.Checked = false;
				chkMalware.Checked = false;
				chkNewFiles.Checked = false;
				chkPhishing.Checked = false;
				chkSystemFiles.Checked = false;
			}
		}

		private void chkNewFiles_CheckedChanged(object sender, System.EventArgs e)
		{
			_computerIncident.UnexplainedNewFilesOrUnfamiliarFileNames = chkNewFiles.Checked;
			if (chkNewFiles.Checked)
			{
				chkDate.Checked = false;
				chkDenialOfService.Checked = false;
				chkFileLength.Checked = false;
				chkMalware.Checked = false;
				chkPhishing.Checked = false;
				chkSystemFiles.Checked = false;
			}
		}

		private void chkMalware_CheckedChanged(object sender, System.EventArgs e)
		{
			_computerIncident.Malware = chkMalware.Checked;
			if (chkMalware.Checked)
			{
				chkDate.Checked = false;
				chkDenialOfService.Checked = false;
				chkFileLength.Checked = false;
				chkNewFiles.Checked = false;
				chkPhishing.Checked = false;
				chkSystemFiles.Checked = false;
			}
		}
	}
}
