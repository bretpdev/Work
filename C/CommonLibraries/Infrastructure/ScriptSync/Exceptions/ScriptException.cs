using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public abstract class ScriptException : Exception
    {
        public ScriptException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidConstructorException : ScriptException
    {
        public InvalidConstructorException(Exception innerException)
            : base("Incorrect arguments were passed to the script constructor.  This is likely an invalid script.", innerException) { }
    }

    public abstract class ReflectionException : ScriptException
    {
        public ReflectionException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class NotLoggedInException : ReflectionException
    {
        public NotLoggedInException(Exception innerException)
            : base("No account has been logged into this reflection session.", innerException) { }
    }

    public class IncorrectRegionException : ReflectionException
    {
        public IncorrectRegionException(Exception innerException)
            : base("This reflection session is not configured to the correct region.", innerException) { }
    }
}
