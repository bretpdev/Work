using COMPFAFSA.DataAccess;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMPFAFSA.Controllers
{
    [RequireHttps]
    public class SecureController : BaseController
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Secure FAFSA Completion Data - Your Students";
            ViewBag.NavHighlight = "Secure";

            List<Models.SecureStudentsModel> students = DataAccess.GetStudents();
            students = students.OrderBy(p => p.School).ThenBy(p => p.DistrictId ?? int.MaxValue).ToList();
            //ADD A TEST STUDENT
            //students.Add(new Models.SecureStudentsModel() { FirstName = "John", LastName = "Doe", DOB = DateTime.Today, HasProblem = "Yes" });

            return View(students);
        }

        [HttpGet]
        [Authorize]
        public FileContentResult GetCSV()
        {
            List<Models.SecureStudentsModel> students = DataAccess.GetStudents();
            students = students.OrderBy(p => p.School).ThenBy(p => p.DistrictId ?? int.MaxValue).ToList();

            string csv = Models.SecureStudentsModel.ToCSV(students);
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "StudentCompletionReport.csv");
        }

    }
}