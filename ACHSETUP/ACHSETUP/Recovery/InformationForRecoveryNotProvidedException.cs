using System;

namespace ACHSETUP
{
    class InformationForRecoveryNotProvidedException : Exception
    {

        public InformationForRecoveryNotProvidedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InformationForRecoveryNotProvidedException(string message)
            : base(message)
        {
        }

        public InformationForRecoveryNotProvidedException()
            : base()
        {
        }
    }
}