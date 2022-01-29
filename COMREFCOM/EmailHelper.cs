using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace COMREFCOM
{
    public static class EmailHelper
    {
        public static string GetEmailRecipientString(string typeKey)
        {
            return string.Join(",", GetEmailRecipientList(typeKey).ToArray());
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetEmailRecipientsByTypeKey")]
        public static List<string> GetEmailRecipientList(string typeKey)
        {
            List<string> windowsUserNames = DataAccessHelper.ExecuteList<string>("GetEmailRecipientsByTypeKey", DataAccessHelper.Database.Bsys, SqlParams.Single("TypeKey", typeKey));
            return windowsUserNames.Select(o => o.Contains("@") ? o : o + "@utahsbr.edu").ToList();
        }
    }
}
