using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// The given field is required (cannot be empty)
    /// </summary>
    public class RequiredAttribute : Attribute
    {
        //no values necessary, if this attribute is applied, the field is required.
    }
}
