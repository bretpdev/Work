using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyCornerstoneLoan
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static Dictionary<string, string> StandardRedirects;
        static MvcApplication()
        {
            var sr = StandardRedirects = new Dictionary<string, string>();
            sr["/contactus.aspx"] = "/ContactUs";
            sr["/terms.aspx"] = "/Terms";
            sr["/duedate"] = "/Account/DueDate";
            sr["/autopay"] = "/Payments/AutoPay";
            sr["/autopay/default.aspx"] = "/Payments/AutoPay";
            sr["/thirdparty"] = "/Payments/AutoPay";
        }

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest()
        {
            string lowPath = Request.Path.ToLower();
            if (StandardRedirects.ContainsKey(lowPath))
                Response.Redirect(StandardRedirects[lowPath]);
            try
            {
                HttpContext.Current.Response.Headers.Remove("Server");
                HttpContext.Current.Response.Headers.Remove("X-Powered-By");
                //no-cache headers
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.AppendCacheExtension("no-store, must-revalidate");
                Response.Headers.Set("Pragma", "no-cache");
                Response.Headers.Set("Expires", "0");
            }
            catch (PlatformNotSupportedException)
            {
                //running from VS
            }
        }
    }
}
