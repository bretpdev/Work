using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORBPROFED
{
    class SessionValidationException : Exception
    {
        public SessionValidationException()
        {

        }

        public SessionValidationException(string message)
            :base(message)
        {

        }

        public SessionValidationException(string message, Exception inner)
            :base(message, inner)
        {

        }
    }
}
