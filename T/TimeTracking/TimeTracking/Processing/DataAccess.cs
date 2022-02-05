using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace TimeTracking
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun) => LogRun = logRun;

        [UsesSproc(CentralData, "GetScriptRoles")]
        public List<string> CollectRoles(string scriptId) =>
            LogRun.LDA.ExecuteList<string>("GetScriptRoles", CentralData,
                Sp("ScriptId", scriptId)).Result;

        [UsesSproc(Csys, "spGetUserIdAndFullName")]
        public List<User> AllUsers() =>
            LogRun.LDA.ExecuteList<User>("spGetUserIdAndFullName", Csys,
                new SqlParameter("Status", true)).Result;

        [UsesSproc(Reporting, "GetAllTimesForUser")]
        public List<UserTime> GetTimesForUser(User selectedUser, bool unstoppedTime = false) =>
            LogRun.LDA.ExecuteList<UserTime>("GetAllTimesForUser", Reporting,
                Sp("SqlUserID", selectedUser.SqlUserId),
                Sp("UnstoppedTime", unstoppedTime)).Result.OrderBy(p => p.TimeTrackingId).ToList();

        [UsesSproc(Reporting, "UpdateRecord")]
        public bool UpdateRecord(UserTime updatedRecord)
        {
            return LogRun.LDA.ExecuteSingle<int>("UpdateRecord", Reporting,
                Sp("TimeTrackingId", updatedRecord.TimeTrackingId),
                Sp("StartTime", updatedRecord.StartTime),
                Sp("EndTime", updatedRecord.EndTime),
                Sp("SqlUserId", GetActiveUserId()),
                Sp("SystemTypeId", updatedRecord.SystemTypeId),
                Sp("CostCenterId", updatedRecord.CostCenterId),
                Sp("BatchProcessing", updatedRecord.BatchProcessing),
                Sp("GenericMeeting", updatedRecord.GenericMeeting),
                Sp("Region", updatedRecord.Region)).Result == 1;
        }

        [UsesSproc(Csys, "spACDC_GetSquid")]
        public int GetActiveUserId() =>
            LogRun.LDA.ExecuteSingle<int>("spACDC_GetSquid", Csys,
                new SqlParameter("WindowsUsername", Environment.UserName.ToLower())).Result;

        [UsesSproc(Reporting, "GetCostCenter")]
        public List<CostCenters> GetCostCenter() =>
            LogRun.LDA.ExecuteList<CostCenters>("GetCostCenter", Reporting).Result;

        [UsesSproc(Reporting, "GetSystems")]
        public List<SystemTypes> GetSystems() =>
            LogRun.LDA.ExecuteList<SystemTypes>("GetSystems", Reporting).Result;

        /// <summary>
        /// Gets the title of the request for 'Letter Tracking', 'Sacker SAS' and 'Sacker Script'
        /// </summary>
        /// <param name="text">The name of the system the request is in. Ex: Letter Tracking, Sacker SAS</param>
        /// <param name="requestNumber">The request number</param>
        /// <returns></returns>
        [UsesSproc(Reporting, "GetTitle")]
        public string GetTitle(string text, int requestNumber, bool cornerstoneRegion) =>
            LogRun.LDA.ExecuteList<string>("GetTitle", Reporting,
                Sp("System", text),
                Sp("RequestNumber", requestNumber),
                Sp("CornerstoneRegion", cornerstoneRegion)).Result.FirstOrDefault();

        [UsesSproc(Reporting, "GetCourtData")]
        public List<CourtTickets> GetCourtData(string userName) =>
            LogRun.LDA.ExecuteList<CourtTickets>("GetCourtData", Reporting,
                Sp("UserName", userName)).Result;

        [UsesSproc(Reporting, "GetTotalHours")]
        public List<Dates> GetTotalHours(DateTime from, DateTime to) =>
            LogRun.LDA.ExecuteList<Dates>("GetTotalHours", Reporting,
                Sp("SqlUserId", GetActiveUserId()),
                Sp("StartDate", from),
                Sp("EndDate", to)).Result;

        [UsesSproc(Reporting, "SetStartTime")]
        public bool StartTime(int sqlUserId, int? ticketId, string region, int? costCenter, int? systemType, string genericMeeting, bool batchProcessing, DateTime startTime)
        {
            try
            {
                LogRun.LDA.Execute("SetStartTime", Reporting,
                    Sp("SqlUserId", sqlUserId),
                    Sp("TicketId", ticketId),
                    Sp("Region", region),
                    Sp("StartTime", startTime),
                    Sp("CostCenterId", costCenter),
                    Sp("SystemTypeId", systemType),
                    Sp("BatchProcessing", batchProcessing),
                    Sp("GenericMeeting", genericMeeting));
            }
            catch (Exception ex)
            {
                string message = "There was an error starting to the timer for User: {sqlUserId}, Ticket Number: {ticketId}, System: {systemType}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
            return true;
        }

        [UsesSproc(Reporting, "StopTime")]
        public bool StopTime(int timeTrackingId, DateTime stopTime)
        {
            try
            {
                LogRun.LDA.Execute("StopTime", Reporting,
                    Sp("TimeTrackingId", timeTrackingId),
                    Sp("EndTime", DateTime.Now));
            }
            catch (Exception ex)
            {
                string message = "There was an error stopping the time to TimeTrackingId: {timeTrackingId}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
            return true;
        }

        public List<int> GetRequestNumbers(string text)
        {
            return new List<int>();
        }

        public List<int> GetPMDRequestNumbers()
        {
            string conn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=X:\PADR\Procedures\Procedures.mdb";
            string select = "SELECT Request FROM datRequests";

            DataSet myDS = new DataSet();
            OleDbConnection accessConn = null;
            try
            {
                accessConn = new OleDbConnection(conn);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            try
            {
                OleDbCommand cmd = new OleDbCommand(select, accessConn);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                accessConn.Open();
                adapter.Fill(myDS, "Request");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                accessConn.Close();
            }

            DataRowCollection drc = myDS.Tables["Request"].Rows;
            List<int> reqNums = new List<int>();
            foreach (DataRow item in drc)
            {
                reqNums.Add((int)item[0]);
            }
            return reqNums;
        }

        public List<int> GetProjectRequestNumbers()
        {
            string conn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=X:\PADR\Projects\Projects.mdb";
            string select = "SELECT Request FROM datPMDRequests";

            DataSet myDS = new DataSet();
            OleDbConnection accessConn = null;
            try
            {
                accessConn = new OleDbConnection(conn);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            try
            {
                OleDbCommand cmd = new OleDbCommand(select, accessConn);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                accessConn.Open();
                adapter.Fill(myDS, "Request");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                accessConn.Close();
            }

            DataRowCollection drc = myDS.Tables["Request"].Rows;
            List<int> reqNums = new List<int>();
            foreach (DataRow item in drc)
            {
                reqNums.Add((int)item[0]);
            }
            return reqNums;
        }

        [UsesSproc(Reporting, "GetNeedHelpRequestNumbers")]
        public List<int> GetNeedHelpRequestNumbers()
        {
            List<long> nums = LogRun.LDA.ExecuteList<long>("GetNeedHelpRequestNumbers", Reporting).Result;
            if (nums != null && nums.Count > 0)
                return nums.ConvertAll(i => (int)i);//return type is List<int> but data being pulled is BigInt so it needs to be converted
            return new List<int>();
        }
        [UsesSproc(Reporting, "GetSackerRequestNumbers")]
        public List<int> GetSackerRequestNumbers(string system) =>
            LogRun.LDA.ExecuteList<int>("GetSackerRequestNumbers", Reporting,
                Sp("System", system)).Result;

        [UsesSproc(Reporting, "GetLetterRequestNumbers")]
        public List<int> GetLetterRequestNumbers() =>
            LogRun.LDA.ExecuteList<int>("GetLetterRequestNumbers", Reporting).Result;

        [UsesSproc(Reporting, "GetLetterTrackingData")]
        public List<TicketData> GetLetterTrackingData() =>
            LogRun.LDA.ExecuteList<TicketData>("GetLetterTrackingData", Reporting).Result;

        //[UsesSproc(Reporting, "GetScriptTrackingData")]
        public List<TicketData> GetScriptTrackingData()
        {
            return null;
            //return LogRun.LDA.ExecuteList<TicketData>("GetScriptTrackingData", Reporting).Result;
        }

        [UsesSproc(Reporting, "GetNeedHelpTrackingData")]
        public List<TicketData> GetNeedHelpTrackingData() =>
            LogRun.LDA.ExecuteList<TicketData>("GetNeedHelpTrackingData", Reporting).Result;

        private SqlParameter Sp(string name, object value) => SqlParams.Single(name, value);
    }
}