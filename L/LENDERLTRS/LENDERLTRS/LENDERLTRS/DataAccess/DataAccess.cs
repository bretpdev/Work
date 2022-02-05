using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace LENDERLTRS
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        /// <summary>
        /// Adds a record to the table in ULS
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "InsertLenderUpdate")]
        public void AddToDb(LenderData data)
        {
            LDA.Execute("InsertLenderUpdate", DataAccessHelper.Database.Uls, DataAccessHelper.GenerateSqlParamsFromObject(data).ToArray());
        }
    }
}