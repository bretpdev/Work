using System;

namespace ACDCAccess
{
    public class KeyAlreadyExistsException : Exception
    {
        public KeyAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public KeyAlreadyExistsException(string message)
            : base(message)
        {
        }

        public KeyAlreadyExistsException()
            : base()
        {
        }
    }
}
