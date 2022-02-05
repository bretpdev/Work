using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace AACQRYTILP
{
    class DataAccess
    {
        public ProcessLogRun logRun { get; set; }
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            this.logRun = logRun;
            this.LDA = logRun.LDA;
        }

        [UsesSproc(DataAccessHelper.Database.Tlp, "spAACExtraction")]
        public DataTable GetAACData()
        {
            //changing to not throw an exception because that is accounted for in the main process if no results are returned
            var result = LDA.ExecuteDataTable("spAACExtraction", DataAccessHelper.Database.Tlp, false).Result;
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Tlp, "spUpdateAfterAACExtraction")]
        public bool UpdateAfterExtraction()
        {
            var result = LDA.Execute("spUpdateAfterAACExtraction", DataAccessHelper.Database.Tlp);
            if(result == false)
            {
                logRun.AddNotification("Failed to update after AAC Extraction.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return result;
        }

        public SqlParameter SP(string parameterName, object parameterValue)
        {
            return SqlParams.Single(parameterName, parameterValue);
        }
    }
}
