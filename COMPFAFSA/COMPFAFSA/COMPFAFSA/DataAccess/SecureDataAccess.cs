using COMPFAFSA.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMPFAFSA.DataAccess
{
    public partial class DataAccessHelper
    {
        public List<SecureStudentsModel> GetStudents()
        {
            return ExecuteList<SecureStudentsModel>("[compfafsa].GetUserStudentData", Sp("User", HttpContext.Current.User.Identity.GetUserId()));
        }
    }
}