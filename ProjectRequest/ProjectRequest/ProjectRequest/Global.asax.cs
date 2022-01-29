using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Uheaa.Common.DataAccess;

namespace ProjectRequest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
#if !DEBUG
            if (!Request.IsSecureConnection)
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
#endif
        }

        ActiveDirectoryHelper AD;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var mode = WebConfigurationManager.AppSettings["Mode"];
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), mode, true);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
        }

        protected void Application_AuthorizeRequest()
        {
            if (new ActiveDirectoryHelper().CurrentUserIsAuthorized != true)
            {
                string noAccessPath = "/Home/NoAccess";
                if (Request.FilePath != noAccessPath)
                    Response.Redirect(noAccessPath);
            }
        }
    }
}