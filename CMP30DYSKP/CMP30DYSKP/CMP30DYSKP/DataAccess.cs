using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CMP30DYSKP
{
    /// <summary>
    /// Enables database communication.
    /// </summary>
    public class DataAccess
    {
        ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Checks the CT30 table to see if the passed-in borrower
        /// already has the passed-in task. Returns true if so,
        /// false otherwise.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Odw, "cmp30dyskp.HasSkipTask")]
        public bool TaskAlreadyExists(SkipTask task)
        {
            return LogRun.LDA.ExecuteSingle<bool>("cmp30dyskp.HasSkipTask", DataAccessHelper.Database.Odw,
                SP("Ssn", task.Ssn), SP("Task", task.Task)).Result;
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
