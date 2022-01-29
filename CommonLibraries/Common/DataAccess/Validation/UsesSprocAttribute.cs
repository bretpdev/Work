using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DataAccess
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UsesSprocAttribute : Attribute
    {
        public string SprocName { get; set; }
        public DataAccessHelper.Database Database { get; set; }
        public UsesSprocAttribute(DataAccessHelper.Database db, string sprocName)
        {
            SprocName = sprocName;
            Database = db;
        }
    }
}
