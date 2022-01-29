using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectRequest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProjectRequest()
        {
            ViewBag.Message = "Project Request Page";

            return View();
        }

        public ActionResult ProductPrioritization()
        {
            ViewBag.Message = "Product Prioritization";
            var prodPrio = new List<Models.ProductPrioritization>();
            prodPrio.Add(new Models.ProductPrioritization { BusinessUnit = "Applications Development", Details = "Web development project to allow networked forms", FinanceScore = 5, ProjectName = "Uheaa Web Forms", RequestorScore = 5, ResourcesScore = 5, TotalScore = 100, UrgencyScore = 5 });
            return View(prodPrio);
        }
    }
}