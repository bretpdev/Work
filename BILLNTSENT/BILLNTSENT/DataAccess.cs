using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace BILLNTSENT
{
    class DataAccess
    {
        public DataAccess()
        {
        }

        public static List<string> GetEmailList()
        {
            string query = "SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE TypeKey = 'Billing Statement not Sent'";
            return DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys).ExecuteQuery<string>(query).ToList();
        }
    }
}
