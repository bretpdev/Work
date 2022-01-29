using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SubSystemShared;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace INCIDENTRP
{
    public class SubSystem : SubSystemBase
    {
        private const string SUBSYSTEM_NAME = "Incident Reporting";
        private const string SUBSYSTEM_ABBREVIATION = "IR";
        private const int FIFTEEN_MINUTES = 54000;
        private const int EIGHTEEN_MINUTES = 64800;

        private DataAccess DA { get; set; }
        private List<SqlUser> FormerEmployees { get; set; }
        private SecurityIncident IncidentEntryForm { get; set; }
        private IncidentDisplay IncidentDisplayForm { get; set; }
        private int IncidentSecondsIdle { get; set; }
        private ThreatCall ThreatEntryForm { get; set; }
        private ThreatDisplay ThreatDisplayForm { get; set; }
        private int ThreatSecondsIdle { get; set; }
        private Timer Timer { get; set; }

        //Collections for the forms' combo boxes:
        private List<BusinessUnit> BusinessUnits { get; set; }
        private List<SqlUser> CurrentEmployees { get; set; }
        private List<string> DataInvolvedOptions { get; set; }
        private List<string> FunctionalAreas { get; set; }
        private List<string> IncidentCauses { get; set; }
        private List<string> IncidentTypes { get; set; }
        private List<string> MembersOfComputerServicesAndInfoSecurity { get; set; }
        private List<string> NotificationMethods { get; set; }
        private List<string> NotifierTypes { get; set; }
        private List<string> Regions { get; set; }
        private List<string> Relationships { get; set; }
        private List<string> States { get; set; }
        private List<string> Urgencies { get; set; }
        protected override string BaseAbbreviation => SUBSYSTEM_ABBREVIATION;
        protected override string BaseName => SUBSYSTEM_NAME;
        public override bool HasAccess => true;

        public SubSystem(SqlUser user, string role, ProcessLogRun logRun)
            : base(user, role, logRun)
        {
            DA = new DataAccess(logRun);
            IncidentSecondsIdle = 0;
            ThreatSecondsIdle = 0;
            Timer = new Timer();
            Timer.Interval = 1000; //Raise the tick event once per second.
            Timer.Tick += new EventHandler(TimerTick);
            Timer.Start();
            LogRun = logRun;
        }

        public override void CreateNewTicket(TicketType ticketType, List<SqlUser> users)
        {
            try
            {
                //Make sure we have lists for the combo boxes that are common to both incident and threat entry forms.
                BusinessUnits = BusinessUnits ?? DA.GetBusinessUnits().OrderBy(p => p.Name).ToList();
                CurrentEmployees = CurrentEmployees ?? users.OrderBy(p => p.FirstName).ToList();
                FunctionalAreas = FunctionalAreas ?? DA.GetBusinessFunctions();
                NotificationMethods = NotificationMethods ?? DA.GetNotificationMethods();
                NotifierTypes = NotifierTypes ?? DA.GetNotifierTypes();

                //Assume that the current user is reporting the incident.
                Reporter reporter = new Reporter();
                reporter.User = User;
                reporter.BusinessUnit = DA.GetBusinessUnit(User);
                string windowsUserId = User.EmailAddress.Substring(0, User.EmailAddress.IndexOf("@"));
                reporter.PhoneNumber = DA.GetPhoneExtension(windowsUserId);

                //Create the ticket based on the type requested.
                if (ticketType.Abbreviation.StartsWith("IR"))
                    CreateIncidentTicket(reporter);
                else
                    CreateThreatTicket(reporter);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error creating a new incident reporting ticket", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                string message = $"Incident Reporting has run into an error and cannot continue. Please contact System Support and reference Process Log Id: {LogRun.ProcessLogId}";
                Dialog.Error.Ok(message, "Incident Reporting");
            }
        }

        public override void DisplayTicket(string ticketCode, long ticketNumber, List<SqlUser> users)
        {
            try
            {
                //Make sure we have lists for the combo boxes that are common to both incident and threat display forms.
                BusinessUnits = BusinessUnits ?? DA.GetBusinessUnits().OrderBy(p => p.Name).ToList();
                CurrentEmployees = CurrentEmployees ?? users.OrderBy(p => p.FirstName).ToList();
                NotificationMethods = NotificationMethods ?? DA.GetNotificationMethods().OrderBy(p => p).ToList();
                NotifierTypes = NotifierTypes ?? DA.GetNotifierTypes().OrderBy(p => p).ToList();

                if (ticketCode.StartsWith("IR"))
                    DisplayIncidentTicket(ticketNumber);
                else
                    DisplayThreatTicket(ticketNumber);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error trying to display the current incident reporting ticket", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                string message = $"Incident Reporting has run into an error and cannot continue. Please contact System Support and reference Process Log Id: {LogRun.ProcessLogId}";
                Dialog.Error.Ok(message, "Incident Reporting");
            }
        }

        public override DateTime GetEarliestTicketCreateDate()
        {
            return DA.GetEarliestTicketCreateDate();
        }

        public override List<SqlUser> GetFormerEmployeeAssignees()
        {
            FormerEmployees = FormerEmployees ?? DA.GetInactiveUsers();
            List<int> assignedToSquids = DA.GetAssignees();
            List<SqlUser> formerEmployeeAssignees = FormerEmployees.Where(p => assignedToSquids.Contains(p.ID)).ToList();
            return formerEmployeeAssignees;
        }

        public override List<SqlUser> GetFormerEmployeeCourts()
        {
            FormerEmployees = FormerEmployees ?? DA.GetInactiveUsers();
            List<int> courtSquids = DA.GetCourts();
            List<SqlUser> formerEmployeeCourts = FormerEmployees.Where(p => courtSquids.Contains(p.ID)).ToList();
            return formerEmployeeCourts;
        }

        public override List<SqlUser> GetFormerEmployeeRequesters()
        {
            FormerEmployees = FormerEmployees ?? DA.GetInactiveUsers();
            List<int> requesterSquids = DA.GetCourts();
            List<SqlUser> formerEmployeeRequesters = FormerEmployees.Where(p => requesterSquids.Contains(p.ID)).ToList();
            return formerEmployeeRequesters;
        }

        public override List<TicketType> GetTicketTypes()
        {
            return DA.GetTicketTypes();
        }

        public override List<string> GetSubjects()
        {
            return DA.GetSubjects();
        }

        public override List<string> GetStatuses()
        {
            return DA.GetStatuses();
        }

        public override List<SearchResultItem> SearchForAllTickets(SearchCriteria criteria)
        {
            if (DA.UserHasInformationSecurityAccess(User))
                return DA.SearchForAllTickets();
            else
                return DA.SearchForTickets(new SearchCriteria() { Court = User });
        }

        public override List<SearchResultItem> SearchForOpenTickets(SearchCriteria criteria)
        {
            if (DA.UserHasInformationSecurityAccess(User))
                return DA.SearchForOpenTickets();
            else
                return DA.SearchForTickets(new SearchCriteria() { Court = User }).Where(p => p.Status != "Closed").ToList();
        }

        public override List<SearchResultItem> SearchForTickets(SearchCriteria criteria)
        {
            if (!DA.UserHasInformationSecurityAccess(User))
                criteria.Court = User;
            else
                criteria.Court = null;
            return DA.SearchForTickets(criteria).ToList();
        }

        private void CancelTicket(object sender, CancelTicketEventArgs e)
        {
            DA.CancelTicket(User.ID, e.TicketType, e.CreateDateTime, DateTime.Now, null);
            if (e.TicketType == Ticket.INCIDENT)
                IncidentEntryForm.Dispose();
            else if (e.TicketType == Ticket.THREAT)
                ThreatEntryForm.Dispose();
            DA.ReleaseLock(User, e.TicketType);
        }

        private void CheckForIncidentTimeout()
        {
            if (IncidentDisplayForm == null || IncidentDisplayForm.IsDisposed)
                return;

            IncidentSecondsIdle++;
            if (IncidentSecondsIdle == FIFTEEN_MINUTES)
            {
                string message = "Your Incident Reporting ticket has been idle for 15 minutes. Do you need additional time?";
                message += Environment.NewLine;
                message += "If you choose no, all work will be saved and the form will close.";
                if (Dialog.Warning.YesNo(message, "Time Out"))
                    IncidentSecondsIdle = 0;
                else
                    IncidentDisplayForm.SaveAndQuit();
            }
            else if (IncidentSecondsIdle == EIGHTEEN_MINUTES)
            {
                IncidentDisplayForm.SaveAndQuit();
            }
        }

        private void CheckForThreatTimeout()
        {
            if (ThreatDisplayForm == null || ThreatDisplayForm.IsDisposed)
                return;

            ThreatSecondsIdle++;
            if (ThreatSecondsIdle == FIFTEEN_MINUTES)
            {
                string message = "Your Threat Report ticket has been idle for 15 minutes. Do you need additional time?";
                message += Environment.NewLine;
                message += "If you choose no, all work will be saved and the form will close.";
                if (Dialog.Warning.YesNo(message, "Time Out"))
                    ThreatSecondsIdle = 0;
                else
                    ThreatDisplayForm.SaveAndQuit();
            }
            else if (ThreatSecondsIdle == EIGHTEEN_MINUTES)
                ThreatDisplayForm.SaveAndQuit();
        }

        private void CreateIncidentTicket(Reporter reporter)
        {
            //Initialize any collections we'll need.
            DataInvolvedOptions = DataInvolvedOptions ?? DA.GetDataInvolvedOptions();
            IncidentCauses = IncidentCauses ?? DA.GetIncidentCauses();
            IncidentTypes = IncidentTypes ?? DA.GetIncidentTypes();
            MembersOfComputerServicesAndInfoSecurity = MembersOfComputerServicesAndInfoSecurity ?? DA.GetMembersOfComputerServicesAndInfoSecurity();
            Regions = Regions ?? DA.GetRegions();
            Relationships = Relationships ?? DA.GetRelationships();
            States = States ?? DA.GetStateCodes(false);
            Urgencies = Urgencies ?? DA.GetUrgencies();

            //If we're already showing another ticket, close that one.
            IncidentEntryForm?.Dispose();

            //Create a new ticket and feed it to a new instance of the form.
            string initialStatus = DA.GetStepsForSpecifiedFlow(GetFlowId(Ticket.INCIDENT)).OrderBy(p => p.FlowStepSequenceNumber).First().Status;
            Ticket ticket = new Ticket(Ticket.INCIDENT, reporter, initialStatus);
            ticket.Incident.Notifier.Method = Notifier.CALL;
            IncidentEntryForm = new SecurityIncident(ticket, null, IncidentCauses, DataInvolvedOptions, States, Regions, IncidentTypes, CurrentEmployees, BusinessUnits, NotificationMethods, NotifierTypes, Urgencies, MembersOfComputerServicesAndInfoSecurity, FunctionalAreas, Relationships);

            //Subscribe to the form's events so we can respond to actions requested.
            IncidentEntryForm.Cancel += new EventHandler<CancelTicketEventArgs>(CancelTicket);
            IncidentEntryForm.Submit += new EventHandler<NextFlowStepEventArgs>(NextFlowStep);

            //All set. Show the ticket.
            IncidentEntryForm.Show();
        }

        private void CreateThreatTicket(Reporter reporter)
        {
            //If we're already showing another ticket, close that one.
            ThreatEntryForm?.Dispose();

            //Create a new ticket and feed it to a new instance of the form.
            string initialStatus = DA.GetStepsForSpecifiedFlow(GetFlowId(Ticket.THREAT)).OrderBy(p => p.FlowStepSequenceNumber).First().Status;
            Ticket ticket = new Ticket(Ticket.THREAT, reporter, initialStatus);
            ticket.Threat.Notifier.Method = Notifier.CALL;
            ThreatEntryForm = new ThreatCall(ticket, CurrentEmployees, BusinessUnits, NotificationMethods, NotifierTypes, FunctionalAreas);

            //Subscribe to the form's events so we can respond to actions requested.
            ThreatEntryForm.Cancel += new EventHandler<CancelTicketEventArgs>(CancelTicket);
            ThreatEntryForm.Submit += new EventHandler<NextFlowStepEventArgs>(NextFlowStep);

            //All set. Show the ticket.
            ThreatEntryForm.Show();
        }

        private void DisplayIncidentTicket(long ticketNumber)
        {
            //The incident display form has more combo boxes that we need to populate.
            IncidentCauses = IncidentCauses ?? DA.GetIncidentCauses().OrderBy(p => p).ToList();
            IncidentTypes = IncidentTypes ?? DA.GetIncidentTypes().OrderBy(p => p).ToList();
            DataInvolvedOptions = DataInvolvedOptions ?? DA.GetDataInvolvedOptions().OrderBy(p => p).ToList();
            Regions = Regions ?? DA.GetRegions().OrderBy(p => p).ToList();
            Relationships = Relationships ?? DA.GetRelationships();
            States = States ?? DA.GetStateCodes(false);
            Urgencies = Urgencies ?? DA.GetUrgencies().OrderBy(p => p).ToList();

            //Unlock any tickets of this type that the user might have a lock on.
            DA.ReleaseLock(User, Ticket.INCIDENT);

            //Load the ticket and see if there's a form ready to display it.
            Ticket ticket = Ticket.Load(DA, ticketNumber, Ticket.INCIDENT, User);
            FlowStep flowStep = DA.GetStepsForSpecifiedFlow(GetFlowId(ticket.Type)).Single(p => p.Status == ticket.Status);
            if (IncidentDisplayForm == null || IncidentDisplayForm.IsDisposed)
            {
                //No form exists. Create a new one and feed it everything it needs.
                IncidentDisplayForm = new IncidentDisplay(ticket, User, flowStep.ControlDisplayText, CurrentEmployees, BusinessUnits, NotificationMethods, NotifierTypes, IncidentCauses, DataInvolvedOptions, IncidentTypes, Regions, States, Relationships, LogRun);

                //Subscribe to the form's events so we can respond to actions requested.
                IncidentDisplayForm.ActivityDetected += new EventHandler<EventArgs>((object sender, EventArgs e) => IncidentSecondsIdle = 0);
                IncidentDisplayForm.NextFlowStepSelected += new EventHandler<NextFlowStepEventArgs>(NextFlowStep);
                IncidentDisplayForm.RefreshSearchResults += new EventHandler<RefreshSearchEventArgs>((object sender, RefreshSearchEventArgs e) => OnRefreshSearchResults(e));
                IncidentDisplayForm.SaveTicket += new EventHandler<SaveTicketEventArgs>(SaveTicket);
                IncidentDisplayForm.UpdateTicket += new EventHandler<UpdateTicketEventArgs>(UpdateTicket);
                IncidentDisplayForm.FormClosed += new FormClosedEventHandler((object sender, FormClosedEventArgs e) => DA.ReleaseLock(User, Ticket.INCIDENT));

                //All set. Show the ticket.
                IncidentDisplayForm.Show();
            }
            else
            {
                //There's already a form up. It just needs to know about the new ticket.
                IncidentDisplayForm.BindToNewTicket(ticket, flowStep.ControlDisplayText);
            }
            IncidentSecondsIdle = 0;
        }

        private void DisplayThreatTicket(long ticketNumber)
        {
            //Unlock any tickets of this type that the user might have a lock on.
            DA.ReleaseLock(User, Ticket.THREAT);

            //Load the ticket and see if there's a form ready to display it.
            Ticket ticket = Ticket.Load(DA, ticketNumber, Ticket.THREAT, User);
            FlowStep flowStep = DA.GetStepsForSpecifiedFlow(GetFlowId(ticket.Type)).Single(p => p.Status == ticket.Status);
            if (ThreatDisplayForm == null || ThreatDisplayForm.IsDisposed)
            {
                //No form exists. Create a new one and feed it everything it needs.
                ThreatDisplayForm = new ThreatDisplay(ticket, User, flowStep.ControlDisplayText, CurrentEmployees, BusinessUnits, NotificationMethods, NotifierTypes, LogRun);

                //Subscribe to the form's events so we can respond to actions requested.
                ThreatDisplayForm.ActivityDetected += new EventHandler<EventArgs>((object sender, EventArgs e) => ThreatSecondsIdle = 0);
                ThreatDisplayForm.NextFlowStepSelected += new EventHandler<NextFlowStepEventArgs>(NextFlowStep);
                ThreatDisplayForm.SaveTicket += new EventHandler<SaveTicketEventArgs>(SaveTicket);
                ThreatDisplayForm.UpdateTicket += new EventHandler<UpdateTicketEventArgs>(UpdateTicket);
                ThreatDisplayForm.FormClosed += new FormClosedEventHandler((object sender, FormClosedEventArgs e) => DA.ReleaseLock(User, Ticket.THREAT));

                //All set. Show the ticket.
                ThreatDisplayForm.Show();
            }
            else
            {
                //There's already a form up. It just needs to know about the new ticket.
                ThreatDisplayForm.BindToNewTicket(ticket, flowStep.ControlDisplayText);
            }
            ThreatSecondsIdle = 0;
        }

        private string GetFlowId(string ticketType)
        {
            string flowId = (ticketType == Ticket.INCIDENT ? "IR" : "THR");
            return flowId;
        }

        private void NextFlowStep(object sender, NextFlowStepEventArgs e)
        {
            //Set some ticket properties based on the next flow step.
            List<FlowStep> flowSteps = DA.GetStepsForSpecifiedFlow(GetFlowId(e.Ticket.Type));
            FlowStep currentStep = flowSteps.Single(p => p.Status == e.Ticket.Status);
            if (flowSteps.Count() == currentStep.FlowStepSequenceNumber)
                return;
            FlowStep nextStep = flowSteps.Single(p => p.FlowStepSequenceNumber == currentStep.FlowStepSequenceNumber + 1);
            e.Ticket.Status = nextStep.Status;
            e.Ticket.Court = DA.GetSqlUsers(false).SingleOrDefault(p => p.ID == nextStep.StaffAssignment);

            //Add a history update noting the status change.
            string statusChangeDescription = nextStep.Description;
            if (!string.IsNullOrEmpty(nextStep.StaffAssignmentLegalName))
                statusChangeDescription = string.Format(statusChangeDescription, nextStep.StaffAssignmentLegalName);
            e.Ticket.AddHistoryRecord(User, e.Ticket.Status, statusChangeDescription, e.UpdateText);

            try
            {
                //Save the ticket and send an e-mail.
                e.Ticket.Save(DA, e.Ticket, e.DisplayForm);
                SendEmail(e.Ticket);

                //Close the form.
                if (e.Ticket.Type == Ticket.INCIDENT)
                {
                    if (IncidentDisplayForm != null && !IncidentDisplayForm.IsDisposed)
                        IncidentDisplayForm.Dispose();
                    if (IncidentEntryForm != null && !IncidentEntryForm.IsDisposed)
                        IncidentEntryForm.Dispose();
                }
                else if (e.Ticket.Type == Ticket.THREAT)
                {
                    if (ThreatDisplayForm != null && !ThreatDisplayForm.IsDisposed)
                        ThreatDisplayForm.Dispose();
                    if (ThreatEntryForm != null && !ThreatEntryForm.IsDisposed)
                        ThreatEntryForm.Dispose();
                }
            }
            catch (Exception ex)
            {
                Dialog.Error.Ok(ex.ToString(), "Error saving ticket!");
            }
        }

        private void SaveTicket(object sender, SaveTicketEventArgs e)
        {
            try
            {
                e.Ticket.Save(DA, e.Ticket, e.DisplayForm);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"There was an error saving the ticket: {e.Ticket.Number}", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Dialog.Error.Ok($"There was an error saving the ticket. Please contact System Support and reference Process Log Id: {LogRun.ProcessLogId}", "Error saving ticket!");
            }
        }

        private void SendEmail(Ticket ticket)
        {
            string subject = $"[{ticket.Priority}] {SUBSYSTEM_ABBREVIATION} {ticket.Number} {ticket.Status}";
            string body = $"Ticket Type: {ticket.Type}{Environment.NewLine}{Environment.NewLine}{ticket.History.AsString()}";
            string recipient = (ticket.Court == null ? ticket.Requester.EmailAddress : ticket.Court.EmailAddress);
            HashSet<string> cc = new HashSet<string>();
            if (User != ticket.Court)
                cc.Add(User.EmailAddress);
            if (ticket.Requester != ticket.Court)
                cc.Add(ticket.Requester.EmailAddress);
            if (cc.Contains(recipient))
                cc.Remove(recipient);
            //Get a list of all users in the Information Security AD groups
            List<string> IS_Email = DA.GetInfoSecurityEmail(CurrentEmployees);
            foreach (string item in IS_Email)
            {
                if (ticket.Court != null && ticket.Court.EmailAddress != item)
                    cc.Add(item);
            }
            EmailHelper.EmailImportance importance = (ticket.Priority == 9 ? EmailHelper.EmailImportance.High : EmailHelper.EmailImportance.Normal);
            EmailHelper.SendMail(DataAccessHelper.TestMode, recipient, "NeedHelp@utahsbr.edu", subject, body, String.Join(",", cc.ToArray()), importance, false);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CheckForIncidentTimeout();
            CheckForThreatTimeout();
        }

        private void UpdateTicket(object sender, UpdateTicketEventArgs e)
        {
            string updateText = e.UpdateText;
            if (!e.Ticket.CheckForSsn(e.Ticket, e.DisplayForm, ref updateText))
                return;
            e.Ticket.AddHistoryRecord(User, e.Ticket.Status, null, updateText);

            try
            {
                e.Ticket.Save(DA, e.Ticket, e.DisplayForm);
                SendEmail(e.Ticket);
            }
            catch (Exception ex)
            {
                Dialog.Error.Ok(ex.ToString(), "Error saving ticket!");
            }

            FlowStep flowStep = DA.GetStepsForSpecifiedFlow(GetFlowId(e.Ticket.Type)).Single(p => p.Status == e.Ticket.Status);
            if (e.Ticket.Type == Ticket.INCIDENT)
                IncidentDisplayForm.BindToNewTicket(e.Ticket, flowStep.ControlDisplayText);
            else if (e.Ticket.Type == Ticket.THREAT)
                ThreatDisplayForm.BindToNewTicket(e.Ticket, flowStep.ControlDisplayText);
        }
    }
}