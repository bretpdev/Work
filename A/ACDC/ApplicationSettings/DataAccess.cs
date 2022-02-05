using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ApplicationSettings
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, false);
        }


        [UsesSproc(DataAccessHelper.Database.ACDC, "AddApplication")]
        public int AddApplication(string applicationName, string accessKey, string startingClass, string startingDll, int sqlUserId, string sourcePath)
        {
            return LDA.ExecuteSingle<int>("AddApplication", DataAccessHelper.Database.ACDC,
                new SqlParameter("ApplicationName", applicationName),
                new SqlParameter("AccessKey", accessKey),
                new SqlParameter("StartingClass", startingClass),
                new SqlParameter("StartingDll", startingDll),
                new SqlParameter("AddedBy", sqlUserId),
                new SqlParameter("SourcePath", sourcePath)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.ACDC, "AddApplicationArguments")]
        public void AddApplicationArguments(int appId, int argId, int argOrder)
        {
            LDA.Execute("AddApplicationArguments", DataAccessHelper.Database.ACDC,
                new SqlParameter("ApplicationId", appId),
                new SqlParameter("ArgumentId", argId),
                new SqlParameter("ArgumentOrder", argOrder));
        }

        [UsesSproc(DataAccessHelper.Database.ACDC, "DeleteApplication")]
        public void DeleteApplication(int appId, int sqlUserId)
        {
            LDA.Execute("DeleteApplication", DataAccessHelper.Database.ACDC, 
                new SqlParameter("ApplicationId", appId),
                new SqlParameter("RemovedBy", sqlUserId));
        }

        [UsesSproc(DataAccessHelper.Database.ACDC, "GetArguments")]
        public List<Arguments> GetArguments()
        {
            return LDA.ExecuteList<Arguments>("GetArguments", DataAccessHelper.Database.ACDC).Result;
        }

        [UsesSproc(DataAccessHelper.Database.ACDC, "GetApplications")]
        public List<Applications> GetApplications()
        {
            List<Applications> apps = LDA.ExecuteList<Applications>("GetApplications", DataAccessHelper.Database.ACDC).Result;
            foreach (Applications app in apps)
            {
                app.Arguments = GetApplicationArguments(app.ApplicationId);
            }
            return apps;
        }

        [UsesSproc(DataAccessHelper.Database.ACDC, "GetApplicationArguments")]
        public List<Arguments> GetApplicationArguments(int applicationId)
        {
            return LDA.ExecuteList<Arguments>("GetApplicationArguments", DataAccessHelper.Database.ACDC, new SqlParameter("ApplicationId", applicationId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetApplicationKeys")]
        public List<AccessKeys> GetAccessKeys()
        {
            return LDA.ExecuteList<AccessKeys>("spSYSA_GetApplicationKeys", DataAccessHelper.Database.Csys).Result.Where(p => p.Type == "Access").ToList();
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetFileSystemObject")]
        public string GetImagePath(string key)
        {
            return LDA.ExecuteSingle<string>("spGENR_GetFileSystemObject", DataAccessHelper.Database.Csys,
                new SqlParameter("Key", key),
                new SqlParameter("TestMode", DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? false : true),
                new SqlParameter("Region", "Uheaa")).Result;
        }
    }
}