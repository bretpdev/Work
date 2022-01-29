using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common.DataAccess;

namespace SchedulerWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = PriorityRequestsModel.ForCurrentUser(new UserHelper(User.Identity));
            if (model.UserType == UserType.Admin)
                return View("Administrators", model);
            if (model.UserType == UserType.ProjectManager)
                return View("ProjectManagers", model);
            return View(model);
        }

        [UsesSproc(DataAccessHelper.Database.Scheduler, "SetRequestParent")]
        [HttpPost]
        public ActionResult Reorder(int requestId, int? newParent)
        {
            var parameters = new
            {
                RequestPriorityId = requestId,
                NewParentId = newParent
            };
            DataAccessHelper.Execute("SetRequestParent", DataAccessHelper.Database.Scheduler, SqlParams.Generate(parameters));
            return Json(new { Success = true });
        }

        public ActionResult TestTypeToggle()
        {
            new UserHelper(User.Identity).TypeToggle();
            return Redirect("Index");
        }
    }
}
