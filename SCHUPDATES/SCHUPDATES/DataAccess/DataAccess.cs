using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SCHUPDATES
{
    public class DataAccess
    {
        ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "GetFedLoansForSchoolUpdate")]
        public List<string> GetFedLoans()
        {
            List<string> loans = new List<string>();
            loans.AddRange(LogRun.LDA.ExecuteList<string>("GetFedLoansForSchoolUpdate", DataAccessHelper.Database.Csys).Result);
            return loans;
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "GetFFELLoanPgms")]
        public List<string> GetFfelLoanPrograms()
        {
            return LogRun.LDA.ExecuteList<string>("GetFFELLoanPgms", DataAccessHelper.Database.Csys).Result;
        }
    }
}