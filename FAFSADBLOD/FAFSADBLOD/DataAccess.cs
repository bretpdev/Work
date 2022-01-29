using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;


namespace FAFSADBLOD
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

        [UsesSproc(DB.CompleteFinancialFafsa, "fafsadblod.LoadRecords")]
        public void LoadFiles(List<IsirRecord> records)
        {
            LDA.Execute("fafsadblod.LoadRecords", DB.CompleteFinancialFafsa, SqlParams.Single("Data", records.ToDataTable()));
        }

        [UsesSproc(DB.CompleteFinancialFafsa, "fafsadblod.AddStudentData")]
        public void AddStudentData()
        {
            LDA.Execute("fafsadblod.AddStudentData", DB.CompleteFinancialFafsa);
        }

        [UsesSproc(DB.CompleteFinancialFafsa, "fafsadblod.AddSchools")]
        public void AddSchools()
        {
            LDA.Execute("fafsadblod.AddSchools", DB.CompleteFinancialFafsa);
        }
    }
}
