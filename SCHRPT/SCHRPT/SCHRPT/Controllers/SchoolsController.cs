using SCHRPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common;
using Uheaa.Common.WebApi;

namespace SCHRPT.Controllers
{
    public class SchoolsController : SchrptController
    {
        protected override void SetBreadcrumbs(Breadcrumbs b)
        {
            b.Add(Breadcrumbs.Dashboard).Add("Schools", "/Schools");
        }
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var model = Session.DA.GetSchool(id.Value);
                return View("EditSchool", model);
            }
            else
            {
                var model = Session.DA.GetSchools();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(School school)
        {
            Session.DA.UpdateSchool(school.SchoolId.Value, school.Name, school.SchoolCode, school.BranchCode);
            Session.PendingMessages.Add($"School {school.Name.ToUpper()} successfully updated.");
            return RedirectToAction("Index", new { id = school.SchoolId.Value });
        }

        public ActionResult Add()
        {
            var model = new School();
            return View("EditSchool", model);
        }

        [HttpPost]
        public ActionResult Add(School school)
        {
            int id = Session.DA.AddSchool(school.Name, school.SchoolCode, school.BranchCode);
            Session.PendingMessages.Add($"School {school.Name.ToUpper()} successfully added.");
            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult Retire(int id)
        {
            var school = Session.DA.GetSchool(id);
            return View(school);
        }

        [HttpPost]
        public ActionResult Retire(School school)
        {
            Session.DA.RetireSchool(school.SchoolId.Value);
            Session.PendingMessages.Add($"School {school.Name} successfully retired.");
            return RedirectToAction("Index", new { id = "" });
        }

        public ActionResult Recipients(int id)
        {
            var model = new SchoolWithRecipients();
            model.School = Session.DA.GetSchool(id);
            model.Recipients = Session.DA.GetSchoolRecipients(id, null).OrderBy(o => o.RecipientName).ToList();
            model.ReportTypes = Session.DA.GetReportTypes();
            model.AllRecipients = Session.DA.GetRecipients();
            var existingIds = model.Recipients.Select(r => r.RecipientId).ToArray();
            var nonRecipients = model.AllRecipients.Where(o => !o.RecipientId.Value.IsIn(existingIds)).ToList();
            var additionalSchoolRecipients = nonRecipients.Select(o => new SchoolRecipient()
            {
                RecipientName = o.Name,
                RecipientEmail = o.Email,
                RecipientCompanyName = o.CompanyName,
                RecipientId = o.RecipientId.Value
            });
            model.Recipients.AddRange(additionalSchoolRecipients.OrderBy(o => o.RecipientName));
            return View(model);
        }

        [HttpPost]
        public ActionResult Recipients(int id, SchoolRecipient[] recipients)
        {
            var existingRecipients = Session.DA.GetSchoolRecipients(id, null);
            List<SchoolRecipient> toAdd = new List<SchoolRecipient>(recipients.Where(o => o.ReportTypeId.HasValue));
            List<SchoolRecipient> toRemove = new List<SchoolRecipient>();
            foreach (var existing in existingRecipients)
            {
                var match = recipients.SingleOrDefault(o => o.RecipientId == existing.RecipientId);
                if (match == null)
                    toRemove.Add(existing);
                else
                {
                    if (match.ReportTypeId == existing.ReportTypeId) //no changes, no updates needed
                        toAdd.Remove(match);
                    else
                        toRemove.Add(existing);
                }
            }
            foreach (var remove in toRemove)
                Session.DA.RetireSchoolRecipient(remove.SchoolRecipientId.Value);
            foreach (var add in toAdd)
                Session.DA.AddSchoolRecipient(id, add.RecipientId, add.ReportTypeId.Value);
            Session.PendingMessages.Add("School Recipients Updated");
            return RedirectToAction("Index");
        }
    }
}