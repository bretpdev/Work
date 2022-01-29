using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCornerstoneLoan.Controllers
{
	public class AccountController : Controller
	{
		public ActionResult Index() { return View(); }
		public ActionResult BorrowerBenefits() { return View(); }
		public ActionResult Delinquency() { return View(); }
		public ActionResult DueDate() { return View(); }
		public ActionResult IDRInfo() { return View(); }
		public ActionResult LoanVerification() { return View(); }
		public ActionResult Consolidation() { return View(); }
		public ActionResult PostponePayments() { return View(); }
		public ActionResult ThirdPartyAuth() { return View(); }
        public ActionResult AlternativeFormat() { return View(); }
        public ActionResult LoanStatement() { return View(); }
	}
}