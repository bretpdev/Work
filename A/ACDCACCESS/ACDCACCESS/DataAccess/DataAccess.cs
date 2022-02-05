using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACDCAccess
{
	public class DataAccess
	{
        public ProcessLogRun LogRun { get; set; }

		public enum Type
		{
			Access,
			Notification
		}

		public DataAccess(ProcessLogRun logRun)
		{
            LogRun = logRun;
        }

        #region Get Data From Database

        /// <summary>
        /// Gets a list of the roles
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetRoles")]
        public List<Role> GetRoles()
		{
			try
			{
				return DataAccessHelper.ExecuteList<Role>("spSYSA_GetRoles", DataAccessHelper.Database.Csys);
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting Roles from CSYS", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return new List<Role>();
			}
		}

        /// <summary>
        /// Gets a list of keys from CSYS
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetApplicationKeys")]
		public List<Key> GetKeys(string application)
		{
			try
			{
				return DataAccessHelper.ExecuteList<Key>("spSYSA_GetApplicationKeys", DataAccessHelper.Database.Csys,
                    SqlParams.Single("Application", application));;
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting application keys from CSYS", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return new List<Key>();
			}
		}

        /// <summary>
        /// Gets the history for each key
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetApplicationKeyHistory")]
		public List<KeyHistory> GetKeyHistory(bool isHistory)
		{
			try
			{
                return DataAccessHelper.ExecuteList<KeyHistory>("spSYSA_GetApplicationKeyHistory", DataAccessHelper.Database.Csys,
                    SqlParams.Single("IsHistory", isHistory));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting history for keys", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return null;
			}
		}

        /// <summary>
        /// Gets a list of keys by their type
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetApplicationKeysByType")]
		public List<Key> GetKeysByType(string application, Type type)
		{
			try
			{
				return DataAccessHelper.ExecuteList<Key>("spSYSA_GetApplicationKeysByType", DataAccessHelper.Database.Csys,
                    SqlParams.Single("Application", application),
                    SqlParams.Single("Type", type.ToString()));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting keys by type", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return new List<Key>();
			}
		}

        /// <summary>
        /// Gets a list of RoleKeys
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetInactiveKeysForRole")]
        public List<RoleKey> GetInactiveKeysForRole(int roleID, DateTime dateDeleted)
		{
			try
			{
                return DataAccessHelper.ExecuteList<RoleKey>("spSYSA_GetInactiveKeysForRole", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleID", roleID),
                    SqlParams.Single("DeleteDate", dateDeleted),
                    SqlParams.Single("DeleteDatePlusMinute", dateDeleted.AddMinutes(1)));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting inactive RoleKeys from CSYS", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return null;
			}
		}

        /// <summary>
        /// Gets the date a role was deleted
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetDateRoleWasDeleted")]
        public DateTime GetDateRoleWasDeleted(int roleID)
        {
            try
            {
                return DataAccessHelper.ExecuteSingle<DateTime>("spSYSA_GetDateRoleWasDeleted", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleID", roleID));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error getting the date a role was deleted", NotificationType.ErrorReport,   NotificationSeverityType.Warning, ex);
                return new DateTime(1900, 1, 1);
            }
        }

        /// <summary>
        /// Gest the key assigned to a role
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetRoleAssignedKeys")]
        public List<Key> GetRoleAssignedKeys(int role, string system)
		{
			try
			{
                return DataAccessHelper.ExecuteList<Key>("spSYSA_GetRoleAssignedKeys", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleID", role),
                    SqlParams.Single("Application", system));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting the keys assigned to a role", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return null;
			}
		}

        /// <summary>
        /// Gets the key history for a role
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetKeysAssignedToRole")]
        public List<KeyHistory> GetKeyAssignedToRoles(int role, bool isHistory)
        {
            try
            {
                return DataAccessHelper.ExecuteList<KeyHistory>("spSYSA_GetKeysAssignedToRole", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleID", role),
                    SqlParams.Single("IsHistory", isHistory));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error getting key history for role " + role.ToString(), NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return null;
            }
        }

        /// <summary>
        /// Gest a list of systems
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetSystemList")]
		public List<string> GetSystems()
		{
			try
			{
                return DataAccessHelper.ExecuteList<string>("spGENR_GetSystemList", DataAccessHelper.Database.Csys);
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting list of systems", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return new List<string>(new string[] { ex.Message });
			}
		}

        /// <summary>
        /// Gets a list of Sql Users
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetSqlUsers")]
        public List<SqlUser> GetUsers()
		{
			try
			{
                return DataAccessHelper.ExecuteList<SqlUser>("spSYSA_GetSqlUsers", DataAccessHelper.Database.Csys);
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting list of sql user data", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return new List<SqlUser>();
			}
		}

        /// <summary>
        /// Gest a list of key ID's
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetKeyID")]
		public int GetKeyID(Key key)
		{
			try
			{
                return DataAccessHelper.ExecuteSingle<int>("spSYSA_GetKeyID", DataAccessHelper.Database.Csys,
                    SqlParams.Single("UserKey", key.Name),
                    SqlParams.Single("Application", key.Application));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting key ID for key " + key.ID, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return 0;
			}
		}

        /// <summary>
        /// Checks to see if a given role name is inactive
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_IsRoleNameInactive")]
		public Role CheckIfRoleNameInactive(string roleName)
		{
			try
			{
                return DataAccessHelper.ExecuteList<Role>("spSYSA_IsRoleNameInactive", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleName", roleName)).FirstOrDefault();
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error getting role from rolename " + roleName, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return new Role();
			}
        }

        /// <summary>
        /// Gets the history for a give role
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetRoleHistory")]
        public List<RoleHistory> GetRoleHistory(bool isHistory)
        {
            try
            {
                return DataAccessHelper.ExecuteList<RoleHistory>("spSYSA_GetRoleHistory", DataAccessHelper.Database.Csys,
                    SqlParams.Single("IsHistory", isHistory));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error getting history for keys", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the splunk events
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetSplunkEventIDs")]
        public List<int> GetSplunkEventID()
        {
            try
            {
                return DataAccessHelper.ExecuteList<int>("spSYSA_GetSplunkEventIDs", DataAccessHelper.Database.Csys);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error getting splunk event IDs", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return null;
            }
        }

        #endregion

        #region Insert Data To Database

        /// <summary>
        /// Checks if a role exists and add it if it does not
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_CheckRoleExistence")]
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_InsertRole")]
        public decimal AddRole(string role)
		{
			if (DataAccessHelper.ExecuteList<string>("spSYSA_CheckRoleExistence", DataAccessHelper.Database.Csys, SqlParams.Single("RoleName", role)).Count == 0)
			{
				try
				{
                    return DataAccessHelper.ExecuteSingle<decimal>("spSYSA_InsertRole", DataAccessHelper.Database.Csys,
                        SqlParams.Single("RoleName", role),
                        SqlParams.Single("SqlUserID", AccessUI._sqlUserId));
				}
				catch (Exception ex)
				{
                    LogRun.AddNotification("Error adding role: " + role, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				}
			}
			return 0;
		}

        /// <summary>
        /// Adds a new key to CSYS
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_AddKey")]
		public bool AddKey(Key key)
		{
			try
			{
				DataAccessHelper.Execute("spSYSA_AddKey", DataAccessHelper.Database.Csys,
                    SqlParams.Single("Application", key.Application),
                    SqlParams.Single("Key", key.Name),
                    SqlParams.Single("Type", key.Type),
                    SqlParams.Single("Description", key.Description),
                    SqlParams.Single("SqlUserID", key.AddedBy));
				return true;
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error adding key " + key.Name, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
				return false;
			}
		}

        /// <summary>
        /// Adds keys to role
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_AddKeyToRole")]
		public void AddKeysToRole(int userKey, int RoleID)
		{
			try
			{
                DataAccessHelper.Execute("spSYSA_AddKeyToRole", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleID", RoleID),
                    SqlParams.Single("UserKeyID", userKey),
                    SqlParams.Single("SqlUserID", AccessUI._sqlUserId));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification(string.Format("Error adding key {0} to role {1}", userKey, RoleID), NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
			}
		}

        #endregion

        #region Update Data In Database

        /// <summary>
        /// Update a role name
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_UpdateRole")]
		public bool UpdateRole(Role roleToChange, string newRoleName)
		{
			try
			{
                DataAccessHelper.Execute("spSYSA_UpdateRole", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleName", newRoleName),
                    SqlParams.Single("RoleID", roleToChange.RoleID));
				return true;
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error updating role name for role " + roleToChange, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
			}
			return false;
        }

        /// <summary>
        /// Updates the role for a user
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_UpdateUserRole")]
        public void ChangeUserRole(Role role, SqlUser user)
        {
            try
            {
                DataAccessHelper.Execute("spSYSA_UpdateUserRole", DataAccessHelper.Database.Csys,
                    SqlParams.Single("SqlUserID", user.ID), 
                    SqlParams.Single("RoleID", role.RoleID));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification(string.Format("Error updating role {0} for user {1}", role.RoleName, user.WindowsUserName), NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }

        #endregion

        #region Delete Data From Database

        /// <summary>
        /// Updates a roles RemovedAt date to delete the role
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_DeleteRole")]
        public void DeleteRole(int roleID)
		{
			try
			{
                DataAccessHelper.Execute("spSYSA_DeleteRole", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleID", roleID),
                    SqlParams.Single("SqlUserID", AccessUI._sqlUserId)); ;
			}
			catch (Exception ex)
			{
                LogRun.AddNotification("Error deleting role " + roleID, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
			}
		}

        /// <summary>
        /// Deletes keys from applications
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_DeleteKey")]
		public void DeleteKey(Key key)
		{
			try
			{
                DataAccessHelper.Execute("spSYSA_DeleteKey", DataAccessHelper.Database.Csys,
                    SqlParams.Single("Application", key.Application),
                    SqlParams.Single("UserKey", key.Name),
                    SqlParams.Single("SqlUserID", AccessUI._sqlUserId));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification(string.Format("Error deleting key {0} for application {1}", key.Name, key.Application), NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
			}
		}

        /// <summary>
        /// Removes roles from keys
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_RemoveKeyFromRole")]
		public void RemoveKeyFromRole(int userKey, int RoleID)
		{
			try
			{
                DataAccessHelper.Execute("spSYSA_RemoveKeyFromRole", DataAccessHelper.Database.Csys,
                    SqlParams.Single("RoleID", RoleID),
                    SqlParams.Single("UserKeyID", userKey),
                    SqlParams.Single("SqlUserID", AccessUI._sqlUserId));
			}
			catch (Exception ex)
			{
                LogRun.AddNotification(string.Format("Error removing key {0} from role {1}", userKey, RoleID), NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
			}
		}

        #endregion

    }
}