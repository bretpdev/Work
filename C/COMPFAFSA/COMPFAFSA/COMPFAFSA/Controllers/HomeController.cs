using COMPFAFSA.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common;
using System.IO;
using Microsoft.AspNet.Identity.Owin;

namespace COMPFAFSA.Controllers
{
    [RequireHttps]
    public class HomeController : BaseController
    {
        public HomeController()
        {

        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.NavHighlight = "Home";

            //If the user re-opens the website and is still logged in through a cookie we need to reset the session variable so that the layout shows the Manage page
            if(User.Identity.IsAuthenticated && (bool?)Session["Admin"] == null)
            {
                var isAdmin = DataAccess.GetUserIsAdmin(User.Identity.Name);
                //Set the status of admin in the session so that we can set up admin user controls based off of this
                Session["Admin"] = isAdmin;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}