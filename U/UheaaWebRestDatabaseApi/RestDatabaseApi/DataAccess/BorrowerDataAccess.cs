using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;

namespace RestDatabaseApi
{
    public class BorrowerDataAccess
    {
        readonly DataAccessHelper.Database db;
        public BorrowerDataAccess(DataAccessHelper.Region region)
        {
            if (region == DataAccessHelper.Region.Uheaa)
                db = DataAccessHelper.Database.Udw;
            else if (region == DataAccessHelper.Region.CornerStone)
                db = DataAccessHelper.Database.Cdw;
            else
                db = (DataAccessHelper.Database)(-1);
        }
        public string GetSsn(string accountNumber)
        {
            try
            {
                return DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", db, Sp("AccountNumber", accountNumber));
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("returned 0 record"))
                    return null;
                throw ex;
            }
        }
        public string GetAccountNumber(string ssn)
        {
            try
            {
                return DataAccessHelper.ExecuteSingle<string>("spGetAccountNumberFromSSN", db, Sp("Ssn", ssn));
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("returned 0 record"))
                    return null;
                throw ex;
            }
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}