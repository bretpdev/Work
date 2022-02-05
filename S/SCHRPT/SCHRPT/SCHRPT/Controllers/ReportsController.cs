using SCHRPT.Models;
using SCHRPT_Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WebApi;

namespace SCHRPT.Controllers
{
    public class ReportsController : SchrptController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BySchool(int[] schoolIds)
        {
            var model = new ReportGeneratorBySchool();
            model.Schools = Session.DA.GetSchools();
            model.Reports = Session.DA.GetReportTypes();
            model.SelectedSchoolIds = (schoolIds ?? new int[] { }).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult BySchool(int reportTypeId, int[] schoolIds)
        {
            var result = new SchrptProcess(Session.PLR, DataAccessHelper.CurrentRegion, null).ProcessManualSchoolRequest(reportTypeId, schoolIds);
            if (result.RowCount == 0)
            {
                Session.PendingErrorMessages.Add("Report generation yielded no results.");
                return View(GetBySchoolModel(schoolIds));
            }
            else
            {
                return File(Encoding.ASCII.GetBytes(result.GeneratedCsv), "text/csv", "report.csv");
            }
        }

        public ActionResult ByRecipient(int? recipientId)
        {
            var model = new ReportGeneratorByRecipient();
            model.Recipients = Session.DA.GetRecipients();
            model.Reports = Session.DA.GetReportTypes();
            model.SelectedRecipientId = recipientId;
            return View(model);
        }

        [HttpPost]
        public ActionResult ByRecipient(int reportTypeId, int? recipientId)
        {
            if (recipientId == null)
            {
                Session.PendingErrorMessages.Add("Please select a Recipient");
                return View(GetByRecipientModel(recipientId));
            }
            var qualifyingSchools = Session.DA.GetSchoolRecipients(null, recipientId);
            var result = new SchrptProcess(Session.PLR, DataAccessHelper.CurrentRegion, null).ProcessManualSchoolRequest(reportTypeId, qualifyingSchools.Select(o => o.SchoolId));
            if (result.RowCount == 0)
            {
                Session.PendingErrorMessages.Add("Report generation yielded no results.");
                return View(GetByRecipientModel(recipientId));
            }
            else
            {
                return File(Encoding.ASCII.GetBytes(result.GeneratedCsv), "text/csv", "report.csv");
            }
        }

        private ReportGeneratorBySchool GetBySchoolModel(int[] schoolIds)
        {
            var model = new ReportGeneratorBySchool();
            model.Schools = Session.DA.GetSchools();
            model.Reports = Session.DA.GetReportTypes();
            model.SelectedSchoolIds = schoolIds.ToList();
            return model;
        }

        private ReportGeneratorByRecipient GetByRecipientModel(int? recipientId)
        {
            var model = new ReportGeneratorByRecipient();
            model.Recipients = Session.DA.GetRecipients();
            model.Reports = Session.DA.GetReportTypes();
            model.SelectedRecipientId = recipientId;
            return model;
        } 

        protected override void SetBreadcrumbs(Breadcrumbs b)
        {
            b.Add(Breadcrumbs.Dashboard).Add("Reports", "/Reports");
        }
    }
}