using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SUBSCRPRT
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Loads data in ULS.subscrprt.PrintData from ODW
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "subscrprt.LoadNewData")]
        public void LoadData()
        {
            LogRun.LDA.Execute("subscrprt.LoadNewData", DataAccessHelper.Database.Uls);
        }

        /// <summary>
        /// Gets all available data from ULS
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "subscrprt.GetAvailableData")]
        public List<PrintData> GetAvailableData()
        {
            return LogRun.LDA.ExecuteList<PrintData>("subscrprt.GetAvailableData", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Sets the current record to processed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "subscrprt.SetProcessed")]
        public void SetProcessed(int id)
        {
            LogRun.LDA.Execute("subscrprt.SetProcessed", DataAccessHelper.Database.Uls,
                SqlParams.Single("PrintDataId", id));
        }
    }
}