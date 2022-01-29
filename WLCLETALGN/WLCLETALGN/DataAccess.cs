using System;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace WLCLETALGN
{
    public static class DataAccess
    {
        /// <summary>
        /// Gets the password for the UT ID of the user that is logged in
        /// </summary>
        /// <param name="userId">The ID of the person logged into the session</param>
        /// <returns>Decrypted password</returns>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spGetDecrpytedPassword")]
        public static string GetPassword(string userId)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, new SqlParameter("UserId", userId));
        }
    }
}
