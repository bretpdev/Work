using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DataAccess
{
    /// <summary>
    /// Indicates the given property will never be used in a stored procedure.
    /// Differs from DbIgnore in that DbReadOnly will still pull the value from SELECT sprocs
    /// </summary>
    public class DbReadOnlyAttribute : Attribute
    {}
}
