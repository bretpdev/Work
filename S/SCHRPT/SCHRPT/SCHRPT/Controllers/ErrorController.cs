using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common.WebApi;

namespace SCHRPT.Controllers
{
    public class ErrorController : SchrptController
    {
        public ActionResult ApiConnection()
        {
            return View();
        }

        protected override void SetBreadcrumbs(Breadcrumbs b)
        {
            //no breadcrumbs for error page
        }
    }
}