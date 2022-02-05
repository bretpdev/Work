using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using System.Net.Http;
using System.Data.SqlClient;
using Uheaa.Common.WebApi;

namespace RestDatabaseApi
{
    public abstract class DbControllerBase : RestDatabaseApiControllerBase
    {
        readonly string[] ignores = new string[] { "apikey", "region", "storedprocedurename" };
        [HttpPost]
        public WebApiResult Execute()
        {
            var result = new WebApiResult();
            try
            {
                string storedProcedureName = HttpContext.Current.Request.Form["storedprocedurename"];
                var db = (DataAccessHelper.Database)Enum.Parse(typeof(DataAccessHelper.Database), DatabaseName, true);
                using (var cmd = DataAccessHelper.GetCommand(storedProcedureName, db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (string formKey in HttpContext.Current.Request.Form.Keys)
                    {
                        if (formKey.ToLower().IsIn(ignores))
                            continue;
                        cmd.Parameters.Add(SqlParams.Single(formKey, HttpContext.Current.Request.Form[formKey]));
                    }
                    result.Result = DataAccessHelper.ExecuteDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                result.ExceptionText = ex.ToString();
            }
            return result;
        }
    }
}