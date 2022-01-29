using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// Thrown when a crawler encounters a screen code that will not allow it to continue processing.
    /// </summary>
    public class EncounteredCancelCodeException : Exception
    {
        public EncounteredCancelCodeException(string message) : base(message) { }
    }
}
