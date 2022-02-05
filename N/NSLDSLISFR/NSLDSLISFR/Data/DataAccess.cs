using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NSLDSLISFR
{
    class DataAccess
    {
        LogDataAccess lda;
        public DataAccess(int processLogId)
        {
            lda = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, true, true);
        }
        [UsesSproc(DataAccessHelper.Database.Cdw, "[nsldslisfr].[GetBorrowersWithOpenIdrQueueTasks]")]
        public ManagedDataResult<List<FinalBorrowerInfo>> GetBorrowersWithOpenIdrQueueTasks()
        {
            return lda.ExecuteList<FinalBorrowerInfo>("[nsldslisfr].[GetBorrowersWithOpenIdrQueueTasks]", DataAccessHelper.Database.Cdw);
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "[nsldslisfr].[BorrowerHasOpenNsldsInfoRequest]")]
        public ManagedDataResult<bool> BorrowerHasOpenNsldsInfoRequest(string ssn)
        {
            return lda.ExecuteSingle<bool>("[nsldslisfr].[BorrowerHasOpenNsldsInfoRequest]", DataAccessHelper.Database.Cdw, SqlParams.Single("Ssn", ssn));
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "[GetBorrowerApplicationSpouses]")]
        public ManagedDataResult<List<SpouseInfo>> GetBorrowerSpouses(string ssn)
        {
            return lda.ExecuteList<SpouseInfo>("[GetBorrowerApplicationSpouses]", DataAccessHelper.Database.Income_Driven_Repayment, SqlParams.Single("Ssn", ssn));
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "[nsldslisfr].[GetBorrowerInfo]")]
        public FinalBorrowerInfo GetBorrowerInfo(string ssn)
        {
            return lda.ExecuteSingle<FinalBorrowerInfo>("[GetBorrowerInfo]", DataAccessHelper.Database.Cdw, SqlParams.Single("Ssn", ssn)).Result;
        }
    }
}
