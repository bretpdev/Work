using System;

namespace SubSystemShared
{
    public class EmailException : Exception
    {

        public EmailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EmailException(string message)
            : base(message)
        {
        }

        public EmailException()
            : base()
        {
        }
    }
}
