using System;

namespace SubSystemShared
{
    public class LookNotAssignedException : Exception
    {

        public LookNotAssignedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public LookNotAssignedException(string message)
            : base(message)
        {
        }

        public LookNotAssignedException()
            : base()
        {
        }
    }
}
