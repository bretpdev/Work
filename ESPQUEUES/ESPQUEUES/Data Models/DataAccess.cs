using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ESPQUEUES
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        /// <summary>
        /// Adds unassigned tasks to the ProcessingQueue table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "espqueues.LoadTasks")]
        public void LoadTasksIntoProcessingQueue()
        {
            LDA.Execute("espqueues.LoadTasks", DataAccessHelper.Database.Uls);
        }

        /// <summary>
        /// Adds unassigned tasks to the ProcessingQueue table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "espqueues.GetUnprocessedTasks")]
        public List<QueueTask> GetUnprocessedTasks()
        {
            return LDA.ExecuteList<QueueTask>("espqueues.GetUnprocessedTasks", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Assigns the ArcAddProcessingId to the record in the ProcessingQueue table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "espqueues.SetArcAddId")]
        public void SetArcAddId(int processingQueueId, int arcAddProcessingId)
        {
            LDA.Execute("espqueues.SetArcAddId", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", processingQueueId), SP("ArcAddProcessingId", arcAddProcessingId));
        }

        /// <summary>
        /// Updates the processing step that the task is at. This enables recovery so that the task
        /// can be picked up where it was left off.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "espqueues.SetProcessingStepId")]
        public void SetProcessingStepId(int processingStepId, int processingQueueId)
        {
            LDA.Execute("espqueues.SetProcessingStepId", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", processingQueueId), SP("ProcessingStepId", processingStepId));
        }

        /// <summary>
        /// Sets the ProcessedAt timestamp for the record.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "espqueues.SetProcessedAt")]
        public void SetProcessedAt(QueueTask task)
        {
            LDA.Execute("espqueues.SetProcessedAt", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", task.ProcessingQueueId));
            task.ProcessedAt = DateTime.Now;
        }

        /// <summary>
        /// Sets the ProcessedAt timestamp for the record.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "espqueues.SetReassignedAt")]
        public void SetReassignedAt(QueueTask task)
        {
            LDA.Execute("espqueues.SetReassignedAt", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", task.ProcessingQueueId));
            task.ReassignedAt = DateTime.Now;
        }

        /// <summary>
        /// Determines whether there is already an open queue task on a given account and for the queue task associated with the given ARC.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "espqueues.HasOpenQueueTask")]
        public bool HasOpenQueueTask(string borrowerSsn, string arc)
        {
            return LDA.ExecuteSingle<bool>("espqueues.HasOpenQueueTask", DataAccessHelper.Database.Uls, SP("Arc", arc), SP("BorrowerSsn", borrowerSsn)).Result;
        }

        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
