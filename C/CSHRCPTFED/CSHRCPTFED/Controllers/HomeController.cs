using System.Web.Mvc;
using CSHRCPTFED.Infrastructure;
using CSHRCPTFED.ViewModels;
using System;
using System.Windows.Forms;

namespace CSHRCPTFED.Controllers
{
    public class HomeController : CshRcptController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new CashReceiptModel());
        }



        [HttpPost]
        public ActionResult Index(
            [Bind(Include = "AccountIdentifier, BorrowerName, CheckId, Amount, CheckDate, Payee")]
            CashReceiptModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.CheckDate > DateTime.Now)
            {
                ViewBag.Error = "Date cannot be in the future.";
                return View(model);
            }

            var identifiers = Session.DA.AccountNumber(model.AccountIdentifier);
            if (model.AccountIdentifier == AccountIdentifiers.NoAcountNumber || model.AccountIdentifier == AccountIdentifiers.NoSsn)
                identifiers = AccountIdentifiers.None();
            else
            {
                var existing = Session.DA.FindExistingCheck(identifiers.AccountNumber, model.CheckId, model.CheckDate, model.Payee);
                if (existing != null)
                {
                    ViewBag.Error = $"This check was already added on {existing.AddedAt} by {existing.AddedBy}";
                    return View(model);
                }
            }

            if (identifiers == null)
            {
                ViewBag.Error = "Unable to locate borrower.";
                return View(model);
            }

            CashReceiptProcessor processor = new CashReceiptProcessor(Session.DA, Session.PLR);
            string successMessage = processor.Process(model, identifiers, Request.LogonUserIdentity.Name);
            if (successMessage != null)
            {
                ViewBag.Info = successMessage;
                ViewData.ModelState.Clear();
                return View(new CashReceiptModel());
            }
            else
            {
                ViewBag.Error = "There was an error during processing.  Please reference Process Log ID #" + Session.PLR.ProcessLogId;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AjaxLookupBorrowerName(string id)
        {
            var name = Session.DA.GetBorrowerName(id ?? "0000000000");
            return Json(name ?? "");
        }

        public ActionResult NoAccess()
        {
            return View();
        }
    }
}