using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    class BorrowerNotFoundInWarehouseException : Exception
    {
        public BorrowerNotFoundInWarehouseException() : base()
        {

        }

        public BorrowerNotFoundInWarehouseException(string message) : base(message)
        {

        }

        public BorrowerNotFoundInWarehouseException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
