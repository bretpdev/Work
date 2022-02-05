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
    public class RecipientsController : SchrptController
    {
        protected override void SetBreadcrumbs(Breadcrumbs b)
        {
            b.Add(Breadcrumbs.Dashboard).Add("Recipients", "/Recipients");
        }
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var model = Session.DA.GetRecipient(id.Value);
                return View("EditRecipient", model);
            }
            else
            {
                var model = Session.DA.GetRecipients();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(Recipient recipient)
        {
            Session.DA.UpdateRecipient(recipient.RecipientId.Value, recipient.Name, recipient.Email, recipient.CompanyName);
            Session.PendingMessages.Add($"Recipient {recipient.Name.ToUpper()} successfully updated.");
            return RedirectToAction("Index");
        }

        public ActionResult Add()
        {
            var model = new Recipient();
            return View("EditRecipient", model);
        }

        [HttpPost]
        public ActionResult Add(Recipient recipient)
        {
            int id = Session.DA.AddRecipient(recipient.Name, recipient.Email, recipient.CompanyName);
            Session.PendingMessages.Add($"Recipient {recipient.Name.ToUpper()} successfully added.");
            return RedirectToAction("Index");
        }

        public ActionResult Retire(int id)
        {
            var school = Session.DA.GetRecipient(id);
            return View(school);
        }

        [HttpPost]
        public ActionResult Retire(Recipient recipient)
        {
            Session.DA.RetireRecipient(recipient.RecipientId.Value);
            Session.PendingMessages.Add($"Recipient {recipient.Name} successfully retired.");
            return RedirectToAction("Index", new { id = "" });
        }

        public ActionResult Schools(int id)
        {
            var model = new RecipientWithSchools();
            model.Recipient = Session.DA.GetRecipient(id);
            model.Schools = Session.DA.GetSchoolRecipients(null, id).OrderBy(o => o.SchoolName).ToList();
            model.ReportTypes = Session.DA.GetReportTypes();
            model.AllSchools = Session.DA.GetSchools();
            var existingIds = model.Schools.Select(s => s.SchoolId).ToArray();
            var nonSchools = model.AllSchools.Where(o => !o.SchoolId.Value.IsIn(existingIds)).ToList();
            var additionalSchoolRecipients = nonSchools.Select(o => new SchoolRecipient()
            {
                SchoolId = o.SchoolId.Value,
                SchoolName = o.Name,
                SchoolCode = o.SchoolCode,
                BranchCode = o.BranchCode,
                RecipientId = model.Recipient.RecipientId.Value
            });
            model.Schools.AddRange(additionalSchoolRecipients.OrderBy(o => o.SchoolName));
            return View(model);
        }

        [HttpPost]
        public ActionResult Schools(int id, SchoolRecipient[] schools)
        {
            var existingSchools = Session.DA.GetSchoolRecipients(null, id);
            List<SchoolRecipient> toAdd = new List<SchoolRecipient>(schools.Where(o => o.ReportTypeId.HasValue));
            List<SchoolRecipient> toRemove = new List<SchoolRecipient>();
            foreach (var existing in existingSchools)
            {
                var match = schools.SingleOrDefault(o => o.SchoolId == existing.SchoolId);
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
                Session.DA.AddSchoolRecipient(add.SchoolId, id, add.ReportTypeId.Value);
            Session.PendingMessages.Add("Recipient Subscriptions Updated");
            return RedirectToAction("Index");
        }
    }
}