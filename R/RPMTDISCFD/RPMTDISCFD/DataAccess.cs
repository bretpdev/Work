using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using System.Data.SqlClient;
using Uheaa.Common.ProcessLogger;

namespace RPMTDISCFD
{
    public class DataAccess
    {
        LogDataAccess lda;
        public DataAccess(int processLogId)
        {
            lda = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, true, false);
        }
        /// <summary>
        /// Returns a decrypted password
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spGetDecrpytedPassword")]
        public ManagedDataResult<string> GetPassword(string userId)
        {
            return lda.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, userId.ToSqlParameter("UserId"));
        }
		/// <summary>
		/// Get ssn for account number
		/// </summary>
		[UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
		public ManagedDataResult<string> GetSsnFromFromAcctNo(string acctNum)
		{
			return lda.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw, new SqlParameter("AccountNumber",acctNum));
		}
    }
}
