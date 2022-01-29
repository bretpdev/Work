using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    class WipeOutCancelledException : Exception
    {
        public WipeOutCancelledException() : base()
        {

        }

        public WipeOutCancelledException(string message) : base(message)
        {

        }

        public WipeOutCancelledException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
