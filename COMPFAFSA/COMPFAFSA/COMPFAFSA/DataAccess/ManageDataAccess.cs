using COMPFAFSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMPFAFSA.DataAccess
{
    public partial class DataAccessHelper
    {
        public List<ManageUser> GetExistingUsers()
        {
            return ExecuteList<ManageUser>("compfafsa.GetExistingUsers");
        }

        public string GetUserName(int userId)
        {
            return ExecuteSingle<string>("compfafsa.GetUserName", Sp("UserId", userId));
        }

        public bool DeleteUser(int userId, string deletingUser)
        {
            var ret = ExecuteSingle<int>("compfafsa.DeleteUser", Sp("UserId", userId), Sp("DeletingUser", deletingUser));
            return Convert.ToBoolean(ret);
        }

        public bool UnlockUser(int userId)
        {
            var ret = ExecuteSingle<int>("compfafsa.UnlockUser", Sp("UserId", userId));
            return Convert.ToBoolean(ret);
        }

        public List<UserSchool> GetUserSchools(int userId)
        {
            return ExecuteList<UserSchool>("compfafsa.GetUserSchools", Sp("UserId", userId));
        }

        public bool DeleteUserSchool(int userId, int schoolId)
        {
            var ret = ExecuteSingle<int>("compfafsa.DeleteUserSchool", Sp("UserId", userId), Sp("SchoolId", schoolId));
            return Convert.ToBoolean(ret);
        }

        public bool AddUserSchool(int userId, string school)
        {
            var ret = ExecuteSingle<int>("compfafsa.AddUserSchool", Sp("UserId", userId), Sp("School", school));
            return Convert.ToBoolean(ret);
        }


    }
}