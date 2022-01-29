using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class RegionNotValidException : Exception
    {
        public RegionNotValidException() : base()
        {

        }

        public RegionNotValidException(string message) : base(message)
        {

        }

        public RegionNotValidException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
