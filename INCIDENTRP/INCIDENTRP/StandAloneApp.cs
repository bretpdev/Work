using NDesk.Options;
using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace INCIDENTRP
{
    public class StandAloneApp
    {
        private BorrowerDataInvolved BorrowerData { get; set; }
        private DataAccess DA { get; set; }
        private SecurityIncident IncidentEntryForm { get; set; }
        private ThreatCall ThreatEntryForm { get; set; }
        private SqlUser User { get; set; }

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
        private static ProcessLogRun LogRun { get; set; }

        [STAThread]
        public static void Main(string[] args)
        {
            //Parse the command-line arguments to see what kind of ticket to create.
            DataAccessHelper.Mode mode = DataAccessHelper.Mode.Live;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = mode;
#if DEBUG
            mode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentMode = mode;
            Dialog.Info.Ok($"Current mode: {mode}", "Current Mode");
#endif
            string accountNumber = null;
            string name = null;
            string state = null;
            string ticketType = null;
            OptionSet p = new OptionSet()
            {
                { "dev", "the dev mode to run the script in.", v => mode = DataAccessHelper.Mode.Dev },
                { "qa", "the QA mode to run the script in.", v => mode = DataAccessHelper.Mode.QA },
                { "test", "the Test mode to run the script in.", v => mode = DataAccessHelper.Mode.Test },
                { "accountnumber=", "the borrower's account number", v => accountNumber = v },
                { "name=", "the borrower's name", v => name = v },
                { "state=", "the borrower's state of residence", v => state = v },
                { "tickettype=", "the ticket type to create ('incident' or 'threat').", v => ticketType = v }
            };
            try
            {
                p.Parse(args.ToList().ConvertAll(d => d.ToLower()));
            }
            catch (OptionException e)
            {
                Dialog.Error.Ok(e.ToString(), "Incident Reporting");
                return;
            }

            //Check that all of the required arguments were present.
            List<string> missingArguments = new List<string>();
            if (string.IsNullOrEmpty(ticketType))
                missingArguments.Add("The --ticketType argument ('Incident' or 'Threat') must be provided.");

            if (missingArguments.Count > 0)
            {
                Dialog.Error.Ok(string.Join(Environment.NewLine, missingArguments.ToArray()), "Incident Reporting");
                return;
            }
            DataAccessHelper.CurrentMode = mode;
            LogRun = new ProcessLogRun("INCIDENTRP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            //Show the appropriate form to create the ticket.
            try
            {
                StandAloneApp app = new StandAloneApp(Environment.UserName.ToLower(), accountNumber, name, state);
                if (ticketType.ToLower() == "incident")
                    app.CreateIncidentTicket();
                else
                    app.CreateThreatTicket();
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Incident Reporting has run into an error creating a new ticket.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                string message = $"Incident Reporting has run into an error and cannot continue. Please contact System Support and reference Process Log Id: {LogRun.ProcessLogId}";
                Dialog.Error.Ok(message, "Incident Reporting");
            }
        }

        private StandAloneApp(string userId, string accountNumber, string name, string state)
        {
            DA = new DataAccess(LogRun);
            if (!string.IsNullOrEmpty(accountNumber))
            {
                BorrowerData = new BorrowerDataInvolved();
                BorrowerData.AccountNumber = accountNumber;
                BorrowerData.Name = name;
                BorrowerData.State = state;
            }
            User = DA.GetSqlUsers(false).Single(p => p.EmailAddress != null && p.EmailAddress.StartsWith(userId + "@"));
        }

        private void CancelTicket(object sender, CancelTicketEventArgs e)
        {
            DA.CancelTicket(User.ID, e.TicketType, e.CreateDateTime, DateTime.Now, e.AccountNumber);
            if (e.TicketType == Ticket.INCIDENT)
                IncidentEntryForm.Dispose();
            else if (e.TicketType == Ticket.THREAT)
                ThreatEntryForm.Dispose();
            DA.ReleaseLock(User, e.TicketType);
        }

        private void CreateIncidentTicket()
        {
            //Initialize any collections we'll need.
            BusinessUnits = BusinessUnits ?? DA.GetBusinessUnits().OrderBy(p => p.Name).ToList();
            CurrentEmployees = CurrentEmployees ?? DA.GetSqlUsers(false).OrderBy(p => p.FirstName).ToList();
            DataInvolvedOptions = DataInvolvedOptions ?? DA.GetDataInvolvedOptions();
            FunctionalAreas = FunctionalAreas ?? DA.GetBusinessFunctions();
            IncidentCauses = IncidentCauses ?? DA.GetIncidentCauses();
            IncidentTypes = IncidentTypes ?? DA.GetIncidentTypes();
            MembersOfComputerServicesAndInfoSecurity = MembersOfComputerServicesAndInfoSecurity ?? DA.GetMembersOfComputerServicesAndInfoSecurity();
            NotificationMethods = NotificationMethods ?? DA.GetNotificationMethods();
            NotifierTypes = NotifierTypes ?? DA.GetNotifierTypes();
            Regions = Regions ?? DA.GetRegions();
            Relationships = Relationships ?? DA.GetRelationships();
            States = States ?? DA.GetStateCodes(true);
            Urgencies = Urgencies ?? DA.GetUrgencies();

            //Assume that the current user is reporting the incident.
            Reporter reporter = new Reporter();
            reporter.User = User;
            reporter.BusinessUnit = DA.GetBusinessUnit(User);
            string windowsUserId = User.EmailAddress.Substring(0, User.EmailAddress.IndexOf("@"));
            reporter.PhoneNumber = DA.GetPhoneExtension(windowsUserId);

            //If we're already showing another ticket, close that one.
            IncidentEntryForm?.Dispose();

            //Create a new ticket and feed it to a new instance of the form.
            string initialStatus = DA.GetStepsForSpecifiedFlow(GetFlowId(Ticket.INCIDENT)).OrderBy(p => p.FlowStepSequenceNumber).First().Status;
            Ticket ticket = new Ticket(Ticket.INCIDENT, reporter, BorrowerData, initialStatus);
            ticket.Incident.Notifier.Method = Notifier.CALL;
            string accountNumber = BorrowerData?.AccountNumber;
            IncidentEntryForm = new SecurityIncident(ticket, accountNumber, IncidentCauses, DataInvolvedOptions, States, Regions, IncidentTypes, CurrentEmployees, BusinessUnits, NotificationMethods, NotifierTypes, Urgencies, MembersOfComputerServicesAndInfoSecurity, FunctionalAreas, Relationships);

            //Subscribe to the form's events so we can respond to actions requested.
            IncidentEntryForm.Cancel += new EventHandler<CancelTicketEventArgs>(CancelTicket);
            IncidentEntryForm.Submit += new EventHandler<NextFlowStepEventArgs>(NextFlowStep);

            //All set. Show the ticket.
            IncidentEntryForm.ShowDialog();
        }

        private void CreateThreatTicket()
        {
            //Initialize any collections we'll need.
            BusinessUnits = BusinessUnits ?? DA.GetBusinessUnits().OrderBy(p => p.Name).ToList();
            CurrentEmployees = CurrentEmployees ?? DA.GetSqlUsers(false).OrderBy(p => p.FirstName).ToList();
            FunctionalAreas = FunctionalAreas ?? DA.GetBusinessFunctions();
            NotificationMethods = NotificationMethods ?? DA.GetNotificationMethods();
            NotifierTypes = NotifierTypes ?? DA.GetNotifierTypes();

            //Assume that the current user is reporting the incident.
            Reporter reporter = new Reporter();
            reporter.User = User;
            reporter.BusinessUnit = DA.GetBusinessUnit(User);
            string windowsUserId = User.EmailAddress.Substring(0, User.EmailAddress.IndexOf("@"));
            reporter.PhoneNumber = DA.GetPhoneExtension(windowsUserId);

            //If we're already showing another ticket, close that one.
            ThreatEntryForm?.Dispose();

            //Create a new ticket and feed it to a new instance of the form.
            string initialStatus = DA.GetStepsForSpecifiedFlow(GetFlowId(Ticket.THREAT)).OrderBy(p => p.FlowStepSequenceNumber).First().Status;
            Ticket ticket = new Ticket(Ticket.THREAT, reporter, initialStatus);
            ticket.Threat.Caller.AccountNumber = BorrowerData?.AccountNumber;
            ticket.Threat.Notifier.Method = Notifier.CALL;
            ThreatEntryForm = new ThreatCall(ticket, CurrentEmployees, BusinessUnits, NotificationMethods, NotifierTypes, FunctionalAreas);

            //Subscribe to the form's events so we can respond to actions requested.
            ThreatEntryForm.Cancel += new EventHandler<CancelTicketEventArgs>(CancelTicket);
            ThreatEntryForm.Submit += new EventHandler<NextFlowStepEventArgs>(NextFlowStep);

            //All set. Show the ticket.
            ThreatEntryForm.ShowDialog();
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
                //Save the ticket.
                e.Ticket.Save(DA, e.Ticket, e.DisplayForm);

                //Close the form.
                if (e.Ticket.Type == Ticket.INCIDENT)
                {
                    if (IncidentEntryForm != null && !IncidentEntryForm.IsDisposed)
                        IncidentEntryForm.Dispose();
                }
                else if (e.Ticket.Type == Ticket.THREAT)
                {
                    if (ThreatEntryForm != null && !ThreatEntryForm.IsDisposed)
                        ThreatEntryForm.Dispose();
                }

                //Send an e-mail.
                string flowId = GetFlowId(e.Ticket.Type);
                string subject = $"[{e.Ticket.Priority}] {flowId} {e.Ticket.Number} ({e.Ticket.Status})";
                string body = $"Ticket Type: {e.Ticket.Type}{Environment.NewLine}{Environment.NewLine}{e.Ticket.History.AsString()}";
                string recipient = e.Ticket.Court.EmailAddress;
                HashSet<string> cc = new HashSet<string>();
                if (User != e.Ticket.Court)
                    cc.Add(User.EmailAddress);
                if (e.Ticket.Requester != e.Ticket.Court)
                    cc.Add(e.Ticket.Requester.EmailAddress);
                //Get a list of all users in the Information Security AD groups
                List<string> IS_Email = DA.GetInfoSecurityEmail(CurrentEmployees);
                foreach (string item in IS_Email)
                {
                    if (e.Ticket.Court.EmailAddress != item)
                        cc.Add(item);
                }
                EmailHelper.EmailImportance importance = (e.Ticket.Priority == 9 ? EmailHelper.EmailImportance.High : EmailHelper.EmailImportance.Normal);
                EmailHelper.SendMail(DataAccessHelper.TestMode, recipient, "NeedHelp@utahsbr.edu", subject, body, String.Join(",", cc.ToArray()), importance, false);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error saving the ticket.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Dialog.Error.Ok($"There was an error saving the ticket. Please contact System Support and reference Process Log Id: {LogRun.ProcessLogId}", "Error saving ticket!");
            }
        }
    }
}