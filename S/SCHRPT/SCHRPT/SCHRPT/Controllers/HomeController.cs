using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common.WebApi;

namespace SCHRPT.Controllers
{
    public class HomeController : SchrptController
    {
        protected override void SetBreadcrumbs(Breadcrumbs b)
        {
            b.Add(Breadcrumbs.Dashboard);
        }

        public ActionResult Index()
        {
            var dashboard = Session.DA.GetDashboardInfo();
            return View(dashboard);
        }

        public ActionResult NoAccess()
        {
            return View();
        }
    }
}