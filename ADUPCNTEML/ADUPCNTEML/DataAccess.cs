using System;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using System.Reflection;
using System.Collections.Generic;

namespace ADUPCNTEML
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, true);
        }

        /// <summary>
        /// Gets a list of borrowers needing a contact email update
        /// </summary>
        /// <returns>List of borrower information</returns>
        [UsesSproc(DataAccessHelper.Database.Udw,"dbo.PullContactEmailUpdateBorrowers")]
        public List<BorrowerData> IdentifyBorrowers()
        {
            return LDA.ExecuteList<BorrowerData>("dbo.PullContactEmailUpdateBorrowers", DataAccessHelper.Database.Udw).Result;
        }
    }
}