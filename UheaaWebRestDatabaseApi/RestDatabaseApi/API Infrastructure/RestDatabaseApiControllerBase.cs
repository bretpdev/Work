using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WebApi;

namespace RestDatabaseApi
{
    public abstract class RestDatabaseApiControllerBase : System.Web.Http.ApiController
    {
        /// <summary>
        /// Set to True if you want to create instances of this class without running Access Checks.  Useful for Reflection.
        /// </summary>
        public static bool SkipAccessCheck { get; set; } = false;
        public abstract WebApiControllers ControllerId { get; }
        public abstract string ControllerFriendlyName { get; }
        public string DatabaseName => this.GetType().Name.Replace("Controller", "");
        public RestDatabaseApiControllerBase()
        {
            if (SkipAccessCheck)
                return;
            string actionName = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["action"];
            var matchingAccess = Info.ControllersWithAccess.Where(o => o.ControllerId == ControllerId && o.ActionName == actionName);
            var da = new RestDatabaseApiManagement();
            if (!matchingAccess.Any())
            {
                da.LogAccessAttempt(HttpContext.Current.Request.RawUrl, Info.ResolvedToken.ApiTokenId, Info.ResolvedToken.UserTokenId, false);
                Error.ReportErrorAndEndRequest($"Token {Info.ApiToken} is not authenticated for Controller {ControllerId}, Region {DataAccessHelper.CurrentRegion}");
            }
            else
                da.LogAccessAttempt(HttpContext.Current.Request.RawUrl, Info.ResolvedToken.ApiTokenId, Info.ResolvedToken.UserTokenId, true);
        }

        protected RequestInfo Info
        {
            get
            {
                return (RequestInfo)HttpContext.Current.Items["requestinfo"];
            }
        }

        protected ErrorHelper Error
        {
            get
            {
                return new ErrorHelper(HttpContext.Current.Response);
            }
        }
    }
}