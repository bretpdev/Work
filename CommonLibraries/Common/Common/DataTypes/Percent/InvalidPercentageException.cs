using System;

namespace Uheaa.Common
{
    public class InvalidPercentageException : Exception
    {
        public InvalidPercentageException(decimal value)
            : base(string.Format("The given value {0} is not a valid percent.  Valid percents must be between 0% and 100% (0 <= decimalValue <= 1.0)", value))
        { }
    }
}
