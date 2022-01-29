using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public struct ValidationResults
    {
        public bool Success { get; internal set; }
        public IEnumerable<string> ErrorMessages { get; internal set; }
        public ValidationResults(bool success, params string[] errorMessages) : this()
        {
            Success = success;
            ErrorMessages = errorMessages;
        }
        public static ValidationResults Successful { get { return new ValidationResults(true); } }
    }
}
