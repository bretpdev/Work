using LSLETTERSU.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace LSLETTERSU.Controllers
{
    public class ErrorController : LslettersuController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ErrorMessage message)
        {
            Session.ProjectData = new ProjectData();
            ProjectData pData = new ProjectData();
            pData.Letters = Session.DA.GetLetterData();
            SetLetterNames(pData);
            pData.LetterTypes = pData.Letters.Select(p => p.LetterType).Distinct().ToList();
            pData.IsLoaded = true;
            Session.ProjectData = pData;
            return View("~/Views/Home/Index.cshtml", pData);
        }

        public void SetLetterNames(ProjectData pData)
        {
            foreach (LetterData letter in pData.Letters.DistinctBy(p => p.LetterId))
            {
                string letterName = Session.DA.GetLetterName(letter.LetterId);
                foreach (LetterData l in pData.Letters.Where(p => p.LetterId == letter.LetterId))
                    l.LetterName = letterName;
            }
        }
    }
}