using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACHSETUPFD
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
