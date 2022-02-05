using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSFREVENTR
{
    class ExitScriptException : SystemException
    {

        public ExitScriptException()
            : base()
        {
        }

        public ExitScriptException(string message)
            : base(message)
        {
        }

        public ExitScriptException(string message, Exception innerException)
            : base(message,innerException)
        {
        }

    }
}
