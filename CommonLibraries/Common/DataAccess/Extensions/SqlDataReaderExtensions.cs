using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Uheaa.Common.DataAccess
{
    public static class SqlDataReaderExtensions
    {
        public static T Get<T>(this SqlDataReader reader, string columnName)
        {
            return (T)reader[columnName];
        }

        public static bool GetBool(this SqlDataReader reader, string columnName)
        {
            object val = reader[columnName];
            if (val is string)
            {
                string sVal = ((string)val).ToLower();
                if (sVal == "y" || sVal == "1")
                    return true;
                if (sVal == "n" || sVal == "0")
                    return false;
            }
            else if (val is bool)
                return (bool)val;
            return false;
        }
    }
}
