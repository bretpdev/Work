using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common;

namespace SchedulerWeb.Controllers
{
    public class SchedulerController : Controller
    {
        public ActionResult GetRunInfo()
        {
            var helper = new SchedulerHelper();
            var lastRunTime = helper.GetLastRunTime();
            return Json(new { IsRunning = helper.IsRunning, LastRunTime = lastRunTime.HasValue ? lastRunTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null });
        }

        public ActionResult Run()
        {
            new SchedulerHelper().Run();
            return Json(PriorityRequest.GetAll());
        }

        public ActionResult MoveToMyCourt(string requestId, string requestType)
        {
            var helper = new CourtHelper(new UserHelper(User.Identity));
            switch (requestType.Trim().ToLower())
            {
                case "sas":
                    helper.MoveSasToCourt(int.Parse(requestId));
                    break;
                case "script":
                    helper.MoveScriptToCourt(int.Parse(requestId));
                    break;
                case "letter":
                    helper.MoveLetterToCourt(int.Parse(requestId));
                    break;
            }
            return Redirect("~/");
        }

    }
}
