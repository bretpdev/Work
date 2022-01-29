using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SubSystemShared;
using System.IO;
using System.Data.Linq;
using System.DirectoryServices;
using System.Collections.Specialized;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace NHGeneral
{
    public class DataAccess : DataAccessBaseShared
    {
        public ProcessLogRun LogRun { get; set; }
        public LogDataAccess LDA { get; set; }

        /// <summary>
        /// These must match the options in the table exactly
        /// </summary>
        private enum PersonAssociationType
        {
            AssignedTo,
            PreviousCourt,
            Court,
            Requester
        }

        /// <summary>
        /// These must match the options in the table exactly
        /// </summary>   
        public enum AssignToOptions
        {
            SystemSupportSpecialist,
            SystemSupportAnalyst,
            SystemSupport,
            Programmer
        }

        public DataAccess(ProcessLogRun logRun)
            : base()
        {
            LogRun = logRun;
            LDA = logRun.LDA;
        }

        /// <summary>
        /// Gets a list of the Commercial subjects that will be loaded into the Search Engine drop down for Subject
        /// </summary>
        /// <returns>List of all subjects for the NeedHelp Commercial</returns>
        public List<string> GetNeedHelpSubjects()
        {
            return LDA.ExecuteList<string>("spGetCommercialSubject", NeedHelpUheaa).Result;
        }

        /// <summary>
        /// Does all searching for tickets based off passed in criteria.
        /// </summary>
        /// <param name="criteria">SearchCriteria object created by SearchEngine</param>
        /// <returns>List of SearchResultsTicket</returns>
        public List<SearchResultItem> TicketSearchingProcessor(SearchCriteria criteria)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            if (criteria.TicketNumber != null && criteria.TicketNumber != "") { parms.Add(new SqlParameter("TicketNumber", long.Parse(criteria.TicketNumber))); }
            parms.Add(new SqlParameter("FunctionalArea", criteria.FunctionalArea));
            parms.Add(new SqlParameter("KeyWord", criteria.KeyWord));
            parms.Add(new SqlParameter("KeyWordScope", criteria.KeyWordSearchScope == null ? null : criteria.KeyWordSearchScope.ToString()));
            if (criteria.AssignedTo != null && criteria.AssignedTo.ID != 0) { parms.Add(new SqlParameter("AssignedTo", criteria.AssignedTo.ID)); }
            if (criteria.Court != null && criteria.Court.ID != 0) { parms.Add(new SqlParameter("Court", criteria.Court.ID)); }
            if (criteria.TicketType != null && criteria.TicketType.Abbreviation != "") { parms.Add(new SqlParameter("Type", criteria.TicketType.Abbreviation)); }
            if (criteria.Requester != null && criteria.Requester.ID != 0) { parms.Add(new SqlParameter("Requester", criteria.Requester.ID)); }
            parms.Add(new SqlParameter("Status", criteria.Status));
            if (criteria.Subject != null && criteria.Subject != "") { parms.Add(new SqlParameter("Subject", criteria.Subject)); }
            if (criteria.BusinessUnit != null && criteria.BusinessUnit.Name != "") { parms.Add(new SqlParameter("BusinessUnit", criteria.BusinessUnit.ID)); }
            if (criteria.CreateDateRangeStart != default(DateTime)) { parms.Add(new SqlParameter("BeginCreateDate", criteria.CreateDateRangeStart)); }
            if (criteria.CreateDateRangeEnd != default(DateTime)) { parms.Add(new SqlParameter("EndCreateDate", criteria.CreateDateRangeEnd.AddHours(23))); }

            return LDA.ExecuteList<SearchResultItem>("spSearchForTickets", NeedHelpUheaa,
                parms.ToArray()).Result;
        }

        public List<SearchResultItem> GetOpenTicket(SearchCriteria criteria)
        {
            return LDA.ExecuteList<SearchResultItem>("spSearchForTickets", NeedHelpUheaa,
                SqlParams.Single("SearchOption", "OpenTickets"),
                SqlParams.Single("Type", criteria.TicketType != null ? criteria.TicketType.Abbreviation : "")).Result;
        }

        public List<SearchResultItem> GetAllTickets(SearchCriteria criteria)
        {
            return LDA.ExecuteList<SearchResultItem>("spSearchForTickets", NeedHelpUheaa,
                SqlParams.Single("SearchOption", "AllTickets"),
                SqlParams.Single("Type", criteria.TicketType != null ? criteria.TicketType.Abbreviation : "")).Result;
        }

        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        public long CreateNewTicket(string flowID, SqlUser user)
        {
            BusinessUnit businessUnit = LDA.ExecuteSingle<BusinessUnit>("spGENR_GetBusinessUnit", Csys,
                SqlParams.Single("SqlUserId", user.ID)).Result;
            string status = GetStepsForSpecifiedFlow(flowID).FirstOrDefault().Status;
            return LDA.ExecuteSingle<long>("spCreateNewTicket", NeedHelpUheaa,
                SqlParams.Single("FlowId", flowID),
                SqlParams.Single("Requester", user.ID),
                SqlParams.Single("BU", businessUnit.ID),
                SqlParams.Single("Status", status)).Result;
        }

        public List<TicketType> GetTicketTypes()
        {
            return LDA.ExecuteList<TicketType>("spNDHP_GetTicketType", Csys,
                SqlParams.Single("FederalSystem", false)).Result;
        }

        public List<string> GetFunctionalArea()
        {
            return LDA.ExecuteList<string>("spGENR_GetBusinessFunctions", Csys).Result;
        }

        public List<string> GetStatus()
        {
            List<string> statuses = LDA.ExecuteList<string>("spNDHP_GetStatus", Csys).Result;
            //These need to be added manually becuase they are not part of the normal flow processing
            statuses.Add(Ticket.ON_HOLD_STATUS_TEXT);
            statuses.Add(Ticket.WITHDRAWN_STATUS_TEXT);
            statuses.Sort();
            return statuses;
        }

        public List<string> GetCause()
        {
            return LDA.ExecuteList<string>("spGetResolutionCauses", NeedHelpUheaa).Result;
        }

        /// <summary>
        /// Gets all data for a given ticket.
        /// </summary>
        /// <param name="ticketID"></param>
        /// <returns></returns>
        public TicketData GetTicket(long ticketID, string ticketCode)
        {
            List<SqlUser> users = LDA.ExecuteList<SqlUser>("spSYSA_GetSqlUsers", Csys,
               SqlParams.Single("IncludeInactiveRecords", false)).Result;
            FlattenedTicketData flattenedTicket = LDA.ExecuteSingle<FlattenedTicketData>("spGetTicket", NeedHelpUheaa, SqlParams.Single("TicketID", ticketID),
                SqlParams.Single("TicketCode", ticketCode)).Result;
            TicketData selectedTicket = new TicketData();
            selectedTicket.Area = flattenedTicket.Area;
            selectedTicket.CatOption = flattenedTicket.CatOption;
            selectedTicket.CCCIssue = flattenedTicket.CCCIssue;
            selectedTicket.Comments = flattenedTicket.Comments;
            selectedTicket.CourtDate = flattenedTicket.CourtDate;
            selectedTicket.History = flattenedTicket.History;
            selectedTicket.Issue = flattenedTicket.Issue;
            selectedTicket.IssueUpdate = flattenedTicket.IssueUpdate;
            selectedTicket.LastUpdated = flattenedTicket.LastUpdated;
            selectedTicket.PreviousCourt = users.SingleOrDefault(p => p.ID == flattenedTicket.PreviousCourt);
            selectedTicket.PreviousStatus = flattenedTicket.PreviousStatus;
            selectedTicket.Priority = flattenedTicket.Priority;
            selectedTicket.RelatedProcedures = flattenedTicket.RelatedProcedures;
            selectedTicket.RelatedQCIssues = flattenedTicket.RelatedQCIssues;
            selectedTicket.Requested = flattenedTicket.Requested;
            selectedTicket.Requester = users.SingleOrDefault(p => p.ID == flattenedTicket.RequesterID);
            selectedTicket.RequestProjectNum = flattenedTicket.RequestProjectNum;
            selectedTicket.Required = flattenedTicket.Required;
            selectedTicket.ResolutionCause = flattenedTicket.ResolutionCause;
            selectedTicket.ResolutionFix = flattenedTicket.ResolutionFix;
            selectedTicket.ResolutionPrevention = flattenedTicket.ResolutionPrevention;
            selectedTicket.Status = flattenedTicket.Status;
            selectedTicket.StatusDate = flattenedTicket.StatusDate;
            selectedTicket.Subject = flattenedTicket.Subject;
            selectedTicket.TicketCode = flattenedTicket.TicketCode;
            selectedTicket.TicketID = flattenedTicket.TicketID;
            selectedTicket.Unit = GetBusinessUnits().SingleOrDefault(p => p.ID == flattenedTicket.Unit);
            selectedTicket.UrgencyOption = flattenedTicket.UrgencyOption;
            selectedTicket.AssignedTo = users.SingleOrDefault(p => p.ID == flattenedTicket.AssignedToID);
            selectedTicket.Court = users.SingleOrDefault(p => p.ID == flattenedTicket.CourtID);
            selectedTicket.Requester = users.SingleOrDefault(p => p.ID == flattenedTicket.RequesterID);
            selectedTicket.UserSelectedEmailRecipients = GetUserSelectedEmailRecipientsForTicket(ticketID);
            selectedTicket.Systems = GetSystemsForTicket(flattenedTicket.TicketID).ToList();
            selectedTicket.AssignProgrammer = CheckForSubjectBoolean(selectedTicket.Subject);

            //Remove users no longer working here
            if (!users.Contains(selectedTicket.AssignedTo)) { selectedTicket.AssignedTo = null; }
            if (!users.Contains(selectedTicket.Court)) { selectedTicket.Court = null; }
            if (!users.Contains(selectedTicket.PreviousCourt)) { selectedTicket.PreviousCourt = null; }
            if (!users.Contains(selectedTicket.Requester)) { selectedTicket.Requester = null; }

            return selectedTicket;
        }

        //gets all user selected email recipients from NeedHelpUheaa
        private List<SqlUser> GetUserSelectedEmailRecipientsForTicket(long ticketID)
        {
            return LDA.ExecuteList<SqlUser>("spGetUserSelectedEmailRecipientsForTicket", NeedHelpUheaa,
                SqlParams.Single("TicketID", ticketID)).Result;
        }

        //gets systems for a ticket;
        private List<string> GetSystemsForTicket(long ticketID)
        {
            return LDA.ExecuteList<string>("spGetSystemsForTicket", NeedHelpUheaa,
                SqlParams.Single("TicketID", ticketID)).Result;
        }

        public List<string> GetListOfSystems()
        {
            return LDA.ExecuteList<string>("spGetListOfSystems", NeedHelpUheaa).Result;
        }

        /// <summary>
        /// Saves ticket.
        /// </summary>
        /// <param name="ticketToUpdate">Ticket to save.</param>
        public void SaveTicket(TicketData ticketToUpdate)
        {
            //Save ticket
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("Area", ticketToUpdate.Area));
            parms.Add(new SqlParameter("CatOption", ticketToUpdate.CatOption));
            parms.Add(new SqlParameter("CCCIssue", ticketToUpdate.CCCIssue));
            parms.Add(new SqlParameter("Comments", ticketToUpdate.Comments));
            parms.Add(new SqlParameter("CourtDate", ticketToUpdate.CourtDate));
            parms.Add(new SqlParameter("History", string.IsNullOrEmpty(ticketToUpdate.NewHistoryComment) ? "" : ticketToUpdate.NewHistoryComment));
            parms.Add(new SqlParameter("Issue", ticketToUpdate.Issue));
            parms.Add(new SqlParameter("IssueUpdate", ticketToUpdate.IssueUpdate));
            parms.Add(new SqlParameter("PreviousStatus", ticketToUpdate.PreviousStatus));
            parms.Add(new SqlParameter("Priority", ticketToUpdate.Priority));
            parms.Add(new SqlParameter("RelatedQCIssues", ticketToUpdate.RelatedQCIssues));
            parms.Add(new SqlParameter("RelatedProcedures", ticketToUpdate.RelatedProcedures));
            parms.Add(new SqlParameter("RequestProjectNum", ticketToUpdate.RequestProjectNum));
            parms.Add(new SqlParameter("Required", ticketToUpdate.Required));
            parms.Add(new SqlParameter("ResolutionCause", ticketToUpdate.ResolutionCause));
            parms.Add(new SqlParameter("ResolutionFix", ticketToUpdate.ResolutionFix));
            parms.Add(new SqlParameter("ResolutionPrevention", ticketToUpdate.ResolutionPrevention));
            parms.Add(new SqlParameter("Status", ticketToUpdate.Status));
            parms.Add(new SqlParameter("StatusDate", ticketToUpdate.StatusDate));
            parms.Add(new SqlParameter("Subject", ticketToUpdate.Subject));
            parms.Add(new SqlParameter("TicketCode", ticketToUpdate.TicketCode));
            parms.Add(new SqlParameter("TicketID", ticketToUpdate.TicketID.ToString()));
            parms.Add(new SqlParameter("Unit", ticketToUpdate.Unit == null ? 0 : ticketToUpdate.Unit.ID));
            parms.Add(new SqlParameter("UrgencyOption", ticketToUpdate.UrgencyOption));
            LDA.Execute("spSaveTicket", NeedHelpUheaa, parms.ToArray());

            //Update the DCR subject status for programmer boolean value
            if (ticketToUpdate.TicketCode == "DCR") { UpdateSubjectBoolean(ticketToUpdate.Subject, ticketToUpdate.AssignProgrammer); }
            //save user associations to ticket
            SaveAndDeleteAssociatedUsersToTicket(ticketToUpdate);
            //remove all recipients from ticket
            RemoveRecipients(ticketToUpdate.TicketID);
            //add back recipients selected by the user
            AddRecipients(ticketToUpdate.TicketID, ticketToUpdate.UserSelectedEmailRecipients);
            //remove systems
            RemoveSystems(ticketToUpdate.TicketID);
            //add back systems selected by the user
            AddSystems(ticketToUpdate.TicketID, ticketToUpdate.Systems);
        }

        //saves associated users to ticket
        private void SaveAndDeleteAssociatedUsersToTicket(TicketData ticketToUpdate)
        {
            //Assigned To
            if (ticketToUpdate.AssignedTo != null) { SaveAndDeleteUserAssociationToTicket(ticketToUpdate.AssignedTo.ID, PersonAssociationType.AssignedTo, ticketToUpdate.TicketID); }
            //Previous Court
            if (ticketToUpdate.PreviousCourt != null) { SaveAndDeleteUserAssociationToTicket(ticketToUpdate.PreviousCourt.ID, PersonAssociationType.PreviousCourt, ticketToUpdate.TicketID); }
            //Court
            if (ticketToUpdate.Court != null) { SaveAndDeleteUserAssociationToTicket(ticketToUpdate.Court.ID, PersonAssociationType.Court, ticketToUpdate.TicketID); }
            //Requester
            if (ticketToUpdate.Requester != null) { SaveAndDeleteUserAssociationToTicket(ticketToUpdate.Requester.ID, PersonAssociationType.Requester, ticketToUpdate.TicketID); }
        }

        //saves or deletes a single users association to a ticket
        private void SaveAndDeleteUserAssociationToTicket(int sqlUserId, PersonAssociationType type, long ticketID)
        {
            if (sqlUserId == 0)
            {
                LDA.Execute("spDeleteAssociationOfUserToTicket", NeedHelpUheaa,
                    SqlParams.Single("TicketID", ticketID),
                    SqlParams.Single("AssociationType", Enum.GetName(typeof(PersonAssociationType), type)));
            }
            else
            {
                LDA.Execute("spSaveAssociatedUserToTicket", NeedHelpUheaa,
                    SqlParams.Single("TicketID", ticketID),
                    SqlParams.Single("AssociationType", Enum.GetName(typeof(PersonAssociationType), type)),
                    SqlParams.Single("SqlUserId", sqlUserId));
            }
        }

        /// <summary>
        /// Closes current status/court history record and creates a new one. 
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="newStatus"></param>
        /// <param name="newCourt"></param>
        public void UpdateStatusAndOrCourtHistory(long ticketID, string newStatus, int newCourt)
        {
            LDA.Execute("spStatusAndOrCourtUpdate", NeedHelpUheaa,
                SqlParams.Single("Ticket", ticketID),
                SqlParams.Single("Status", newStatus),
                SqlParams.Single("Agent", newCourt));
        }

        /// <summary>
        /// Gets next two staff members to be assigned to tickets
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public List<int> GetNextTwoAssignedToOptions(AssignToOptions option)
        {
            return LDA.ExecuteList<int>("spNextSystemSupportSpecialistForAssignment", NeedHelpUheaa,
                SqlParams.Single("AssignmentOption", Enum.GetName(typeof(AssignToOptions), option))).Result;
        }

        /// <summary>
        /// Gets All DCR Subject Options.
        /// </summary>
        /// <returns></returns>
        public List<DCRSubjectOption> GetDCRSubjectOptions()
        {
            return LDA.ExecuteList<DCRSubjectOption>("spGetAllDCRSubjectOptions", NeedHelpUheaa).Result;
        }

        /// <summary>
        /// <summary>
        /// Gets the manager's windows network id associated with the business unit.
        /// Get Category options list for Need Help
        /// </summary>
        /// </summary>
        /// <param name="businessUnit"></param>
        /// <returns></returns>
        public string GetManager(BusinessUnit businessUnit)
        {
            string manager = "";
            try
            {
                manager = LDA.ExecuteSingle<string>("spNDHP_GetManager", Csys,
                    SqlParams.Single("BusinessUnit", businessUnit.ID)).Result;
            }
            catch (InvalidOperationException ex)
            {
                string message = string.Format("Need Help is attempting to get the manager for the business unit {0} but there is not a manager listed in SYSA_DAT_Users in CSYS. Please contact System Support", businessUnit.Name);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            return manager;
        }

        /// <summary>
        /// Gets the category options for interfaces 1, 2, 4, & 5
        /// </summary>
        /// <returns></returns>
        public List<string> GetCategoryOptions()
        {
            return LDA.ExecuteList<string>("spNDHP_GetCategoryOptions", Csys).Result;
        }

        /// <summary>
        /// Gets list of Category options for interface 5
        /// </summary>n
        /// <returns></returns>
        public List<TextValueOption> GetCategoryOptionsForFacilities()
        {
            return LDA.ExecuteList<TextValueOption>("spGetFacilitiesCategory", NeedHelpUheaa).Result;
        }

        /// <summary>
        /// Gets list of Urgency options interfaces 1, 2, 4 & 5
        /// </summary>
        /// <returns></returns>
        public List<string> GetUrgencyOptions()
        {
            return LDA.ExecuteList<string>("spNDHP_GetUrgencyOptions", Csys).Result;
        }

        /// <summary>
        /// Gets list of Urgency options for interface 3
        /// </summary>
        /// <returns></returns>
        public List<TextValueOption> GetUrgencyOptionsForFacilities()
        {
            return LDA.ExecuteList<TextValueOption>("spGetFacilitiesUrgencies", NeedHelpUheaa).Result;
        }

        /// <summary>
        /// Calculates the priority according to the category and urgency for interfaces 1, 2, 4 & 5
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public short GetCalculatedPriority(string category, string urgency)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(urgency))
                return 0;
            else
            {
                if (!string.IsNullOrEmpty(category))
                    param.Add(new SqlParameter("CatOption", category));
                if (!string.IsNullOrEmpty(urgency))
                    param.Add(new SqlParameter("UrgOption", urgency));
                try
                {
                    return LDA.ExecuteList<short>("spNDHP_GetPriority", Csys,
                        param.ToArray()).Result.FirstOrDefault();
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Calculates the priority according to the category and urgency for interface 3
        /// </summary>
        /// <param name="category"></param>
        /// <param name="urgency"></param>
        /// <returns></returns>
        public short GetCalculatedPriorityForFacilities(string category, string urgency)
        {
            if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(urgency))
                return 9; //Ticket Type BAC will always be empty in order to have a 9 priority
            else
            {
                List<SqlParameter> param = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(category))
                    param.Add(new SqlParameter("CatOption", LDA.ExecuteList<string>("spGetFacilitiesCategoryOption", NeedHelpUheaa,
                        SqlParams.Single("CatOption", category)).Result.FirstOrDefault()));
                if (!string.IsNullOrEmpty(urgency))
                    param.Add(new SqlParameter("UrgOption", LDA.ExecuteList<string>("spGetFacilitiesUrgencyOption", NeedHelpUheaa,
                        SqlParams.Single("UrgOption", urgency)).Result.FirstOrDefault()));
                try
                {
                    return LDA.ExecuteList<short>("spGetFacilitiesPriority", NeedHelpUheaa,
                        param.ToArray()).Result.FirstOrDefault();
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets previous status from history table if needed.
        /// </summary>
        /// <param name="ticketID"></param>
        /// <returns></returns>
        public string GetPreviousStatusFromHistoryTable(long ticketID)
        {
            return LDA.ExecuteSingle<string>("spGetPreviousStatusFromHistoryTable", NeedHelpUheaa,
                SqlParams.Single("TicketID", ticketID)).Result;
        }

        /// <summary>
        /// Removes all recipients for a given ticket
        /// </summary>
        /// <param name="ticketCode"></param>
        public void RemoveRecipients(long ticketID)
        {
            LDA.Execute("spRemoveAllRecipients", NeedHelpUheaa,
                SqlParams.Single("TicketID", ticketID));
        }

        /// <summary>
        /// Removes all systems associated with a ticket
        /// </summary>
        /// <param name="ticketID"></param>
        public void RemoveSystems(long ticketID)
        {
            LDA.Execute("spRemoveAllSystems", NeedHelpUheaa,
                SqlParams.Single("TicketID", ticketID));
        }

        /// <summary>
        /// Adds all the recipients for a given ticket
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <param name="recipients"></param>
        public void AddRecipients(long ticketID, List<SqlUser> recipients)
        {
            foreach (SqlUser recip in recipients)
            {
                LDA.Execute("spAddRecipients", NeedHelpUheaa,
                    SqlParams.Single("TicketID", ticketID),
                    SqlParams.Single("SqlUserId", recip.ID));
            }
        }

        /// <summary>
        /// Adds systems to a ticket
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="systems"></param>
        public void AddSystems(long ticketID, List<string> systems)
        {
            foreach (string sys in systems)
            {
                LDA.Execute("spAddSystem", NeedHelpUheaa,
                    SqlParams.Single("TicketID", ticketID),
                    SqlParams.Single("System", sys));
            }
        }

        public List<string> GetSubjectList()
        {
            return LDA.ExecuteList<string>("spGetSubjectList", NeedHelpUheaa).Result;
        }

        public bool AddNewSubjectForDCR(string subject, bool assignProgrammer)
        {
            try
            {
                LDA.Execute("spAddNewSubjectForDCR", NeedHelpUheaa,
                    SqlParams.Single("Subject", subject),
                    SqlParams.Single("AssignProgrammer", assignProgrammer));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string CheckForNewSubject(string subject)
        {
            try
            {
                return LDA.ExecuteSingle<string>("spCheckForNewSubject", NeedHelpUheaa,
                    SqlParams.Single("Subject", subject)).Result;
            }
            catch (SqlException)
            {
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public bool CheckForSubjectBoolean(string subject)
        {
            try
            {
                if (LDA.ExecuteSingle<bool>("spCheckForSubjectBoolean", NeedHelpUheaa,
                    SqlParams.Single("Subject", subject)).Result)
                    return true;
            }
            catch (Exception)
            { }
            return false;
        }

        public void UpdateSubjectBoolean(string subject, bool isProgrammer)
        {
            try
            {
                LDA.Execute("spUpdateSubjectBoolean", NeedHelpUheaa,
                    SqlParams.Single("Subject", subject),
                    SqlParams.Single("isProgrammer", isProgrammer));
            }
            catch (Exception)
            {
            }
        }

        public string GetLockedTicketUser(Ticket activeTicet)
        {
            return LDA.ExecuteSingle<string>("spGetLockedTicketUser", NeedHelpUheaa,
                SqlParams.Single("TicketID", activeTicet.Data.TheTicketData.TicketID)).Result;
        }

        public bool GetUserKeyAssignment(string role, string userKey)
        {
            try
            {
                return LDA.ExecuteSingle<bool>("spSYSA_CheckIfRoleHasAccess", Csys,
                    SqlParams.Single("UserKey", userKey),
                    SqlParams.Single("RoleName", role)).Result;
            }
            catch (Exception)
            { }
            return false;
        }

        public DateTime GetEarliestCreateDate()
        {
            return LDA.ExecuteSingle<DateTime>("spGetEarliestCreateDate", NeedHelpUheaa).Result;
        }

        public List<SqlUser> DeleteOldEmployees(List<SqlUser> users, long TicketId)
        {
            List<SqlUser> currentEmployees = LDA.ExecuteList<SqlUser>("spSYSA_GetSqlUsers", Csys,
                SqlParams.Single("IncludeInactiveRecords", true)).Result;
            List<SqlUser> usersToDelete = new List<SqlUser>();
            //Remove all the former employees from the ticket
            foreach (SqlUser emailUser in users)
            {
                SqlUser tempUser = currentEmployees.Where(p => p.ID == emailUser.ID && p.Status == "Inactive").SingleOrDefault();
                if (tempUser != null) { usersToDelete.Add(tempUser); }
            }
            if (usersToDelete.Count > 0)
            {
                foreach (SqlUser removeUser in usersToDelete)
                {
                    users.Remove(removeUser);
                    RemoveUserFromTicket(removeUser, TicketId);
                }
            }
            return users;
        }

        public void RemoveUserFromTicket(SqlUser userToRemove, long TicketId)
        {
            LDA.Execute("spRemoveInactiveUser", NeedHelpUheaa,
                SqlParams.Single("TicketID", TicketId),
                SqlParams.Single("SqlUserId", userToRemove.ID));
        }

        private class FlattenedTicketData
        {
            public long TicketID { get; set; }
            public string TicketCode { get; set; }
            public string Subject { get; set; }
            public bool AssignProgrammer { get; set; }
            public DateTime Requested { get; set; }
            public int Unit { get; set; }
            public string Area { get; set; }
            public DateTime Required { get; set; }
            public string Issue { get; set; }
            public string ResolutionCause { get; set; }
            public string ResolutionFix { get; set; }
            public string ResolutionPrevention { get; set; }
            public string Status { get; set; }
            public DateTime StatusDate { get; set; }
            public DateTime CourtDate { get; set; }
            public string IssueUpdate { get; set; }
            public string History { get; set; }
            public string PreviousStatus { get; set; }
            public string UrgencyOption { get; set; }
            public string CatOption { get; set; }
            public short Priority { get; set; }
            public DateTime LastUpdated { get; set; }
            public string CCCIssue { get; set; }
            public string RequestProjectNum { get; set; }
            public string Comments { get; set; }
            public string RelatedQCIssues { get; set; }
            public string RelatedProcedures { get; set; }
            public int PreviousCourt { get; set; }
            public int AssignedToID { get; set; }
            public int CourtID { get; set; }
            public int RequesterID { get; set; }

        }

        /// <summary>
        /// Returns a list of users and their step in the court rotation list
        /// </summary>
        /// <returns>List of CourtRotation</returns>
        public List<CourtRotation> GetRotationList()
        {
            return LDA.ExecuteList<CourtRotation>("spGetNextCourt", NeedHelpUheaa).Result;
        }

        /// <summary>
        /// Updates the location of the user in the rotation list
        /// </summary>
        /// <param name="sqlUserId">The user to update</param>
        /// <param name="rotation">The new rotation number</param>
        public void SetRotation(int sqlUserId, int rotation)
        {
            LDA.Execute("spSetRotationList", NeedHelpUheaa,
                SqlParams.Single("SqlUserId", sqlUserId),
                SqlParams.Single("Rotation", rotation));
        }

        /// <summary>
        /// Gets a list of users and the priority number
        /// </summary>
        /// <returns>List of PriorityEmail</returns>
        public List<PriorityEmail> GetPriorityEmail()
        {
            return LDA.ExecuteList<PriorityEmail>("spGENR_GetPriorityEmail", Csys).Result;
        }

        /// <summary>
        /// Gets the maximun flow step for the given flow ID
        /// </summary>
        /// <param name="flowID">The FlowID to check for</param>
        /// <returns>A number representing the maximum flow step</returns>
        public int GetMaxSequenceNumber(string flowID)
        {
            return LDA.ExecuteSingle<int>("spFLOW_GetMaxSequenceNumber", Csys,
                SqlParams.Single("FlowID", flowID)).Result;
        }

        /// <summary>
        /// Gets all of the elapsed times a user worked a ticket and adds them together to figure out the amount of time
        /// the ticket was being worked.
        /// </summary>
        /// <param name="activeTicket">The current ticket</param>
        /// <param name="user">The user opening the ticket</param>
        /// <returns>The elapsed time in a string format</returns>
        public TimeSpan GetTicketTime(Ticket activeTicket, SqlUser user)
        {
            string region = "uheaa";
            List<TimeTracking> times = GetAllTimesForUser(user.ID, false, activeTicket.Data.TheTicketData.TicketID).Where(r => r.Region == region).ToList();

            TimeSpan span = new TimeSpan();
            foreach (TimeTracking time in times)
            {
                if (time.EndTime != null)
                    span += time.EndTime.Value - time.StartTime;
            }

            return span;
        }

        /// <summary>
        /// Gets all the times for a user and ticket number
        /// </summary>
        /// <param name="userId">The SqlUserId for the user</param>
        /// <param name="unstoppedTime">True to include unstopped times, false to not include</param>
        /// <param name="ticketId">The ticket number</param>
        /// <returns>A list of TimeTracking objects</returns>
        private List<TimeTracking> GetAllTimesForUser(int userId, bool unstoppedTime, long ticketId)
        {
            return LDA.ExecuteList<TimeTracking>("spGetAllTimesForUser", Reporting,
                SqlParams.Single("SqlUserID", userId),
                SqlParams.Single("UnstoppedTime", unstoppedTime),
                SqlParams.Single("TicketId", ticketId)).Result;
        }

        /// <summary>
        /// Starts the timer for the user
        /// </summary>
        /// <param name="_activeTicket">The active ticket</param>
        /// <param name="_user">The user accessing the ticket</param>
        public bool StartTime(Ticket _activeTicket, SqlUser _user)
        {
            try
            {
                LDA.Execute("spSetStartTime", Reporting,
                    SqlParams.Single("SqlUserID", _user.ID),
                    SqlParams.Single("TicketID", _activeTicket.Data.TheTicketData.TicketID),
                    SqlParams.Single("Region", "uheaa"),
                    SqlParams.Single("StartTime", DateTime.Now));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Stops the timer for the user
        /// </summary>
        /// <param name="_activeTicket">The active ticket</param>
        /// <param name="_user">The active user</param>
        /// <param name="elapsedTime">The amount of time the ticket was being timed</param>
        public bool StopTime(Ticket _activeTicket, SqlUser _user)
        {
            try
            {
                LDA.Execute("spStopTime", Reporting,
                    SqlParams.Single("SqlUserID", _user.ID),
                    SqlParams.Single("TicketID", _activeTicket.Data.TheTicketData.TicketID),
                    SqlParams.Single("Region", "uheaa"),
                    SqlParams.Single("EndTime", DateTime.Now));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DateTime? CheckIfTimerIsRunning(long ticketID, SqlUser user)
        {
            try
            {
                return LDA.ExecuteList<DateTime?>("spGetRunningTicket", Reporting,
                    SqlParams.Single("TicketID", ticketID),
                    SqlParams.Single("SqlUserID", user.ID),
                    SqlParams.Single("Region", "uheaa")).Result.SingleOrDefault();
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}