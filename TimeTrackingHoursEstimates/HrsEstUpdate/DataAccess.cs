using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace HrsEstUpdate
{
    class DataAccess
    {
        public static List<Estimates> GetEst()
        {
            return DataAccessHelper.ExecuteList<Estimates>("GetEmployeeEstimate", DataAccessHelper.Database.AppDev, SqlParams.Single("UserName", Environment.UserName));
        }

        public static void SaveData(Estimates est)
        {
            DataAccessHelper.Execute("InsertEstimate", DataAccessHelper.Database.AppDev, SqlParams.Single("RequestType", est.RequestType), SqlParams.Single("RequestNumber", est.RequestNumber),
                SqlParams.Single("EstimatedHours", est.EstimatedHours),SqlParams.Single("ReasonForAdjustment", est.ReasonForAdjustment), SqlParams.Single("AttachmentFileName", est.AttachmentFileName), SqlParams.Single("AdditionalHrs", est.AdditionalHrs),
                SqlParams.Single("Employee", Environment.UserName), SqlParams.Single("TestHours", est.TestHours));
        }
    }
}
