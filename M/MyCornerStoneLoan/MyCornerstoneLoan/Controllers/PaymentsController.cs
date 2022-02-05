using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCornerstoneLoan.Controllers
{
    public class PaymentsController : Controller
    {
        public ActionResult Index() { return View(); }
        public ActionResult AutoPay() { return View(); }
        public ActionResult Bills() { return View(); }
        public ActionResult PayOff() { return View(); }
        public ActionResult RepaymentPlans() { return View(); }
        public ActionResult PayAsYouEarn() { return View(); }
        public ActionResult HowArePaymentsApplied() { return View(); }

    }
}