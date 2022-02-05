using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace RestDatabaseApi
{
    public class ErrorHelper
    {
        private HttpResponse response;
        public ErrorHelper(HttpResponse response)
        {
            this.response = response;
        }
        public void ReportErrorAndEndRequest(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            response.StatusCode = (int)statusCode;
            response.Write(message);
            response.End();
        }
    }
}