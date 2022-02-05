using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACHRIRDF
{
    public class DataAccess
    {
        LogDataAccess lda;
        public DataAccess(int processLogId)
        {
            this.lda = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, false, false);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[achrirdf].[CheckExistingArc]")]
        public long? CheckExistingArc(string accountNumber, string arc, string comment)
        {
            List<long> results = lda.ExecuteList<long>("[achrirdf].[CheckExistingArc]", DataAccessHelper.Database.Uls, Sp("AccountNumber", accountNumber), Sp("Arc", arc), Sp("Comment", comment)).Result;
            if (results == null || results.Count == 0 || results.FirstOrDefault() == -1)
            {
                return null;
            }
            else
            {
                return results.FirstOrDefault();
            }
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[achrirdf].[AddNewWorkToQueue]")]
        public void AddNewWorkToQueue()
        {
            using (var comm = DataAccessHelper.GetCommand("[achrirdf].[AddNewWorkToQueue]", DataAccessHelper.Database.Uls, DataAccessHelper.CurrentMode))
            {
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.CommandTimeout = 0;
                comm.ExecuteNonQuery();
            }
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[achrirdf].[GetPendingQueueWork]")]
        public ManagedDataResult<List<QueueRecord>> GetPendingQueueWork()
        {
            return lda.ExecuteList<QueueRecord>("[achrirdf].[GetPendingQueueWork]", DataAccessHelper.Database.Uls);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[achrirdf].[MarkQueueAsProcessed]")]
        public void MarkQueueAsProcessed(int processQueueId)
        {
            lda.Execute("[achrirdf].[MarkQueueAsProcessed]", DataAccessHelper.Database.Uls, Sp("ProcessQueueId", processQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[achrirdf].[BorrowerHasUnprocessedRIRAJ]")]
        public bool BorrowerHasUnprocessedRIRAJ(string accountNumber)
        {
            return lda.ExecuteSingle<bool>("[achrirdf].[BorrowerHasUnprocessedRIRAJ]", DataAccessHelper.Database.Uls, Sp("AccountNumber", accountNumber)).Result;
        }

        private SqlParameter Sp(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
