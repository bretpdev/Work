using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CURRACCLAT
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) => LDA = lda;

        [UsesSproc(DataAccessHelper.Database.Ols, "[curracclat].GetAvailableProcessData")]
        public void PullDataFromWarehouse() => LDA.Execute("[curracclat].GetAvailableProcessData", DataAccessHelper.Database.Ols);

        [UsesSproc(DataAccessHelper.Database.Ols, "[curracclat].GetData")]
        public List<BorrowerData> GetData() => LDA.ExecuteList<BorrowerData>("[curracclat].GetData", DataAccessHelper.Database.Ols).Result;

        [UsesSproc(DataAccessHelper.Database.Ols, "[curracclat].SetProcessedAt")]
        public void SetProcessed(int processDataId) => LDA.Execute("[curracclat].SetProcessedAt", DataAccessHelper.Database.Ols,
                SqlParams.Single("ProcessDataId", processDataId));
    }
}