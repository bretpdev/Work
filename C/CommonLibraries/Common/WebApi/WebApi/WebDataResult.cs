using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.WebApi
{
    public class WebDataResult<T> : WebResult
    {
        public T Result { get; set; }

        public WebDataResult() { }
        public WebDataResult(WebResult previous)
        {
            DatabaseCallSuccessful = previous.DatabaseCallSuccessful;
            DatabaseConnectionSuccessful = previous.DatabaseConnectionSuccessful;
            ExceptionText = previous.ExceptionText;
        }
    }
}
