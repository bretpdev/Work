using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;


namespace PIFLTR
{
    class DataAccess
    {
        private ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// This method will return a list of R9 task to be processed.
        /// </summary>
        /// <returns>A list of TaskData objects</returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "pifltr.GetUnprocessedTasks")]
        public List<TaskData> GetUnprocessedTasks()
        {
            return LogRun.LDA.ExecuteList<TaskData>("pifltr.GetUnprocessedTasks", DataAccessHelper.Database.Uls).Result ?? new List<TaskData>();
        }

        /// <summary>
        /// This method will mark the processed field in the pifltr.ProcessingQueue table
        /// </summary>
        /// <param name="letter"></param>
        [UsesSproc(DataAccessHelper.Database.Uls, "pifltr.SetProcessed")]
        public bool SetProcessedAt(TaskData task)
        {
            return LogRun.LDA.Execute("pifltr.SetProcessed", DataAccessHelper.Database.Uls,
                SqlParams.Single("ProcessQueueId", task.ProcessQueueId));
        }


        /// <summary>
        /// Inserts queues into QueueCompleter table and populates the QueueCompleterId in the ProcessingQueue table.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "pifltr.UpdateQueueCompleter")]
        public bool UpdateQueueCompleter(TaskData task)
        {
            return LogRun.LDA.Execute("pifltr.UpdateQueueCompleter", DataAccessHelper.Database.Uls,
                SqlParams.Single("ProcessQueueId", task.ProcessQueueId),
                SqlParams.Single("Ssn", task.Ssn),
                SqlParams.Single("TaskControlNumber", task.TaskControlNumber),
                SqlParams.Single("Queue", task.Queue),
                SqlParams.Single("SubQueue", task.SubQueue));
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "dbo.GetBorrUcode")]
        public string GetBorrowerCostCenterCode(TaskData tasks)
        {
            return LogRun.LDA.ExecuteSingle<string>("GetBorrUcode", DataAccessHelper.Database.Udw,
                SqlParams.Single("AccountNumber", tasks.AccountNumber),
                SqlParams.Single("LoanSeq", tasks.LoanSeq)).CheckResult();
        }

        /// <summary>
        /// Method will insert all co borrower printprocessingid into ULS.pifltr.ProcessingQueue 
        /// </summary>
        /// <param name="task"></param>
        [UsesSproc(DataAccessHelper.Database.Udw, "pifltr.UpdateCoBwrPrintProcessingId")]
        internal bool UpdateCoBwrPrintProcessingId(TaskData task)
        {
            return LogRun.LDA.Execute("pifltr.UpdateCoBwrPrintProcessingId", DataAccessHelper.Database.Uls,
                SqlParams.Single("ProcessQueueId", task.ProcessQueueId),
                SqlParams.Single("CoBwrPrintProcessingId", task.CoBwrPrintProcessingId));
        }

        /// <summary>
        /// Method will insert all printprocessingid into ULS.pifltr.ProcessingQueue 
        /// </summary>
        /// <param name="task"> </param>
        /// <returns> </returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "pifltr.UpdatePrintProcessingId")]
        public bool UpdatePrintProcessingId(TaskData task)
        {
            return LogRun.LDA.Execute("pifltr.UpdatePrintProcessingId", DataAccessHelper.Database.Uls,
                SqlParams.Single("ProcessQueueId", task.ProcessQueueId),
                SqlParams.Single("PrintProcessingId", task.PrintProcessingId));
        }

        

        /// <summary>
        /// This method will find and return all ucodes for uheaa guarenteed loans
        /// </summary>
        /// <returns> a list of ucodes for all uheaa guarenteed loans </returns>
        [UsesSproc(DataAccessHelper.Database.Bsys, "dbo.GetUcodes")]
        public List<String> GetUcodes()
        {
            return LogRun.LDA.ExecuteList<String>("dbo.GetUcodes", DataAccessHelper.Database.Bsys).CheckResult();
        }


        /// <summary>
        /// This method will find and return all the borrowers' demographics.
        /// </summary>
        /// <returns> Borrower </returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "pifltr.GetBorrowerDemographics")]
        public Borrower GetBorrowerDemographics(string accountNumber)
        {
            return LogRun.LDA.ExecuteSingle<Borrower>("pifltr.GetBorrowerDemographics", DataAccessHelper.Database.Uls,
                SqlParams.Single("AccountNumber", accountNumber)).Result;
        }

        /// <summary>
        /// Method gathers co-borrower demographics.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "pifltr.GetCoBorrowerDemographics")]
        public CoBorrower GetCoBorrowerDemographics(string coBorrowerSsn)
        {
            return LogRun.LDA.ExecuteSingle<CoBorrower>("pifltr.GetCoBorrowerDemographics", DataAccessHelper.Database.Uls,
                SqlParams.Single("CoBorrowerSsn", coBorrowerSsn)).Result;
        }
    }
}