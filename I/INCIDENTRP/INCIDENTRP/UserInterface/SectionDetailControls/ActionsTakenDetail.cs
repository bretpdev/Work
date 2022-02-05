using System;
using System.Collections.Generic;
using System.Linq;

namespace INCIDENTRP
{
	partial class ActionsTakenDetail : BaseDetail
	{
		private List<ActionTaken> _actionsTaken;

		public ActionsTakenDetail(List<ActionTaken> actionsTaken, List<string> membersOfComputerServicesAndInfoSecurity)
		{
			InitializeComponent();
			_actionsTaken = actionsTaken;
			List<string> itIsoSource = new List<string>(membersOfComputerServicesAndInfoSecurity);
			itIsoSource.Sort();
			itIsoSource.Insert(0, "");
			cmbItIsoPersonContacted.DataSource = itIsoSource;
			contactedInfoTechBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE);
			contactedLawEnforcementBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.CONTACTED_LAW_ENFORCEMENT);
			notifiedAffectedIndividualBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.NOTIFIED_AFFECTED_INDIVIDUAL);
			askedCallerToReturnCorrespondenceBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.ASKED_CALLER_TO_RETURN_CORRESPONDENCE);
			deletedFilesBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.DELETED_FILES);
			correctedDataBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.CORRECTED_DATA);
			removedSystemFromNetworkBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.REMOVED_SYSTEM_FROM_NETWORK);
			rebootedSystemBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.REBOOTED_SYSTEM);
			loggedOffSystemBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.LOGGED_OFF_SYSTEM);
			shutDownSystemBindingSource.DataSource = _actionsTaken.Single(p => p.Action == ActionTaken.SHUT_DOWN_SYSTEM);
			ActionsTakenDate.MaxDate = AskedDate.MaxDate = ContactedLawDate.MaxDate = CorrectedDataDate.MaxDate = DeletedFilesDate.MaxDate =
				LoggedOffDate.MaxDate = NotifiedDate.MaxDate = RebootDate.MaxDate = RemovedDate.MaxDate = ShutDownDate.MaxDate = DateTime.Now.Date;
		}

		public override void CheckValidity()
		{
			bool isValid = (_actionsTaken.Where(p => p.ActionWasTaken && p.IsComplete()).Count() > 0);
			if (isValid != _isValidated)
			{
				_isValidated = isValid;
			}
		}

		private void chkContactedInfoTech_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE).ActionWasTaken = chkContactedInfoTech.Checked;
			grpContactedInfoTech.Visible = chkContactedInfoTech.Checked;
			CheckValidity();
		}

		private void chkContactedLawEnforcement_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.CONTACTED_LAW_ENFORCEMENT).ActionWasTaken = chkContactedLawEnforcement.Checked;
			grpContactedLawEnforcement.Visible = chkContactedLawEnforcement.Checked;
			CheckValidity();
		}

		private void chkNotifiedAffectedIndividual_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.NOTIFIED_AFFECTED_INDIVIDUAL).ActionWasTaken = chkNotifiedAffectedIndividual.Checked;
			grpNotifiedAffectedIndividual.Visible = chkNotifiedAffectedIndividual.Checked;
			CheckValidity();
		}

		private void chkAskedCallerToReturnCorrespondence_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.ASKED_CALLER_TO_RETURN_CORRESPONDENCE).ActionWasTaken = chkAskedCallerToReturnCorrespondence.Checked;
			grpAskedCallerToReturnCorrespondence.Visible = chkAskedCallerToReturnCorrespondence.Checked;
			CheckValidity();
		}

		private void chkDeletedFiles_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.DELETED_FILES).ActionWasTaken = chkDeletedFiles.Checked;
			grpDeletedFiles.Visible = chkDeletedFiles.Checked;
			CheckValidity();
		}

		private void chkCorrectedData_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.CORRECTED_DATA).ActionWasTaken = chkCorrectedData.Checked;
			grpCorrectedData.Visible = chkCorrectedData.Checked;
			CheckValidity();
		}

		private void chkRemovedSystemFromNetwork_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.REMOVED_SYSTEM_FROM_NETWORK).ActionWasTaken = chkRemovedSystemFromNetwork.Checked;
			grpRemovedSystemFromNetwork.Visible = chkRemovedSystemFromNetwork.Checked;
			CheckValidity();
		}

		private void chkRebootedSystem_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.REBOOTED_SYSTEM).ActionWasTaken = chkRebootedSystem.Checked;
			grpRebootedSystem.Visible = chkRebootedSystem.Checked;
			CheckValidity();
		}

		private void chkLoggedOffSystem_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.LOGGED_OFF_SYSTEM).ActionWasTaken = chkLoggedOffSystem.Checked;
			grpLoggedOffSystem.Visible = chkLoggedOffSystem.Checked;
			CheckValidity();
		}

		private void chkShutDownSystem_CheckedChanged(object sender, System.EventArgs e)
		{
			_actionsTaken.Single(p => p.Action == ActionTaken.SHUT_DOWN_SYSTEM).ActionWasTaken = chkShutDownSystem.Checked;
			grpShutDownSystem.Visible = chkShutDownSystem.Checked;
			CheckValidity();
		}

		private void cmbItIsoPersonContacted_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			CheckValidity();
		}

		private void textBox3_TextChanged(object sender, System.EventArgs e)
		{
			CheckValidity();
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			CheckValidity();
		}
	}
}
