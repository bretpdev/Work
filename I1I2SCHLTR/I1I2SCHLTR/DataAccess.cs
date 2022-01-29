using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace I1I2SCHLTR
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        private ProcessLogRun LogRun { get; set; }
        public DataAccess(ProcessLogRun logRun)
        {
            this.LogRun = logRun;
            LDA = logRun.LDA;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetLenderCodes")]
        public bool IsAffiliatedLender(string lenderId)
        {
            List<string> lenderCodes = LDA.ExecuteList<string>("GetLenderCodes", DataAccessHelper.Database.Bsys, new SqlParameter("Affiliation", "UHEAA")).Result;
            return lenderCodes.Any(p => p.Contains(lenderId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.AddWork")]
        public bool AddWork()
        {
            bool result = LDA.Execute("i1i2schltr.AddWork", DataAccessHelper.Database.Uls);
            if(!result)
            {
                LogRun.AddNotification($"Failed to add new work to processing Comment/Print/QueueTask tables", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.GetUnprocessedPrintData")]
        public List<PrintData> GetUnprocessedPrintData()
        {
            return LDA.ExecuteList<PrintData>("i1i2schltr.GetUnprocessedPrintData", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.GetUnprocessedQueueTaskData")]
        public List<QueueTaskData> GetUnprocessedQueueTaskData()
        {
            return LDA.ExecuteList<QueueTaskData>("i1i2schltr.GetUnprocessedQueueTaskData", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.GetUnprocessedCommentData")]
        public List<BorrowerData> GetUnprocessedCommentData()
        {
            List<BorrowerData> borrowerData = LDA.ExecuteList<BorrowerData>("i1i2schltr.GetUnprocessedCommentData", DataAccessHelper.Database.Uls).Result;
            //Because some files in the R2 may be processed we need to look at R2 files that are processed on the same SAS file so we don't have missing schools on someone's school list
            List<SchoolDataIntermediate> schoolData = GetSchoolsForRunDate(borrowerData.Select(p => p.RunDateId).Distinct().Select(p => new RunDateIds() { RunDateId = p}).ToList());
            foreach(BorrowerData data in borrowerData)
            {
                IEnumerable<SchoolDataIntermediate> sData = schoolData.Where(p => p.SSN == data.SSN && p.RunDateId == data.RunDateId);
                List<Schools> schools = new List<Schools>();
                foreach(SchoolDataIntermediate s in sData)
                {
                    schools.Add(new Schools() { School = s.School, SchoolStatus = s.SchoolStatus });
                }
                data.Schools = schools;
            }

            return borrowerData;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.GetSchoolsForRunDate")]
        public List<SchoolDataIntermediate> GetSchoolsForRunDate(List<RunDateIds> runDateIds)
        {
            List<SchoolDataIntermediate> schoolData = LDA.ExecuteList<SchoolDataIntermediate>("i1i2schltr.GetSchoolsForRunDate", DataAccessHelper.Database.Uls, SP("RunDateIds", runDateIds.ToDataTable())).Result;
            return schoolData;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.MarkPrintDataProcessed")]
        public bool MarkPrintDataProcessed(int printDataId)
        {
            return LDA.Execute("i1i2schltr.MarkPrintDataProcessed", DataAccessHelper.Database.Uls, SP("PrintDataId", printDataId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.MarkQueueTaskDataProcessed")]
        public bool MarkQueueTaskDataProcessed(int queueTaskDataId)
        {
            return LDA.Execute("i1i2schltr.MarkQueueTaskDataProcessed", DataAccessHelper.Database.Uls, SP("QueueTaskDataId", queueTaskDataId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.MarkCommentTaskProcessed")]
        public bool MarkCommentTaskProcessed(int commentDataId)
        {
            return LDA.Execute("i1i2schltr.MarkCommentTaskProcessed", DataAccessHelper.Database.Uls, SP("CommentDataId", commentDataId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.MarkCommentProcessed")]
        public bool MarkCommentProcessed(int commentDataId)
        {
            return LDA.Execute("i1i2schltr.MarkCommentProcessed", DataAccessHelper.Database.Uls, SP("CommentDataId", commentDataId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.GetSchoolDemographics")]
        public List<SchoolData> GetSchoolDemographics(string schoolId, string borrower)
        {
            return LDA.ExecuteList<SchoolData>("i1i2schltr.GetSchoolDemographics", DataAccessHelper.Database.Uls, SP("SchoolId", schoolId), SP("Borrower", borrower)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "spGetAccountNumberFromSsn")]
        public string GetAccountNumberFromSsn(string ssn)
        {
            return LDA.ExecuteSingle<string>("spGetAccountNumberFromSsn", DataAccessHelper.Database.Udw, SP("Ssn", ssn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "i1i2schltr.CloseQueueTask")]
        public bool CloseQueueTask(string queue, string subqueue, string ssn)
        {
            return LDA.Execute("i1i2schltr.CloseQueueTask", DataAccessHelper.Database.Uls, SP("Queue",queue), SP("Subqueue", subqueue), SP("Ssn", ssn));
        }

        public SqlParameter SP(string parameterName, object parameterValue)
        {
            return SqlParams.Single(parameterName, parameterValue);
        }
    }
}
