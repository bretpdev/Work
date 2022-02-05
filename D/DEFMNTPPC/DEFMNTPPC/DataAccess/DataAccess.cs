using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DEFMNTPPC
{
    public class DataAccess
    {
        LogDataAccess LogData { get; set; }
        public DataAccess(ProcessLogRun logRun)
        {
            LogData = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, false);

        }

        public string GetSsn(string acctNum)
        {
            return LogData.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw,
                  SqlParams.Single("AccountNumber", acctNum)).Result;
        }

    }

}
