using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    partial class SecurityIncidentSection : UserControl
    {
        public enum AssociatedDetail
        {
            None,
            IncidentReporter,
            IncidentDetail,
            IncidentType,
            DataInvolved,
            Cause,
            IncidentNarrative,
            ActionsTaken,
            Summary
        }

        private BaseDetail DetailControl { get; set; }
        private Color SelectedColor { get; set; }
        private Ticket Ticket { get; set; }
        //Combo box contents:
        private List<BusinessUnit> BusinessUnits { get; set; }
        private List<string> Causes { get; set; }
        private List<SqlUser> CurrentEmployees { get; set; }
        private List<string> DataInvolvedOptions { get; set; }
        private List<string> FunctionalAreas { get; set; }
        private List<string> IncidentTypes { get; set; }
        private List<string> MembersOfComputerServicesAndInfoSecurity { get; set; }
        private List<string> NotificationMethods { get; set; }
        private List<string> NotifierTypes { get; set; }
        private List<string> Regions { get; set; }
        private List<string> Relationships { get; set; }
        private List<string> States { get; set; }
        private List<string> Urgencies { get; set; }

        public Panel ContentPanel { get; set; }

        private AssociatedDetail _detailType;
        public AssociatedDetail DetailType
        {
            get
            {
                return _detailType;
            }
            set
            {
                _detailType = value;
                //Set the navigation text and detail control.
                switch (value)
                {
                    case AssociatedDetail.IncidentReporter:
                        lblNavigationText.Text = "Incident Reporter";
                        DetailControl = new IncidentReporterDetail(Ticket.Incident, CurrentEmployees, BusinessUnits, Urgencies);
                        break;
                    case AssociatedDetail.IncidentDetail:
                        lblNavigationText.Text = "Incident Detail";
                        DetailControl = new IncidentDetail(Ticket, NotificationMethods, NotifierTypes, FunctionalAreas, Relationships);
                        break;
                    case AssociatedDetail.IncidentType:
                        lblNavigationText.Text = "Incident Type";
                        DetailControl = new IncidentTypeDetail(Ticket.Incident, IncidentTypes);
                        break;
                    case AssociatedDetail.DataInvolved:
                        lblNavigationText.Text = "Data Involved";
                        DetailControl = new DataInvolvedDetail(Ticket.Incident, DataInvolvedOptions, States, Regions);
                        break;
                    case AssociatedDetail.Cause:
                        lblNavigationText.Text = "Cause";
                        DetailControl = new CauseDetail(Ticket.Incident, Causes);
                        break;
                    case AssociatedDetail.IncidentNarrative:
                        lblNavigationText.Text = "Narrative of Incident";
                        DetailControl = new IncidentDetailNarrative(Ticket.Incident);
                        break;
                    case AssociatedDetail.ActionsTaken:
                        lblNavigationText.Text = "Actions Taken";
                        DetailControl = new ActionsTakenDetail(Ticket.Incident.ActionsTaken, MembersOfComputerServicesAndInfoSecurity);
                        break;
                    case AssociatedDetail.Summary:
                        lblNavigationText.Text = "Summary";
                        DetailControl = new IncidentDetailSummary(Ticket);
                        DetailControl.btnSubmit.Click += new EventHandler(BtnSubmit_Click);
                        break;
                }
                SelectedColor = DetailControl.BackColor;
                DetailControl.btnCancel.Click += new EventHandler(BtnCancel_Click);
                DetailControl.btnNext.Click += new EventHandler(BtnNext_Click);
                DetailControl.btnPrevious.Click += new EventHandler(BtnPrevious_Click);
            }
        }

        public bool IsSelected { get; set; }

        public Panel NavigationPanel { get; set; }

        public SecurityIncidentSection(Ticket ticket, AssociatedDetail detailType, Panel contentPanel, Panel navigationPanel, List<string> causes, List<string> dataInvolvedOptions, List<string> states, List<string> regions, List<string> incidentTypes, List<SqlUser> currentEmployees, List<BusinessUnit> businessUnits, List<string> notificationMethods, List<string> notifierTypes, List<string> urgencies, List<string> membersOfComputerServicesAndInfoSecurity, List<string> functionalAreas, List<string> relationships)
        {
            InitializeComponent();
            BusinessUnits = businessUnits;
            Causes = causes;
            CurrentEmployees = currentEmployees;
            DataInvolvedOptions = dataInvolvedOptions;
            FunctionalAreas = functionalAreas;
            IncidentTypes = incidentTypes;
            MembersOfComputerServicesAndInfoSecurity = membersOfComputerServicesAndInfoSecurity;
            NotificationMethods = notificationMethods;
            NotifierTypes = notifierTypes;
            Regions = regions;
            Relationships = relationships;
            States = states;
            Ticket = ticket;
            Urgencies = urgencies;
            IsSelected = false;
            DetailType = detailType;
            ContentPanel = contentPanel;
            NavigationPanel = navigationPanel;
        }

        public void SelectOption()
        {
            //Unselect the currently selected option.
            if (ContentPanel.Controls.Count > 0)
            {
                for (int i = 0; i < NavigationPanel.Controls.Count; i++)
                {
                    SecurityIncidentSection navControl = NavigationPanel.Controls[i] as SecurityIncidentSection;
                    if (navControl.IsSelected)
                    {
                        navControl.UnselectOption();
                        break;
                    }
                }
                ContentPanel.Controls.RemoveAt(0);
            }
             //Select the new option.
            IsSelected = true;
            BackColor = SelectedColor;
            ContentPanel.Controls.Add(DetailControl);
            DetailControl.CheckValidity();
            DetailControl.btnPrevious.Visible = (GetIndexOfSelectedOption() != 0);
            DetailControl.btnNext.Visible = (GetIndexOfSelectedOption() != NavigationPanel.Controls.Count - 1);
            //If this is the Summary option, enable or disable the Submit button depending on whether the ticket is complete.
            if (DetailType == AssociatedDetail.Summary)
                DetailControl.btnSubmit.Enabled = Ticket.Incident.IsComplete();
        }

        private int GetIndexOfSelectedOption()
        {
            int i = 0;
            while (i < NavigationPanel.Controls.Count && !(NavigationPanel.Controls[i] as SecurityIncidentSection).IsSelected)
                i++;
            return i;
        }

        private void UnselectOption()
        {
            IsSelected = false;
            BackColor = Color.White;
        }

        private void BaseDetail_ValidityChanged(object sender, ValidityChangedEventArgs e)
        {
            pbxValid.Image = (e.IsValid ? Properties.Resources.icon_checkmark_128 : Properties.Resources.wrongx_icon);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            OnCancel(new CancelEventArgs());
        }

        void BtnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmit(new SubmitEventArgs(Ticket));
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            int currentIndex = GetIndexOfSelectedOption();
            (NavigationPanel.Controls[currentIndex + 1] as SecurityIncidentSection).SelectOption();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            int currentIndex = GetIndexOfSelectedOption();
            (NavigationPanel.Controls[currentIndex - 1] as SecurityIncidentSection).SelectOption();
        }

        private void LblNavigationText_Click(object sender, EventArgs e)
        {
            SelectOption();
        }

        #region Custom events

        public class SubmitEventArgs : EventArgs
        {
            public readonly Ticket Ticket;

            public SubmitEventArgs(Ticket ticket)
            {
                Ticket = ticket;
            }
        }
        public event EventHandler<SubmitEventArgs> Submit;
        protected virtual void OnSubmit(SubmitEventArgs e)
        {
            Submit?.Invoke(this, e);
        }

        public class CancelEventArgs : EventArgs
        {
            public CancelEventArgs() { }
        }
        public event EventHandler<CancelEventArgs> Cancel;
        protected virtual void OnCancel(CancelEventArgs e)
        {
            Cancel?.Invoke(this, e);
        }

        #endregion
    }
}
