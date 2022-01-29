using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;

namespace RestDatabaseApi
{
    public class RestDatabaseApiManagement
    {
        DataAccessHelper.Database DB = DataAccessHelper.Database.UheaaWebManagement;
        public List<ControllerAccess> GetUserTokenInformation(string token)
        {
            return DataAccessHelper.ExecuteList<ControllerAccess>("webapi.GetUserTokenAccess", DB, Sp("Token", token));
        }
        public List<ControllerAccess> GetApiTokenInformation(string token)
        {
            return DataAccessHelper.ExecuteList<ControllerAccess>("webapi.GetApiTokenAccess", DB, Sp("Token", token));
        }

        public Guid? GetUserTokenByUsername(string username)
        {
            return DataAccessHelper.ExecuteList<Guid>("webapi.GetUserTokenByUsername", DB, Sp("AssociatedWindowsUsername", username)).SingleOrDefault();
        }

        public ResolvedToken ResolveToken(string token)
        {
            Guid outer;
            if (!Guid.TryParse(token, out outer))
                return null;
            return DataAccessHelper.ExecuteList<ResolvedToken>("webapi.ResolveToken", DB, Sp("Token", token)).SingleOrDefault();
        }

        public void LogAccessAttempt(string relativeUrl, int? apiTokenId, int? userTokenId, bool successfulAccess)
        {
            DataAccessHelper.Execute("webapi.LogAccessAttempt", DB, Sp("RelativeUrl", relativeUrl), Sp("ApiTokenId", apiTokenId), Sp("UserTokenId", userTokenId), Sp("SuccessfulAccess", successfulAccess));
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}