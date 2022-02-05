using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common.WebApi;
using UheaaWebManager.Models;

namespace UheaaWebManager.Controllers
{
    public class RolesController : UwmController
    {
        public override void SetBreadcrumbs(Breadcrumbs b)
        {
            b.Add("Roles", "/Roles");
        }
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var model = Session.DA.GetRoles();
                return View(model);
            }
            else
            {
                var model = Session.DA.GetRoleById(id.Value);
                if (model.InactivatedAt.HasValue)
                    return View("RetireRole", model);
                model.AvailableGroupNames = AvailableGroups(model.ActiveDirectoryRoleName);
                return View("EditRole", model);
            }
        }

        [HttpPost]
        public ActionResult SaveRole(RoleDetailedModel model)
        {
            if (Session.DA.SaveRole(model))
            {
                Session.PendingMessages.AddMessage(Request, "Role/Access successfully updated.");
                return RedirectToAction("Index", new { id = model.RoleId });
            }
            else
            {
                Session.PendingMessages.AddMessage(Request, "Error saving Role.", true);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Add()
        {
            var model = new RoleDetailedModel()
            {
                ControllerAccess = Session.DA.GetRoleAccessById(null),
                AvailableGroupNames = AvailableGroups()
            };

            return View("EditRole", model);
        }

        public ActionResult Retire(int id)
        {
            var role = Session.DA.GetRoleById(id);
            return View("RetireRole", role);
        }

        [HttpPost]
        public ActionResult Retire(RoleDetailedModel model)
        {
            if (Session.DA.RetireRole(model.RoleId.Value))
            {
                Session.PendingMessages.AddMessage(Request, "Role successfully retired.");
                return RedirectToAction("Index", new { });
            }
            else
            {
                Session.PendingMessages.AddMessage(Request, "Unable to retire role.", true);
                return RedirectToAction("Index", new { id = model.RoleId.Value });
            }
        }

        private List<string> AvailableGroups(string itemToInclude = null)
        {
            var results = new ActiveDirectoryUsers().AvailableGroups;
            var usedGroups = Session.DA.GetRoles();
            foreach (var usedGroup in usedGroups)
                results = results.Where(o => o != usedGroup.ActiveDirectoryRoleName).ToList();
            if (itemToInclude != null)
                results.Add(itemToInclude);
            return results.OrderBy(o => o).ToList();
        }

        public ActionResult Retired()
        {
            var retiredRoles = Session.DA.GetRetiredRoles();
            return View(retiredRoles);
        }
    }
}