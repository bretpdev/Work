using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DataAccess
{
    /// <summary>
    /// Specifies the name of the column in the database, if it differs from the property name.
    /// </summary>
    public class DbNameAttribute : Attribute
    {
        public string Name { get; set; }
        public DbNameAttribute(string name)
        {
            Name = name;
        }
    }
}
