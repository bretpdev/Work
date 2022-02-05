using System;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OPSActivityComments
{
    public class ActivityComments
    {
        public ProcessLogData LogData { get; set; }
        public DataAccessHelper.Region Region { get; set; }
        public DataAccessHelper.Database Db { get; set; }

        public ActivityComments(ProcessLogData logData, DataAccessHelper.Region region)
        {
            LogData = logData;
            Region = region;
            if (Region == DataAccessHelper.Region.CornerStone)
                Db = DataAccessHelper.Database.Cls;
            else
                Db = DataAccessHelper.Database.Uls;
        }

        /// <summary>
        /// Calls the stored procedure for the given region to move the data from fp.FileProcessing to dbo.ArcAddProcessing
        /// </summary>
        /// <returns>True if there were no errors in processing</returns>
        public bool ProcessComment()
        {
            try
            {
                Console.WriteLine("");
                Console.WriteLine(string.Format("Processing comments for {0} region", Region.ToString()));
                DataAccessHelper.Execute("OPSInsertARCs", Db);
                Console.WriteLine("");
                Console.WriteLine(string.Format("Finished processing for {0} region", Region.ToString()));
            }
            catch (Exception ex)
            {
                string message = string.Format("There was an error adding the {0} ARCs to the ArcAddProcessing database for OPS Activity Comments", Region.ToString());
                ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, LogData.ExecutingAssembly, ex);
                return false;
            }
            return true;
        }
    }
}