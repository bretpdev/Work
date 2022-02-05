using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WebApi;

namespace RestDatabaseApi_SampleUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            var da = new WebApiDataAccess(DataAccessHelper.CurrentMode);
            var result = da.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw, SqlParams.Single("AccountNumber", ""));
            Console.WriteLine(result);
        }
    }
}
