using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Uheaa.Common.WebApi;

namespace RestDatabaseApi
{
    public class BorrowerController : RestDatabaseApiControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.Borrower;
        public override string ControllerFriendlyName => "Borrower Controller";

        [HttpPost]
        public string GetSsn()
        {
            string accountNumber = HttpContext.Current.Request.Form["accountnumber"];
            var da = new BorrowerDataAccess(Info.Region);
            var ssn = da.GetSsn(accountNumber);
            if (ssn != null)
            {
                return ssn;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [HttpPost]
        public string GetAccountNumber()
        {
            string ssn = HttpContext.Current.Request.Form["ssn"];
            var da = new BorrowerDataAccess(Info.Region);
            var accountNumber = da.GetAccountNumber(ssn);
            if (accountNumber != null)
            {
                return accountNumber;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}