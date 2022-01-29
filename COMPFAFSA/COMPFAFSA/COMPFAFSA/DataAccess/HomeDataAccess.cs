using COMPFAFSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMPFAFSA.DataAccess
{
    public partial class DataAccessHelper
    {
        public List<string> GetNotificationRecipients()
        {
            return ExecuteList<string>("[compfafsa].GetNotificationRecipients");
        }

        public List<string> GetSchools()
        {
            return ExecuteList<string>("compfafsa.GetSchools");
        }

        public List<string> GetClasses()
        {
            return ExecuteList<string>("compfafsa.GetClasses");
        }
    }
}