using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;


namespace BBREININSC
{
    public class DataAccess
    {
        private LogDataAccess LDA_Manage { get; set; }
        private LogDataAccess LDA { get; set; }
        private ProcessLogRun PLR { get; set; }

        public DataAccess(ProcessLogRun plr)
        {
            LDA_Manage = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, true);
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, false);
            PLR = plr;
        }

        [UsesSproc(DB.Uls, "bbreininsc.SetProcessedAt")]
        public void SetProcessedAt(int recordId)
        {
            LDA_Manage.Execute("bbreininsc.SetProcessedAt", DB.Uls, SqlParams.Single("recordId", recordId));
        }

        [UsesSproc(DB.Uls, "bbreininsc.GetRecordsToProcess")]
        public List<ReinstatementRecord> GetReinstatementRecords()
        {
            List<ReinstatementRecord> list = LDA.ExecuteList<ReinstatementRecord>("bbreininsc.GetRecordsToProcess", DB.Uls).Result;
            return list;
        }

        [UsesSproc(DB.Uls, "bbreininsc.LoadU36Records")]
        [UsesSproc(DB.Uls, "bbreininsc.LoadR48Records")]
        [UsesSproc(DB.Uls, "bbreininsc.LoadN48Records")]
        [UsesSproc(DB.Uls, "bbreininsc.LoadU48Records")]
        public void LoadFiles()
        {
            LDA.Execute("bbreininsc.LoadU36Records", DB.Uls);
            LDA.Execute("bbreininsc.LoadR48Records", DB.Uls);
            LDA.Execute("bbreininsc.LoadN48Records", DB.Uls);
            LDA.Execute("bbreininsc.LoadU48Records", DB.Uls);
        }
    }
}
