using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    public partial class NeedHelpTickets : Form
    {
        private Ticket ActiveTicket { get; set; }
        private List<SqlUser> UserList { get; set; }
        private List<string> UrgencyList { get; set; }
        private List<string> CategoryList { get; set; }
        private List<TextValueOption> UrgencyListForFacilities { get; set; }
        private List<TextValueOption> CategoryListForFacilities { get; set; }
        private List<string> FunctionalAreaList { get; set; }
        private List<BusinessUnit> BusinessUnitList { get; set; }
        private List<string> CauseList { get; set; }
        private List<string> SubjectList { get; set; }
        private List<string> SystemList { get; set; }
        private DataAccess DA { get; set; }
        List<SqlUser> EmailUser { get; set; }
        private SqlUser User { get; set; }
        BaseNeedHelpTicketDetail ControlToLoad = null;
        private string Role { get; set; }
        DateTime? LastStartTime { get; set; }
        TimeSpan Span { get; set; }
        public ProcessLogRun LogRun { get; set; }

        /// <summary>
        /// Constructor for General Tickets
        /// </summary>
        /// <param name="activeTicket"></param>
        /// <param name="businessUnit"></param>
        /// <param name="categoryList"></param>
        /// <param name="functionalList"></param>
        /// <param name="causeList"></param>
        /// <param name="subjectList"></param>
        /// <param name="systemList"></param>
        /// <param name="urgencyList"></param>
        /// <param name="userList"></param>
        public NeedHelpTickets(DataAccess dataAccess, SqlUser user, Ticket activeTicket, List<BusinessUnit> businessUnit, List<string> categoryList, List<string> functionalList, List<string> causeList, List<string> subjectList, List<string> systemList, List<string> urgencyList, List<SqlUser> userList, string role, ProcessLogRun logRun)
        {
            try
            {
                InitializeComponent();
                Version.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version} :: {(DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? DataAccessHelper.CurrentMode.ToString() : "")}";
                this.Text = $"UNH - {activeTicket.Data.TheTicketData.TicketID}";
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                DA = dataAccess;
                ActiveTicket = activeTicket;
                BusinessUnitList = businessUnit;
                CategoryList = categoryList;
                FunctionalAreaList = functionalList;
                CauseList = causeList;
                SubjectList = subjectList;
                SystemList = systemList;
                UrgencyList = urgencyList;
                UserList = userList.ToList();
                UserList.RemoveAll(p => p.WindowsUserName.ToUpper().Contains("TRAINING"));
                User = user;
                Role = role;
                LogRun = logRun;
                LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, true);
                lblTicketNumber.Text = ActiveTicket.Data.TheTicketData.TicketID + " - " + ActiveTicket.Data.TheFlowData.ControlDisplayText;
                BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                ChangeImages(activeTicket.Data.TheTicketData.Priority);
                UserList.Insert(0, new SqlUser());
                cboEmailRecipients.ComboBox.DataSource = UserList;
                cboEmailRecipients.ComboBox.DisplayMember = "LegalName";
                cboEmailRecipients.ComboBox.ValueMember = "ID";
                EmailUser = new List<SqlUser>(activeTicket.Data.TheTicketData.UserSelectedEmailRecipients);
                LoadPreviousStatusDropDown(activeTicket, dataAccess);
                SetNotifyType(activeTicket);
                SetTicketTimer();
                ToolTips();
                DisableSubmittingLinks(ActiveTicket.Data.TheFlowStep);
            }
            catch (Exception ex)
            {
                string message = $"There was an error opening the ticket. Please contact System Support with ticket number {activeTicket.Data.TheTicketData.TicketID}.";
                LogRun.AddNotification("There was an error opening the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Dialog.Error.Ok(message);
            }
        }

        /// <summary>
        /// Constructor for Change Form & DCR Request Tickets
        /// </summary>
        /// <param name="activeTicket"></param>
        /// <param name="businessUnit"></param>
        /// <param name="categoryList"></param>
        /// <param name="subjectList"></param>
        /// <param name="systemList"></param>
        /// <param name="urgencyList"></param>
        /// <param name="userList"></param>
        public NeedHelpTickets(DataAccess dataAccess, SqlUser user, Ticket activeTicket, List<BusinessUnit> businessUnit, List<string> categoryList, List<string> subjectList, List<string> systemList, List<string> urgencyList, List<SqlUser> userList, string role, ProcessLogRun logRun)
        {
            try
            {
                InitializeComponent();
                Version.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version} :: {(DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? DataAccessHelper.CurrentMode.ToString() : "")}";
                this.Text = $"UNH - {activeTicket.Data.TheTicketData.TicketID}";
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                DA = dataAccess;
                ActiveTicket = activeTicket;
                BusinessUnitList = businessUnit;
                CategoryList = categoryList;
                SubjectList = subjectList;
                SystemList = systemList;
                UrgencyList = urgencyList;
                UserList = userList.ToList();
                UserList.RemoveAll(p => p.WindowsUserName.ToUpper().Contains("TRAINING"));
                User = user;
                Role = role;
                LogRun = logRun;
                LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, true);
                lblTicketNumber.Text = ActiveTicket.Data.TheTicketData.TicketID + " - " + ActiveTicket.Data.TheFlowData.ControlDisplayText;
                BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                ChangeImages(activeTicket.Data.TheTicketData.Priority);
                List<SqlUser> users = new List<SqlUser>(UserList);
                users.Insert(0, new SqlUser());
                cboEmailRecipients.ComboBox.DataSource = users;
                cboEmailRecipients.ComboBox.DisplayMember = "LegalName";
                cboEmailRecipients.ComboBox.ValueMember = "ID";
                EmailUser = new List<SqlUser>(activeTicket.Data.TheTicketData.UserSelectedEmailRecipients);
                LoadPreviousStatusDropDown(activeTicket, dataAccess);
                SetNotifyType(activeTicket);
                SetTicketTimer();
                ToolTips();
                DisableSubmittingLinks(ActiveTicket.Data.TheFlowStep);
            }
            catch (Exception ex)
            {
                string message = $"There was an error opening the ticket. Please contact System Support with ticket number {activeTicket.Data.TheTicketData.TicketID}.";
                LogRun.AddNotification("There was an error opening the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Dialog.Error.Ok(message);
            }
        }

        /// <summary>
        /// Constructor for Facilities Tickets
        /// </summary>
        /// <param name="activeTicket"></param>
        /// <param name="categoryList"></param>
        /// <param name="subjectList"></param>
        /// <param name="urgencyList"></param>
        /// <param name="userList"></param>
        public NeedHelpTickets(DataAccess dataAccess, SqlUser user, Ticket activeTicket, List<TextValueOption> categoryListForFacilities, List<string> subjectList, List<TextValueOption> urgencyListForFacilities, List<SqlUser> userList, string role, ProcessLogRun logRun)
        {
            try
            {
                InitializeComponent();
                Version.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version} :: {(DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? DataAccessHelper.CurrentMode.ToString() : "")}";
                this.Text = $"UNH - {activeTicket.Data.TheTicketData.TicketID}";
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                DA = dataAccess;
                ActiveTicket = activeTicket;
                CategoryListForFacilities = categoryListForFacilities;
                SubjectList = subjectList;
                UrgencyListForFacilities = urgencyListForFacilities;
                UserList = userList.ToList();
                UserList.RemoveAll(p => p.WindowsUserName.ToUpper().Contains("TRAINING"));
                User = user;
                Role = role;
                LogRun = logRun;
                LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, true);
                lblTicketNumber.Text = ActiveTicket.Data.TheTicketData.TicketID + " - " + ActiveTicket.Data.TheFlowData.ControlDisplayText;
                BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                ChangeImages(activeTicket.Data.TheTicketData.Priority);
                List<SqlUser> users = new List<SqlUser>(UserList);
                users.Insert(0, new SqlUser());
                cboEmailRecipients.ComboBox.DataSource = users;
                cboEmailRecipients.ComboBox.DisplayMember = "LegalName";
                cboEmailRecipients.ComboBox.ValueMember = "ID";
                EmailUser = new List<SqlUser>(activeTicket.Data.TheTicketData.UserSelectedEmailRecipients);
                LoadPreviousStatusDropDown(activeTicket, dataAccess);
                SetNotifyType(activeTicket);
                SetTicketTimer();
                ToolTips();
                DisableSubmittingLinks(ActiveTicket.Data.TheFlowStep);
            }
            catch (Exception ex)
            {
                string message = $"There was an error opening the ticket. Please contact System Support with ticket number {activeTicket.Data.TheTicketData.TicketID}.";
                LogRun.AddNotification("There was an error opening the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Dialog.Error.Ok(message);
            }
        }

        /// <summary>
        /// Adds all the data in the active ticket to the current ticket being opened
        /// </summary>
        /// <param name="activeTicket">The active ticket selected from the search form</param>
        public void BindNewTicket(Ticket activeTicket)
        {
            ActiveTicket = activeTicket;
            LoadUserControl(activeTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);
            SetTicketTimer();
        }

        public void DisableSubmittingLinks(FlowStep step)
        {
            if (step != null && step.FlowStepSequenceNumber == 1)
            {
                lnkUpdate.Enabled = false;
                lnkReturn.Enabled = false;
                lnkHold.Enabled = false;
            }
        }

        /// <summary>
        /// Determines which user control to load and resizes the form to match the user control
        /// </summary>
        /// <param name="indicator">Determines which interface to display</param>
        private void LoadUserControl(string indicator, bool isCalledFromSubSystem)
        {
            //Get new existing ticket. This is any time LoadUserControl is called from anywhere other than the constructor
            if (!isCalledFromSubSystem)
            {
                long ticketNumber = ActiveTicket.Data.TheTicketData.TicketID;
                string ticketCode = ActiveTicket.Data.TheTicketData.TicketCode;
                ActiveTicket = null;
                ActiveTicket = new Ticket(ticketCode, ticketNumber, User, UserList, LogRun);
            }

            bool hasAcces = false;
            string system = "Need Help General Help";
            if (DA.HasAccess("DCR Subject Creator", system, User))
                hasAcces = true;

            switch (indicator)
            {
                case "Interface1":
                    if (ControlToLoad == null) { ControlToLoad = new General(ActiveTicket, BusinessUnitList, CategoryList, FunctionalAreaList, CauseList, SystemList, UrgencyList, UserList, LogRun); }
                    else
                        ControlToLoad.BindNewTicket(ActiveTicket);
                    break;
                case "Interface2":
                    if (ControlToLoad == null) { ControlToLoad = new ChangeRequest(ActiveTicket, BusinessUnitList, CategoryList, SystemList, UrgencyList, UserList, LogRun); }
                    else
                        ControlToLoad.BindNewTicket(ActiveTicket);
                    break;
                case "Interface3":
                    if (ControlToLoad == null) { ControlToLoad = new Building(ActiveTicket, CategoryListForFacilities, UrgencyListForFacilities, UserList, LogRun); }
                    else
                        ControlToLoad.BindNewTicket(ActiveTicket);
                    break;
                case "Interface4":
                    if (ControlToLoad == null) { ControlToLoad = new ChangeRequest(ActiveTicket, BusinessUnitList, CategoryList, SystemList, UrgencyList, UserList, LogRun); }
                    else
                        ControlToLoad.BindNewTicket(ActiveTicket);
                    break;
                case "Interface5":
                    if (ControlToLoad == null) { ControlToLoad = new DCRRequest(ActiveTicket, BusinessUnitList, CategoryList, SubjectList, SystemList, UrgencyList, UserList, hasAcces, LogRun); }
                    else
                        ControlToLoad.BindNewTicket(ActiveTicket);
                    break;
            }
            this.Text = $"UNH - {ActiveTicket.Data.TheTicketData.TicketID}";
            ControlToLoad.CategoryChanged += new EventHandler<BaseNeedHelpTicketDetail.PriorityOptionEventArgs>(CboCategory_SelectedIndexChanged);
            ControlToLoad.UrgencyChanged += new EventHandler<BaseNeedHelpTicketDetail.PriorityOptionEventArgs>(CboCategory_SelectedIndexChanged);
            ControlToLoad.EmailRecipientChanged += new EventHandler<BaseNeedHelpTicketDetail.EmailRecipientEvantArgs>(ResetEmailRecipients);

            SetNotifyType(ActiveTicket);
            lblTicketNumber.Text = ActiveTicket.Data.TheTicketData.TicketID + " - " + ActiveTicket.Data.TheFlowData.ControlDisplayText;
            LoadPreviousStatusDropDown(ActiveTicket, DA);
            //Checks to see if the ticket is in the right status
            if (!TicketDetailLinkButtonCoordinator.CalculateLinkButtonAppearanceAndStatus(DA, User, PutTogetherTicketDetailLinkButtonDataForCoord(ActiveTicket)))
            {
                //Allows SS to change the ticket status to the correct status
                if (DA.HasAccess("Status Update", system, User))
                {
                    List<FlowStep> steps = DA.GetStepsForSpecifiedFlow(ActiveTicket.Data.TheTicketData.TicketCode);
                    StatusUpdate update = new StatusUpdate(steps, ActiveTicket);
                    if (update.ShowDialog() != DialogResult.OK)
                        return;
                    ActiveTicket.Data.TheFlowStep = ActiveTicket.GetFlowStepForSpecifedStatus(ActiveTicket.Data.TheTicketData.Status);
                }
                else
                {
                    //Displays message to the user that the ticket is in the wrong status and needs SS to fix it
                    pnlTickets.Controls.Add(new Label()
                    {
                        Text = "This ticket has a status that is no longer a valid status. Please contact System Support at SSHELP@utahsbr.edu.",
                        Size = new Size(800, 100),
                        ForeColor = Color.White,
                        Location = new Point(50, 100),
                        Font = new Font(new FontFamily("Microsoft Sans Serif"), 20, FontStyle.Bold)
                    });
                    return;
                }
            }
            if (ActiveTicket.Data.TheFlowStep != null && ActiveTicket.Data.TheFlowStep.ControlDisplayText.Contains("Submit")) { ControlToLoad.SetIssueToReadOnly(false); }
            else { ControlToLoad.SetIssueToReadOnly(true); }
            SetUploadAbility();
            pnlTickets.Controls.Clear();
            pnlTickets.Controls.Add(ControlToLoad);
            if (ActiveTicket.Data.TheTicketData.TicketCode.ToLower() == "bac")
                ControlToLoad.SetPriority();
        }

        /// <summary>
        /// Saves the ticket and moves it to the next status according to the flow step it is in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkTicketStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //This will lock the buttons, process and unlock them when processing is done.
            LockButtons(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Worker));

            if (CheckNotifyType())
            {
                BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                NotifyType.Type type = GetNotifyType();
                TicketData dataToUpdate = activeControl.GetModifiedTicketData(activeControl.TheTicket, UserList, type);
                try
                {
                    if (!ActiveTicket.NextStep(dataToUpdate, this))
                        return;
                    //enable link buttons and disable buttons accordingly also change all needed link button text other than edit/save button.
                    TicketDetailLinkButtonCoordinator.CalculateLinkButtonAppearanceAndStatus(DA, User, PutTogetherTicketDetailLinkButtonDataForCoord(ActiveTicket));
                    if (type == NotifyType.Type.Individual)
                    {
                        dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.Text.ToUpper()).FirstOrDefault());
                        if (dataToUpdate.NotifyEmailList[0] == null)
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName == ((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).SelectedItem.ToString()).FirstOrDefault());
                    }
                    else if (type == NotifyType.Type.Court)
                    {
                        if (dataToUpdate.Court != null)
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == dataToUpdate.Court.LegalName.ToUpper()).FirstOrDefault());
                    }
                    else if (type == NotifyType.Type.All)
                    {
                        foreach (SqlUser item in dataToUpdate.UserSelectedEmailRecipients)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.ID == item.ID).SingleOrDefault());
                        }
                        if (dataToUpdate.Court != null && dataToUpdate.Court.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Court); }
                        if (dataToUpdate.Requester != null && dataToUpdate.Requester.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Requester); }
                        if (dataToUpdate.AssignedTo != null && dataToUpdate.AssignedTo.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.AssignedTo); }
                    }
                    ActiveTicket.SendEmailNotificationUpdate(dataToUpdate);
                    //Check if the new subject being added exists in the NeedHelpUheaa
                    if (dataToUpdate.TicketCode.Contains("DCR") && DA.CheckForNewSubject(dataToUpdate.Subject.Trim()).IsNullOrEmpty())
                    {
                        //Add the new subject if not already in NeedHelpUheaa
                        DA.AddNewSubjectForDCR(dataToUpdate.Subject, dataToUpdate.AssignProgrammer);
                    }
                    LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);
                    OnRefreshSearchResults(new RefreshSearchEventArgs());
                }
                catch (FlowChangeException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (InvalidUserInputException ex)
                {
                    if (ex.ErrorMessages != null && ex.ErrorMessages.Count > 0)
                    {
                        List<string> messages = ex.ErrorMessages;
                        string message = string.Empty;
                        for (int i = 0; i < messages.Count; i++)
                        {
                            message += messages[i].ToString() + "\n";
                        }
                        MessageBox.Show(message);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the lock from the ticket and closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Saves all the data in the ticket but does not move the ticket status to the next flow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //This will lock the buttons, process and unlock them when processing is done.
                LockButtons(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.Worker));

                SqlUser currentCourt = ActiveTicket.Data.TheTicketData.Court;
                try
                {
                    BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                    NotifyType.Type type = GetNotifyType();
                    TicketData dataToUpdate = activeControl.GetModifiedTicketData(ActiveTicket, UserList, type);
                    if (type == NotifyType.Type.Individual)
                    {
                        dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.Text.ToUpper()).FirstOrDefault());
                        if (dataToUpdate.NotifyEmailList[0] == null)
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName == ((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).SelectedItem.ToString()).FirstOrDefault());
                    }
                    else if (type == NotifyType.Type.Court)
                    {
                        dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == dataToUpdate.Court.LegalName.ToUpper()).FirstOrDefault());
                    }
                    else if (type == NotifyType.Type.All)
                    {
                        foreach (SqlUser item in dataToUpdate.UserSelectedEmailRecipients)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.ID == item.ID).SingleOrDefault());
                        }
                        if (dataToUpdate.Court != null && dataToUpdate.Court.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Court); }
                        if (dataToUpdate.Requester != null && dataToUpdate.Requester.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Requester); }
                        if (dataToUpdate.AssignedTo != null && dataToUpdate.AssignedTo.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.AssignedTo); }
                    }
                    if (!ActiveTicket.Save(dataToUpdate, this))
                        return;
                    LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);

                    if (dataToUpdate.TicketCode == "DCR" && DA.CheckForNewSubject(dataToUpdate.Subject) == string.Empty)
                    {
                        //Add the new subject if not already in NeedHelpUheaa
                        DA.AddNewSubjectForDCR(dataToUpdate.Subject, dataToUpdate.AssignProgrammer);
                    }
                    //just in case the ticket type changes be sure to update the ticket ticket type header
                    lblTicketNumber.Text = activeControl.TheTicket.Data.TheTicketData.TicketID + " - " + activeControl.TheTicket.Data.TheFlowData.ControlDisplayText;
                }
                catch (FlowChangeException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (currentCourt.ID != ActiveTicket.Data.TheTicketData.Court.ID)
                    OnRefreshSearchResults(new RefreshSearchEventArgs());

                //enable link buttons and disable buttons accordingly also change all needed link button text other than edit/save button.
                TicketDetailLinkButtonCoordinator.CalculateLinkButtonAppearanceAndStatus(DA, User, PutTogetherTicketDetailLinkButtonDataForCoord(ActiveTicket));
                DisableSubmittingLinks(ActiveTicket.Data.TheFlowStep);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error saving the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Saves all data and adds the new update to the history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //This will lock the buttons, process and unlock them when processing is done.
                LockButtons(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.Worker));

                if (CheckNotifyType())
                {
                    SqlUser currentCourt = ActiveTicket.Data.TheTicketData.Court;
                    BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                    NotifyType.Type type = GetNotifyType();
                    TicketData dataToUpdate = activeControl.GetModifiedTicketData(ActiveTicket, UserList, type);
                    string message = string.Empty;
                    try
                    {
                        if (type == NotifyType.Type.Individual)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.Text.ToUpper()).FirstOrDefault());
                            if (dataToUpdate.NotifyEmailList[0] == null)
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName == ((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).SelectedItem.ToString()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.Court)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == dataToUpdate.Court.LegalName.ToUpper()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.All)
                        {
                            foreach (SqlUser item in dataToUpdate.UserSelectedEmailRecipients)
                            {
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.ID == item.ID).SingleOrDefault());
                            }
                            if (dataToUpdate.Court != null && dataToUpdate.Court.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Court); }
                            if (dataToUpdate.Requester != null && dataToUpdate.Requester.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Requester); }
                            if (dataToUpdate.AssignedTo != null && dataToUpdate.AssignedTo.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.AssignedTo); }
                        }
                        if (!ActiveTicket.Update(dataToUpdate, this))
                            return;
                        LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);
                    }
                    catch (InvalidUserInputException ex)
                    {
                        List<string> messages = ex.ErrorMessages;
                        for (int i = 0; i < messages.Count; i++)
                        {
                            message += messages[i].ToString() + "\r\n";
                        }
                    }
                    catch (FlowChangeException ex)
                    {
                        message = ex.Message;
                    }
                    if (message != "") { MessageBox.Show(message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error); }

                    if (currentCourt.ID != ActiveTicket.Data.TheTicketData.Court.ID)
                        OnRefreshSearchResults(new RefreshSearchEventArgs());
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error updating the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            this.BringToFront();
        }

        /// <summary>
        /// Returns the ticket to submitting status to be started over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkReturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //This will lock the buttons, process and unlock them when processing is done.
                LockButtons(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.Worker));

                if (CheckNotifyType())
                {
                    BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                    NotifyType.Type type = GetNotifyType();
                    TicketData dataToUpdate = activeControl.GetModifiedTicketData(ActiveTicket, UserList, type);
                    try
                    {
                        if (type == NotifyType.Type.Individual)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.Text.ToUpper()).FirstOrDefault());
                            if (dataToUpdate.NotifyEmailList[0] == null)
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName == ((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).SelectedItem.ToString()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.Court)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == dataToUpdate.Court.LegalName.ToUpper()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.All)
                        {
                            foreach (SqlUser item in dataToUpdate.UserSelectedEmailRecipients)
                            {
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.ID == item.ID).SingleOrDefault());
                            }
                            if (dataToUpdate.Court != null && dataToUpdate.Court.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Court); }
                            if (dataToUpdate.Requester != null && dataToUpdate.Requester.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Requester); }
                            if (dataToUpdate.AssignedTo != null && dataToUpdate.AssignedTo.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.AssignedTo); }
                        }
                        if (!ActiveTicket.ReturnForRevisions(dataToUpdate, this))
                            return;
                        TicketDetailLinkButtonCoordinator.CalculateLinkButtonAppearanceAndStatus(DA, User, PutTogetherTicketDetailLinkButtonDataForCoord(ActiveTicket));
                        LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);
                    }
                    catch (InvalidUserInputException ex)
                    {
                        MessageBox.Show(string.Join(Environment.NewLine, ex.ErrorMessages.ToArray()), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error returning the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Places the ticket on hold
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkHold_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //This will lock the buttons, process and unlock them when processing is done.
                LockButtons(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.Worker));

                if (CheckNotifyType())
                {
                    BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                    NotifyType.Type type = GetNotifyType();
                    TicketData dataToUpdate = activeControl.GetModifiedTicketData(ActiveTicket, UserList, type);
                    try
                    {
                        if (type == NotifyType.Type.Individual)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.Text.ToUpper()).FirstOrDefault());
                            if (dataToUpdate.NotifyEmailList[0] == null)
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName == ((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).SelectedItem.ToString()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.Court)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == dataToUpdate.Court.LegalName.ToUpper()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.All)
                        {
                            foreach (SqlUser item in dataToUpdate.UserSelectedEmailRecipients)
                            {
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.ID == item.ID).SingleOrDefault());
                            }
                            if (dataToUpdate.Court != null && dataToUpdate.Court.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Court); }
                            if (dataToUpdate.Requester != null && dataToUpdate.Requester.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Requester); }
                            if (dataToUpdate.AssignedTo != null && dataToUpdate.AssignedTo.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.AssignedTo); }
                        }
                        if (lnkHold.Text == TicketDetailLinkButtonCoordinator.HOLD_TEXT)
                        {
                            if (MessageBox.Show("Is the ticket in the correct court?  Click OK to continue or cancel to go back and change the court.", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                            { return; }
                        }
                        if (!ActiveTicket.HoldAndRelease(dataToUpdate, this))
                            return;
                        TicketDetailLinkButtonCoordinator.CalculateLinkButtonAppearanceAndStatus(DA, User, PutTogetherTicketDetailLinkButtonDataForCoord(ActiveTicket));
                        LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);
                    }
                    catch (InvalidUserInputException ex)
                    {
                        MessageBox.Show(string.Join(Environment.NewLine, ex.ErrorMessages.ToArray()), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error placing the ticket on hold.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Withdraws the ticket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkWithdraw_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //This will lock the buttons, process and unlock them when processing is done.
                LockButtons(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.Worker));

                if (CheckNotifyType())
                {
                    BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                    NotifyType.Type type = GetNotifyType();
                    TicketData dataToUpdate = activeControl.GetModifiedTicketData(ActiveTicket, UserList, type);
                    try
                    {
                        if (type == NotifyType.Type.Individual)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.Text.ToUpper()).FirstOrDefault());
                            if (dataToUpdate.NotifyEmailList[0] == null)
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName == ((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).SelectedItem.ToString()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.Court)
                        {
                            dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == dataToUpdate.Court.LegalName.ToUpper()).FirstOrDefault());
                        }
                        else if (type == NotifyType.Type.All)
                        {
                            foreach (SqlUser item in dataToUpdate.UserSelectedEmailRecipients)
                            {
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.ID == item.ID).SingleOrDefault());
                            }
                            if (dataToUpdate.Court != null && dataToUpdate.Court.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Court); }
                            if (dataToUpdate.Requester != null && dataToUpdate.Requester.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Requester); }
                            if (dataToUpdate.AssignedTo != null && dataToUpdate.AssignedTo.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.AssignedTo); }
                        }
                        if (!ActiveTicket.Withdraw(dataToUpdate, this))
                            return;
                        TicketDetailLinkButtonCoordinator.CalculateLinkButtonAppearanceAndStatus(DA, User, PutTogetherTicketDetailLinkButtonDataForCoord(ActiveTicket));
                        LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);
                    }
                    catch (InvalidUserInputException ex)
                    {
                        MessageBox.Show(string.Join(Environment.NewLine, ex.ErrorMessages.ToArray()), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error withdrawing the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Sends the ticket to the status that is selected in the Previous Status drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkPrevious_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //This will lock the buttons, process and unlock them when processing is done.
                LockButtons(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.Worker));

                if (CheckNotifyType())
                {
                    if (cboPreviousStatus.Text == "")
                    {
                        MessageBox.Show("Please provide a previous status.", "Information Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        BaseNeedHelpTicketDetail activeControl = ControlToLoad;
                        NotifyType.Type type = GetNotifyType();
                        TicketData dataToUpdate = activeControl.GetModifiedTicketData(ActiveTicket, UserList, type);
                        string message = string.Empty;
                        try
                        {
                            if (type == NotifyType.Type.Individual)
                            {
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.Text.ToUpper()).FirstOrDefault());
                                if (dataToUpdate.NotifyEmailList[0] == null)
                                    dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName == ((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).SelectedItem.ToString()).FirstOrDefault());
                            }
                            else if (type == NotifyType.Type.Court)
                            {
                                dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.LegalName.ToUpper() == dataToUpdate.Court.LegalName.ToUpper()).FirstOrDefault());
                            }
                            else if (type == NotifyType.Type.All)
                            {
                                foreach (SqlUser item in dataToUpdate.UserSelectedEmailRecipients)
                                {
                                    dataToUpdate.NotifyEmailList.Add(UserList.Where(p => p.ID == item.ID).SingleOrDefault());
                                }
                                if (dataToUpdate.Court != null && dataToUpdate.Court.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Court); }
                                if (dataToUpdate.Requester != null && dataToUpdate.Requester.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.Requester); }
                                if (dataToUpdate.AssignedTo != null && dataToUpdate.AssignedTo.EmailAddress != string.Empty) { dataToUpdate.NotifyEmailList.Add(dataToUpdate.AssignedTo); }
                            }
                            if (!ActiveTicket.PreviousStatus(cboPreviousStatus.Text, dataToUpdate, this))
                                return;
                            TicketDetailLinkButtonCoordinator.CalculateLinkButtonAppearanceAndStatus(DA, User, PutTogetherTicketDetailLinkButtonDataForCoord(ActiveTicket));
                            LoadUserControl(ActiveTicket.Data.TheFlowData.UserInterfaceDisplayIndicator, false);
                            LoadPreviousStatusDropDown(ActiveTicket, DA);
                        }
                        catch (InvalidUserInputException ex)
                        {
                            List<string> messages = ex.ErrorMessages;
                            for (int i = 0; i < messages.Count; i++)
                            {
                                message += messages[i].ToString() + "\r\n";
                            }
                            MessageBox.Show(message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error opening the previous ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Puts together a TicketDetailLinkButtonData object to be used for LinkButton coordination.
        /// </summary>
        /// <param name="theTicket">The activeTicket</param>
        /// <returns>TicketDetailLinkButtonData object</returns>
        public TicketDetailLinkButtonData PutTogetherTicketDetailLinkButtonDataForCoord(Ticket theTicket)
        {
            TicketDetailLinkButtonData buttonData = new TicketDetailLinkButtonData();
            buttonData.StatusChanger = lnkTicketStatus;
            buttonData.EditOrSave = lnkSave;
            buttonData.Hold = lnkHold;
            buttonData.PreviousStatus = lnkPrevious;
            buttonData.Return = lnkReturn;
            buttonData.Withdraw = lnkWithdraw;
            buttonData.UpdateLink = lnkUpdate;
            buttonData.TheTicket = theTicket;
            return buttonData;
        }

        /// <summary>
        /// changes the image for the ticket priority
        /// </summary>
        private void ChangeImages(short priorityNumber)
        {
            switch (priorityNumber)
            {
                case 1:
                    pbxPriority.Image = Properties.Resources.Priority1;
                    break;
                case 2:
                    pbxPriority.Image = Properties.Resources.Priority2;
                    break;
                case 3:
                    pbxPriority.Image = Properties.Resources.Priority3;
                    break;
                case 4:
                    pbxPriority.Image = Properties.Resources.Priority4;
                    break;
                case 5:
                    pbxPriority.Image = Properties.Resources.Priority5;
                    break;
                case 6:
                    pbxPriority.Image = Properties.Resources.Priority6;
                    break;
                case 7:
                    pbxPriority.Image = Properties.Resources.Priority7;
                    break;
                case 8:
                    pbxPriority.Image = Properties.Resources.Priority8;
                    break;
                case 9:
                    pbxPriority.Image = Properties.Resources.Priority9;
                    break;
                case 0:
                    pbxPriority.Image = null;
                    break;
            }

            pbxCompanyLogo.Image = Properties.Resources.UheaaLogo;
        }

        /// <summary>
        /// Adds the selected user in the cboRecipients drop down to the email list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddRecipient_Click(object sender, EventArgs e)
        {
            if (cboEmailRecipients.Text.Trim() != string.Empty)
            {
                SqlUser emailUser = UserList.Where(p => p.LegalName.ToUpper() == cboEmailRecipients.ComboBox.Text.ToUpper()).FirstOrDefault();
                if ((emailUser != null && emailUser.LegalName.IsPopulated()) && (!((ListBox)ControlToLoad.Controls["lbxEmailRecip"]).Items.Contains(emailUser.LegalName)))
                {
                    EmailUser.Add(emailUser);
                    ControlToLoad.SetEmailRecipientDataSource(EmailUser);
                }
            }
            else
            {
                MessageBox.Show("Please select an email recipient from the drop down in the top left corner.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Removes all the email recipients attached to the ticket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetEmailRecipients(object sender, BaseNeedHelpTicketDetail.EmailRecipientEvantArgs e)
        {
            foreach (SqlUser user in e.EmailUsers)
            {
                EmailUser.Remove(EmailUser.Where(p => p.ID == user.ID).SingleOrDefault());
            }
        }

        /// <summary>
        /// Selects the category and sets up the priorities that go with the category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboCategory_SelectedIndexChanged(object sender, BaseNeedHelpTicketDetail.PriorityOptionEventArgs e)
        {
            string category = e.Category;
            string urgency = e.Urgency;
            string ticketType = e.TicketType;
            short priority = (ticketType.IsIn("FAC", "BAC") ? DA.GetCalculatedPriorityForFacilities(category, urgency) : DA.GetCalculatedPriority(category, urgency));
            ActiveTicket.Data.TheTicketData.Priority = priority;
            ChangeImages(priority);
        }

        /// <summary>
        /// Uploads a file to the ticket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                lblUploadComplete.Visible = false;
                string fullFileName = string.Empty;
                using (fileDialog)
                {
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        fullFileName = fileDialog.FileName;
                    }
                    else { return; }
                }
                string fileName = Path.GetFileName(fullFileName);
                string savedFilePath = string.Empty;

                savedFilePath = string.Format(@"{0} - {1}\{2}", EnterpriseFileSystem.GetPath("NHUHTicket", DataAccessHelper.Region.Uheaa), ActiveTicket.Data.TheTicketData.TicketID.ToString(), fileName);

                string folder = savedFilePath.Remove(savedFilePath.LastIndexOf('\\'));
                //Check if the folder exits and create a new one if not
                if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }
                //check if the file needs to be incremented
                if (File.Exists(savedFilePath)) { savedFilePath = GetFileNumbering(savedFilePath); }
                //Copy the file into the ticket folder
                File.Copy(fullFileName, savedFilePath, false);
                //Check if the file copied and display message if it did
                if (File.Exists(savedFilePath)) { lblUploadComplete.Visible = true; }
                int indexOf = savedFilePath.LastIndexOf('\\') + 1;
                DateTime ftime = DateTime.Now;
                ControlToLoad.AddUploadedFile(new AttachedFile(savedFilePath));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error uploading the file to the ticket.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Any file that is already attached and is being attached again will be incremented with a number on the end
        /// </summary>
        /// <param name="savedFilePath">The path and file name of the file being uploaded</param>
        /// <returns>The path and file name with an incremented number on the end</returns>
        private string GetFileNumbering(string savedFilePath)
        {
            int number = 1;
            savedFilePath = savedFilePath.Insert(savedFilePath.LastIndexOf('.'), number.ToString());
            while (File.Exists(savedFilePath))
            {
                int incrementNumber = ++number;
                savedFilePath = savedFilePath.Remove(savedFilePath.LastIndexOf('.') - 1, 1);
                savedFilePath = savedFilePath.Insert(savedFilePath.LastIndexOf('.'), incrementNumber.ToString());
            }
            return savedFilePath;
        }

        /// <summary>
        /// Notifies user if the ticket is locked and by who
        /// </summary>
        private void SetTextAndGiveLockedPopup()
        {
            string userName = DA.GetLockedTicketUser(ActiveTicket);
            string lockedText = "This ticket has been locked by " + userName;
            this.Text += " - " + lockedText;
            MessageBox.Show(lockedText);
        }

        /// <summary>
        /// Loads all the previous flow steps that are available to move back to
        /// </summary>
        /// <param name="activeTicket"></param>
        /// <param name="dataAccess"></param>
        private void LoadPreviousStatusDropDown(Ticket activeTicket, DataAccess dataAccess)
        {
            cboPreviousStatus.Text = "";
            List<FlowStep> steps = dataAccess.GetStepsForSpecifiedFlow(activeTicket.Data.TheFlowData.FlowID);
            cboPreviousStatus.Items.Clear();
            if (steps.Count > 0)
                cboPreviousStatus.Items.Add("");
            foreach (FlowStep step in steps)
            {
                if (activeTicket.Data.TheTicketData.Status == step.Status)
                {
                    return;
                }
                else
                {
                    cboPreviousStatus.Items.Add(step.Status);
                }
            }
        }

        /// <summary>
        /// Changes the email to be sent to the court the ticket is in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdoCourt_CheckedChanged(object sender, EventArgs e)
        {
            btnAddRecipient.Enabled = true;
            cboEmailRecipients.BackColor = Color.White;
        }

        /// <summary>
        /// Adds all users attached to the ticket to the email list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdoAll_CheckedChanged(object sender, EventArgs e)
        {
            btnAddRecipient.Enabled = true;
            cboEmailRecipients.BackColor = Color.White;
        }

        /// <summary>
        /// Allows user to select an individual to receive the email update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdoIndividual_CheckedChanged(object sender, EventArgs e)
        {
            btnAddRecipient.Enabled = false;
            cboEmailRecipients.BackColor = Color.White;
        }

        /// <summary>
        /// List of users to that can be selected to receive email updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboEmailRecipients_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboEmailRecipients.BackColor = Color.White;
        }

        /// <summary>
        /// Validates that a user was chosen from the cboEmailRecipients drop down if the individual radio button was selected
        /// </summary>
        /// <returns>True if user was selected, false if not</returns>
        private bool CheckNotifyType()
        {
            if (rdoIndividual.Checked && cboEmailRecipients.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please provide a recipient to receive an email", "Recipient Required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                cboEmailRecipients.BackColor = Color.Yellow;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks which flow step the ticket is in and sets the email selection radio button accordingly
        /// </summary>
        /// <param name="activeTicket"></param>
        private void SetNotifyType(Ticket activeTicket)
        {
            string type = string.Empty;
            if (activeTicket.Data.TheFlowStep == null || (activeTicket.Data.TheFlowStep != null && activeTicket.Data.TheFlowStep.NotificationType == null))
                type = "Court";
            else
                type = activeTicket.Data.TheFlowStep.NotificationType.ToString();
            if (type == NotifyType.Type.Court.ToString())
            {
                rdoCourt.Checked = true;
            }
            else if (type == NotifyType.Type.All.ToString())
            {
                rdoAll.Checked = true;
            }
            else if (type == NotifyType.Type.Individual.ToString())
            {
                rdoIndividual.Checked = true;
            }
        }

        /// <summary>
        /// Checks which radio button was selected and set the NotifyType
        /// </summary>
        /// <returns>The NotifyType</returns>
        private NotifyType.Type GetNotifyType()
        {
            if (rdoCourt.Checked)
            {
                return NotifyType.Type.Court;
            }
            else if (rdoAll.Checked)
            {
                return NotifyType.Type.All;
            }
            else if (rdoIndividual.Checked)
            {
                return NotifyType.Type.Individual;
            }
            return NotifyType.Type.Court;
        }

        /// <summary>
        /// Turns off the ability to upload files if the ticket is withdrawn or completed
        /// </summary>
        private void SetUploadAbility()
        {
            int maxSequence = DA.GetMaxSequenceNumber(ActiveTicket.Data.TheFlowData.FlowID);
            if (ActiveTicket.Data.TheTicketData.Status == "Withdrawn" || (ActiveTicket.Data.TheFlowStep != null && ActiveTicket.Data.TheFlowStep.FlowStepSequenceNumber == maxSequence))
            {
                btnUploadFile.Enabled = false;
                btnAddRecipient.Enabled = false;
                lnkUpdate.Enabled = false;
                lnkSave.Enabled = true;
                rdoAll.Enabled = false;
                rdoCourt.Enabled = false;
                rdoIndividual.Enabled = false;
                foreach (Control item in ControlToLoad.Controls)
                {
                    item.Enabled = false;
                }
                ControlToLoad.Controls["txtHistory"].Enabled = true;
                ControlToLoad.Controls["lnkExport"].Enabled = true;
                ControlToLoad.Controls["lbxEmailRecip"].Enabled = true;
                if (ControlToLoad.Controls.ContainsKey("cboBussUnit"))
                {
                    ControlToLoad.Controls["cboBussUnit"].Enabled = true;
                }
                if (ControlToLoad.Controls["lbxCurrentFiles"] != null)
                    ControlToLoad.Controls["lbxCurrentFiles"].Enabled = true;
            }
        }

        /// <summary>
        /// Starts and stops the timer and stopwatch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartStop_Click(object sender, EventArgs e)
        {
            if (HasTimerAccess())
            {
                try
                {
                    if (StartStop.Text == "Start")
                    {
                        if (DA.StartTime(ActiveTicket, User))
                        {
                            StartStop.Text = "Stop";
                            StartTimer.Start();
                        }
                    }
                    else
                    {
                        if (DA.StopTime(ActiveTicket, User))
                        {
                            StartStop.Text = "Start";
                            StartTimer.Stop();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogRun.AddNotification("There was an error starting or stopping the timer on the ticket", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }
            }
        }

        /// <summary>
        /// Updates the time label to count up every second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTimer_Tick(object sender, EventArgs e)
        {
            if (HasTimerAccess())
            {
                Span = Span.Add(new TimeSpan(0, 0, 1));
                int days = Span.Days;
                string hours = Span.Hours.ToString();
                string minutes = Span.Minutes.ToString();
                string seconds = Span.Seconds.ToString();
                TimeElapsed.Text = (days > 0 ? days.ToString() + " Days " : "") + hours + ":" + minutes + ":" + seconds;
            }
        }

        /// <summary>
        /// Figures out if the ticket is already being timed and then sets the time label to the amount of time it has been timing
        /// </summary>
        private void SetTicketTimer()
        {
            if (HasTimerAccess())
            {
                //if the span is null, create a new one
                Span = new TimeSpan();
                IsTimerRunning();
                Span = Span.Add(DA.GetTicketTime(ActiveTicket, User));
                int days = Span.Days;
                string hours = Span.Hours.ToString();
                string minutes = Span.Minutes.ToString();
                string seconds = Span.Seconds.ToString();
                TimeElapsed.Text = (days > 0 ? days.ToString() + " Days " : "") + hours + ":" + minutes + ":" + seconds;
            }
        }

        /// <summary>
        /// Checks to see if there is a start time but no end time and adds that time to the timer
        /// </summary>
        private void IsTimerRunning()
        {
            if (HasTimerAccess())
            {
                //Check to see if there is a start time with no end time
                LastStartTime = DA.CheckIfTimerIsRunning(ActiveTicket.Data.TheTicketData.TicketID, User);

                if (LastStartTime != null)
                {
                    //Get the amount of time the ticket has been open by subtracting that time from the current time
                    Span = Span.Add(DateTime.Now.Subtract(LastStartTime.Value));
                    StartStop.Text = "Stop";
                    //watch.Start();
                    StartTimer.Start();
                }
            }
        }

        /// <summary>
        /// Checks to see if the user has access to the timer
        /// </summary>
        /// <returns></returns>
        private bool HasTimerAccess()
        {
            if (DA.HasAccess("Ticket Start Timer", "Need Help General Help", User))
            {
                StartStop.Enabled = true;
                TimeElapsed.Enabled = true;
                TimerSeparator.Enabled = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Locks the buttons so users can't click a button twice. It's easier to teach people how to use
        /// a computer but we'll do it this way because it's hard to teach to the unteachable.
        /// </summary>
        private void LockButtons(bool isEnabled)
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(() => this.LockButtons(isEnabled)));
            else
                this.Enabled = isEnabled;
        }

        private void Worker(object state)
        {
            System.Threading.Thread.Sleep(2000);

            this.LockButtons(true); ;
        }

        /// <summary>
        /// This will set the tool tips for the controls on the form
        /// </summary>
        private void ToolTips()
        {
            ToolTip myTip = new ToolTip();
            myTip.SetToolTip(lnkReturn, "This will return the ticket to the previous status");
            myTip.SetToolTip(lnkSave, "This will save the ticket but does not change the status");
            myTip.SetToolTip(lnkTicketStatus, "This will update the ticket and change it to the next status");
            myTip.SetToolTip(lnkUpdate, "This will update the ticket, send an email and keep in the same status");
            myTip.SetToolTip(lnkHold, "This will place the ticket on hold locking the ticket down");
            myTip.SetToolTip(lnkWithdraw, "This will withdraw the ticket");
            myTip.SetToolTip(lnkPrevious, "This will change the ticket status to the selected status in the drop down");
        }

        #region Custom Events

        public event EventHandler<ChangeTicketEventArgs> FirstTicketRequested;
        protected virtual void OnFirstTicketRequested(ChangeTicketEventArgs e)
        {
            if (FirstTicketRequested != null) { FirstTicketRequested(this, e); }
        }

        public event EventHandler<ChangeTicketEventArgs> LastTicketRequested;
        protected virtual void OnLastTicketRequested(ChangeTicketEventArgs e)
        {
            if (LastTicketRequested != null) { LastTicketRequested(this, e); }
        }

        public event EventHandler<ChangeTicketEventArgs> NextTicketRequested;
        protected virtual void OnNextTicketRequested(ChangeTicketEventArgs e)
        {
            if (NextTicketRequested != null) { NextTicketRequested(this, e); }
        }

        public event EventHandler<ChangeTicketEventArgs> PreviousTicketRequested;
        protected virtual void OnPreviousTicketRequested(ChangeTicketEventArgs e)
        {
            if (PreviousTicketRequested != null) { PreviousTicketRequested(this, e); }
        }

        public event EventHandler<RefreshSearchEventArgs> RefreshSearchResults;
        protected virtual void OnRefreshSearchResults(RefreshSearchEventArgs e)
        {
            if (RefreshSearchResults != null) { RefreshSearchResults(this, e); }
        }

        #endregion

        private void cboPreviousStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPreviousStatus.Text.IsPopulated())
                lnkPrevious.Enabled = true;
            else
                lnkPrevious.Enabled = false;
        }
    }
}