using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace DUEDTECNG
{
    public class DataAccess
    {
        private LogDataAccess lda;
        public DataAccess(LogDataAccess lda)
        {
            this.lda = lda;
        }

        [UsesSproc(DB.Cls, "duedtecng.GetAvailableWork")]
        public List<BorrowerData> GetAvailableWork()
        {
            return lda.ExecuteList<BorrowerData>("duedtecng.GetAvailableWork", DB.Cls).Result;
        }

        [UsesSproc(DB.Cls, "duedtecng.MarkDueDateAsProcessed")]
        public void MarkDueDateAsProcessed(int dueDateChangeId, bool manualReviewNeeded)
        {
            lda.Execute("duedtecng.MarkDueDateAsProcessed", DB.Cls, Sp("DueDateChangeId", dueDateChangeId), Sp("ManualReviewNeeded", manualReviewNeeded));
        }

        [UsesSproc(DB.Cls, "duedtecng.AddNewWork")]
        public int AddNewWork(byte targetDay, byte destinationDay, int? workAddLimit)
        {
            return lda.ExecuteSingle<int>("duedtecng.AddNewWork", DB.Cls, Sp("TargetDay", targetDay), Sp("DestinationDay", destinationDay), Sp("WorkAddLimit", workAddLimit)).Result;
        }

        [UsesSproc(DB.Cls, "duedtecng.GetAppSettings")]
        public AppSettings GetAppSettings()
        {
            return lda.ExecuteSingle<AppSettings>("duedtecng.GetAppSettings", DB.Cls).Result;
        }

        public void MarkAccountUnprocessed(string accountIdentifier)
        {
            lda.Execute("duedtecng.MarkAccountUnprocessed", DB.Cls, Sp("AccountIdentifier", accountIdentifier));
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
