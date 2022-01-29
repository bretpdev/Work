using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    partial class ThreatDisplay : Form
    {
        private Ticket ThreatTicket { get; set; }
        private SqlUser User { get; set; }
        private DataAccess DA { get; set; }

        public ThreatDisplay(Ticket ticket, SqlUser user, string nextFlowStep, List<SqlUser> currentEmployees, List<BusinessUnit> businessUnits, List<string> notificationMethods, List<string> notifierTypes, ProcessLogRun logRun)
        {
            InitializeComponent();
            DA = new DataAccess(logRun);
            pbxCompanyLogo.Image = Properties.Resources.UheaaLogo;
            User = user;

            //Populate the combo boxes.
            cmbBusinessUnit.DataSource = businessUnits;
            List<SqlUser> courts = new List<SqlUser>(currentEmployees);
            courts.Insert(0, new SqlUser());
            cmbCourt.DataSource = courts;
            cmbNotificationMethod.DataSource = notificationMethods;
            cmbNotifierType.DataSource = notifierTypes;
            cmbReporter.DataSource = new List<SqlUser>(currentEmployees);
            dtpIncidentDate.MaxDate = ContactITDate.MaxDate = ContactLawDate.MaxDate = DateTime.Now.Date;

            BindToNewTicket(ticket, nextFlowStep);
            WatchForActivity(this);
        }

        /// <summary>
        /// Changes the ticket data in the form to reflect the passed-in ticket.
        /// </summary>
        /// <param name="ticket">The Ticket object to display in the form.</param>
        public void BindToNewTicket(Ticket ticket, string nextFlowStep)
        {
            ThreatTicket = ticket;

            //Set data sources for Ticket members.
            callerBindingSource.DataSource = ThreatTicket.Threat.Caller;
            callerBindingSource.ResetBindings(false); //Call reset binding to update all fields
            infoTechActionBindingSource.DataSource = ThreatTicket.Threat.InformationTechnologyAction;
            infoTechActionBindingSource.ResetBindings(false); //Call reset binding to update all fields
            bombInfoBindingSource.DataSource = ThreatTicket.Threat.BombInfo;
            bombInfoBindingSource.ResetBindings(false); //Call reset binding to update all fields
            threatInfoBindingSource.DataSource = ThreatTicket.Threat.Info;
            threatInfoBindingSource.ResetBindings(false); //Call reset binding to update all fields
            notifierBindingSource.DataSource = ThreatTicket.Threat.Notifier;
            notifierBindingSource.ResetBindings(false); //Call reset binding to update all fields
            threatBindingSource.DataSource = ThreatTicket.Threat;
            threatBindingSource.ResetBindings(false); //Call reset binding to update all fields
            lawActionBindingSource.DataSource = ThreatTicket.Threat.LawEnforcementAction;
            lawActionBindingSource.ResetBindings(false); //Call reset binding to update all fields
            reporterBindingSource.DataSource = ThreatTicket.Threat.Reporter;
            reporterBindingSource.ResetBindings(false); //Call reset binding to update all fields
            backgroundNoiseBindingSource.DataSource = ThreatTicket.Threat.Caller.BackgroundNoise;
            backgroundNoiseBindingSource.ResetBindings(false); //Call reset binding to update all fields
            voiceBindingSource.DataSource = ThreatTicket.Threat.Caller.Voice;
            voiceBindingSource.ResetBindings(false); //Call reset binding to update all fields
            mannerBindingSource.DataSource = ThreatTicket.Threat.Caller.Manner;
            mannerBindingSource.ResetBindings(false); //Call reset binding to update all fields
            dialectBindingSource.DataSource = ThreatTicket.Threat.Caller.Dialect;
            dialectBindingSource.ResetBindings(false); //Call reset binding to update all fields
            languageBindingSource.DataSource = ThreatTicket.Threat.Caller.Language;
            languageBindingSource.ResetBindings(false); //Call reset binding to update all fields
            ticketBindingSource.DataSource = ThreatTicket;
            ticketBindingSource.ResetBindings(false); //Call reset binding to update all fields

            //Push data into controls that either can't be bound or don't seem to want to acknowledge the data source.
            cmbBusinessUnit.Text = ThreatTicket.Threat.Reporter.BusinessUnit.Name;
            cmbCourt.Text = (ThreatTicket.Court == null ? "" : ThreatTicket.Court.ToString());
            cmbNotificationMethod.Text = ThreatTicket.Threat.Notifier.Method;
            cmbNotifierType.Text = ThreatTicket.Threat.Notifier.Type;
            cmbReporter.Text = ThreatTicket.Threat.Reporter.User.ToString();
            chkContactedInformationSecurity.Checked = ThreatTicket.Threat.InformationTechnologyAction.ActionWasTaken;
            chkContactedLawEnforcement.Checked = ThreatTicket.Threat.LawEnforcementAction.ActionWasTaken;
            txtReporterEmailAddress.Text = ThreatTicket.Threat.Reporter.User.EmailAddress;
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
            switch (ThreatTicket.Threat.Caller.Sex)
            {
                case Caller.MALE:
                    chkGenderMale.Checked = true;
                    break;
                case Caller.FEMALE:
                    chkGenderFemale.Checked = true;
                    break;
                case Caller.UNSURE:
                    chkGenderUnsure.Checked = true;
                    break;
            }

            UpdateHistory();

            const int SUBJECT_LENGTH = 100;
            string natureOfCall = ticket.Threat.Info.NatureOfCall ?? "";
            string subject = (natureOfCall.Length > SUBJECT_LENGTH ? natureOfCall.Substring(0, SUBJECT_LENGTH) + "..." : natureOfCall);
            this.Text = $"[{ticket.Priority}] {ticket.Type} {ticket.Number} - {subject} ({ticket.Status})";

            if (User == ticket.LockHolder)
                UnlockForm();
            else
                LockForm($"{ticket.LockHolder.FirstName} {ticket.LockHolder.LastName}");
        }

        public void SaveAndQuit()
        {
            OnSaveTicket(new SaveTicketEventArgs(ThreatTicket, this));
            Close();
        }

        private void LockForm(string lockHolderName)
        {
            this.Text += $" - Locked by {lockHolderName}";
            this.Enabled = false;
            string lockMessage = $"This ticket is locked by {lockHolderName}.";
            Dialog.Info.Ok(lockMessage, "Threat Reporting");
        }

        private void UnlockForm()
        {
            this.Enabled = true;
        }

        private void UpdateHistory()
        {
            txtUpdate.Clear();
            txtHistory.Clear();
            txtHistory.AppendText(ThreatTicket.History.AsString());
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

        private void ChkRecognizedVoiceYes_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRecognizedVoiceYes.Checked)
                chkRecognizedVoiceNo.Checked = false;
        }

        private void ChkRecognizedVoiceNo_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRecognizedVoiceNo.Checked)
                chkRecognizedVoiceYes.Checked = false;
        }

        private void ChkFamiliarYes_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFamiliarYes.Checked)
                chkFamiliarNo.Checked = false;
        }

        private void ChkFamiliarNo_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFamiliarNo.Checked)
                chkFamiliarYes.Checked = false;
        }

        private void ChkGenderMale_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkGenderMale.Checked)
            {
                ThreatTicket.Threat.Caller.Sex = Caller.MALE;
                chkGenderFemale.Checked = false;
                chkGenderUnsure.Checked = false;
            }
        }

        private void ChkGenderFemale_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkGenderFemale.Checked)
            {
                ThreatTicket.Threat.Caller.Sex = Caller.FEMALE;
                chkGenderMale.Checked = false;
                chkGenderUnsure.Checked = false;
            }
        }

        private void ChkGenderUnsure_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkGenderUnsure.Checked)
            {
                ThreatTicket.Threat.Caller.Sex = Caller.UNSURE;
                chkGenderMale.Checked = false;
                chkGenderFemale.Checked = false;
            }
        }

        private void ChkContactedInformationSecurity_CheckedChanged(object sender, EventArgs e)
        {
            ThreatTicket.Threat.InformationTechnologyAction.ActionWasTaken = chkContactedInformationSecurity.Checked;
            grpInformationSecurityContactInfo.Enabled = chkContactedInformationSecurity.Checked;
        }

        private void ChkContactedLawEnforcement_CheckedChanged(object sender, EventArgs e)
        {
            ThreatTicket.Threat.LawEnforcementAction.ActionWasTaken = chkContactedLawEnforcement.Checked;
            grpLawEnforcementContactInfo.Enabled = chkContactedLawEnforcement.Checked;
        }

        private void CmbCourt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ThreatTicket == null)
                return;
            if (!(cmbCourt.SelectedItem is SqlUser selectedCourt) || selectedCourt.ID == 0)
                ThreatTicket.Court = null;
            else
                ThreatTicket.Court = selectedCourt;
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
            OnNextFlowStepSelected(new NextFlowStepEventArgs(ThreatTicket, txtUpdate.Text, this));
        }

        private void LnkSave_Click(object sender, EventArgs e)
        {
            OnSaveTicket(new SaveTicketEventArgs(ThreatTicket, this));
        }

        private void LnkUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUpdate.Text.Trim()))
            {
                Dialog.Info.Ok("You must provide an update message.");
                return;
            }
            //Fire an event for a subscriber to handle the update.
            OnUpdateTicket(new UpdateTicketEventArgs(ThreatTicket, txtUpdate.Text, this));
        }

        #region Custom events

        public event EventHandler<EventArgs> ActivityDetected;
        protected virtual void OnActivityDetected(EventArgs e)
        {
            ActivityDetected?.Invoke(this, e);
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