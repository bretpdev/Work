using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Uheaa.Common.WebApi;

namespace UheaaWebManager.Controllers
{
    public class HomeController : UwmController
    {
        public override void SetBreadcrumbs(Breadcrumbs b)
        {
        }
        public ActionResult Index()
        {
            var dashboardModel = Session.DA.GetDashboard();
            return View(dashboardModel);
        }
        public ActionResult NoAccess()
        {
            ViewBag.Breadcrumbs.Clear();
            return View();
        }
    }
}