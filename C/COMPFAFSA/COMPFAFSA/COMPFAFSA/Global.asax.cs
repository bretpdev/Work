using COMPFAFSA.DataAccess;
using COMPFAFSA.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace COMPFAFSA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            Application["Mode"] = (ConfigurationManager.AppSettings["Mode"] ?? "dev") == "live" ? DataAccessHelper.Mode.Live : DataAccessHelper.Mode.Dev;
            GlobalFilters.Filters.Add(new RequireHstsAttribute(31536000) { IncludeSubDomains = true });
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true; //X-Frame-Options are configured in IIS, multiple headers is causing an issue.
        }
        protected void Application_EndRequest()
        {
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // this code will mark the forms authentication cookie and the
            // session cookie as Secure.
            //if (Response.Cookies.Count > 0)
            //{
            //    foreach (string s in Response.Cookies.AllKeys)
            //    {
            //        if (s == FormsAuthentication.FormsCookieName || "asp.net_sessionid".Equals(s, StringComparison.InvariantCultureIgnoreCase) || s.ToLowerInvariant().StartsWith("sessioncookie"))
            //        {
            //            Response.Cookies[s].Secure = true;
            //        }
            //    }
            //}
        }

    }
}
