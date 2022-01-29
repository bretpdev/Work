using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Data.SqlClient;

namespace UNEXDASCRB
{
    class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public DataAccess(LogDataAccess lda) => LDA = lda;

        [UsesSproc(DataAccessHelper.Database.UnexsysReports, "unexdascrb.GetScrubbableColumns")]
        public List<string> GetScrubbableColumns()
        {
            return LDA.ExecuteList<string>("unexdascrb.GetScrubbableColumns", DataAccessHelper.Database.UnexsysReports).Result;
        }

        [UsesSproc(DataAccessHelper.Database.UnexsysReports, "unexdascrb.GetScrubbableRows")]

        public List<string> GetScrubbableRows(string columnName)
        {
            return LDA.ExecuteList<string>("unexdascrb.GetScrubbableRows", DataAccessHelper.Database.UnexsysReports, Sp("ColumnName", columnName)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.UnexsysReports, "unexdascrb.ScrubRow")]

        public bool ScrubRow(string callId, string columnName)
        {
            return LDA.Execute("unexdascrb.ScrubRow", DataAccessHelper.Database.UnexsysReports, Sp("CallId", callId), Sp("ColumnName", columnName));
        }

        private SqlParameter Sp(string name, object val) => SqlParams.Single(name, val);
    }
}
