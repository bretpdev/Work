namespace INCIDENTRP
{
	partial class DisposalOrDestructionIncidentType : BaseIncidentType
	{
		private DisposalOrDestructionIncident _disposalOrDestructionIncident;

		public DisposalOrDestructionIncidentType(DisposalOrDestructionIncident disposalOrDestructionIncident)
		{
			InitializeComponent();
			_disposalOrDestructionIncident = disposalOrDestructionIncident;

			if (_disposalOrDestructionIncident.ElectronicMediaRecordsWereDestroyedInError)
                chkElectronicError.Checked = true;
			if (_disposalOrDestructionIncident.ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod)
                chkElectronicMethod.Checked = true;
			if (_disposalOrDestructionIncident.MicrofilmWithRecordsWasDestroyedInError)
                chkMicrofilmError.Checked = true;
			if (_disposalOrDestructionIncident.MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod)
                chkMicrofilmMethod.Checked = true;
			if (_disposalOrDestructionIncident.PaperRecordsWereDestroyedInError)
                chkPaperError.Checked = true;
			if (_disposalOrDestructionIncident.PaperRecordsWereDestroyedUsingIncorrectMethod)
                chkPaperMethod.Checked = true;
		}

		private void chkElectronicError_CheckedChanged(object sender, System.EventArgs e)
		{
			_disposalOrDestructionIncident.ElectronicMediaRecordsWereDestroyedInError = chkElectronicError.Checked;
			if (chkElectronicError.Checked)
			{
				chkElectronicMethod.Checked = false;
				chkMicrofilmError.Checked = false;
				chkMicrofilmMethod.Checked = false;
				chkPaperError.Checked = false;
				chkPaperMethod.Checked = false;
			}
		}

		private void chkElectronicMethod_CheckedChanged(object sender, System.EventArgs e)
		{
			_disposalOrDestructionIncident.ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod = chkElectronicMethod.Checked;
			if (chkElectronicMethod.Checked)
			{
				chkElectronicError.Checked = false;
				chkMicrofilmError.Checked = false;
				chkMicrofilmMethod.Checked = false;
				chkPaperError.Checked = false;
				chkPaperMethod.Checked = false;
			}
		}

		private void chkMicrofilmError_CheckedChanged(object sender, System.EventArgs e)
		{
			_disposalOrDestructionIncident.MicrofilmWithRecordsWasDestroyedInError = chkMicrofilmError.Checked;
			if (chkMicrofilmError.Checked)
			{
				chkElectronicError.Checked = false;
				chkElectronicMethod.Checked = false;
				chkMicrofilmMethod.Checked = false;
				chkPaperError.Checked = false;
				chkPaperMethod.Checked = false;
			}
		}

		private void chkMicrofilmMethod_CheckedChanged(object sender, System.EventArgs e)
		{
			_disposalOrDestructionIncident.MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod = chkMicrofilmMethod.Checked;
			if (chkMicrofilmMethod.Checked)
			{
				chkElectronicError.Checked = false;
				chkElectronicMethod.Checked = false;
				chkMicrofilmError.Checked = false;
				chkPaperError.Checked = false;
				chkPaperMethod.Checked = false;
			}
		}

		private void chkPaperError_CheckedChanged(object sender, System.EventArgs e)
		{
			_disposalOrDestructionIncident.PaperRecordsWereDestroyedInError = chkPaperError.Checked;
			if (chkPaperError.Checked)
			{
				chkElectronicError.Checked = false;
				chkElectronicMethod.Checked = false;
				chkMicrofilmError.Checked = false;
				chkMicrofilmMethod.Checked = false;
				chkPaperMethod.Checked = false;
			}
		}

		private void chkPaperMethod_CheckedChanged(object sender, System.EventArgs e)
		{
			_disposalOrDestructionIncident.PaperRecordsWereDestroyedUsingIncorrectMethod = chkPaperMethod.Checked;
			if (chkPaperMethod.Checked)
			{
				chkElectronicError.Checked = false;
				chkElectronicMethod.Checked = false;
				chkMicrofilmError.Checked = false;
				chkMicrofilmMethod.Checked = false;
				chkPaperError.Checked = false;
			}
		}
	}
}
