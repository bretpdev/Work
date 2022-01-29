using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCornerstoneLoan.Controllers
{
	public class ServiceMembersController : Controller
	{
		public ActionResult Index() { return View(); }
		public ActionResult Benefits() { return View(); }
		public ActionResult Deferment() { return View(); }
		public ActionResult PostDeferment() { return View(); }
		public ActionResult Resources() { return View(); }
		public ActionResult SCRA() { return View(); }
		public ActionResult Tips() { return View(); }
	}
}