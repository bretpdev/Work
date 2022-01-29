using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common.WebApi;
using UheaaWebManager.Models;

namespace UheaaWebManager.Controllers
{
    public class ApiTokensController : UwmController
    {
        public override void SetBreadcrumbs(Breadcrumbs b)
        {
            b.Add("API Tokens", "/ApiTokens");
        }
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var model = Session.DA.GetApiTokens();
                return View(model);
            }
            else
            {
                var model = Session.DA.GetApiTokenById(id.Value);
                if (model.InactivatedAt.HasValue)
                    return View("RetireApiToken", model);
                return View("EditApiToken", model);
            }
        }

        [HttpPost]
        public ActionResult SaveApiToken(ApiTokenDetailedModel model)
        {
            if (Session.DA.SaveApiToken(model))
            {
                Session.PendingMessages.AddMessage(Request, "API Token successfully updated.");
                return RedirectToAction("Index", new { id = model.ApiTokenId });
            }
            else
            {
                Session.PendingMessages.AddMessage(Request, "Error saving API Token.", true);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Add()
        {
            var model = new ApiTokenDetailedModel()
            {
                GeneratedToken = Guid.NewGuid(),
                StartDate = DateTime.Now,
                ControllerAccess = Session.DA.GetApiTokenAccessById(null)
            };
            return View("EditApiToken", model);
        }

        public ActionResult Retire(int id)
        {
            var userToken = Session.DA.GetApiTokenById(id);
            return View("RetireApiToken", userToken);
        }

        [HttpPost]
        public ActionResult Retire(ApiTokenDetailedModel model)
        {
            if (Session.DA.RetireApiToken(model.ApiTokenId.Value))
            {
                Session.PendingMessages.AddMessage(Request, "API Token successfully retired.");
                return RedirectToAction("Index", new { });
            }
            else
            {
                Session.PendingMessages.AddMessage(Request, "Unable to retire API Token.", true);
                return RedirectToAction("Index", new { id = model.ApiTokenId.Value });
            }
        }

        public ActionResult Retired()
        {
            var retired = Session.DA.GetRetiredApiTokens();
            return View(retired);
        }

        [HttpPost]
        public ActionResult AjaxGenerateGuid()
        {
            return Json(Guid.NewGuid().ToString());
        }
    }
}