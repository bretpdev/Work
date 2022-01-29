using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    class BorrowerCanNotBeProcessedException : Exception
    {
        public BorrowerCanNotBeProcessedException() : base()
        {

        }

        public BorrowerCanNotBeProcessedException(string message) : base(message)
        {

        }

        public BorrowerCanNotBeProcessedException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
