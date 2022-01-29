using DPLTRS.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DPLTRS
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public DataAccess(ProcessLogRun logRun)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, true);
        }

        [UsesSproc(DataAccessHelper.Database.Odw, "dbo.GetCoBorrowersForBorrower")]
        public List<CoBorrowerRecord> GetCoBorrowersForBorrower(string borrowerSsn)
        {
            return LDA.ExecuteList<CoBorrowerRecord>("dbo.GetCoBorrowersForBorrower", DataAccessHelper.Database.Odw, SqlParams.Single("BorrowerSSN", borrowerSsn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "dbo.CheckUserHasAccess")]
        public bool? CheckUserHasAccess(string uid, string typeKey)
        {
            return LDA.ExecuteSingle<bool?>("dbo.CheckUserHasAccess", DataAccessHelper.Database.Bsys, SqlParams.Single("UID", uid), SqlParams.Single("TypeKey", typeKey)).Result;
        }

    }
}
