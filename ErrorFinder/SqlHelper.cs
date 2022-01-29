using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ErrorFinder
{
    public static class SqlHelper
    {
        public static bool CanAccessOpsdev
        {
            get
            {
                using (SqlConnection conn = new SqlConnection("Data Source=opsdev;Initial Catalog=EA27_BANA;Integrated Security=SSPI;Connect Timeout=5000"))
                {
                    try
                    {
                        conn.Open();
                        return true;
                    }
                    catch (SqlException se)
                    {
                        return false;
                    }
                }
            }
        }
    }
}
