using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace ACURINTC
{
    public class DataAccess
    {
        private string loanServicingManagerId;
        public ProcessLogRun LogRun { get; private set; }
        public LogDataAccess LDA { get; private set; }
        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            LDA = new LogDataAccess(LogRun.Mode, LogRun.ProcessLogId, false, false);
        }

        /// <summary>
        /// Gets the user ID of the manager of Borrower Services.
        /// </summary>
        [UsesSproc(DB.Bsys, "GetManagerOfBusinessUnit")]
        public string LoanServicingManagerId(ProcessLogRun LogRun)
        {
            string buName = EnterpriseFileSystem.GetPath($"{ReflectionManager.ScriptId}-BU");
            try
            {
                loanServicingManagerId = LDA.ExecuteSingle<string>("GetManagerOfBusinessUnit", DB.Bsys, Sp("BusinessUnit", buName)).Result;
            }
            catch (Exception)
            {
                string message = "A unique user ID for the manager of " + buName + " was not found. Please contact Process Automation for assistance.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return loanServicingManagerId;
        }

        /// <summary>
        /// Gets a list of sources and their associated locate types,
        /// OneLINK source codes, and COMPASS source codes from BSYS.
        /// </summary>
        [UsesSproc(DB.Uls, "[acurintc].[GetSystemSources]")]
        public List<SystemCode> SystemCodes()
        {
            return LDA.ExecuteList<SystemCode>("[acurintc].[GetSystemSources]", DB.Uls).Result;
        }

        /// <summary>
        /// Gets a list of the queues to process and methods for parsing and processing them.
        /// </summary>
        [UsesSproc(DB.Uls, "acurintc.GetQueueInfo")]
        public List<QueueInfo> GetQueues()
        {
            return LDA.ExecuteList<QueueInfo>("acurintc.GetQueueInfo", DB.Uls).Result;
        }

        [UsesSproc(DB.Uls, "[acurintc].[GetDemographicsSources]")]
        public List<DemographicsSourceInfo> GetDemographicsSources()
        {
            return LDA.ExecuteList<DemographicsSourceInfo>("[acurintc].[GetDemographicsSources]", DB.Uls).Result;
        }

        /// <summary>
        /// Gets a list of locate-related queue names from BSYS.
        /// </summary>
        [UsesSproc(DB.Bsys, "GetLocateQueues")]
        public List<string> LocateQueues()
        {
            return DataAccessHelper.ExecuteList<string>("GetLocateQueues", DB.Bsys);
        }

        [UsesSproc(DB.Bsys, "GetDepartmentForQueue")]
        public string GetDepartment(string queue)
        {
            return LDA.ExecuteSingle<string>("GetDepartmentForQueue", DB.Bsys, Sp("Queue", queue)).Result;
        }

        /// <summary>
        /// Gets a list of demographics sources, reject reasons, and their associated
        /// action codes for address and phone processing from BSYS.
        /// </summary>
        [UsesSproc(DB.Uls, "acurintc.GetRejectActionsByDemographicsSource")]
        public List<RejectAction> GetRejectActions(DemographicsSource source)
        {
            return LDA.ExecuteList<RejectAction>("acurintc.GetRejectActionsByDemographicsSource", DB.Uls, Sp("DemographicsSourceId", (int)source)).Result;
        }


        [UsesSproc(DB.Uls, "acurintc.GetRejectReasons")]
        public List<RejectReasonInfo> GetRejectReasons()
        {
            return LDA.ExecuteList<RejectReasonInfo>("acurintc.GetRejectReasons", DB.Uls).Result;
        }

        /// <summary>
        /// Gets a list of state codes from BSYS.
        /// </summary>
        [UsesSproc(DB.Bsys, "GetStateCodes")]
        public List<string> StateCodes()
        {
            return LDA.ExecuteList<string>("GetStateCodes", DB.Bsys).Result;
        }

        [UsesSproc(DB.Uls, "acurintc.LoadCompassPdemData")]
        public void LoadCompassPdemData(string queue, string subqueue)
        {
            LDA.Execute("acurintc.LoadCompassPdemData", DB.Uls, Sp("Queue", queue), Sp("Subqueue", subqueue));
        }

        [UsesSproc(DB.Uls, "acurintc.LoadCompassCommaData")]
        public void LoadCompassCommaData(string queue, string subqueue)
        {
            LDA.Execute("acurintc.LoadCompassCommaData", DB.Uls, Sp("Queue", queue), Sp("Subqueue", subqueue));
        }

        [UsesSproc(DB.Uls, "acurintc.LoadOnelinkCommaData")]
        public void LoadOnelinkCommaData(string queue, string subqueue)
        {
            LDA.Execute("acurintc.LoadOnelinkCommaData", DB.Uls, Sp("Queue", queue), Sp("Subqueue", subqueue));
        }

        [UsesSproc(DB.Uls, "acurintc.LoadOnelinkPdemData")]
        public void LoadOnelinkPdemData(string queue, string subqueue)
        {
            LDA.Execute("acurintc.LoadOnelinkPdemData", DB.Uls, Sp("Queue", queue), Sp("Subqueue", subqueue));
        }

        [UsesSproc(DB.Uls, "acurintc.GetUnprocessedQueues")]
        public List<PendingDemos> GetCompassQueueData()
        {
            var result = LDA.ExecuteList<PendingDemos>("acurintc.GetUnprocessedQueues", DB.Uls);
            var list = result.Result;
            foreach (var item in list)
                item.ZipCode = (item.ZipCode ?? "").Replace("-", ""); //sanitize zip codes
            return list;
        }

        [UsesSproc(DB.Uls, "acurintc.SetQueueProcessed")]
        public void UpdateProcessed(int processQueueId)
        {
            LDA.Execute("acurintc.SetQueueProcessed", DB.Uls, Sp("ProcessQueueId", processQueueId));
        }

        [UsesSproc(DB.Uls, "acurintc.GetBorrowerRecentAddressHistory")]
        public List<AddressHistory> GetBorrowerRecentAddressHistory(string ssn)
        {
            return LDA.ExecuteList<AddressHistory>("acurintc.GetBorrowerRecentAddressHistory", DB.Uls, Sp("Ssn", ssn)).Result;
        }

        [UsesSproc(DB.Uls, "acurintc.GetBorrowerRecentPhoneHistory")]
        public List<PhoneHistory> GetBorrowerRecentPhoneHistory(string ssn)
        {
            return LDA.ExecuteList<PhoneHistory>("acurintc.GetBorrowerRecentPhoneHistory", DB.Uls, Sp("Ssn", ssn)).Result;
        }


        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}