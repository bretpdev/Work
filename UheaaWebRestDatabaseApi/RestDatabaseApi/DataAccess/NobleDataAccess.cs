using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace RestDatabaseApi
{
    public class NobleDataAccess
    {
        readonly DataAccessHelper.Database db;
        public NobleDataAccess(DataAccessHelper.Region region)
        {
            if (region == DataAccessHelper.Region.Uheaa)
                db = DataAccessHelper.Database.Udw;
            else if (region == DataAccessHelper.Region.CornerStone)
                db = DataAccessHelper.Database.Cdw;
            else
                db = (DataAccessHelper.Database)(-1);
        }

        public DialerInfo GetDialerInfo(string accountNumber)
        {
            try
            {
                return DataAccessHelper.ExecuteSingle<DialerInfo>("[restapi].[GetDialerInfo]", db, Sp("AccountNumber", accountNumber));
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("returned 0 records"))
                    return null;
                throw ex;
            }
        }

        public List<ManualDialing> GetManualDialingList(string storedProcedureName, string agent, string campaign)
        {
            try
            {
                return DataAccessHelper.ExecuteList<ManualDialing>(storedProcedureName, db, new SqlParameter("Agent", agent), new SqlParameter("Campaign", campaign));
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("returned 0 records"))
                    return null;
                throw ex;
            }
        }

        public string ValidateSproc(string campaign)
        {
            string sproc;
            try
            {
                sproc = DataAccessHelper.ExecuteSingle<string>("[NobleController].[GetSprocForCallingCampaign]", db, Sp("Campaign", campaign));
                if (sproc.IsPopulated())
                    return sproc;
            }
            catch(InvalidOperationException ex)
            {
                return null;
            } //no results returned
            return null;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}