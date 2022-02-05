using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Uheaa.Common.DataAccess
{
    public static class EnterpriseFileSystem
    {
        public static string TempFolder
        {
            get
            {
                return GetPath("TEMP");
            }
        }
        public static string FtpFolder
        {
            get
            {
                return GetPath("FTP");
            }
        }
        public static string LogsFolder
        {
            get
            {
                return GetPath("LOGS");
            }
        }

        public static string SessionsFolder
        {
            get
            {
                return GetPath("Sessions");
            }
        }

        private static Dictionary<string, string> FileCache = new Dictionary<string, string>();
        public static string GetPath(string fileSystemKey, DataAccessHelper.Region? region = null)
        {
            var currentRegion = (region ?? DataAccessHelper.CurrentRegion).ToString();
            var cacheKey = currentRegion + ":" + fileSystemKey;
            if (FileCache.ContainsKey(cacheKey)) 
                return FileCache[cacheKey];
            try
            {
                string path = DataAccessHelper.ExecuteSingle<string>("spGENR_GetFileSystemObject", DataAccessHelper.Database.Csys,
                    SqlParams.Single("Key", fileSystemKey), SqlParams.Single("TestMode", DataAccessHelper.TestMode),
                    SqlParams.Single("Region", currentRegion));

                FileCache[cacheKey] = path;
                return path;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(string.Format("The file key {0} was not found in EnterpriseFileSystem.  Please contact Systems Support.", fileSystemKey), ex);
            }
        }

        public static void Invalidate()
        {
            FileCache.Clear();
        }
    }
}
