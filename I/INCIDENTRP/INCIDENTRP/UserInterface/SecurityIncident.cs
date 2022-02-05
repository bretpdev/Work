using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    partial class SecurityIncident : Form
    {
        //Info needed when canceling a ticket:
        private string _accountNumber;
        private DateTime _createDateTime;

        public SecurityIncident(Ticket incidentTicket, string accountNumber, List<string> causes, List<string> dataInvolvedOptions, List<string> states, List<string> regions, List<string> incidentTypes, List<SqlUser> users, List<BusinessUnit> businessUnits, List<string> notificationMethods, List<string> notifierTypes, List<string> urgencies, List<string> membersOfComputerServicesAndInfoSecurity, List<string> functionalAreas, List<string> relationships)
        {
            InitializeComponent();

            _accountNumber = accountNumber;
            _createDateTime = incidentTicket.CreateDateTime;
            pnlNavigation.Controls.Clear();
            SecurityIncidentSection incidentReporter = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.IncidentReporter, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            incidentReporter.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            pnlNavigation.Controls.Add(incidentReporter);
            SecurityIncidentSection incidentDetail = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.IncidentDetail, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            incidentDetail.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            pnlNavigation.Controls.Add(incidentDetail);
            SecurityIncidentSection incidentType = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.IncidentType, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            incidentType.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            pnlNavigation.Controls.Add(incidentType);
            SecurityIncidentSection dataInvolved = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.DataInvolved, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            dataInvolved.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            pnlNavigation.Controls.Add(dataInvolved);
            SecurityIncidentSection cause = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.Cause, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            cause.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            pnlNavigation.Controls.Add(cause);
            SecurityIncidentSection incidentNarrative = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.IncidentNarrative, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            incidentNarrative.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            pnlNavigation.Controls.Add(incidentNarrative);
            SecurityIncidentSection actionsTaken = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.ActionsTaken, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            actionsTaken.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            pnlNavigation.Controls.Add(actionsTaken);
            SecurityIncidentSection summary = new SecurityIncidentSection(incidentTicket, SecurityIncidentSection.AssociatedDetail.Summary, pnlContentHolder, pnlNavigation, causes, dataInvolvedOptions, states, regions, incidentTypes, users, businessUnits, notificationMethods, notifierTypes, urgencies, membersOfComputerServicesAndInfoSecurity, functionalAreas, relationships);
            summary.Cancel += new EventHandler<SecurityIncidentSection.CancelEventArgs>(SecurityIncident_Cancel);
            summary.Submit += new EventHandler<SecurityIncidentSection.SubmitEventArgs>(SecurityIncident_Submit);
            pnlNavigation.Controls.Add(summary);
            incidentReporter.SelectOption();
        }

        void SecurityIncident_Cancel(object sender, SecurityIncidentSection.CancelEventArgs e)
        {
            OnCancel(new CancelTicketEventArgs(Ticket.INCIDENT, _createDateTime, _accountNumber));
        }

        void SecurityIncident_Submit(object sender, SecurityIncidentSection.SubmitEventArgs e)
        {
            OnSubmit(new NextFlowStepEventArgs(e.Ticket, null, this));
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

        #endregion
    }
}