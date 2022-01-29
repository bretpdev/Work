using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MassAssignRangeAssignment
{
    public class DataAccess
    {

        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Gets a list of all the active queues
        /// </summary>
        /// <returns>List of strings</returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "[mssasgndft].GetQueueList")]
        public List<QueueNames> GetQueueList()
        {
            return LogRun.LDA.ExecuteList<QueueNames>("[mssasgndft].GetQueueList", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Inserts a new Queue
        /// </summary>
        /// <param name="queueName"></param>
        [UsesSproc(DataAccessHelper.Database.Uls, "[mssasgndft].InsertQueue")]
        public void InsertQueue(QueueNames queue)
        {
            LogRun.LDA.Execute("[mssasgndft].InsertQueue", DataAccessHelper.Database.Uls,
                SqlParams.Single("QueueName", queue.QueueName),
                SqlParams.Single("FutureDated", queue.FutureDated));
        }

        /// <summary>
        /// Updates the RemovedBy and RemovedOn fields to show the queue is no longer active.
        /// </summary>
        /// <param name="queueName"></param>
        [UsesSproc(DataAccessHelper.Database.Uls, "[mssasgndft].RemoveQueue")]
        public void RemoveQueue(int queueId)
        {
            LogRun.LDA.Execute("[mssasgndft].RemoveQueue", DataAccessHelper.Database.Uls,
                SqlParams.Single("QueueId", queueId));
        }

        /// <summary>
        /// Removes all records from the table and adds the new records
        /// </summary>
        /// <param name="ranges"></param>
        [UsesSproc(DataAccessHelper.Database.Uls, "[mssasgndft].UpdateRangeHistory")]
        [UsesSproc(DataAccessHelper.Database.Uls, "[mssasgndft].UpdateRange")]
        public bool InsertRangeAssignment(List<UserRange> ranges)
        {
            bool success = false;
            // Move all the records to the history table, which then clears out the Range table
            success = LogRun.LDA.Execute("[mssasgndft].UpdateRangeHistory", DataAccessHelper.Database.Uls);

            if (!success)
            {
                LogRun.AddNotification("Error updating the Range History table", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok("There was an error updating the Range History table. Please contact System Support", "Error Upating");
                return success;
            }

            foreach (UserRange range in ranges) //Insert all records
            {
                success = LogRun.LDA.Execute("[mssasgndft].UpdateRange", DataAccessHelper.Database.Uls,
                    SqlParams.Single("AesId", range.AesId),
                    SqlParams.Single("UserId", range.UserId),
                    SqlParams.Single("BeginRange", range.BeginRange),
                    SqlParams.Single("EndRange", range.EndRange));
                if (!success)
                {
                    string message = $"Error inserting range assignment for UserId: {range.UserId}, Being range: {range.BeginRange}, End range: {range.EndRange}. Please contact System Support";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Error.Ok(message, "Error Inserting Range");
                    return success;
                }
            }
            return success;
        }

        /// <summary>
        /// Gets the list of current range assignments.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "[mssasgndft].GetRanges")]
        public List<UserRange> GetCurrentUsers()
        {
            return LogRun.LDA.ExecuteList<UserRange>("[mssasgndft].GetRanges", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[mssasgndft].UpdateQueue")]
        public void UpdateQueue(QueueNames queueNames)
        {
            LogRun.LDA.Execute("[mssasgndft].UpdateQueue", DataAccessHelper.Database.Uls,
                SqlParams.Single("QueueId", queueNames.QueueId),
                SqlParams.Single("FutureDated", queueNames.FutureDated));
        }
    }
}