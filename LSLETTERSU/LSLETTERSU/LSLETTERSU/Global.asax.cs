﻿using Microsoft.AspNet.Identity;
using System;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace LSLETTERSU
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ProcessLogRun PLR;
        static ActiveDirectoryUsers AD;

        public MvcApplication()
        {
            var mode = WebConfigurationManager.AppSettings["Mode"];
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), mode, true);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        protected void Session_Start()
        {
            var ses = new LslettersuSession();
            ses.SetSession(Session);
            ses.PLR = PLR;
        }

        protected void Session_End()
        {
            var ses = new LslettersuSession();
            ses.SetSession(Session);
            ses.PLR?.LogEnd();
        }

        protected void Application_AuthorizeRequest()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            PLR = PLR ?? new ProcessLogRun("LSLETTERSU", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false, false, Request.LogonUserIdentity.Name.Replace("UHEAA\\", ""));
            AD = AD ?? new ActiveDirectoryUsers(new DataAccess(PLR));
            AD.EnsureCache();

            string noAccessPath = "/Home/NoAccess";
            bool userIsValid = AD.ValidateUser(Request.LogonUserIdentity.Name.Replace("UHEAA\\", ""));
            bool isContent = Request.Path.ToLower().Contains("/content/") || Request.Path.ToLower().Contains("/bundles/");
            bool alreadyRedirected = Request.Path.ToLower().Contains(noAccessPath.ToLower());
            if (!userIsValid && !isContent && !alreadyRedirected)
            {
                string errorMessage = $"Failed Login Attempt.  User: { ActiveDirectoryUsers.UserName }. Valid Groups: {string.Join(",", AD.AuthorizedGroups)}.";
                PLR.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                HttpContext.Current.RewritePath(noAccessPath);
            }
        }
    }
}