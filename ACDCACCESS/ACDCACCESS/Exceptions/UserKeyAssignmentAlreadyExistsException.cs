using System;

namespace ACDCAccess
{
    class UserKeyAssignmentAlreadyExistsException : Exception
    {
        public UserKeyAssignmentAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UserKeyAssignmentAlreadyExistsException(string message)
            : base(message)
        {
        }

        public UserKeyAssignmentAlreadyExistsException()
            : base()
        {
        }
    }
}
