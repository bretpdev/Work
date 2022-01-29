using System;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OPSACTCMAA
{
    public class ActivityComments
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccessHelper.Database Db { get; set; }

        public ActivityComments(ProcessLogRun logRun)
        {
            LogRun = logRun;
            Db = DataAccessHelper.Database.Uls;
        }

        /// <summary>
        /// Calls the stored procedure to move the data from fp.FileProcessing to dbo.ArcAddProcessing
        /// </summary>
        /// <returns>True if there were no errors in processing</returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "OPSInsertARCs")]
        public bool ProcessComment()
        {
            Console.WriteLine("\r\nProcessing comments.");
            bool result = LogRun.LDA.Execute("OPSInsertARCs", Db);
            Console.WriteLine("\r\nFinished processing.");

            return result;
        }
    }
}