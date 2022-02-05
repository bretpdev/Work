using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using SubSystemShared;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    partial class ThreatCall : Form
    {
        private ThreatCaller Caller { get; set; }
        private Thread CallerThread { get; set; }
        private Ticket ThreatTicket { get; set; }

        public ThreatCall(Ticket threatTicket, List<SqlUser> users, List<BusinessUnit> businessUnits, List<string> notificationMethods, List<string> notifierTypes, List<string> functionalAreas)
        {
            InitializeComponent();
            ThreatTicket = threatTicket;
            Caller = new ThreatCaller(ThreatTicket.Threat);
            Caller.txtRemarks.TextChanged += new EventHandler(ThreatCaller_txtRemarks_TextChanged);
            cmbUser.DataSource = users;
            cmbBusinessUnit.DataSource = businessUnits;
            cmbBusinessUnit.DisplayMember = "Name";
            cmbFunctionalArea.DataSource = functionalAreas;
            cmbNotifierMethod.DataSource = notificationMethods;
            cmbNotifiedBy.DataSource = notifierTypes;
            ticketBindingSource.DataSource = ThreatTicket;
            notifierBindingSource.DataSource = ThreatTicket.Threat.Notifier;
            reporterBindingSource.DataSource = ThreatTicket.Threat.Reporter;
            infoTechActionBindingSource.DataSource = ThreatTicket.Threat.InformationTechnologyAction;
            lawActionBindingSource.DataSource = ThreatTicket.Threat.LawEnforcementAction;
            threatInfoBindingSource.DataSource = ThreatTicket.Threat.Info;
            bombInfoBindingSource.DataSource = ThreatTicket.Threat.BombInfo;
            callerBindingSource.DataSource = ThreatTicket.Threat.Caller;
            cmbUser.Text = threatTicket.Threat.Reporter.User.ToString();
            cmbBusinessUnit.Text = threatTicket.Threat.Reporter.BusinessUnit.Name;
            txtReporterEmail.Text = threatTicket.Threat.Reporter.User.EmailAddress;
            IncidentDate.MaxDate = ContactDate.MaxDate = ContactLawDate.MaxDate = DateTime.Now.Date;
        }

        public new void Show()
        {
            base.Show();
            Caller.Show();
        }

        public new void ShowDialog()
        {
            CallerThread = new Thread(new ThreadStart(Caller.ShowDialog));
            CallerThread.Start();
            base.ShowDialog();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (CallerThread != null && CallerThread.IsAlive)
                CallerThread.Abort();
            Caller.Hide();
            Caller.Dispose();
            base.Dispose(disposing);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            //Verify that required information is supplied:
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(ThreatTicket.Threat.Info.WordingOfThreat))
                errors.Add("Exact wording of the threat");
            if (ThreatTicket.Threat.InformationTechnologyAction.ActionWasTaken && !ThreatTicket.Threat.InformationTechnologyAction.IsComplete())
                errors.Add("Person contacted in Information Technology/Information Security");
            if (ThreatTicket.Threat.LawEnforcementAction.ActionWasTaken && !ThreatTicket.Threat.LawEnforcementAction.IsComplete())
                errors.Add("Person contacted in Law Enforcement");
            if (errors.Count > 0)
            {
                errors.Insert(0, "Please provide the following details:");
                string message = string.Join(Environment.NewLine, errors.ToArray());
                Dialog.Info.Ok(message, "Missing Details");
                return;
            }
            OnSubmit(new NextFlowStepEventArgs(ThreatTicket, null, this));
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            OnCancel(new CancelTicketEventArgs(ThreatTicket.Type, ThreatTicket.CreateDateTime, ThreatTicket.Threat.Caller.AccountNumber));
        }

        private void ChkRecognizedVoiceYes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecognizedVoiceYes.Checked)
                chkRecognizedVoiceNo.Checked = false;
        }

        private void ChkRecognizedVoiceNo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecognizedVoiceNo.Checked)
                chkRecognizedVoiceYes.Checked = false;
        }

        private void ChkFamiliarYes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFamiliarYes.Checked)
                chkFamiliarNo.Checked = false;
        }

        private void ChkFamiliarNo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFamiliarNo.Checked)
                chkFamiliarYes.Checked = false;
        }

        private void ChkGenderMale_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGenderMale.Checked)
            {
                ThreatTicket.Threat.Caller.Sex = INCIDENTRP.Caller.MALE;
                chkGenderFemale.Checked = false;
                chkGenderUnsure.Checked = false;
            }
        }

        private void ChkGenderFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGenderFemale.Checked)
            {
                ThreatTicket.Threat.Caller.Sex = INCIDENTRP.Caller.FEMALE;
                chkGenderMale.Checked = false;
                chkGenderUnsure.Checked = false;
            }
        }

        private void ChkGenderUnsure_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGenderUnsure.Checked)
            {
                ThreatTicket.Threat.Caller.Sex = INCIDENTRP.Caller.UNSURE;
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

        //I couldn't get the ThreatInfo object to bind to both forms,
        //so we need to watch the ThreatCaller form's txtRemarks for changes.
        private void ThreatCaller_txtRemarks_TextChanged(object sender, EventArgs e)
        {
            ThreatTicket.Threat.Info.AdditionalRemarks = (sender as TextBox).Text;
        }

        #region Custom events

        public event EventHandler<CancelTicketEventArgs> Cancel;
        protected virtual void OnCancel(CancelTicketEventArgs e)
        {
            Cancel?.Invoke(this, e);
        }

        public event EventHandler<NextFlowStepEventArgs> Submit;
        protected virtual void OnSubmit(NextFlowStepEventArgs e)
        {
            Submit?.Invoke(this, e);
        }

        #endregion Custom events

        private void CmbNotifierMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNotifierMethod.Text == Notifier.OTHER)
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

        private void CmbNotifiedBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNotifiedBy.Text == Notifier.OTHER)
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
    }
}