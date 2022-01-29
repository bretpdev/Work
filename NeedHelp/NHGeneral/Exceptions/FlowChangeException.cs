using System;

namespace NHGeneral
{
    public class FlowChangeException : Exception
    {
        public FlowChangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FlowChangeException(string message)
            : base(message)
        {
        }

        public FlowChangeException()
            : base()
        {
        }
    }
}