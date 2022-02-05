using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    partial class IncidentDisplay : Form
    {
        private Ticket IncidentTicket { get; set; }
        private SqlUser User { get; set; }

        //Collections for combo boxes that may or may not be used (because they're on user controls that may or may not be loaded):
        List<string> Regions { get; set; }
        List<string> States { get; set; }
        private DataAccess DA { get; set; }

        public IncidentDisplay()
        {
            InitializeComponent();
        }

        public IncidentDisplay(Ticket ticket, SqlUser user, string nextFlowStep, List<SqlUser> currentEmployees, List<BusinessUnit> businessUnits, List<string> notificationMethods, List<string> notifierTypes, List<string> causes, List<string> dataInvolvedOptions, List<string> incidentTypes, List<string> regions, List<string> states, List<string> relationships, ProcessLogRun logRun)
        {
            InitializeComponent();
            DA = new DataAccess(logRun);
            pbxCompanyLogo.Image = Properties.Resources.UheaaLogo;
            User = user;
            Regions = regions;
            States = states;

            //Populate the combo boxes.
            cmbBusinessUnit.DataSource = businessUnits;
            List<SqlUser> courts = new List<SqlUser>(currentEmployees);
            courts.Insert(0, new SqlUser());
            cmbCourt.DataSource = courts;
            cmbNotificationMethod.DataSource = notificationMethods;
            cmbNotifierType.DataSource = notifierTypes;
            cmbNotifierRelationship.DataSource = relationships;
            cmbReporterName.DataSource = new List<SqlUser>(currentEmployees);
            cmbCause.DataSource = causes;
            cmbDataInvolved.DataSource = dataInvolvedOptions;
            cmbIncidentType.DataSource = incidentTypes;

            BindToNewTicket(ticket, nextFlowStep);
            WatchForActivity(this);
            AskedDate.MaxDate = ContactedDate.MaxDate = ContactedITDate.MaxDate = CorrectedDate.MaxDate = DeletedDate.MaxDate = IncidentDate.MaxDate =
                LoggedOffDate.MaxDate = NotifiedDate.MaxDate = RebootedDate.MaxDate = RemovedDate.MaxDate = ShutDownDate.MaxDate = DateTime.Now.Date;
        }

        /// <summary>
        /// Changes the ticket data in the form to reflect the passed-in ticket.
        /// </summary>
        /// <param name="ticket">The Ticket object to display in the form.</param>
        public void BindToNewTicket(Ticket ticket, string nextFlowStep)
        {
            IncidentTicket = ticket;

            //Set data sources for Ticket members.
            ticketBindingSource.DataSource = IncidentTicket;
            ticketBindingSource.ResetBindings(false); //Call reset binding to update all fields
            incidentBindingSource.DataSource = IncidentTicket.Incident;
            incidentBindingSource.ResetBindings(false); //Call reset binding to update all fields
            notifierBindingSource.DataSource = IncidentTicket.Incident.Notifier;
            notifierBindingSource.ResetBindings(false); //Call reset binding to update all fields
            reporterBindingSource.DataSource = IncidentTicket.Incident.Reporter;
            reporterBindingSource.ResetBindings(false); //Call reset binding to update all fields
            contactedInfoTechBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE);
            contactedInfoTechBindingSource.ResetBindings(false); //Call reset binding to update all fields
            notifiedAffectedIndividualBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.NOTIFIED_AFFECTED_INDIVIDUAL);
            notifiedAffectedIndividualBindingSource.ResetBindings(false); //Call reset binding to update all fields
            askedCallerToReturnCorrespondenceBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.ASKED_CALLER_TO_RETURN_CORRESPONDENCE);
            askedCallerToReturnCorrespondenceBindingSource.ResetBindings(false); //Call reset binding to update all fields
            deletedFilesBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.DELETED_FILES);
            deletedFilesBindingSource.ResetBindings(false); //Call reset binding to update all fields
            correctedDataBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.CORRECTED_DATA);
            correctedDataBindingSource.ResetBindings(false); //Call reset binding to update all fields
            removedSystemBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.REMOVED_SYSTEM_FROM_NETWORK);
            removedSystemBindingSource.ResetBindings(false); //Call reset binding to update all fields
            rebootedSystemBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.REBOOTED_SYSTEM);
            rebootedSystemBindingSource.ResetBindings(false); //Call reset binding to update all fields
            loggedOffSystemBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.LOGGED_OFF_SYSTEM);
            loggedOffSystemBindingSource.ResetBindings(false); //Call reset binding to update all fields
            shutDownSystemBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.SHUT_DOWN_SYSTEM);
            shutDownSystemBindingSource.ResetBindings(false); //Call reset binding to update all fields
            contactedLawEnforcementBindingSource.DataSource = IncidentTicket.Incident.ActionsTaken.Single(p => p.Action == ActionTaken.CONTACTED_LAW_ENFORCEMENT);
            contactedLawEnforcementBindingSource.ResetBindings(false); //Call reset binding to update all fields

            //Push data into controls that either can't be bound or don't seem to want to acknowledge the data source.
            cmbBusinessUnit.Text = IncidentTicket.Incident.Reporter.BusinessUnit.Name;
            cmbCourt.Text = (IncidentTicket.Court == null ? "" : IncidentTicket.Court.ToString());
            cmbNotificationMethod.Text = IncidentTicket.Incident.Notifier.Method;
            cmbNotifierType.Text = IncidentTicket.Incident.Notifier.Type;
            cmbReporterName.Text = IncidentTicket.Incident.Reporter.User.ToString();
            txtReporterEmailAddress.Text = IncidentTicket.Incident.Reporter.User.EmailAddress;
            ChangeDataInvolvedControl(IncidentTicket.Incident.DataInvolved);
            ChangeIncidentTypeControl(IncidentTicket.Incident.Type);
            if (string.IsNullOrEmpty(nextFlowStep))
            {
                lnkNextFlowStep.Text = "";
                lnkNextFlowStep.Visible = false;
            }
            else
            {
                lnkNextFlowStep.Text = nextFlowStep;
                if (nextFlowStep == "Close" && DA.HasIRAccess("IR Close", "Incident Reporting Module", User))
                    lnkNextFlowStep.Visible = true;
            }

            UpdateHistory();

            const int SUBJECT_LENGTH = 100;
            string narrative = ticket.Incident.Narrative ?? "";
            string subject = (narrative.Length > SUBJECT_LENGTH ? narrative.Substring(0, SUBJECT_LENGTH) + "..." : narrative);
            this.Text = $"[{ticket.Priority}] {ticket.Type} {ticket.Number} - {subject} ({ticket.Status})";

            if (User == ticket.LockHolder)
                UnlockForm();
            else
                LockForm(ticket.LockHolder.FirstName + " " + ticket.LockHolder.LastName);
        }

        public void SaveAndQuit()
        {
            OnSaveTicket(new SaveTicketEventArgs(IncidentTicket, this));
            Close();
        }

        private void ChangeDataInvolvedControl(string dataInvolved)
        {
            pnlDataInvolved.Controls.Clear();
            switch (dataInvolved)
            {
                case Incident.AGENCY_DATA:
                    pnlDataInvolved.Controls.Add(new AgencyDataInvolvedOption(IncidentTicket.Incident.AgencyDataInvolved));
                    break;
                case Incident.AGENCY_EMPLOYEE_HR_DATA:
                    pnlDataInvolved.Controls.Add(new AgencyEmployeeHrDataInvolvedOption(IncidentTicket.Incident.AgencyEmployeeDataInvolved, States));
                    break;
                case Incident.BORROWER_DATA:
                    pnlDataInvolved.Controls.Add(new BorrowerDataInvolvedOption(IncidentTicket.Incident.BorrowerDataInvolved, States, Regions));
                    break;
                case Incident.THIRD_PARTY_DATA:
                    pnlDataInvolved.Controls.Add(new ThirdPartyDataInvolvedOption(IncidentTicket.Incident.ThirdPartyDataInvolved, States, Regions));
                    break;
            }
            WhitenText(pnlDataInvolved);
        }

        private void ChangeIncidentTypeControl(string incidentType)
        {
            pnlIncidentTypeDetails.Controls.Clear();
            switch (incidentType)
            {
                case Incident.ACCESS_CONTROL:
                    pnlIncidentTypeDetails.Controls.Add(new AccessControlIncidentType(IncidentTicket.Incident.AccessControlIncident));
                    break;
                case Incident.DATA_ENTRY:
                    pnlIncidentTypeDetails.Controls.Add(new DataEntryIncidentType(IncidentTicket.Incident.DataEntryIncident));
                    break;
                case Incident.DISPOSAL_DESTRUCTION:
                    pnlIncidentTypeDetails.Controls.Add(new DisposalOrDestructionIncidentType(IncidentTicket.Incident.DisposalOrDestructionIncident));
                    break;
                case Incident.ELECTRONIC_MAIL_DELIVERY:
                    pnlIncidentTypeDetails.Controls.Add(new ElectronicMailDeliveryIncidentType(IncidentTicket.Incident.ElectronicMailDeliveryIncident));
                    break;
                case Incident.FAX:
                    pnlIncidentTypeDetails.Controls.Add(new FaxIncidentType(IncidentTicket.Incident.FaxIncident));
                    break;
                case Incident.ODD_COMPUTER_BEHAVIOR:
                    pnlIncidentTypeDetails.Controls.Add(new OddComputerBehaviorIncidentType(IncidentTicket.Incident.OddComputerBehaviorIncident));
                    break;
                case Incident.PHYSICAL_DAMAGE_LOSS_THEFT:
                    pnlIncidentTypeDetails.Controls.Add(new PhysicalDamageIncidentType(IncidentTicket.Incident.PhysicalDamageLossOrTheftIncident));
                    break;
                case Incident.REGULAR_MAIL_DELIVERY:
                    pnlIncidentTypeDetails.Controls.Add(new RegularMailDeliveryIncidentType(IncidentTicket.Incident.RegularMailDeliveryIncident));
                    break;
                case Incident.SCANS_PROBES:
                    pnlIncidentTypeDetails.Controls.Add(new ScansProbesIncidentType(IncidentTicket.Incident.ScanOrProbeIncident));
                    break;
                case Incident.SYSTEM_OR_NETWORK_UNAVAILABLE:
                    pnlIncidentTypeDetails.Controls.Add(new SystemOrNetworkUnavailableIncidentType(IncidentTicket.Incident.SystemOrNetworkUnavailableIncident));
                    break;
                case Incident.TELEPHONE:
                    pnlIncidentTypeDetails.Controls.Add(new TelephoneIncidentType(IncidentTicket.Incident.TelephoneIncident));
                    break;
                case Incident.UNAUTHORIZED_PHYSICAL_ACCESS:
                    pnlIncidentTypeDetails.Controls.Add(new UnauthorizedPhysicalAccessIncidentType(IncidentTicket.Incident.UnauthorizedPhysicalAccessIncident));
                    break;
                case Incident.UNAUTHORIZED_SYSTEM_ACCESS:
                    pnlIncidentTypeDetails.Controls.Add(new UnauthorizedSystemAccessIncidentType(IncidentTicket.Incident.UnauthorizedSystemAccessIncident));
                    break;
                case Incident.VIOLATION_OF_ACCEPTABLE_USE:
                    pnlIncidentTypeDetails.Controls.Add(new ViolationOfAcceptableUseIncidentType(IncidentTicket.Incident.ViolationOfAcceptableUseIncident));
                    break;
            }
            WhitenText(pnlIncidentTypeDetails);
        }

        private void LockForm(string lockHolderName)
        {
            this.Text += $" - Locked by {lockHolderName}";
            this.Enabled = false;
            string lockMessage = $"This ticket is locked by {lockHolderName}.";
            Dialog.Info.Ok(lockMessage, "Incident Reporting");
        }

        private void UnlockForm()
        {
            this.Enabled = true;
        }

        private void UpdateHistory()
        {
            txtUpdate.Clear();
            txtHistory.Clear();
            txtHistory.AppendText(IncidentTicket.History.AsString());
            //Scroll to the top of the text box to make the most recent updates visible.
            txtHistory.Select(0, 0);
            txtHistory.ScrollToCaret();
        }

        //Recursive function that forwards Click and KeyPress events for all controls to the OnActivityDetected() method.
        private void WatchForActivity(Control control)
        {
            control.Click += new EventHandler((object sender, EventArgs e) => OnActivityDetected(e));
            control.KeyPress += new KeyPressEventHandler((object sender, KeyPressEventArgs e) => OnActivityDetected(e));
            foreach (Control childControl in control.Controls)
                WatchForActivity(childControl);
        }

        /// <summary>
        /// Changes the ForeColor of a control and its child controls (recursively) to white.
        /// It only takes effect for CheckBox, Label, Panel, and RadioButton controls.
        /// </summary>
        /// <param name="control">The outer-most control from which to start.</param>
        /// <remarks>
        /// This is handy for re-using DataInvolvedOptions and IncidentTypes controls, which already have data-binding set up.
        /// </remarks>
        private void WhitenText(Control control)
        {
            if (control is CheckBox checkBox)
                checkBox.ForeColor = Color.White;

            if (control is Label label)
                label.ForeColor = Color.White;

            if (control is Panel panel)
                panel.ForeColor = Color.White;

            if (control is RadioButton radioButton)
                radioButton.ForeColor = Color.White;

            foreach (Control childControl in control.Controls)
                WhitenText(childControl);
        }

        private void ChkContactedInfoTech_CheckedChanged(object sender, EventArgs e)
        {
            grpContactedInfoTech.Visible = chkContactedInfoTech.Checked;
        }

        private void ChkNotifiedAffectedIndividual_CheckedChanged(object sender, EventArgs e)
        {
            grpNotifiedAffectedIndividual.Visible = chkNotifiedAffectedIndividual.Checked;
        }

        private void ChkAskedCallerToReturnCorrespondence_CheckedChanged(object sender, EventArgs e)
        {
            grpAskedCallerToReturnCorrespondence.Visible = chkAskedCallerToReturnCorrespondence.Checked;
        }

        private void ChkDeletedFiles_CheckedChanged(object sender, EventArgs e)
        {
            grpDeletedFiles.Visible = chkDeletedFiles.Checked;
        }

        private void ChkCorrectedData_CheckedChanged(object sender, EventArgs e)
        {
            grpCorrectedData.Visible = chkCorrectedData.Checked;
        }

        private void ChkRemovedSystemFromNetwork_CheckedChanged(object sender, EventArgs e)
        {
            grpRemovedSystemFromNetwork.Visible = chkRemovedSystemFromNetwork.Checked;
        }

        private void ChkRebootedSystem_CheckedChanged(object sender, EventArgs e)
        {
            grpRebootedSystem.Visible = chkRebootedSystem.Checked;
        }

        private void ChkLoggedOffSystem_CheckedChanged(object sender, EventArgs e)
        {
            grpLoggedOffSystem.Visible = chkLoggedOffSystem.Checked;
        }

        private void ChkShutDownSystem_CheckedChanged(object sender, EventArgs e)
        {
            grpShutDownSystem.Visible = chkShutDownSystem.Checked;
        }

        private void ChkContactedLawEnforcement_CheckedChanged(object sender, EventArgs e)
        {
            grpContactedLawEnforcement.Visible = chkContactedLawEnforcement.Checked;
        }

        private void CmbCourt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IncidentTicket == null)
                return;
            if (!(cmbCourt.SelectedItem is SqlUser selectedCourt) || selectedCourt.ID == 0)
                IncidentTicket.Court = null;
            else
                IncidentTicket.Court = selectedCourt;
        }

        private void CmbDataInvolved_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDataInvolvedControl(cmbDataInvolved.Text);
        }

        private void CmbIncidentType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ChangeIncidentTypeControl(cmbIncidentType.Text);
        }

        private void CmbNotificationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNotificationMethod.Text == Notifier.OTHER)
            {
                lblOtherMethod.Enabled = true;
                txtOtherMethod.Enabled = true;
            }
            else
            {
                lblOtherMethod.Enabled = false;
                txtOtherMethod.Clear();
                txtOtherMethod.Enabled = false;
            }
        }

        private void CmbNotifierRelationship_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNotifierRelationship.Text == Notifier.OTHER)
            {
                lblOtherRelationship.Enabled = true;
                txtOtherRelationship.Enabled = true;
            }
            else
            {
                lblOtherRelationship.Enabled = false;
                txtOtherRelationship.Clear();
                txtOtherRelationship.Enabled = false;
            }
        }

        private void CmbNotifierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNotifierType.Text == Notifier.OTHER)
            {
                lblOtherType.Enabled = true;
                txtOtherType.Enabled = true;
            }
            else
            {
                lblOtherType.Enabled = false;
                txtOtherType.Clear();
                txtOtherType.Enabled = false;
            }
        }

        private void LnkNextFlowStep_Click(object sender, EventArgs e)
        {
            OnNextFlowStepSelected(new NextFlowStepEventArgs(IncidentTicket, txtUpdate.Text, this));
        }

        private void LnkSave_Click(object sender, EventArgs e)
        {
            OnSaveTicket(new SaveTicketEventArgs(IncidentTicket, this));
        }

        private void LnkUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUpdate.Text.Trim()))
            {
                Dialog.Info.Ok("You must provide an update message.");
                return;
            }
            //Fire an event for a subscriber to handle the update.
            OnUpdateTicket(new UpdateTicketEventArgs(IncidentTicket, txtUpdate.Text, this));
        }

        #region Custom events

        public event EventHandler<EventArgs> ActivityDetected;
        protected virtual void OnActivityDetected(EventArgs e)
        {
            ActivityDetected?.Invoke(this, e);
        }

        public event EventHandler<RefreshSearchEventArgs> RefreshSearchResults;
        protected virtual void OnRefreshSearchResults(RefreshSearchEventArgs e)
        {
            RefreshSearchResults?.Invoke(this, e);
        }

        public event EventHandler<NextFlowStepEventArgs> NextFlowStepSelected;
        protected virtual void OnNextFlowStepSelected(NextFlowStepEventArgs e)
        {
            NextFlowStepSelected?.Invoke(this, e);
        }

        public event EventHandler<SaveTicketEventArgs> SaveTicket;
        protected virtual void OnSaveTicket(SaveTicketEventArgs e)
        {
            SaveTicket?.Invoke(this, e);
        }

        public event EventHandler<UpdateTicketEventArgs> UpdateTicket;
        protected virtual void OnUpdateTicket(UpdateTicketEventArgs e)
        {
            UpdateTicket?.Invoke(this, e);
        }

        #endregion Custom events
    }
}