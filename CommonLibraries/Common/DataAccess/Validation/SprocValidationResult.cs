using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DataAccess
{
   public class SprocValidationResult
    {
        public UsesSprocAttribute Attribute { get; set; }
        public SprocResult Result { get; set; }
        public Exception Exception { get; set; }

        public static SprocValidationResult Failure(UsesSprocAttribute attribute)
        {
            SprocValidationResult result = new SprocValidationResult();
            result.Attribute = attribute;
            result.Result = SprocResult.Failure;
            return result;
        }

        public static SprocValidationResult Error(UsesSprocAttribute attribute, Exception exception)
        {
            SprocValidationResult result = new SprocValidationResult();
            result.Attribute = attribute;
            result.Result = SprocResult.Error;
            result.Exception = exception;
            return result;
        }
    }

    public enum SprocResult
    {
        Success,
        Failure,
        Error
    }
}
