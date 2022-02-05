using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;

namespace Uheaa.Common.DataAccess
{
    [Obsolete("This functionality removed in favor of SqlParams class.  Please remove when safe to do so")]
    public static class SqlParameterHelper
    {
        public static SqlParameter[] SqlParameters(this object o, params string[] names)
        {
            var lowerNames = names.Select(n => n.ToLower());
            List<SqlParameter> parameters = new List<SqlParameter>();
            Type type = o.GetType();
            var properties = type.GetProperties();
            if (names.Length == 0)
                names = properties.Select(x => x.Name).ToArray();
            foreach (string name in names)
            {
                var prop = properties.Where(p => p.Name.ToLower() == name.ToLower());
                if (prop.Count() == 1)
                    if (!prop.Single().GetGetMethod().IsStatic)
                        parameters.Add(new SqlParameter(name, prop.Single().GetValue(o, new object[] { })));
            }
            return parameters.ToArray();
        }
    }
}
