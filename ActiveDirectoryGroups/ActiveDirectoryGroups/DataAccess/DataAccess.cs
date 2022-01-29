using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ActiveDirectoryGroups
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }


        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Returns a list of Users from SYSA_DAT_Users
        /// </summary>
        /// <param name="viewMode">The status of the users</param>
        /// <returns>List of BaseUser</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetUserData")]
        public List<BaseUser> GetTableData(string status)
        {
            try
            {
                return LogRun.LDA.ExecuteList<BaseUser>("spGENR_GetUserData", DataAccessHelper.Database.Csys,
                    SP("Status", status)).Result;
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error retrieving User Data", NotificationType.Other, NotificationSeverityType.Informational, ex);
                return null;
            }
        }

        /// <summary>
        /// Changes the user status from Active to Inactive
        /// </summary>
        /// <param name="testMode"></param>
        /// <param name="userId">SqlUserID of user being updated</param>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_InactivateUser")]
        public void SetUserInactive(int userId)
        {
            try
            {
                LogRun.LDA.Execute("spGENR_InactivateUser", DataAccessHelper.Database.Csys,
                    SP("SqlUserId", userId));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"There was an error setting {userId} as inactive", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }

        /// <summary>
        /// Inserts a new ActiveDirectoryUser in the SYSA_DAT_Users table
        /// </summary>
        /// <param name="user">ActiveDirectoryUser</param>
        /// <returns>True if insert was successful, False if not</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_InsertUser")]
        public bool InsertUser(ActiveDirectoryUser user)
        {
            try
            {
                LogRun.LDA.Execute("spGENR_InsertUser", DataAccessHelper.Database.Csys,
                    SP("WindowsUsreName", user.WindowsUserName),
                    SP("FirstName", user.FirstName),
                    SP("MiddleInitial", user.MiddleInitial),
                    SP("LastName", user.LastName),
                    SP("EMail", user.EmailAddress),
                    SP("Extension", user.Extension),
                    SP("Extension2", user.Extension2),
                    SP("BusinessUnit", user.BusinessUnit.ID),
                    SP("Role", user.Role.RoleID),
                    SP("Status", user.Status == true ? "Active" : "Inactive"),
                    SP("Title", user.Title),
                    SP("AesUserId", user.AesUserID));
                return true;
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"Error Adding User {user.WindowsUserName}", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
        }

        /// <summary>
        /// Updates user data in SYSA_DAT_Users
        /// </summary>
        /// <param name="user">ActiveDirectoryUser</param>
        /// <returns>True if update successful, False if not</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_UpdateUser")]
        public bool UpdateData(ActiveDirectoryUser user)
        {
            try
            {
                LogRun.LDA.Execute("spSYSA_UpdateUser", DataAccessHelper.Database.Csys,
                    SP("SqlUserId", user.SqlUserID),
                    SP("WindowsUserName", user.WindowsUserName),
                    SP("FirstName", user.FirstName),
                    SP("MiddleInitial", user.MiddleInitial),
                    SP("LastName", user.LastName),
                    SP("EMail", user.EmailAddress),
                    SP("Extension", user.Extension),
                    SP("Extension2", user.Extension2),
                    SP("BusinessUnit", user.BusinessUnit.ID),
                    SP("Role", user.Role.RoleID),
                    SP("Status", user.Status == true ? "Active" : "Inactive"),
                    SP("Title", user.Title),
                    SP("AesUserId", user.AesUserID));
                return true;
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"Error updating user {user.WindowsUserName}", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
        }

        /// <summary>
        /// Returns a list of the available Roles
        /// </summary>
        /// <returns>List of Role</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetRoles")]
        public List<Role> GetRoles()
        {
            try
            {
                return LogRun.LDA.ExecuteList<Role>("spSYSA_GetRoles", DataAccessHelper.Database.Csys).Result;
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error retrieving roles", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return null;
            }
        }

        /// <summary>
        /// Returns a list of the available Business Units
        /// </summary>
        /// <returns>List of BusinessUnit</returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetBusinessUnits")]
        public List<BusinessUnit> GetBusinessUnits()
        {
            try
            {
                return LogRun.LDA.ExecuteList<BusinessUnit>("spGENR_GetBusinessUnits", DataAccessHelper.Database.Csys).Result;
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error Retrieving Business Units", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return null;
            }
        }

        /// <summary>
        /// Adds a new AES Account number associated with an employee
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_InsertAesAccount")]
        public void AddAesAccount(int sqlUserId, string aesAccount)
        {
            LogRun.LDA.Execute("spSYSA_InsertAesAccount", DataAccessHelper.Database.Csys,
                SP("SqlUserId", sqlUserId),
                SP("AesAccount", aesAccount));
        }

        /// <summary>
        /// Deletes an AES Account number associated with an employee
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_DeleteAesAccount")]
        public void DeleteAesAccount(int aesAccountId)
        {
            LogRun.LDA.Execute("spSYSA_DeleteAesAccount", DataAccessHelper.Database.Csys,
                SP("AesAccountId", aesAccountId));
        }

        /// <summary>
        /// Returns a list of all the aes accounts associated with the employee
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetAesAccounts")]
        public List<AesAccountIds> GetAesAccounts(int sqlUserId)
        {
            return LogRun.LDA.ExecuteList<AesAccountIds>("spSYSA_GetAesAccounts", DataAccessHelper.Database.Csys,
                SP("SqlUserId", sqlUserId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Tlp, "GetAccount")]
        public TilpUser GetTilpUser(string userId)
        {
            return LogRun.LDA.ExecuteSingle<TilpUser>("GetAccount", DataAccessHelper.Database.Tlp,
                SP("UserId", userId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Tlp, "GetAuthList")]
        public List<AuthList> GetAuthList()
        {
            return LogRun.LDA.ExecuteList<AuthList>("GetAuthList", DataAccessHelper.Database.Tlp).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Tlp, "AddUser")]
        public void InsertTilpUser(TilpUser user)
        {
            LogRun.LDA.Execute("AddUser", DataAccessHelper.Database.Tlp,
                SP("UserId", user.UserId),
                SP("AuthLevel", user.AuthLevel),
                SP("Valid", user.Valid));
        }

        [UsesSproc(DataAccessHelper.Database.Tlp, "UpdateUser")]
        public void UpdateTilpUser(TilpUser user)
        {
            LogRun.LDA.Execute("UpdateUser", DataAccessHelper.Database.Tlp,
                SP("UserId", user.UserId),
                SP("AuthLevel", user.AuthLevel),
                SP("Valid", user.Valid));
        }

        private SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}