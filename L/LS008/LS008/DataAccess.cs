using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace LS008
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(int plId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plId, false, true);
        }

        public List<DbData> GetAllProcesses()
        {
            return LDA.ExecuteList<DbData>("[ls008].GetLs008Data", DataAccessHelper.Database.Pls).CheckResult();
        }

        public void InsertHoldData(LS008Data qData, DateTime followUpDate, string userId, string holdReason)
        {
            LDA.Execute("[ls008].InsertHoldData", DataAccessHelper.Database.Pls, SqlParams.Single("TaskControlNumber", qData.TaskControlNumber),
                SqlParams.Single("DocumentControlNumber", qData.CorrDocNum), SqlParams.Single("AccountNumber", qData.AccountNumber),
                SqlParams.Single("ActivitySeq", qData.ActivitySeq), SqlParams.Single("FollowUpDate", followUpDate),
                 SqlParams.Single("PheaaUserId", userId), SqlParams.Single("HoldReason", holdReason));
        }
    }
}
