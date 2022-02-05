using System;

namespace SubSystemShared
{
    public class SecurityVariablesNotSetToValueException : Exception
    {

        public SecurityVariablesNotSetToValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SecurityVariablesNotSetToValueException(string message)
            : base(message)
        {
        }

        public SecurityVariablesNotSetToValueException()
            : base()
        {
        }

    }
}
