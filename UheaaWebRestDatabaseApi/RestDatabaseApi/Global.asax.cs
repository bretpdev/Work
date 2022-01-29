using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Uheaa.Common.DataAccess;

namespace RestDatabaseApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), WebConfigurationManager.AppSettings["DataAccessHelperMode"], true);
        }

        protected void Application_BeginRequest()
        {
            if (!Request.IsSecureConnection && !Context.Request.IsLocal)
            {
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
                return;
            }

            var error = new ErrorHelper(Response);
            var da = new RestDatabaseApiManagement();
            var info = new RequestInfo()
            {
                ApiToken = Request.Form["apikey"]
            };
            info.ResolvedToken = da.ResolveToken(info.ApiToken);
            info.ControllersWithAccess = da.GetApiTokenInformation(info.ApiToken);
            if (!info.ControllersWithAccess.Any())
                info.ControllersWithAccess = da.GetUserTokenInformation(info.ApiToken);
            if (!info.ControllersWithAccess.Any())
            {
                da.LogAccessAttempt(Request.RawUrl, info.ResolvedToken?.ApiTokenId, info.ResolvedToken?.UserTokenId, false);
                error.ReportErrorAndEndRequest("Unable to authenticate API Key " + info.ApiToken, System.Net.HttpStatusCode.Unauthorized);
                return;
            }
            var region = Request.Form["region"] ?? "";

            region = region.ToLower();
            if (region == "cornerstone")
            {
                info.Region = DataAccessHelper.Region.CornerStone;
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            }
            else if (region == "uheaa")
            {
                info.Region = DataAccessHelper.Region.Uheaa;
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            }
            //else
            //{
            //    da.LogAccessAttempt(Request.RawUrl, info.ResolvedToken?.ApiTokenId, info.ResolvedToken?.UserTokenId, false);
            //    error.ReportErrorAndEndRequest("No region specified.  Please specify region=cornerstone or region=uheaa", System.Net.HttpStatusCode.BadRequest);
            //    return;
            //}
            Context.Items["requestinfo"] = info;
        }
    }
}
