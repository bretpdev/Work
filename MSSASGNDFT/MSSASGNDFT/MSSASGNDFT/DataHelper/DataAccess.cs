using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace MassAssignBatch
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun) =>
            LogRun = logRun;

        /// <summary>
        /// Checks to see if the user has access to run the batch job or to change range assignments.
        /// </summary>
        /// <returns>True: user has range assignment access; False: user has batch access.</returns>
        [UsesSproc(Csys, "spSYSA_CheckIfRoleHasAccess")]
        [UsesSproc(Csys, "spSYSA_GetRoles")]
        public bool HasAccess(string key, MassAssignRangeAssignment.SqlUser user)
        {
            string roleName = LogRun.LDA.ExecuteList<Role>("spSYSA_GetRoles", Csys).Result
                .Where(r => r.RoleId == user.Role).Single().RoleName;
            return LogRun.LDA.ExecuteSingle<bool>("spSYSA_CheckIfRoleHasAccess", Csys,
                Sp("UserKey", key),
                Sp("RoleName", roleName)).Result;
        }

        /// <summary>
        /// Returns a list of SqlUsers for all active users.
        /// </summary>
        [UsesSproc(Csys, "spSYSA_GetSqlUsers")]
        public List<MassAssignRangeAssignment.SqlUser> GetUsers() =>
            LogRun.LDA.ExecuteList<MassAssignRangeAssignment.SqlUser>("spSYSA_GetSqlUsers", Csys).Result;

        /// <summary>
        /// Gets a count of the number of records for the given queue
        /// </summary>
        [UsesSproc(Uls, "mssasgndft.GetQueueCount")]
        public int GetQueueCount(string queue, bool futureDated = false) =>
            LogRun.LDA.ExecuteSingle<int>("mssasgndft.GetQueueCount", Uls,
                Sp("Queue", queue),
                Sp("FutureDated", futureDated)).Result;

        /// <summary>
        /// Gets a list of all the active queues
        /// </summary>
        [UsesSproc(Uls, "[mssasgndft].GetQueueList")]
        public List<QueueNames> GetQueueList() =>
            LogRun.LDA.ExecuteList<QueueNames>("[mssasgndft].GetQueueList", Uls).Result;

        /// <summary>
        /// Gets the manager of Loan Managements UT ID
        /// </summary>
        [UsesSproc(Csys, "spGetManagersUtId")]
        public string GetManagerId()
        {
            string manager = "";
            try
            {
                manager = LogRun.LDA.ExecuteSingle<string>("spGetManagersUtId", Csys,
                    Sp("BusinessUnitId", EnterpriseFileSystem.GetPath("LoanManagementBU_ID").ToInt())).Result;
            }
            catch (Exception ex)
            {
                string unit = ((BusinessUnit)LogRun.LDA.ExecuteList<BusinessUnit>("spGENR_GetBusinessUnits", Csys).Result.Where(p => p.ID == 19)).Name;
                string message = string.Format("There was an error finding the manager for business unit {0}", unit);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return "";
            }
            return manager;
        }

        /// <summary>
        /// Gets the list of current range assignments.
        /// </summary>
        [UsesSproc(Uls, "[mssasgndft].GetRanges")]
        public List<UserRange> GetCurrentRanges() =>
            LogRun.LDA.ExecuteList<UserRange>("[mssasgndft].GetRanges", Uls).Result;

        /// <summary>
        /// Gets the manager email address for business unit 19
        /// </summary>
        [UsesSproc(Csys, "spNDHP_GetManagerEmail")]
        public string GetManagerOfBU() =>
            LogRun.LDA.ExecuteSingle<string>("spNDHP_GetManagerEmail", Csys,
                Sp("BusinessUnit", EnterpriseFileSystem.GetPath("LoanManagementBU_ID").ToInt())).Result;

        public SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}