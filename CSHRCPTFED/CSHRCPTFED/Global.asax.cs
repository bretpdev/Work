using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Web.Configuration;
using CSHRCPTFED.Infrastructure;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WebApi;


namespace CSHRCPTFED
{
    public class MvcApplication : HttpApplication
    {

        string noAccessPath = "/Home/NoAccess";
        static ProcessLogRun PLR;
        static ActiveDirectoryUsers AD;
        public MvcApplication()
        {
            var mode = WebConfigurationManager.AppSettings["Mode"];
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), mode, true);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        }
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PLR = new ProcessLogRun("CSHRCPTFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            AD = new ActiveDirectoryUsers(new DataAccess(PLR.LDA));
            AD.EnsureCache();
            PLR.LogEnd();

        }

        protected void Session_Start()
        {
            var ses = new CshRcptSession();
            ses.SetSession(Session);
            ses.PLR = new ProcessLogRun("CSHRCPTFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
        }

        protected void Session_End()
        {
            var ses = new CshRcptSession();
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
            bool userIsValid = AD.ValidateUser(Request.LogonUserIdentity.Name);
            bool isContent = Request.Path.ToLower().Contains("/content/") || Request.Path.ToLower().Contains("/bundles/");
            bool alreadyRedirected = Request.Path.ToLower().Contains(noAccessPath.ToLower());
            if (!userIsValid && !isContent && !alreadyRedirected)
            {
                string errorMessage = $"Failed Login Attempt.  User: { Request.LogonUserIdentity.Name }. Valid Groups: {string.Join(",",  AD.AuthorizedGroups )} ";
                PLR.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                HttpContext.Current.RewritePath(noAccessPath);
            }
        }
    }
}
