using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    class EncounteredForwardCodeException : Exception
    {
        public EncounteredForwardCodeException(string message) : base(message) { }
    }
}
