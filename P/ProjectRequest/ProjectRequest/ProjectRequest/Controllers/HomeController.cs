using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ProjectRequest.Controllers
{
    public class HomeController : Controller
    {
        private ActiveDirectoryHelper AD = new ActiveDirectoryHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminTools()
        {
            if (!AD.CurrentUserAuthorization.Admin)
            {
                return RedirectToAction("NoAccess");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Project Request";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "UHEAA";

            return View();
        }

        [HttpGet]
        public ActionResult ProjectRequest(int? id)
        {
            if (AD.UserHasCreatePermissions())
            {
                ViewBag.Readonly = false;
            }
            else if (AD.UserHasReadPermissions() && id.HasValue)
            {
                ViewBag.Readonly = true;
            }
            else
            {
                return RedirectToAction("NoAccess");
            }

            ViewBag.EditMode = "Edit";
            ViewBag.Message = "Project Request Page";
            Models.ProjectRequest project = new Models.ProjectRequest();
            if (id.HasValue)
            {
                project = DataAccess.GetProject(id.Value);
                if (AD.UserHasEditPermissions())
                {
                    ViewBag.Readonly = false;
                }
            }
            else
            {
                ViewBag.EditMode = "Create";
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectRequest(Models.ProjectRequest projectRequest)
        {
            //CREATE
            if (ModelState.IsValid && !projectRequest.ProjectRequestId.HasValue)
            {
                if (!AD.UserHasCreatePermissions())
                {
                    return RedirectToAction("NoAccess");
                }
                DataAccess.InsertProjectRequest(projectRequest);
                //Set up list of recipients
                List<string> notificationRecipients = DataAccess.GetNotificationRecipients();
                string recipients = "";
                foreach (string recipient in notificationRecipients)
                {
                    recipients += recipient + ";";
                }
                recipients = recipients.Remove(recipients.Length - 1, 1);
                //Build message body
                string subject = $"Project {projectRequest.ProjectName} Submitted";
                string body = $"Project Name: {projectRequest.ProjectName}\nSubmitted By: {projectRequest.SubmittedBy}\nStatus: {projectRequest.Status}\nDepartment: {projectRequest.Department}\nTime Submitted: {projectRequest.Date}\n\nProject Summary: {projectRequest.ProjectSummary}\n\nBusiness Need: {projectRequest.BusinessNeed}\n\nBenefits: {projectRequest.Benefits}\n\nImplementation Approach: {projectRequest.ImplementationApproach ?? ""}\n";
                //Notify email was sent
                bool testMode = false;
#if DEBUG
                testMode = true;
#endif
                EmailHelper.SendMail(testMode, recipients/*TO*/, "donotreply@uheaa.org"/*FROM*/, subject, body, ""/*CC*/, EmailHelper.EmailImportance.Normal, false);// ,"smtp.uheaa.org);
                return RedirectToAction("ProductPrioritization");
            }
            //EDIT
            else if (ModelState.IsValid && projectRequest.ProjectRequestId.HasValue)
            {
                if (!AD.UserHasEditPermissions())
                {
                    return RedirectToAction("NoAccess");
                }

                DataAccess.UpdateProjectRequest(projectRequest);
                return RedirectToAction("ProductPrioritization");
            }
            return View(projectRequest);
        }

        public ActionResult ProductPrioritization(string sortOrder, string currentFilter, string searchString, int? page, IEnumerable<bool> archived)
        {
            sortOrder = HttpUtility.HtmlEncode(sortOrder);
            currentFilter = HttpUtility.HtmlEncode(currentFilter);
            searchString = HttpUtility.HtmlEncode(searchString);
            if (!CheckRegex(sortOrder, currentFilter, searchString))
                return RedirectToAction("Error");
            bool checkArchive = false;
            //Active Directory Authentication
            var permissions = AD.CurrentUserAuthorization;
            if (!permissions.Read && !permissions.Admin)
            {
                return RedirectToAction("NoAccess");
            }
            else
            {

                //Setup button viewbag
                ViewBag.EditViewLink = "";
                ViewBag.ScoringLink = "";
                ViewBag.ArchiveLink = "";
                if (permissions.Read)
                {
                    ViewBag.EditViewLink = "View";
                }
                if (archived == null || (archived != null && archived.Count() == 1)) //Checkbox sends 2 for true so we want it to be 1, not checked
                {
                    if (permissions.Admin)
                    {
                        ViewBag.EditViewLink = "Edit";
                    }
                    if (permissions.Archive || permissions.Admin)
                    {
                        ViewBag.ArchiveLink = "Archive";
                    }
                    if (permissions.Score || permissions.ScoreFinance || permissions.ScoreRequestor || permissions.ScoreResources || permissions.ScoreUrgency || permissions.ScoreRisk || permissions.Admin)
                    {
                        ViewBag.ScoringLink = "Score";
                    }
                }
                else
                    checkArchive = true;
            }
            ViewBag.Message = "Product Prioritization";

            //Initialize Paging
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.currentFilter = searchString;

            //Set Up Sort Structure
            ViewBag.NameSort = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.BusinessSort = sortOrder == "business_unit" ? "business_unit_desc" : "business_unit";
            ViewBag.DetailsSort = sortOrder == "details" ? "details_desc" : "details";
            ViewBag.FinanceSort = sortOrder == "finance_score" ? "finance_score_desc" : "finance_score";
            ViewBag.RequestorSort = sortOrder == "requestor_score" ? "requestor_score_desc" : "requestor_score";
            ViewBag.UrgencySort = sortOrder == "urgency_score" ? "urgency_score_desc" : "urgency_score";
            ViewBag.ResourcesSort = sortOrder == "resource_score" ? "resource_score_desc" : "resource_score";
            ViewBag.RiskSort = sortOrder == "risk_score" ? "risk_score_desc" : "risk_score";
            ViewBag.StatusSort = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.TotalSort = string.IsNullOrEmpty(sortOrder) || sortOrder == "total_score_desc" ? "total_score" : "total_score_desc";

            IEnumerable<Models.ProductPrioritization> prodPrio;

            //Apply Filters
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper().Replace(" ", "");
                prodPrio = DataAccess.GetProductPrioritization(checkArchive).Where(r => r.ProjectName.ToUpper().Replace(" ", "").Contains(searchString) || r.BusinessUnit.ToUpper().Replace(" ", "").Contains(searchString));
            }
            else
            {
                prodPrio = DataAccess.GetProductPrioritization(checkArchive).OrderByDescending(r => r.TotalScore);
            }

            //Apply Ordering
            prodPrio = Models.ProductPrioritization.ApplySort(prodPrio, sortOrder);

            //Finish Paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(prodPrio.ToPagedList(pageNumber, pageSize));
        }

        private bool CheckRegex(string sortOrder, string currentFilter, string searchString)
        {
            Regex regex = new Regex("[a-zA-Z0-9_\\s]+");
            if (sortOrder.IsPopulated() && !regex.IsMatch(sortOrder))
                return false;
            if (currentFilter.IsPopulated() && !regex.IsMatch(currentFilter))
                return false;
            if (searchString.IsPopulated() && !regex.IsMatch(searchString))
                return false;
            return true;
        }

        public ActionResult ArchiveProject(int id)
        {
            if (!AD.UserHasArchivePermissions())
            {
                return RedirectToAction("NoAccess");
            }

            DataAccess.ArchiveProject(id);

            return RedirectToAction("ProductPrioritization");
        }

        public ActionResult ScoreOverview(int? id)
        {
            var permissions = AD.CurrentUserAuthorization;
            if (!permissions.Score && !permissions.ScoreFinance && !permissions.ScoreRequestor && !permissions.ScoreResources && !permissions.ScoreUrgency && !permissions.ScoreRisk && !permissions.Admin)
            {
                return RedirectToAction("NoAccess");
            }
            else
            {
                ViewBag.ScoreFinanceLink = "";
                ViewBag.ScoreRequestorLink = "";
                ViewBag.ScoreResourcesLink = "";
                ViewBag.ScoreUrgencyLink = "";
                if (permissions.Score || permissions.Admin || permissions.ScoreFinance)
                {
                    ViewBag.ScoreFinanceLink = "Score";
                }
                if (permissions.Score || permissions.Admin || permissions.ScoreRequestor)
                {
                    ViewBag.ScoreRequestorLink = "Score";
                }
                if (permissions.Score || permissions.Admin || permissions.ScoreResources)
                {
                    ViewBag.ScoreResourcesLink = "Score";
                }
                if (permissions.Score || permissions.Admin || permissions.ScoreUrgency)
                {
                    ViewBag.ScoreUrgencyLink = "Score";
                }
                if (permissions.Score || permissions.Admin || permissions.ScoreRisk)
                {
                    ViewBag.ScoreRiskLink = "Score";
                }
            }

            ViewBag.Message = "Score Overview";
            List<Models.ScoreType> scores = DataAccess.GetScoreOverview(id);
            return View(scores);
        }

        [HttpGet]
        public ActionResult ProjectScoring(int projectId, int scoreTypeId, int? scoreId)
        {
            ViewBag.Message = "Product Scoring";
            var scoringDepartment = new Models.ProjectScoring(projectId, scoreTypeId);

            var permissions = AD.CurrentUserAuthorization;
            if (!permissions.Score && !permissions.ScoreFinance && !permissions.ScoreRequestor && !permissions.ScoreResources && !permissions.ScoreUrgency && !permissions.ScoreRisk && !permissions.Admin)
            {
                return RedirectToAction("NoAccess");
            }
            else
            {
                if (scoreId.HasValue)
                {
                    scoringDepartment = DataAccess.GetScore(scoreId.Value);
                }
                else
                {
                    scoringDepartment.ScoringDepartment = DataAccess.GetScoreType(scoringDepartment.ScoreTypeId);
                }

                if (scoringDepartment.ScoringDepartment == "Finance Score" && (!permissions.ScoreFinance && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (scoringDepartment.ScoringDepartment == "Requestor Score" && (!permissions.ScoreRequestor && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (scoringDepartment.ScoringDepartment == "Urgency Score" && (!permissions.ScoreUrgency && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (scoringDepartment.ScoringDepartment == "Resources Score" && (!permissions.ScoreResources && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (scoringDepartment.ScoringDepartment == "Risk Score" && (!permissions.ScoreRisk && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (scoringDepartment.ScoringDepartment == null)
                {
                    return RedirectToAction("NoAccess");
                }
            }

            return View(scoringDepartment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectScoring(Models.ProjectScoring projectScoring)
        {
            if (projectScoring == null || projectScoring.ScoringDepartment == null)
            {
                return RedirectToAction("NoAccess");
            }
            var permissions = AD.CurrentUserAuthorization;
            if (!permissions.Score && !permissions.ScoreFinance && !permissions.ScoreRequestor && !permissions.ScoreResources && !permissions.ScoreUrgency && !permissions.ScoreRisk && !permissions.Admin)
            {
                return RedirectToAction("NoAccess");
            }
            else
            {
                if (projectScoring.ScoringDepartment == "Finance Score" && (!permissions.ScoreFinance && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (projectScoring.ScoringDepartment == "Requestor Score" && (!permissions.ScoreRequestor && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (projectScoring.ScoringDepartment == "Urgency Score" && (!permissions.ScoreUrgency && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (projectScoring.ScoringDepartment == "Resources Score" && (!permissions.ScoreResources && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
                if (projectScoring.ScoringDepartment == "Risk Score" && (!permissions.ScoreRisk && !permissions.Score && !permissions.Admin))
                {
                    return RedirectToAction("NoAccess");
                }
            }

            //CREATE NEW AND LINK OLD
            if (ModelState.IsValid)
            {
                DataAccess.InsertScore(projectScoring);
                return RedirectToAction("ScoreOverview", new { id = projectScoring.ProjectId });
            }
            return View(projectScoring);
        }

        [HttpGet]
        public ActionResult DownloadAllProjects()
        {
            var permissions = AD.CurrentUserAuthorization;
            if (!permissions.Admin)
            {
                return RedirectToAction("NoAccess");
            }

            var document = Infrastructure.SSRSReporting.GetAllProjects();

            if (document.ReportData != null)
            {
                //xlsx format
                //string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //return File(document, contentType, "reports.xlsx");

                string contentType = "application/vnd.ms-excel";
                return File(document.ReportData, contentType, "reports.xls");
            }
            else
            {
                return Content($"Report not found. EX: {document.CaughtException}");
            }
        }

        public ActionResult NoAccess()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}