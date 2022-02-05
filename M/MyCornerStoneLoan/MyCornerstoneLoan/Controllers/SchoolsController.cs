using MyCornerstoneLoan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCornerstoneLoan.Controllers
{
	public class SchoolsController : Controller
	{
		public ActionResult Index() { return View(); }
		public ActionResult DelinquencyManagement() { return View(); }
		public ActionResult Plan() { return View(); }
		public ActionResult Communicate() { return View(); }
		public ActionResult Track() { return View(); }
		public ActionResult Publications() { return View(); }
	}
}