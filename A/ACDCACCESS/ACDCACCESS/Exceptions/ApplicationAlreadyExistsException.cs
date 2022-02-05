using System;

namespace ACDCAccess
{
    public class ApplicationAlreadyExistsException : Exception
    {
        public ApplicationAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ApplicationAlreadyExistsException(string message)
            : base(message)
        {
        }

        public ApplicationAlreadyExistsException()
            : base()
        {
        }
    }
}
