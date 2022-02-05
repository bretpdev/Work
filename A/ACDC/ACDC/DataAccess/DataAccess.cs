using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace ACDC
{
    static class DataAccess
    {
        /// <summary>
        /// Checks whether a user has a given access key.
        /// </summary>
        /// <param name="role">The current user's role</param>
        /// <param name="key">The name of the access key to check for.</param>
        [UsesSproc(DataAccessHelper.Database.Csys, "ACDC_CheckIfRoleHasAccess")]
        public static bool UserHasAccess(string role, string key)
        {
            return DataAccessHelper.ExecuteSingle<bool>("ACDC_CheckIfRoleHasAccess", DataAccessHelper.Database.Csys,
                SqlParams.Single("UserKey", key),
                SqlParams.Single("RoleName", role));
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "ACDC_GetSquid")]
        public static int SqlUserId(string windowsUserName)
        {
            return DataAccessHelper.ExecuteSingle<int>("ACDC_GetSquid", DataAccessHelper.Database.Csys, new SqlParameter("WindowsUserName", windowsUserName));
        }

        /// <summary>
        /// All applications available
        /// </summary>
        /// <returns>List of Applications</returns>
        [UsesSproc(DataAccessHelper.Database.ACDC, "GetApplications")]
        public static List<Applications> Applications()
        {
            return DataAccessHelper.ExecuteList<Applications>("GetApplications", DataAccessHelper.Database.ACDC);
        }

        /// <summary>
        /// All arguments available for given application
        /// </summary>
        /// <param name="appID">Application Id</param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ACDC, "GetApplicationArguments")]
        public static List<Arguments> Arguments(int appID)
        {
            return DataAccessHelper.ExecuteList<Arguments>("GetApplicationArguments", DataAccessHelper.Database.ACDC, new SqlParameter("ApplicationId", appID));
        }

        /// <summary>
        /// Gets the path for the image
        /// </summary>
        /// <param name="key">The key containing the path information for the image</param>
        /// <returns>Image Path</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetFileSystemObject")]
        public static string GetImagePath(string key)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGENR_GetFileSystemObject", DataAccessHelper.Database.Csys,
                new SqlParameter("Key", key),
                new SqlParameter("TestMode", DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? false : true),
                new SqlParameter("Region", "Uheaa"));
        }

    }
}