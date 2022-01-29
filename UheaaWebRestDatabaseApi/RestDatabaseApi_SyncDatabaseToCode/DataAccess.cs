using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace RestDatabaseApi_SyncDatabaseToCode
{
    class DataAccess
    {
        public List<ControllerInfo> GetControllers(bool gatherAllData = false)
        {
            string query =
@"
    SELECT
        ControllerId, Name [ControllerName]
    FROM
        webapi.Controllers
    WHERE
        RetiredAt IS NULL

";
            using (var cmd = DataAccessHelper.GetCommand(query, DataAccessHelper.Database.UheaaWebManagement))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var results = DataAccessHelper.ExecuteList<ControllerInfo>(cmd);
                if (gatherAllData)
                    foreach (var controller in results)
                        controller.ActionNames.AddRange(GetControllerActions((int)controller.ControllerId));
                return results;
            }
        }

        public List<string> GetControllerActions(int controllerId)
        {
            string query =
@"
    SELECT
        ActionName
    FROM
        webapi.ControllerActions
    WHERE
        ControllerId = @ControllerId
        AND
        RetiredAt IS NULL

";
            using (var cmd = DataAccessHelper.GetCommand(query, DataAccessHelper.Database.UheaaWebManagement))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("ControllerId", controllerId);
                return DataAccessHelper.ExecuteList<string>(cmd);
            }
        }

    }
}
