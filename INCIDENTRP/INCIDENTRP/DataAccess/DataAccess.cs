//See the other partial DataAccess classes for methods specific to Incidents and Threats.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SubSystemShared;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Data.SqlClient;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace INCIDENTRP
{
    public partial class DataAccess : DataAccessBaseShared
    {
        private const string System = "Incident Reporting Module";
        private LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        [UsesSproc(IncidentReportingUheaa, "spAddHistory")]
        public void AddHistoryRecord(long ticketNumber, string ticketType, HistoryRecord record)
        {
            LDA.Execute("spAddHistory", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType),
                SqlParams.Single("UpdateDateTime", record.UpdateDateTime),
                SqlParams.Single("SqlUserId", record.User.ID),
                SqlParams.Single("Status", record.Status),
                SqlParams.Single("StatusChangeDescription", record.StatusChangeDescription),
                SqlParams.Single("UpdateText", record.UpdateText));
        }

        [UsesSproc(IncidentReportingUheaa, "spCancelTicket")]
        public void CancelTicket(int userId, string ticketType, DateTime createDateTime, DateTime cancelDateTime, string accountNumber)
        {
            LDA.Execute("spCancelTicket", IncidentReportingUheaa,
            SqlParams.Single("SqlUserId", userId),
            SqlParams.Single("TicketType", ticketType),
            SqlParams.Single("CreateDateTime", createDateTime),
            SqlParams.Single("CancelDateTime", cancelDateTime),
            SqlParams.Single("AccountNumber", accountNumber));
        }

        [UsesSproc(IncidentReportingUheaa, "spDeleteActionTaken")]
        public void DeleteActionTaken(string action, long ticketNumber, string ticketType)
        {
            LDA.Execute("spDeleteActionTaken", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType),
                SqlParams.Single("Action", action));
        }

        [UsesSproc(IncidentReportingUheaa, "spDeleteNotifier")]
        public void DeleteNotifier(long ticketNumber, string ticketType)
        {
            LDA.Execute("spDeleteNotifier", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType));
        }

        [UsesSproc(Csys, "spSYSA_GetSqlUsers")]
        public List<SqlUser> GetSqlUsers(bool includeInactive)
        {
            return LDA.ExecuteList<SqlUser>("spSYSA_GetSqlUsers", Csys,
                SqlParams.Single("IncludeInactiveRecords", includeInactive)).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetAssignees")]
        public List<int> GetAssignees()
        {
            return LDA.ExecuteList<int>("spGetAssignees", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetCourts")]
        public List<int> GetCourts()
        {
            return LDA.ExecuteList<int>("spGetCourts", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetDataInvolvedOptions")]
        public List<string> GetDataInvolvedOptions()
        {
            return LDA.ExecuteList<string>("spGetDataInvolvedOptions", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetEarliestCreateDate")]
        public DateTime GetEarliestTicketCreateDate()
        {
            return LDA.ExecuteSingle<DateTime>("spGetEarliestCreateDate", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetHistory")]
        public List<HistoryRecord> GetHistory(long ticketNumber, string ticketType)
        {
            List<FlattenedHistoryRecord> flats = LDA.ExecuteList<FlattenedHistoryRecord>("spGetHistory", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType)).Result;
            List<SqlUser> users = GetSqlUsers(true);
            List<HistoryRecord> records = new List<HistoryRecord>();
            foreach (FlattenedHistoryRecord flat in flats)
            {
                HistoryRecord record = new HistoryRecord();
                record.Status = flat.Status;
                record.StatusChangeDescription = flat.StatusChangeDescription;
                record.UpdateDateTime = flat.UpdateDateTime;
                record.UpdateText = flat.UpdateText;
                record.User = users.SingleOrDefault(p => p.ID == flat.SqlUserId) ?? new SqlUser();
                records.Add(record);
            }
            return records;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetIncidentCauses")]
        public List<string> GetIncidentCauses()
        {
            return LDA.ExecuteList<string>("spGetIncidentCauses", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetIncidentPriority")]
        public int GetIncidentPriority(string urgency)
        {
            return LDA.ExecuteSingle<int>("spGetIncidentPriority", IncidentReportingUheaa,
                SqlParams.Single("Urgency", urgency ?? "")).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetIncidentTypes")]
        public List<string> GetIncidentTypes()
        {
            return LDA.ExecuteList<string>("spGetIncidentTypes", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spSetLock")]
        [UsesSproc(IncidentReportingUheaa, "spGetLockHolder")]
        public SqlUser GetLock(Ticket ticket, SqlUser user)
        {
            try
            {
                LDA.Execute("spSetLock", IncidentReportingUheaa,
                    SqlParams.Single("TicketNumber", ticket.Number),
                    SqlParams.Single("TicketType", ticket.Type),
                    SqlParams.Single("SqlUserId", user.ID));
                return user;
            }
            catch (Exception)
            {
                int lockSquid = LDA.ExecuteSingle<int>("spGetLockHolder", IncidentReportingUheaa,
                    SqlParams.Single("TicketNumber", ticket.Number),
                    SqlParams.Single("TicketType", ticket.Type)).Result;
                return GetSqlUsers(true).Single(p => p.ID == lockSquid);
            }
        }

        [UsesSproc(IncidentReportingUheaa, "spGetMembersOfIS")]
        public List<string> GetMembersOfComputerServicesAndInfoSecurity()
        {
            //Send in business unit 16 for Information Security
            return LDA.ExecuteList<string>("spGetMembersOfIS", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetNotificationMethods")]
        public List<string> GetNotificationMethods()
        {
            return LDA.ExecuteList<string>("spGetNotificationMethods", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetNotifierTypes")]
        public List<string> GetNotifierTypes()
        {
            return LDA.ExecuteList<string>("spGetNotifierTypes", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetRegions")]
        public List<string> GetRegions()
        {
            List<string> regions = LDA.ExecuteList<string>("spGetRegions", IncidentReportingUheaa).Result;
            regions.Remove("CornerStone");
            if (regions.Count == 3) //the first instance is blank leaving Uheaa and Both. If a new region is added, both will be available
                regions.Remove("Both");
            return regions;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetRelationships")]
        public List<string> GetRelationships()
        {
            return LDA.ExecuteList<string>("spGetRelationships", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetStatuses")]
        public List<string> GetStatuses()
        {
            return LDA.ExecuteList<string>("spGetStatuses", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetSubjects")]
        public List<string> GetSubjects()
        {
            //SQL Server won't do a DISTINCT on text columns, so we need to do it here.
            return LDA.ExecuteList<string>("spGetSubjects", IncidentReportingUheaa).Result.Distinct().ToList();
        }

        [UsesSproc(IncidentReportingUheaa, "spGetTicketTypes")]
        public List<TicketType> GetTicketTypes()
        {
            return LDA.ExecuteList<TicketType>("spGetTicketTypes", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetPriorities")]
        public List<string> GetUrgencies()
        {
            return LDA.ExecuteList<string>("spGetPriorities", IncidentReportingUheaa).Result;
        }

        public List<SqlUser> GetInactiveUsers()
        {
            return GetSqlUsers(true).Where(p => p.Status == "Inactive").ToList();
        }

        [UsesSproc(Csys, "spSYSA_CheckIfRoleHasAccess")]
        public bool GetUserKeyAssignment(string role, string userKey)
        {
            try
            {
                return LDA.ExecuteSingle<bool>("spSYSA_CheckIfRoleHasAccess", Csys,
                    SqlParams.Single("UserKey", userKey),
                    SqlParams.Single("RoleName", role)).Result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [UsesSproc(IncidentReportingUheaa, "spGetActionTaken")]
        public ActionTaken LoadActionTaken(long ticketNumber, string ticketType, string action)
        {
            FlattenedActionTaken flat = LDA.ExecuteList<FlattenedActionTaken>("spGetActionTaken", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType),
                SqlParams.Single("Action", action)).Result.SingleOrDefault();
            ActionTaken actionTaken = new ActionTaken(action);
            if (flat != null)
            {
                actionTaken.ActionWasTaken = true;
                actionTaken.DateTime = flat.ActionDateTime;
                actionTaken.PersonContacted = flat.PersonContacted;
            }
            return actionTaken;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetNotifier")]
        public Notifier LoadNotifier(long ticketNumber, string ticketType)
        {
            Notifier notifier = LDA.ExecuteList<Notifier>("spGetNotifier", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType)).Result.SingleOrDefault();
            return notifier ?? new Notifier();
        }

        [UsesSproc(IncidentReportingUheaa, "spGetReporter")]
        public Reporter LoadReporter(long ticketNumber, string ticketType)
        {
            FlattenedReporter flat = LDA.ExecuteSingle<FlattenedReporter>("spGetReporter", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType)).Result;
            Reporter reporter = new Reporter();
            if (flat != null)
            {
                reporter.User = GetSqlUsers(true).SingleOrDefault(p => p.ID == flat.SqlUserId);
                reporter.BusinessUnit = GetBusinessUnits().SingleOrDefault(p => p.ID == flat.BusinessUnitId);
                reporter.Location = flat.Location;
                reporter.PhoneNumber = flat.PhoneNumber;
            }
            return reporter;
        }

        [UsesSproc(IncidentReportingUheaa, "spGetTicket")]
        public Ticket LoadTicket(long ticketNumber, string ticketType)
        {
            FlattenedTicket flat = LDA.ExecuteList<FlattenedTicket>("spGetTicket", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType)).Result.SingleOrDefault();
            Ticket ticket = new Ticket();
            if (flat != null)
            {
                List<SqlUser> users = GetSqlUsers(true);
                ticket.Number = ticketNumber;
                ticket.Type = ticketType;
                ticket.IncidentDateTime = flat.IncidentDateTime;
                ticket.CreateDateTime = flat.CreateDateTime;
                ticket.Requester = users.SingleOrDefault(p => p.ID == flat.Requester);
                ticket.FunctionalArea = flat.FunctionalArea;
                ticket.Priority = flat.Priority;
                ticket.Status = flat.Status;
                ticket.Court = users.SingleOrDefault(p => p.ID == flat.Court);
                ticket.AssignedTo = users.SingleOrDefault(p => p.ID == flat.AssignedTo);
            }
            return ticket;
        }

        [UsesSproc(IncidentReportingUheaa, "spReleaseLock")]
        public void ReleaseLock(SqlUser user, string ticketType)
        {
            LDA.Execute("spReleaseLock", IncidentReportingUheaa,
                SqlParams.Single("SqlUserId", user.ID),
                SqlParams.Single("TicketType", ticketType));
        }

        [UsesSproc(IncidentReportingUheaa, "spSetActionTaken")]
        public void SaveActionTaken(ActionTaken action, long ticketNumber, string ticketType)
        {
            LDA.Execute("spSetActionTaken", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType),
                SqlParams.Single("Action", action.Action),
                SqlParams.Single("ActionDateTime", action.DateTime),
                SqlParams.Single("PersonContacted", action.PersonContacted));
        }

        [UsesSproc(IncidentReportingUheaa, "spSetNotifier")]
        public void SaveNotifier(Notifier notifier, long ticketNumber, string ticketType)
        {
            LDA.Execute("spSetNotifier", IncidentReportingUheaa,
                SqlParams.Single("TicketNumber", ticketNumber),
                SqlParams.Single("TicketType", ticketType),
                SqlParams.Single("NotifierType", notifier.Type),
                SqlParams.Single("OtherNotifierType", notifier.OtherType),
                SqlParams.Single("NotificationMethod", notifier.Method),
                SqlParams.Single("OtherNotificationMethod", notifier.OtherMethod),
                SqlParams.Single("Name", notifier.Name),
                SqlParams.Single("EmailAddress", notifier.EmailAddress),
                SqlParams.Single("PhoneNumber", notifier.PhoneNumber),
                SqlParams.Single("Relationship", notifier.Relationship),
                SqlParams.Single("OtherRelationship", notifier.OtherRelationship));
        }

        [UsesSproc(IncidentReportingUheaa, "spSetReporter")]
        public void SaveReporter(Reporter reporter, long ticketNumber, string ticketType)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("TicketNumber", ticketNumber));
            parms.Add(new SqlParameter("TicketType", ticketType));
            parms.Add(new SqlParameter("PhoneNumber", reporter.PhoneNumber));
            parms.Add(new SqlParameter("Location", reporter.Location));
            if (reporter.User != null)
                parms.Add(new SqlParameter("SqlUserId", reporter.User.ID));
            if (reporter.BusinessUnit != null)
                parms.Add(new SqlParameter("BusinessUnitID", reporter.BusinessUnit.ID));
            LDA.Execute("spSetReporter", IncidentReportingUheaa, parms.ToArray());
        }

        [UsesSproc(IncidentReportingUheaa, "spSetTicket")]
        public long SaveTicket(Ticket ticket)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("TicketNumber", ticket.Number));
            parms.Add(new SqlParameter("TicketType", ticket.Type));
            parms.Add(new SqlParameter("IncidentDateTime", ticket.IncidentDateTime));
            parms.Add(new SqlParameter("CreateDateTime", ticket.CreateDateTime));
            if (ticket.Requester != null)
                parms.Add(new SqlParameter("Requester", ticket.Requester.ID));
            parms.Add(new SqlParameter("FunctionalArea", ticket.FunctionalArea));
            parms.Add(new SqlParameter("Priority", ticket.Priority));
            parms.Add(new SqlParameter("Status", ticket.Status));
            if (ticket.Court != null && ticket.Court.ID != 0)
                parms.Add(new SqlParameter("Court", ticket.Court.ID));
            if (ticket.AssignedTo != null && ticket.AssignedTo.ID != 0)
                parms.Add(new SqlParameter("AssignedTo", ticket.AssignedTo.ID));
            return LDA.ExecuteList<long>("spSetTicket", IncidentReportingUheaa, parms.ToArray()).Result.SingleOrDefault();
        }

        [UsesSproc(IncidentReportingUheaa, "spSearchForAllTickets")]
        public List<SearchResultItem> SearchForAllTickets()
        {
            return LDA.ExecuteList<SearchResultItem>("spSearchForAllTickets", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spSearchForOpenTickets")]
        public List<SearchResultItem> SearchForOpenTickets()
        {
            return LDA.ExecuteList<SearchResultItem>("spSearchForOpenTickets", IncidentReportingUheaa).Result;
        }

        [UsesSproc(IncidentReportingUheaa, "spSearchForTickets")]
        public List<SearchResultItem> SearchForTickets(SearchCriteria criteria)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            int ticketNumber;
            if (int.TryParse(criteria.TicketNumber, out ticketNumber))
                parms.Add(new SqlParameter("TicketNumber", ticketNumber));
            if (criteria.KeyWordSearchScope != null && criteria.KeyWordSearchScope != KeyWordScope.None)
                parms.Add(new SqlParameter("KeyWordSearchScope", criteria.KeyWordSearchScope.ToString()));
            if (!string.IsNullOrEmpty(criteria.KeyWord))
                parms.Add(new SqlParameter("KeyWord", criteria.KeyWord));
            if (!string.IsNullOrEmpty(criteria.Subject))
                parms.Add(new SqlParameter("Subject", criteria.Subject));
            if (criteria.Court != null && criteria.Court.ID != 0)
                parms.Add(new SqlParameter("Court", criteria.Court.ID));
            if (criteria.Requester != null && criteria.Requester.ID != 0)
                parms.Add(new SqlParameter("Requester", criteria.Requester.ID));
            if (!string.IsNullOrEmpty(criteria.Status))
                parms.Add(new SqlParameter("Status", criteria.Status));
            if (criteria.BusinessUnit != null && criteria.BusinessUnit.ID != 0)
                parms.Add(new SqlParameter("BusinessUnitId", criteria.BusinessUnit.ID));
            if (criteria.TicketType != null && !string.IsNullOrEmpty(criteria.TicketType.Description))
                parms.Add(new SqlParameter("TicketType", criteria.TicketType.Description));
            if (!string.IsNullOrEmpty(criteria.FunctionalArea))
                parms.Add(new SqlParameter("FunctionalArea", criteria.FunctionalArea));
            if (criteria.AssignedTo != null && criteria.AssignedTo.ID != 0)
                parms.Add(new SqlParameter("AssignedTo", criteria.AssignedTo.ID));
            parms.Add(new SqlParameter("CreateDateRangeStart", criteria.CreateDateRangeStart));
            parms.Add(new SqlParameter("CreateDateRangeEnd", criteria.CreateDateRangeEnd));
            return LDA.ExecuteList<SearchResultItem>("spSearchForTickets", IncidentReportingUheaa, parms.ToArray()).Result;
        }

        public bool HasIRAccess(string key, string system, SqlUser user)
        {
            DataAccessBaseShared shared = new DataAccessBaseShared();
            return shared.HasAccess(key, system, user);
        }

        public bool UserHasInformationSecurityAccess(SqlUser user)
        {
            DataAccessBaseShared shared = new DataAccessBaseShared();
            return shared.HasAccess("IR Search", System, user);
        }

        public List<string> GetInfoSecurityEmail(List<SqlUser> currentEmployees)
        {
            return new List<string>() { EnterpriseFileSystem.GetPath("SecurityEmail", DataAccessHelper.Region.Uheaa) };
        }

        [UsesSproc(Csys, "spGENR_GetStateCodes")]
        public List<string> GetStateCodes(bool includeTerritories)
        {
            return LDA.ExecuteList<string>("spGENR_GetStateCodes", Csys,
                SqlParams.Single("IncludeTerritories", includeTerritories)).Result;
        }

        #region Projection Classes

        private class FlattenedActionTaken
        {
            public DateTime ActionDateTime { get; set; }
            public string PersonContacted { get; set; }
        }

        private class FlattenedHistoryRecord
        {
            public DateTime UpdateDateTime { get; set; }
            public int SqlUserId { get; set; }
            public string Status { get; set; }
            public string StatusChangeDescription { get; set; }
            public string UpdateText { get; set; }
        }

        private class FlattenedReporter
        {
            public int SqlUserId { get; set; }
            public int BusinessUnitId { get; set; }
            public string PhoneNumber { get; set; }
            public string Location { get; set; }
        }

        private class FlattenedTicket
        {
            public DateTime IncidentDateTime { get; set; }
            public DateTime CreateDateTime { get; set; }
            public int Requester { get; set; }
            public string FunctionalArea { get; set; }
            public int Priority { get; set; }
            public string Status { get; set; }
            public int Court { get; set; }
            public int AssignedTo { get; set; }
        }

        #endregion Projection Classes
    }
}