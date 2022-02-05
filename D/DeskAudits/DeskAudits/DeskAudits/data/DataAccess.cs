using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DeskAudits
{
    public class DataAccess
    {
        ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.CentralData, "deskaudits.GetCommonFailReasons")]
        public List<FailReason> GetCommonFailReasons()
        {
            return LogRun.LDA.ExecuteList<FailReason>("deskaudits.GetCommonFailReasons", DataAccessHelper.Database.CentralData).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "deskaudits.GetActiveAgentsInSameBuAsUser")]
        public List<Agent> GetPossibleAuditees(string userName)
        {
            return LogRun.LDA.ExecuteList<Agent>("deskaudits.GetActiveAgentsInSameBuAsUser", DataAccessHelper.Database.Csys, SP("UserName", userName)).Result;
        }


        [UsesSproc(DataAccessHelper.Database.Csys, "deskaudits.GetUserAccess")]
        public UserAccess GetUserAccess(string userName)
        {
            return LogRun.LDA.ExecuteSingle<UserAccess>("deskaudits.GetUserAccess", DataAccessHelper.Database.Csys, SP("UserName", userName)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.CentralData, "deskaudits.GetAuditees")]
        public List<string> GetLoggedAuditees()
        {
            return LogRun.LDA.ExecuteList<string>("deskaudits.GetAuditees", DataAccessHelper.Database.CentralData).Result;
        }

        [UsesSproc(DataAccessHelper.Database.CentralData, "deskaudits.GetAuditors")]
        public List<string> GetLoggedAuditors()
        {
            return LogRun.LDA.ExecuteList<string>("deskaudits.GetAuditors", DataAccessHelper.Database.CentralData).Result;
        }

        [UsesSproc(DataAccessHelper.Database.CentralData, "deskaudits.AddAudit")]
        public bool AddAudit(Audit audit)
        {
            return LogRun.LDA.Execute("deskaudits.AddAudit", DataAccessHelper.Database.CentralData, SP("Auditor", audit.Auditor), SP("Auditee", audit.Auditee), SP("Passed", audit.Passed), SP("CommonFailReasonId", audit.CommonFailReasonId), SP("CustomFailReasonDescription", audit.CustomFailReasonDescription), SP("AuditDate", audit.AuditDate));
        }

        [UsesSproc(DataAccessHelper.Database.CentralData, "deskaudits.GetAudits")]
        public List<Audit> GetAudits(Audit audit, DateTime? beginDate, DateTime? endDate)
        {
            return LogRun.LDA.ExecuteList<Audit>("deskaudits.GetAudits", DataAccessHelper.Database.CentralData, SP("Auditor", audit.Auditor), SP("Auditee", audit.Auditee), SP("Passed", audit.Passed), SP("CommonFailReasonId", audit.CommonFailReasonId), SP("BeginDate", beginDate), SP("EndDate", endDate)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.CentralData, "deskaudits.AuditExists")]
        public bool AuditExists(Audit audit)
        {
            return LogRun.LDA.ExecuteSingle<bool>("deskaudits.AuditExists", DataAccessHelper.Database.CentralData, SP("Auditor", audit.Auditor), SP("Auditee", audit.Auditee), SP("Passed", audit.Passed), SP("CommonFailReasonId", audit.CommonFailReasonId), SP("CustomFailReasonDescription", audit.CustomFailReasonDescription), SP("AuditDate", audit.AuditDate)).Result;
        }

        /// <summary>
        /// SQL parameterization wrapper: 
        /// parameterizes a string as the field name and
        /// an object as the value to be used for DB calls.
        /// </summary>
        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
