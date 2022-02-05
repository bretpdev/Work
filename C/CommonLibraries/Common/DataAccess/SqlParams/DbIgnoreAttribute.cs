using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DataAccess
{
    /// <summary>
    /// Indicates that the given property should be ignored by SqlParams entirely.
    /// Differs from ReadOnly in that the given value won't be pulled from the database either.
    /// </summary>
    public class DbIgnoreAttribute : Attribute
    {}
}
