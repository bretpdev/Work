using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SubSystemShared
{
    public class DataAccessBaseShared
    {

        public const string DEFAULT_DROP_DOWN_OPTION = "Please Select...";

		public List<string> GetBusinessFunctions()
		{
            return DataAccessHelper.ExecuteList<string>("spGENR_GetBusinessFunctions", DataAccessHelper.Database.Csys);
		}

		public BusinessUnit GetBusinessUnit(SqlUser user)
		{
            return DataAccessHelper.ExecuteSingle<BusinessUnit>("spGENR_GetBusinessUnit", DataAccessHelper.Database.Csys,
                SqlParams.Single("SqlUserId", user.ID));
		}

		public List<BusinessUnit> GetBusinessUnits()
        {
            return DataAccessHelper.ExecuteList<BusinessUnit>("spGENR_GetBusinessUnits", DataAccessHelper.Database.Csys);
		}

		public string GetPhoneExtension(string windowsUserId)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGENR_GetPhoneExtension", DataAccessHelper.Database.Csys,
                SqlParams.Single("WindowsUserId", windowsUserId));
		}

		/// <summary>
        /// Gathers all possible keys a given user has access to for a given system.  If system is "NO_SYSTEM" then method returns all keys a user has access to.
        /// </summary>
        /// <param name="system">Pass "NO_SYSTEM" for all keys a user has access to.</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<UserAccessKey> GatherExistingKeysForSystemUserCombo(string system, int user)
        {
            return DataAccessHelper.ExecuteList<UserAccessKey>("spSYSA_UserAccessForApplication", DataAccessHelper.Database.Csys,
                SqlParams.Single("SqlUserId", user),
                SqlParams.Single("Application", system));
        }

        /// <summary>
        /// Checks if a user has access for a specified key
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool HasAccess(string accessKey, string system, SqlUser user)
        {
            //get list our of session variable
            List<UserAccessKey> usersAccessKeys = GatherExistingKeysForSystemUserCombo(system, user.ID);
            //check if the user has access to the passed in key
            return (0 < (from k in usersAccessKeys
                         where k.Name == accessKey && k.Application == system
                         select k).Count());
        }

        /// <summary>
        /// Gets all systems listed for portal.
        /// </summary>
        /// <returns></returns>
        public List<TextValueOption> GetSystems()
        {
            return DataAccessHelper.ExecuteList<TextValueOption>("spSYSA_GetSystems", DataAccessHelper.Database.Csys);
        }

        /// <summary>
        /// Gets all steps for a given flow.
        /// </summary>
        /// <param name="flowID"></param>
        /// <returns></returns>
        public List<FlowStep> GetStepsForSpecifiedFlow(string flowID)
        {
            return DataAccessHelper.ExecuteList<FlowStep>("spFLOW_GetStepsForFlow", DataAccessHelper.Database.Csys,
                SqlParams.Single("FlowId", flowID));
        }

        /// <summary>
        /// Retrieves specified flow from DB.
        /// </summary>
        /// <param name="flowID"></param>
        /// <returns></returns>
        public Flow GetSpecifiedFlow(string flowID)
        {
            return DataAccessHelper.ExecuteSingle<Flow>("spFLOW_GetSpecifiedFlow", DataAccessHelper.Database.Csys,
                SqlParams.Single("FlowId", flowID));
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spACDC_GetRoleNames")]
        public static List<string> Roles()
        {
            return DataAccessHelper.ExecuteList<string>("spACDC_GetRoleNames", DataAccessHelper.Database.Csys);
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spACDC_GetUserAssignedRole")]
        public static string UserAssignedRole(string userName)
        {
            return DataAccessHelper.ExecuteSingle<string>("spACDC_GetUserAssignedRole", DataAccessHelper.Database.Csys, new SqlParameter("WindowsUserName", userName));
        }
    }
}