using System;

namespace GeneralHelp
{
    public class TicketAlreadyLockedException : Exception
    {

        public TicketAlreadyLockedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TicketAlreadyLockedException(string message)
            : base(message)
        {
        }

        public TicketAlreadyLockedException()
            : base()
        {
        }

    }
}
