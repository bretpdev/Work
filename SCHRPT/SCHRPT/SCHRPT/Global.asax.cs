using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WebApi;

namespace SCHRPT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            var mode = WebConfigurationManager.AppSettings["Mode"];
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), mode, true);
            AD = new ActiveDirectoryUsers();
        }
        ActiveDirectoryUsers AD;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            AD.EnsureCache();
        }

        protected void Session_Start()
        {
            var ses = new SchrptSession();
            ses.SetSession(Session);
            ses.PLR = new ProcessLogRun("SCHRPT", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
        }

        protected void Session_End()
        {
            var ses = new SchrptSession();
            ses.SetSession(Session);
            ses.PLR?.LogEnd();
        }

        protected void Application_BeginRequest()
        {
            if (!Request.IsSecureConnection && !Context.Request.IsLocal)
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));

            Request.ServerVariables["requestid"] = Guid.NewGuid().ToString().Replace("-", "").ToLowerInvariant();

            try
            {
                HttpContext.Current.Response.Headers.Remove("Server");
                HttpContext.Current.Response.Headers.Remove("X-Powered-By");
                //no-cache headers
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.AppendCacheExtension("no-store, must-revalidate");
                Response.Headers.Set("Pragma", "no-cache");
                Response.Headers.Set("Expires", "0");
                Response.Headers.Set("X-Content-Type-Options", "nosniff");
                Response.Headers.Set("X-XSS-Protection", "1; mode=block");
            }
            catch (PlatformNotSupportedException)
            {
                //running from VS, don't worry about it
            }
        }

        protected void Application_AuthorizeRequest()
        {
            if (AD?.CurrentUserIsAuthorized != true)
            {
                string noAccessPath = "/Home/NoAccess";
                if (Request.FilePath != noAccessPath)
                    Response.Redirect(noAccessPath);
            }
        }
    }
}
