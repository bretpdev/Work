using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMPMTHIST
{
    public class InvalidTS26BorrowerException : Exception
    {

        public InvalidTS26BorrowerException()
            : base()
        {
        }

        public InvalidTS26BorrowerException(string message)
            : base(message)
        {
        }

        public InvalidTS26BorrowerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
