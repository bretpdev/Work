using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WebApi;

namespace UheaaWebManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), ConfigurationManager.AppSettings["Mode"]);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            new ActiveDirectoryUsers().EnsureCache();
        }

        protected void Session_Start()
        {
            var ses = new UwmSession();
            ses.SetSession(Session);
            ses.PLR = new ProcessLogRun("UWM", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
        }

        protected void Session_End()
        {
            var ses = new UwmSession();
            ses.SetSession(Session);
            ses.PLR?.LogEnd();
        }

        protected void Application_BeginRequest()
        {
            if (!Request.IsSecureConnection && !Context.Request.IsLocal)
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
            PendingMessages.BeginRequest(Request);
        }

        protected void Application_AuthorizeRequest()
        {
            if (!new ActiveDirectoryUsers().CurrentUserIsAuthorized && Request.FilePath != "/Authentication")
            {
                string noAccessPath = "/Home/NoAccess";
                if (Request.FilePath != noAccessPath)
                    Response.Redirect(noAccessPath);
            }
        }
    }
}
