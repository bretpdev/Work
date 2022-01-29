using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace QSTATSEXTR
{
    class DataAccess
    {
        readonly ProcessLogRun PLR;
        readonly LogDataAccess LDA;
        const DB Bsys = DB.Bsys;
        public DataAccess(ProcessLogRun plr)
        {
            this.PLR = plr;
            this.LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, true);
        }

        [UsesSproc(Bsys, "qstatsextr.GetQueuesWithLateDays")]
        public List<QueueDetail> GetLateQueueDetails()
        {
            return LDA.ExecuteList<QueueDetail>("qstatsextr.GetQueuesWithLateDays", Bsys).Result;
        }

        [UsesSproc(Bsys, "qstatsextr.AddQueueData")]
        public void AddQueueData(QueueData data)
        {
            LDA.Execute("qstatsextr.AddQueueData", Bsys, SqlParams.Generate(data));
        }

        [UsesSproc(Bsys, "qstatsextr.AddUserData")]
        public void AddUserData(UserData data)
        {
            LDA.Execute("qstatsextr.AddUserData", Bsys,
                Sp("RuntimeDate", data.RuntimeDate),
                Sp("Queue", data.Queue),
                Sp("UserId", data.UserId),
                Sp("StatusCode", data.Status.ToString()),
                Sp("CountInStatus", data.CountInStatus),
                Sp("TotalTime", data.TotalTime),
                Sp("AvgTime", data.AverageTime));
        }

        [UsesSproc(Bsys, "qstatsextr.GetBadNoPopulateQueues")]
        [UsesSproc(Bsys, "qstatsextr.GetQueuesNeedingDaysLatePopulated")]
        [UsesSproc(Bsys, "qstatsextr.GetQueuesWithBlankManagers")]
        [UsesSproc(Bsys, "qstatsextr.GetQueuesWithNoDescription")]
        [UsesSproc(Bsys, "qstatsextr.GetQueuesWithNoManager")]
        [UsesSproc(Bsys, "qstatsextr.GetErrorEmailAddresses")]
        public QueueErrors GetQueueErrors(DateTime runtimeDate)
        {
            var errors = new QueueErrors();
            errors.BadNoPopulateQueues = LDA.ExecuteList<string>("qstatsextr.GetBadNoPopulateQueues", Bsys, Sp("RunTimeDate", runtimeDate)).Result;
            errors.QueuesNeedingDaysLatePopulated = LDA.ExecuteList<string>("qstatsextr.GetQueuesNeedingDaysLatePopulated", Bsys).Result;
            errors.QueuesWithBlankManagers = LDA.ExecuteList<string>("qstatsextr.GetQueuesWithBlankManager", Bsys).Result;
            errors.QueuesWithNoDescription = LDA.ExecuteList<string>("qstatsextr.GetQueuesWithNoDescription", Bsys).Result;
            errors.QueuesWithNoManagers = LDA.ExecuteList<string>("qstatsextr.GetQueuesWithNoManager", Bsys, Sp("RunTimeDate", runtimeDate)).Result;
            errors.ErrorEmailAddresses = LDA.ExecuteList<string>("qstatsextr.GetErrorEmailAddresses", Bsys).Result;
            return errors;
        }

        [UsesSproc(Bsys, "qstatsextr.GetBusinessUnitsForReports")]
        public List<string> GetBusinessUnitsForReports()
        {
            return LDA.ExecuteList<string>("[qstatsextr].[GetBusinessUnitsForReports]", Bsys).Result;
        }

        [UsesSproc(Bsys, "qstatsextr.GetReportData")]
        public DataTable GetReportData(DateTime runTimeDate, string businessUnit)
        {
            return LDA.ExecuteDataTable("qstatsextr.GetReportData", Bsys, false, Sp("RunTimeDate", runTimeDate), Sp("BusinessUnit", businessUnit)).Result;
        }

        [UsesSproc(Bsys, "qstatsextr.GetMostRecentRuntimeDate")]
        public DateTime GetMostRecentRunTimeDate()
        {
            return LDA.ExecuteSingle<DateTime>("qstatsextr.GetMostRecentRuntimeDate", Bsys).Result;
        }

        [UsesSproc(Bsys, "qstatsextr.GetQcAndSupervisorRecipients")]
        public string GetReportRecipients(string businessUnit)
        {
            var recipientList = LDA.ExecuteList<string>("qstatsextr.GetQcAndSupervisorRecipients", Bsys, Sp("BusinessUnit", businessUnit)).Result;
            string recipients = string.Join(",", recipientList);

            return recipients;
        }

        SqlParameter Sp(string name, object val) => SqlParams.Single(name, val);
    }
}
