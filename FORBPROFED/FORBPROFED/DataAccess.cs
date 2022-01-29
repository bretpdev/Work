using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace FORBPROFED
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, true);
        }

        public List<ForbProcessingRecord> GetUnprocessedForb()
        {
            return LDA.ExecuteList<ForbProcessingRecord>("forb.[GetUnprocessedForb]", DataAccessHelper.Database.Cls).Result ?? new List<ForbProcessingRecord>();
        }

        public List<int> GetSelectedLoans(long forbProcessingId)
        {
            List<short> lonSeq =  LDA.ExecuteList<short>("forb.[GetSelectedLoans]", DataAccessHelper.Database.Cls, Sq("ForbProcessingId", forbProcessingId)).Result;
            return lonSeq.Select(s => (int)s).ToList();
        }

        public BusinessUnits GetBusinessUnit(long businessUnitId)
        {
            return LDA.ExecuteSingle<BusinessUnits>("forb.[GetBusinessUnit]", DataAccessHelper.Database.Cls, Sq("BusinessUnitId", businessUnitId)).Result;
        }

        public bool SetProcessedOn(long forbProcessingId, int arcAddProcessingId)
        {
            return LDA.Execute("forb.[SetProcessedOn]", DataAccessHelper.Database.Cls, Sq("ForbProcessingId", forbProcessingId), Sq("ArcAddProcessingId", arcAddProcessingId));
        }

        public SqlParameter Sq(string dbName, object dbValue)
        {
            return SqlParams.Single(dbName, dbValue);
        }
    }
}
