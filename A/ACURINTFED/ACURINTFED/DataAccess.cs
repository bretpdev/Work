using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using Uheaa.Common.DataAccess;

namespace ACURINTFED
{
    public class DataAccess
    {
        private string _userIdForReassignedTasks;
        public const string SCRIPTID = "ACURINTFED";

        public DataAccess()
        {
            _userIdForReassignedTasks = null;
        }

        /// <summary>
        /// Get email address for error notification from CSYS
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetEmailForKey")]
        public string GetErrorNotificationRecipients()
        {
            List<string> emails = DataAccessHelper.ExecuteList<string>("spSYSA_GetEmailForKey", DataAccessHelper.Database.Csys, new SqlParameter("Key", "Accurint-FED"), new SqlParameter("BU", 35));
            emails.AddRange(DataAccessHelper.ExecuteList<string>("spSYSA_GetEmailForKey", DataAccessHelper.Database.Csys, new SqlParameter("Key", "Accurint-FED"), new SqlParameter("BU", 10)));

            return string.Join(",", emails.ToArray());
        }

        /// <summary>
        /// Gets FTP credentials 
        /// </summary>
        public Credentials GetFtpCredentials()
        {
            Credentials credentials = new Credentials();
            credentials.UserName = "b1025930";
            credentials.Password = GetPasswordForBatchProcessing(credentials.UserName);
            return credentials;
        }

        /// <summary>
        /// Gets the login info for the current User running the script
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spGetDecrpytedPassword")]
        public string GetPasswordForBatchProcessing(string userId)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, new SqlParameter("UserId", userId));
        }

        /// <summary>
        /// Gets the Manager of BU 3
        /// </summary>
        /// <returns>Managers UT id</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGetManagersUtId")]
        public string GetUserIdForReassignedTasks()
        {
            //We may use this multiple times, so keep it cached.
            if (_userIdForReassignedTasks == null)
            {
                _userIdForReassignedTasks = DataAccessHelper.ExecuteSingle<string>("spGetManagersUtId", DataAccessHelper.Database.Csys, new SqlParameter("BusinessUnitId", 3));
            }

            return _userIdForReassignedTasks;
        }

        /// <summary>
        /// Gets the queues to process
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGetAccurintFedQueues")]
        public string[] GetQueues()
        {
            return DataAccessHelper.ExecuteList<string>("spGetAccurintFedQueues", DataAccessHelper.Database.Csys).ToArray();
        }

        /// <summary>
        /// Gets the account number from borrowers ssn.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetAccountNumberFromSsn")]
        public static string GetAccount( string ssn)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetAccountNumberFromSsn", DataAccessHelper.Database.Cdw,
            new SqlParameter("SSN", ssn));
        }


    }//class
}//namespace
