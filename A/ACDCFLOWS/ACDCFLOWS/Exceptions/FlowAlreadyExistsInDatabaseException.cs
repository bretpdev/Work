using System;
using Q;

namespace ACDCFlows
{
    public class FlowAlreadyExistsInDatabaseException : Exception
    {
        public FlowAlreadyExistsInDatabaseException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public FlowAlreadyExistsInDatabaseException(string message)
            : base(message)
        {
        }

        public FlowAlreadyExistsInDatabaseException()
            : base()
        {
        }

    }
}
