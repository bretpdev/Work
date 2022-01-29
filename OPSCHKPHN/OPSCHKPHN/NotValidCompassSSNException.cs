using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPSCHKPHN
{
    class NotValidCompassSSNException : Exception
    {

        public NotValidCompassSSNException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotValidCompassSSNException(string message)
            : base(message)
        {
        }

        public NotValidCompassSSNException()
            : base()
        {
        }

    }
}
