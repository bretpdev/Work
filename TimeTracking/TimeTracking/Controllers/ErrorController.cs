using System.Web.Mvc;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    public class ErrorController : TimeTrackingController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ErrorMessage message)
        {
            Session.ProjectData = new ProjectData();
            ProjectData pData = new ProjectData();

            Session.ProjectData = pData;
            return View("~/Views/Home/Index.cshtml", pData);
        }
    }
}