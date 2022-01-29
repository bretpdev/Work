using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using UheaaWebManager.Models;

namespace UheaaWebManager
{
    public class DataAccess
    {
        const DataAccessHelper.Database DB = DataAccessHelper.Database.UheaaWebManagement;
        public LogDataAccess LDA { get; set; }
        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        public DashboardModel GetDashboard()
        {
            return LDA.ExecuteSingle<DashboardModel>("webmanager.GetDashboard", DB).Result;
        }

        public List<ApiTokenSimpleModel> GetApiTokens()
        {
            return LDA.ExecuteList<ApiTokenSimpleModel>("webmanager.GetApiTokens", DB).Result;
        }

        public List<ApiTokenSimpleModel> GetRetiredApiTokens()
        {
            return LDA.ExecuteList<ApiTokenSimpleModel>("webmanager.GetRetiredApiTokens", DB).Result;
        }

        public ApiTokenDetailedModel GetApiTokenById(int apiTokenId)
        {
            var model = LDA.ExecuteSingle<ApiTokenDetailedModel>("webmanager.GetApiTokenById", DB, Sp("ApiTokenId", apiTokenId)).Result;
            model.ControllerAccess = GetApiTokenAccessById(apiTokenId);
            return model;
        }

        public List<ControllerAccessModel> GetApiTokenAccessById(int? apiTokenId)
        {
            return LDA.ExecuteList<ControllerAccessModel>("webmanager.GetApiTokenAccessById", DB, Sp("ApiTokenId", apiTokenId)).Result;
        }

        public bool SaveApiToken(ApiTokenDetailedModel token)
        {
            var result = LDA.ExecuteSingle<int>("webmanager.SaveApiToken", DB,
                Sp("ApiTokenId", token.ApiTokenId),
                Sp("GeneratedToken", token.GeneratedToken),
                Sp("StartDate", token.StartDate),
                Sp("EndDate", token.EndDate),
                Sp("Notes", token.Notes),
                Usr(),
                Sp("TokenAccess", GenerateDataTable(token.ControllerAccess))
            );
            token.ApiTokenId = result.Result;
            return result.DatabaseCallSuccessful;
        }

        public bool RetireApiToken(int apiTokenId)
        {
            return LDA.Execute("webmanager.RetireApiToken", DB, Sp("ApiTokenId", apiTokenId), Usr());
        }

        public List<RoleSimpleModel> GetRoles()
        {
            return LDA.ExecuteList<RoleSimpleModel>("webmanager.GetRoles", DB).Result;
        }

        public List<RoleSimpleModel> GetRetiredRoles()
        {
            return LDA.ExecuteList<RoleSimpleModel>("webmanager.GetRetiredRoles", DB).Result;
        }

        public RoleDetailedModel GetRoleById(int roleId)
        {
            var model = LDA.ExecuteSingle<RoleDetailedModel>("webmanager.GetRoleById", DB, Sp("RoleId", roleId)).Result;
            model.ControllerAccess = GetRoleAccessById(roleId);
            return model;
        }

        public List<ControllerAccessModel> GetRoleAccessById(int? roleId)
        {
            return LDA.ExecuteList<ControllerAccessModel>("webmanager.GetRoleAccessById", DB, Sp("RoleId", roleId)).Result;
        }

        public bool SaveRole(RoleDetailedModel role)
        {
            var result = LDA.ExecuteSingle<int>("webmanager.SaveRole", DB,
                Sp("RoleId", role.RoleId),
                Sp("ActiveDirectoryRoleName", role.ActiveDirectoryRoleName),
                Sp("Notes", role.Notes),
                Usr(),
                Sp("RoleAccess", GenerateDataTable(role.ControllerAccess))
            );
            role.RoleId = result.Result;
            return result.DatabaseCallSuccessful;
        }

        public bool RetireRole(int roleId)
        {
            return LDA.Execute("webmanager.RetireRole", DB, Sp("RoleId", roleId), Usr());
        }

        public List<UserTokenSimpleModel> GetUserTokens()
        {
            return LDA.ExecuteList<UserTokenSimpleModel>("webmanager.GetUserTokens", DB).Result;
        }

        public List<UserTokenSimpleModel> GetRetiredUserTokens()
        {
            return LDA.ExecuteList<UserTokenSimpleModel>("webmanager.GetREtiredUserTokens", DB).Result;
        }

        public UserTokenDetailedModel GetUserTokenById(int userTokenId)
        {
            var model = LDA.ExecuteSingle<UserTokenDetailedModel>("webmanager.GetUserTokenById", DB, Sp("UserTokenId", userTokenId)).Result;
            model.AvailableRoles = GetRoles();
            var allUsernames = new ActiveDirectoryUsers().WindowsUsers;
            var namesInUse = GetWindowsUsernamesInUse(userTokenId);
            var availableUsernames = allUsernames.Where(o => !namesInUse.Contains(o.AccountName));
            model.AvailableWindowsUsernames = availableUsernames.ToList();
            return model;
        }

        public List<UserTokenSimpleModel> GetUserTokensByRoleId(int roleId)
        {
            var model = LDA.ExecuteList<UserTokenSimpleModel>("webmanager.GetUserTokensByRoleId", DB, Sp("RoleId", roleId)).Result;
            var users = new ActiveDirectoryUsers().WindowsUsers;
            foreach (var item in model)
            {
                var match = users.SingleOrDefault(o => o.AccountName == item.AssociatedWindowsUserName);
                if (match != null)
                    item.AssociatedWindowsUserName = match.CalculatedName;
            }
            return model;
        }

        public List<string> GetWindowsUsernamesInUse(int? exceptUserTokenId = null)
        {
            return LDA.ExecuteList<string>("webmanager.GetWindowsUsernamesInUse", DB, Sp("ExceptUserTokenId", exceptUserTokenId)).Result;
        }

        public bool SaveUserToken(UserTokenDetailedModel token)
        {
            var result = LDA.ExecuteSingle<int>("webmanager.SaveUserToken", DB,
                Sp("UserTokenId", token.UserTokenId),
                Sp("GeneratedToken", token.GeneratedToken),
                Sp("RoleId", token.RoleId),
                Sp("AssociatedWindowsUsername", token.AssociatedWindowsUsername),
                Sp("StartDate", token.StartDate),
                Sp("EndDate", token.EndDate),
                Sp("Notes", token.Notes),
                Usr()
            );
            token.UserTokenId = result.Result;
            return result.DatabaseCallSuccessful;
        }

        public bool RetireUserToken(int userTokenId)
        {
            return LDA.Execute("webmanager.RetireUserToken", DB, Sp("UserTokenId", userTokenId), Usr());
        }

        private DataTable GenerateDataTable(List<ControllerAccessModel> access)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ControllerActionId");
            foreach (var item in access.Where(o => o.HasAccess))
                dt.Rows.Add(item.ControllerActionId);
            return dt;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }

        private SqlParameter Usr()
        {
            return Sp("WindowsUsername", Environment.UserName);
        }
    }
}