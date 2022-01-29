using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    public class Ticket
    {

        public const string WITHDRAWN_STATUS_TEXT = "Withdrawn";
        public const string ON_HOLD_STATUS_TEXT = "Hold";
        public const string SUBMIT_STATUS_TEXT = "Submitting";

        private TicketData TheTicket { get; set; }
        private Flow Flow { get; set; }
        private List<FlowStep> FlowSteps { get; set; }
        private string UsersFullName { get; set; }
        private DataAccess DA { get; set; }
        private List<SqlUser> UserList { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public TicketAndFlowData Data
        {
            get
            {
                TicketAndFlowData data = new TicketAndFlowData();
                data.TheTicketData = TheTicket;
                data.TheFlowData = Flow;
                data.TheFlowStep = GetFlowStepForSpecifedStatus(TheTicket.Status);
                return data;
            }
        }

        /// <summary>
        /// Use this constructor for existing tickets.
        /// </summary>
        /// <param name="ticketNumber"></param>
        /// <param name="userID"></param>
        public Ticket(string ticketCode, long ticketNumber, SqlUser user, List<SqlUser> userList, ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun);
            UsersFullName = user.FirstName + " " + user.LastName;
            UserList = userList.ToList();
            //remove users locks on all other tickets
            //get ticket data from NeedHelpUheaa
            TheTicket = DA.GetTicket(ticketNumber, ticketCode);
            //get flow data
            Flow = DA.GetSpecifiedFlow(TheTicket.TicketCode);
            FlowSteps = DA.GetStepsForSpecifiedFlow(TheTicket.TicketCode);
            if (TheTicket.UserSelectedEmailRecipients.Count > 0)
            {
                //Check all users in the list and delete all the inactive employees, return new list of current employees
                TheTicket.UserSelectedEmailRecipients = DA.DeleteOldEmployees(TheTicket.UserSelectedEmailRecipients, TheTicket.TicketID);
            }
        }

        /// <summary>
        /// Use this constructor when a new ticket is desired.
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="requestor"></param>
        public Ticket(string ticketCode, SqlUser user, List<SqlUser> userList, ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun);
            UserList = userList.ToList();
            //remove users locks on all other tickets
            //create new ticket in NeedHelpUheaa
            long newTicketID = DA.CreateNewTicket(ticketCode, user);
            //add status/court history record
            string status = DA.GetStepsForSpecifiedFlow(ticketCode).FirstOrDefault().Status;
            DA.UpdateStatusAndOrCourtHistory(newTicketID, status, user.ID);
            //get ticket data from NeedHelpUheaa
            TheTicket = DA.GetTicket(newTicketID, ticketCode);
            //get flow data
            Flow = DA.GetSpecifiedFlow(TheTicket.TicketCode);
            FlowSteps = DA.GetStepsForSpecifiedFlow(TheTicket.TicketCode);
            if (TheTicket.UserSelectedEmailRecipients.Count > 0)
            {
                //Check all users in the list and delete all the inactive employees, return new list of current employees
                TheTicket.UserSelectedEmailRecipients = DA.DeleteOldEmployees(TheTicket.UserSelectedEmailRecipients, TheTicket.TicketID);
            }
        }

        /// <summary>
        /// Saves ticket.  Throws FlowChangeException if flow/ticket type is changed at any point past flow step one.
        /// </summary>
        /// <param name="ticketToBeSaved"></param>
        public bool Save(TicketData ticketToBeSaved, NeedHelpTickets ticketForm)
        {
            if (!CheckForSsn(ticketToBeSaved, ticketForm))
                return false;
            //do flow/ticket type change functionality steps if needed
            if (TheTicket.TicketCode != ticketToBeSaved.TicketCode)
            {
                //if not on first step of the flow then throw exception because the system can't do that.
                if (TheTicket.Status != SUBMIT_STATUS_TEXT)
                    throw new FlowChangeException("The flow or ticket type can only be changed while in a submitting status.  Please ensure that the ticket is in a submitting status before proceeding.");
                //change flow data if ticket type has changed.
                Flow = DA.GetSpecifiedFlow(ticketToBeSaved.TicketCode);
                FlowSteps = DA.GetStepsForSpecifiedFlow(ticketToBeSaved.TicketCode);
            }
            //update previous status and previous court fields
            UpdatePreviousCourtAndPreviousStatusAsWellAsCourtAndStatusDates(ticketToBeSaved);
            //Update History
            CheckForCourtChanges(ticketToBeSaved);
            //save the ticket
            DA.SaveTicket(ticketToBeSaved);
            //update status and/or court history if needed
            UpdateStatusAndOrCourtHistoryIfNeeded(ticketToBeSaved.Court, ticketToBeSaved.Status);
            //last but not least change current ticket data to new ticket data
            TheTicket = ticketToBeSaved;
            return true;
        }

        /// <summary>
        /// Updates the ticket and sends out a email notification.   Throws FlowChangeException if flow/ticket type is changed at any point past flow step one.
        /// </summary>
        /// <param name="ticketToBeUpdated"></param>
        public bool Update(TicketData ticketToBeUpdated, NeedHelpTickets ticketForm)
        {
            if (!CheckForSsn(ticketToBeUpdated, ticketForm))
                return false;
            if (TheTicket.TicketCode != ticketToBeUpdated.TicketCode)
                throw new FlowChangeException("The flow or ticket type can only be changed through a save.  After it is done through a save it can be moved to the next flow step or updated.");
            //anytime an update is provided be sure update text is provided
            if (string.IsNullOrEmpty(ticketToBeUpdated.IssueUpdate))
            {
                List<string> errorMessages = new List<string>();
                errorMessages.Add("An update must be provided");
                throw new InvalidUserInputException(errorMessages);
            }
            //update history
            UpdateHistory(ticketToBeUpdated, false);
            //update previous status and previous court fields
            UpdatePreviousCourtAndPreviousStatusAsWellAsCourtAndStatusDates(ticketToBeUpdated);
            //save ticket
            DA.SaveTicket(ticketToBeUpdated);
            //update status and/or court history if needed
            UpdateStatusAndOrCourtHistoryIfNeeded(ticketToBeUpdated.Court, ticketToBeUpdated.Status);
            //send email update
            SendEmailNotificationUpdate(ticketToBeUpdated);
            //last but not least change current ticket data to new ticket data
            TheTicket = ticketToBeUpdated;
            return true;
        }

        public bool ReturnForRevisions(TicketData ticketToBeReturned, NeedHelpTickets ticketForm)
        {
            //anytime an update is provided be sure update text is provided
            if (string.IsNullOrEmpty(ticketToBeReturned.IssueUpdate))
            {
                List<string> errorMessages = new List<string>();
                errorMessages.Add("An update must be provided");
                throw new InvalidUserInputException(errorMessages);
            }
            ticketToBeReturned.Court = ticketToBeReturned.Requester;
            return PreviousStatus("Submitting", ticketToBeReturned, ticketForm);
        }

        public bool PreviousStatus(string statusToChangeTo, TicketData ticketToBeUpdated, NeedHelpTickets ticketForm)
        {
            if (!CheckForSsn(ticketToBeUpdated, ticketForm))
                return false; ;
            //calculate court and assigned to if applicable for new status
            FlowCalculationProcessorCoordinator.CalculateNeededCourtAndAssignToValues(ticketToBeUpdated, GetFlowStepForSpecifedStatus(ticketToBeUpdated.Status), UserList.ToList(), LogRun);
            //anytime an update is provided be sure update text is provided
            if (string.IsNullOrEmpty(ticketToBeUpdated.IssueUpdate))
            {
                List<string> errorMessages = new List<string>();
                errorMessages.Add("An update must be provided");
                throw new InvalidUserInputException(errorMessages);
            }
            //Commenting out the DataValidator.IsValidData because we do not need to validate since we are going back, not forward.
            //do data validation (will throw exception for UI to catch if errors are found)
            //DataValidator.IsValidData(ticketToBeUpdated, Data.TheFlowStep);
            //calculate next step
            ticketToBeUpdated.Status = statusToChangeTo;
            //update history
            UpdateHistory(ticketToBeUpdated, false);
            //update previous status and previous court fields
            UpdatePreviousCourtAndPreviousStatusAsWellAsCourtAndStatusDates(ticketToBeUpdated);
            //save ticket
            DA.SaveTicket(ticketToBeUpdated);
            //update status and/or court history if needed
            UpdateStatusAndOrCourtHistoryIfNeeded(ticketToBeUpdated.Court, ticketToBeUpdated.Status);
            //send email update
            SendEmailNotificationUpdate(ticketToBeUpdated);
            //last but not least change current ticket data to new ticket data
            TheTicket = ticketToBeUpdated;
            return true;
        }

        public bool HoldAndRelease(TicketData ticketToBeUpdated, NeedHelpTickets ticketForm)
        {
            if (!CheckForSsn(ticketToBeUpdated, ticketForm))
                return false;
            //calculate next step
            if (TheTicket.Status == ON_HOLD_STATUS_TEXT)
            {
                ticketToBeUpdated.Status = TheTicket.PreviousStatus;
                ticketToBeUpdated.IssueUpdate = $"Releasing from hold: {Environment.NewLine}{Environment.NewLine}{ticketToBeUpdated.IssueUpdate}";
            }
            else
            {
                //if putting on hold then be sure update is provided
                if (string.IsNullOrEmpty(ticketToBeUpdated.IssueUpdate))
                {
                    List<string> errorMessages = new List<string>();
                    errorMessages.Add("An update must be provided");
                    throw new InvalidUserInputException(errorMessages);
                }
                ticketToBeUpdated.Status = ON_HOLD_STATUS_TEXT;
                ticketToBeUpdated.IssueUpdate = $"Ticket Placed On Hold: {Environment.NewLine}{Environment.NewLine}{ticketToBeUpdated.IssueUpdate}";
            }
            //update history
            UpdateHistory(ticketToBeUpdated, false);
            //update previous status and previous court fields
            UpdatePreviousCourtAndPreviousStatusAsWellAsCourtAndStatusDates(ticketToBeUpdated);
            //save ticket
            DA.SaveTicket(ticketToBeUpdated);
            //update status and/or court history if needed
            UpdateStatusAndOrCourtHistoryIfNeeded(ticketToBeUpdated.Court, ticketToBeUpdated.Status);
            //send email update
            SendEmailNotificationUpdate(ticketToBeUpdated);
            //last but not least change current ticket data to new ticket data
            TheTicket = ticketToBeUpdated;
            return true;
        }

        public bool Withdraw(TicketData ticketToBeUpdated, NeedHelpTickets ticketForm)
        {
            if (!CheckForSsn(ticketToBeUpdated, ticketForm))
                return false;
            //anytime user trys to withdraw ticket be sure update is provided
            if (string.IsNullOrEmpty(ticketToBeUpdated.IssueUpdate))
            {
                List<string> errorMessages = new List<string>();
                errorMessages.Add("A update must be provided");
                throw new InvalidUserInputException(errorMessages);
            }
            ticketToBeUpdated.IssueUpdate = $"Ticket Withdrawn: {Environment.NewLine}{Environment.NewLine}{ticketToBeUpdated.IssueUpdate}";
            //update history
            UpdateHistory(ticketToBeUpdated, false);
            //calculate next step
            ticketToBeUpdated.Status = WITHDRAWN_STATUS_TEXT;
            //blank court
            ticketToBeUpdated.Court = new SqlUser();
            //update previous status and previous court fields
            UpdatePreviousCourtAndPreviousStatusAsWellAsCourtAndStatusDates(ticketToBeUpdated);
            //save ticket
            DA.SaveTicket(ticketToBeUpdated);
            //update status and/or court history if needed
            UpdateStatusAndOrCourtHistoryIfNeeded(ticketToBeUpdated.Court, ticketToBeUpdated.Status);
            //send email update
            SendEmailNotificationUpdate(ticketToBeUpdated);
            //last but not least change current ticket data to new ticket data
            TheTicket = ticketToBeUpdated;
            return true;
        }

        /// <summary>
        /// Move to the next flow step. Throws FlowChangeException if flow/ticket type is changed.
        /// </summary>
        /// <param name="ticketToBeUpdated"></param>
        public bool NextStep(TicketData ticketToBeUpdated, NeedHelpTickets ticketForm)
        {
            if (TheTicket.TicketCode != ticketToBeUpdated.TicketCode)
                throw new FlowChangeException("The flow or ticket type can only be changed through a save.  After it is done through a save it can be moved to the next flow step or updated.");
            return MoveToNextStep(ticketToBeUpdated, ticketForm);
        }

        //moves ticket for one step to another
        private bool MoveToNextStep(TicketData ticketToBeUpdated, NeedHelpTickets ticketForm)
        {
            if (!CheckForSsn(ticketToBeUpdated, ticketForm))
                return false;
            //do data validation (will throw exception for UI to catch if errors are found)
            DataValidator.IsValidData(ticketToBeUpdated, Data.TheFlowStep);
            //calculate next step
            ticketToBeUpdated.Status = (from fs in FlowSteps
                                        where fs.FlowStepSequenceNumber == (Data.TheFlowStep.FlowStepSequenceNumber + 1)
                                        select fs.Status).SingleOrDefault<string>();
            //figure out if history needs issue included
            bool addIssueToHistory = false;
            if (TheTicket.Status == SUBMIT_STATUS_TEXT)
                addIssueToHistory = true;
            //update history
            UpdateHistory(ticketToBeUpdated, addIssueToHistory);
            //calculate court and assigned to if applicable for new status
            FlowCalculationProcessorCoordinator.CalculateNeededCourtAndAssignToValues(ticketToBeUpdated, GetFlowStepForSpecifedStatus(ticketToBeUpdated.Status), UserList.ToList(), LogRun);
            //update previous status and previous court fields
            UpdatePreviousCourtAndPreviousStatusAsWellAsCourtAndStatusDates(ticketToBeUpdated);
            //save ticket
            DA.SaveTicket(ticketToBeUpdated);
            //update status and/or court history if needed
            UpdateStatusAndOrCourtHistoryIfNeeded(ticketToBeUpdated.Court, ticketToBeUpdated.Status);
            //last but not least change current ticket data to new ticket data
            TheTicket = ticketToBeUpdated;
            return true;
        }

        //puts together the history text
        private void UpdateHistory(TicketData ticketToBeUpdated, bool includeIssueText)
        {
            StringBuilder newHistory = new StringBuilder();
            if (UserList[0].ID == 0)
                UserList.RemoveAt(0);
            newHistory.AppendFormat("{0} - {1} - {2}", ((SqlUser)UserList.Where(p => p.WindowsUserName.ToLower() == Environment.UserName.ToLower()).FirstOrDefault()).LegalName, DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), ticketToBeUpdated.Status);
            newHistory.AppendLine(string.Empty);
            if (!string.IsNullOrEmpty(ticketToBeUpdated.IssueUpdate))
            {
                //put together issue update if one was provided.
                newHistory.AppendLine(string.Empty);
                newHistory.Append(ticketToBeUpdated.IssueUpdate);
                newHistory.AppendLine(string.Empty);
            }
            if (includeIssueText)
            {
                newHistory.AppendLine(string.Empty);
                newHistory.AppendLine("Issue:");
                newHistory.Append(ticketToBeUpdated.Issue);
                newHistory.AppendLine(string.Empty);
            }
            //if resolution information is given then add it to the history
            if (string.IsNullOrEmpty(ticketToBeUpdated.ResolutionCause) == false ||
                string.IsNullOrEmpty(ticketToBeUpdated.ResolutionFix) == false ||
                string.IsNullOrEmpty(ticketToBeUpdated.ResolutionPrevention) == false)
            {
                newHistory.AppendLine(string.Empty);
                newHistory.AppendLine("Cause:");
                newHistory.AppendLine((string.IsNullOrEmpty(ticketToBeUpdated.ResolutionCause) ? string.Empty : ticketToBeUpdated.ResolutionCause));
                if (!string.IsNullOrEmpty(ticketToBeUpdated.ResolutionCause)) { newHistory.AppendLine(string.Empty); }
                newHistory.AppendLine("Fix:");
                newHistory.AppendLine((string.IsNullOrEmpty(ticketToBeUpdated.ResolutionFix) ? string.Empty : ticketToBeUpdated.ResolutionFix));
                if (!string.IsNullOrEmpty(ticketToBeUpdated.ResolutionFix)) { newHistory.AppendLine(string.Empty); }
                newHistory.AppendLine("Prevention:");
                newHistory.AppendLine((string.IsNullOrEmpty(ticketToBeUpdated.ResolutionPrevention) ? string.Empty : ticketToBeUpdated.ResolutionPrevention));
                newHistory.AppendLine(string.Empty);
            }
            newHistory.AppendLine();
            ticketToBeUpdated.NewHistoryComment = newHistory.ToString();
            ticketToBeUpdated.History = newHistory.ToString() + ticketToBeUpdated.History;
            ticketToBeUpdated.IssueUpdate = string.Empty;
        }

        /// <summary>
        /// Iterates through all the text being added to a ticket and checks to make sure there is not a 9 digit number
        /// </summary>
        /// <returns>True if no ssn being added or if SSN has been masked</returns>
        private bool CheckForSsn(TicketData ticket, NeedHelpTickets ticketForm)
        {
            SsnHelper helper = new SsnHelper(ticketForm);
            ticket.Issue = helper.MaskSsnIfExists(ticket.Issue);
            ticket.IssueUpdate = helper.MaskSsnIfExists(ticket.IssueUpdate);
            ticket.Subject = helper.MaskSsnIfExists(ticket.Subject);
            return helper.FormNotCanceled;
        }

        //updates status and/or court history if needed
        private void UpdateStatusAndOrCourtHistoryIfNeeded(SqlUser newCourt, string newStatus)
        {
            if (newStatus != null && newCourt != null && newCourt.ID != 0 && TheTicket.Court != null && TheTicket.Status != null)
                if (newCourt != null && (newCourt.ID != TheTicket.Court.ID || newStatus != TheTicket.Status))
                    DA.UpdateStatusAndOrCourtHistory(TheTicket.TicketID, newStatus, newCourt.ID);
        }

        //updates ticket level previous court and previous status
        private void UpdatePreviousCourtAndPreviousStatusAsWellAsCourtAndStatusDates(TicketData ticketToBeUpdated)
        {
            //Court
            if (ticketToBeUpdated.Court != TheTicket.Court)
            {
                ticketToBeUpdated.PreviousCourt = TheTicket.Court;
                ticketToBeUpdated.CourtDate = DateTime.Today;
            }
            else
            {
                ticketToBeUpdated.PreviousCourt = TheTicket.PreviousCourt;
                ticketToBeUpdated.CourtDate = TheTicket.CourtDate;
            }
            //Status
            if (ticketToBeUpdated.Status != TheTicket.Status)
            {
                ticketToBeUpdated.PreviousStatus = TheTicket.Status;
                ticketToBeUpdated.StatusDate = DateTime.Today;
            }
            else
            {
                ticketToBeUpdated.PreviousStatus = TheTicket.PreviousStatus;
                ticketToBeUpdated.StatusDate = TheTicket.StatusDate;
            }
        }

        //Gets flow step for a specific status
        public FlowStep GetFlowStepForSpecifedStatus(string status)
        {
            return (from fs in FlowSteps
                    where fs.Status == status
                    select fs).SingleOrDefault();
        }

        //sends email update notification
        public void SendEmailNotificationUpdate(TicketData ticketToBeUpdated)
        {
            //create subject line
            string subjectLine = $"[{ticketToBeUpdated.Priority}] UNH {ticketToBeUpdated.TicketID} - {ticketToBeUpdated.Subject} ({ticketToBeUpdated.Status})";
            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
            {
                //add test text to subject line if needed
                subjectLine = $"THIS IS A TEST - {subjectLine}";
            }
            //create body text
            string bodyText = $"Ticket Type: {Flow.ControlDisplayText}{Environment.NewLine}{Environment.NewLine}{ticketToBeUpdated.History}";

            HashSet<string> to = new HashSet<string>();
            if (ticketToBeUpdated.NotifyEmailList.Count > 0)
            {
                if (ticketToBeUpdated.NotifyEmailList[0] != null)
                    to.Add(String.Join(",", ticketToBeUpdated.NotifyEmailList.Where(s => s.EmailAddress != null).Select(p => p.EmailAddress).Distinct().ToArray()).ToString());
                else if (ticketToBeUpdated.NotifyEmailList.Count > 1 && ticketToBeUpdated.NotifyEmailList[0] == null)
                {
                    List<SqlUser> remove = new List<SqlUser>(ticketToBeUpdated.NotifyEmailList);
                    foreach (SqlUser item in remove)
                        if (item == null)
                            ticketToBeUpdated.NotifyEmailList.Remove(item);
                    foreach (SqlUser user in ticketToBeUpdated.NotifyEmailList)
                        to.Add(user.EmailAddress);
                }
            }
            HashSet<string> mailTo = new HashSet<string>(to);
            foreach (string item in mailTo)
            {
                if (item == "")
                    to.Remove(item);
            }
            string cc = String.Join(",", GetPriorityEmail(ticketToBeUpdated.Priority, ticketToBeUpdated).Distinct().ToArray());
            try
            {
                if (ticketToBeUpdated.TicketCode.ToLower() == "bac")
                    to.Add(EnterpriseFileSystem.GetPath("SecurityEmail"));
                if (to.Count > 0)
                    SendMail(String.Join(",", to.ToArray()), subjectLine, bodyText, cc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendMail(string to, string subject, string body, string cc)
        {
            try
            {
                SmtpClient client = new SmtpClient("mail.utahsbr.edu")
                {
                    Timeout = 20000
                };
                MailMessage message = new MailMessage();
                message.Priority = MailPriority.Normal;
                if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
                {
                    to = $"{to},{Environment.UserName}@utahsbr.edu";
                    subject += "-- THIS IS A TEST --";
                    body += "THIS IS A TEST \r\n";
                }
                message.From = new MailAddress("NeedHelp@utahsbr.edu");
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;
                foreach (string email in cc.Split(';').Where(p => p != null && p != ""))
                    message.To.Add(email);
                client.Send(message);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error sending the email.", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }

        private List<string> GetPriorityEmail(short priority, TicketData ticketToBeUpdated)
        {
            List<string> email = new List<string>();
            if (!ticketToBeUpdated.TicketCode.ToLower().IsIn("fac", "bac"))
            {
                List<PriorityEmail> pEmail = DA.GetPriorityEmail();
                foreach (PriorityEmail item in pEmail)
                    if (priority >= item.Priority)
                        email.Add(item.EmailAddress);
            }
            return email;
        }

        //checks if flow has changed and if it has then checks to be sure it is allowable at this point in the flow.  If not then throws FlowChangeException.
        private void ChangeFlowCheckAndChangeIfAllowable(TicketData ticketToBeUpdated)
        {
            if (TheTicket.TicketCode != ticketToBeUpdated.TicketCode)
            {
                //if not on first step then the ticket type/flow can't be changed.
                if (Data.TheFlowStep.FlowStepSequenceNumber != 1)
                    throw new FlowChangeException("The ticket must be in the very first step of processing in order for it's type to be changed.");
                else
                {
                    //get flow data
                    Flow = DA.GetSpecifiedFlow(ticketToBeUpdated.TicketCode);
                    FlowSteps = DA.GetStepsForSpecifiedFlow(ticketToBeUpdated.TicketCode);
                }
            }
        }

        private void CheckForCourtChanges(TicketData ticketToBeUpdated)
        {
            StringBuilder newHistory = new StringBuilder();
            newHistory.AppendFormat("{0} - {1} - {2}", ((SqlUser)UserList.Where(p => p.WindowsUserName.ToLower() == Environment.UserName.ToLower()).SingleOrDefault()).LegalName, DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), ticketToBeUpdated.Status);
            newHistory.AppendLine(string.Empty);
            bool didChange = false;

            if (ticketToBeUpdated.Court != null && TheTicket.Court != null && ticketToBeUpdated.Court.ID != TheTicket.Court.ID)
            {
                didChange = true;
                newHistory.AppendLine(string.Empty);
                newHistory.Append($"Court changed from {TheTicket.Court.LegalName} to {ticketToBeUpdated.Court.LegalName}");
                newHistory.AppendLine(string.Empty);
            }
            if (ticketToBeUpdated.Requester != null && TheTicket.Requester != null && ticketToBeUpdated.Requester.ID != TheTicket.Requester.ID)
            {
                didChange = true;
                newHistory.AppendLine(string.Empty);
                newHistory.Append($"Requester changed from {TheTicket.Requester.LegalName} to {ticketToBeUpdated.Requester.LegalName}");
                newHistory.AppendLine(string.Empty);
            }
            if (ticketToBeUpdated.AssignedTo != null && TheTicket.AssignedTo != null && ticketToBeUpdated.AssignedTo.ID != TheTicket.AssignedTo.ID)
            {
                didChange = true;
                newHistory.AppendLine(string.Empty);
                newHistory.Append($"Assigned To changed from {TheTicket.AssignedTo.LegalName} to {ticketToBeUpdated.AssignedTo.LegalName}");
                newHistory.AppendLine(string.Empty);
            }
            if (didChange)
            {
                newHistory.AppendLine();
                ticketToBeUpdated.NewHistoryComment = newHistory.ToString();
                ticketToBeUpdated.History = newHistory.ToString() + ticketToBeUpdated.History;
                ticketToBeUpdated.IssueUpdate = string.Empty;
            }
        }
    }
}