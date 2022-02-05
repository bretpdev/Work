using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracking.Models;
using Uheaa.Common;
using TimeTracking.Processing;

namespace TimeTracking.Controllers
{
    public class HomeController : TimeTrackingController
    {
        public ActionResult Index(IEnumerable<bool> unstopped, string sortOrder, string from, string to)
        {
            sortOrder = HttpUtility.HtmlEncode(sortOrder);
            if (!CheckRegex(sortOrder))
                return RedirectToAction("Error");
            ProjectData projectData = Session.ProjectData ?? new ProjectData();
            projectData.Systems = GetSystems();
            projectData.From = from.ToDateNullable() ?? DateTime.Now.GetCurrentWeekSunday();
            projectData.To = to.ToDateNullable() ?? DateTime.Now.GetNextSaturday();
            projectData.SelectedUser = Session.DA.AllUsers().Where(p => p.WindowsUserName == Session.UserName).FirstOrDefault();
            if (projectData.SelectedUser.SqlUserId != 0)
            {
                List<UserTime> times = Session.DA.GetTimesForUser(projectData.SelectedUser, unstopped != null && unstopped.FirstOrDefault());
                projectData.Times = times.Where(p => (p.EndTime.HasValue && p.StartTime.Date >= projectData.From.Date && p.EndTime.Value.Date <= projectData.To.Date) || (!p.EndTime.HasValue && p.StartTime.Date >= projectData.From.Date && p.StartTime.Date <= projectData.To.Date)).ToList();
            }
            //projectData.Times = .Where(p => (p.StartTime >= projectData.From && p.EndTime <= projectData.To) || (p.StartTime >= projectData.From && p.EndTime == null)).ToList();
            else
                return View("Error", new ErrorMessage() { Message = $"The user {Session.UserName} was not found in the list of active users." });
            if (sortOrder.IsPopulated())
            {
                projectData.SortOrder = sortOrder;
                projectData.Times = SetOrder(projectData);
            }
            Session.ProjectData = projectData;
            return View("~/Views/Home/Index.cshtml", projectData);
        }

        private bool CheckRegex(string sortOrder)
        {
            Regex regex = new Regex("[a-zA-Z0-9_\\s]+");
            if (sortOrder.IsPopulated() && !regex.IsMatch(sortOrder))
                return false;
            return true;
        }

        private List<SystemTypes> GetSystems()
        {
            List<SystemTypes> systems = Session.DA.GetSystems();
            systems.Insert(0, new SystemTypes { SystemType = "", SystemTypeId = 0 });
            return systems;
        }

        private List<UserTime> SetOrder(ProjectData projectData)
        {
            projectData.Predicate = p => p.TicketID;
            switch (projectData.SortOrder)
            {
                case "ticketid":
                    projectData.Predicate = p => p.TicketID;
                    break;
                case "systemtype":
                    projectData.Predicate = p => p.SystemType;
                    break;
                case "starttime":
                    projectData.Predicate = p => p.StartTime;
                    break;
                case "endtime":
                    projectData.Predicate = p => p.EndTime;
                    break;
                case "costcenter":
                    projectData.Predicate = p => p.CostCenter;
                    break;
                case "batch":
                    projectData.Predicate = p => p.BatchProcessing;
                    break;
                case "genericmeeting":
                    projectData.Predicate = p => p.GenericMeeting;
                    break;
            }
            projectData.IsDescending = !projectData.IsDescending;


            if (projectData.IsDescending)
                return projectData.Times.OrderBy(projectData.Predicate).ToList();
            return projectData.Times.OrderByDescending(projectData.Predicate).ToList();
        }

        [HttpGet]
        public ActionResult TrackedTime(int? id)
        {
            TrackedTime trackedTime = null;
            if (id.HasValue)
            {
                ViewBag.EditMode = "Edit";
                ViewBag.Message = $"Editing Tracking Id: {id}";
                UserTime time = Session.ProjectData.Times.Where(p => p.TimeTrackingId == id.Value).FirstOrDefault();
                trackedTime = new TrackedTime();
                trackedTime.Data = Session.ProjectData;
                trackedTime.Data.CurrentTime = time;
            }
            else
            {
                ViewBag.EditMode = "Create";
                ViewBag.Message = "Adding new time tracking record";
            }
            return View(trackedTime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrackedTime(TrackedTime time)
        {
            if (ModelState.IsValid)
            {

            }

            ProjectData projectData = Session.ProjectData;
            TrackedTime addTime = new TrackedTime();
            addTime.Data = projectData;
            return View(addTime);
        }

        [HttpGet]
        public ActionResult StartStop(int? id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddTime()
        {
            ProjectData projectData = Session.ProjectData;
            TrackedTime addTime = new TrackedTime();
            addTime.Data = projectData;
            addTime.Data.CurrentTime = null;
            return View(addTime);
        }

        [HttpPost]
        public ActionResult GetRequestNumbers(string type, string number)
        {
            List<int> reqNums = new List<int>();
            switch (type)
            {
                case "Need Help":
                    reqNums = Session.DA.GetNeedHelpRequestNumbers();
                    break;
                case "Sacker Script":
                    reqNums = Session.DA.GetSackerRequestNumbers("script");
                    break;
                case "Sacker SAS":
                    reqNums = Session.DA.GetSackerRequestNumbers("sas");
                    break;
                case "Letter Tracking":
                    reqNums = Session.DA.GetLetterRequestNumbers();
                    break;
                case "Projects":
                    reqNums = Session.DA.GetProjectRequestNumbers();
                    break;
                case "PMD":
                    reqNums = Session.DA.GetPMDRequestNumbers();
                    break;
                case "FSA CR":
                    break;
                default:
                    reqNums = new List<int>();
                    break;
            }
            List<string> nums = reqNums.ConvertAll(i => i.ToString());
            string foundNumber = nums.Where(p => p == number).FirstOrDefault();
            return Json(foundNumber ?? "");
        }

        public ActionResult NoAccess()
        {
            return View();
        }
    }
}