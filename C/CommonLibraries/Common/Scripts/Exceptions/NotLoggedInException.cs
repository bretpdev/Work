using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
    public class NotLoggedInException : Exception
    {
        public NotLoggedInException() : base("An attempt to initialize a script against an unauthenticated session occurred.  Please login first and try again.") { }
    }
}
