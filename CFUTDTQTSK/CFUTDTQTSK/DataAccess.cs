using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CFUTDTQTSK
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Checks to see if the passed in account number has an associated SSN in PD10
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "spGetSSNFromAcctNumber")]
        public bool ValidateAccountNumber(string accountNumber)
        {
            List<string> ssn = LogRun.LDA.ExecuteList<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, SqlParams.Single("AccountNumber", accountNumber)).Result;
            if (ssn?.Count > 0)
                return true;
            return false;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "CheckForFutureArc")]
        public bool CheckForDuplicate(string account, string recipient, string arc, string comment, DateTime processOn)
        {
            return LogRun.LDA.ExecuteSingle<int>("CheckForFutureArc", DataAccessHelper.Database.Uls,
                SqlParams.Single("AccountNumber", account),
                SqlParams.Single("RecipientId", recipient),
                SqlParams.Single("Arc", arc),
                SqlParams.Single("Comment", comment),
                SqlParams.Single("ProcessOn", processOn)).Result > 0;
        }
    }
}