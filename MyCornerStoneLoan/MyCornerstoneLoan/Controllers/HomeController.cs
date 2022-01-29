using MyCornerstoneLoan.Models;
using SimpleMvcSitemap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MyCornerstoneLoan.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index() { return View(); }
        public ActionResult About() { return RedirectToAction("AboutUs"); }
		public ActionResult AboutUs() { return View(); }
		public ActionResult EmailSent() { return View(); }
		public ActionResult Calculators() { return View(); }
		public ActionResult FAQ() { return View(); }
		public ActionResult Jobs() { return View(); }
		public ActionResult ContactUs() { return View(); }
		public ActionResult Privacy() { return View(); }
		public ActionResult Terms() { return View(); }
		public ActionResult Forms()
		{
            var formGroups = GetFormGroups();
			return View(formGroups);
		}

        private List<FormGroup> GetFormGroups()
        {
            var formGroups = new List<FormGroup>
            {
                new FormGroup("af", "Account Forms",
                    "Third Party Authorization", "forms/CS Third Party Auth Form.pdf", "", "formthirdparty.htm"
                ),
                new FormGroup("cf", "Consolidation Forms")
                    .Append(new Form(){ Name="Consolidation Application", EnglishSpanishOverride = "<a href='https://studentloans.gov/myDirectLoan/launchConsolidation.action' title='Link to StudentLoans.gov Application' rel='external' target='_blank'>Download</a>"} ),
                new FormGroup("df", "Deferment Forms",
                    "Economic Hardship Deferment", "forms/EconomicHardship2018.pdf", "forms/Spanish/EconomicHardship2018ES.pdf", "defermenteconomichardship.htm",
                    "In-School Deferment", "forms/1845-0011_09-30-18_IS-DR_08-02-16.pdf", "forms/Spanish/1845-0011_09-30-18_IS-DR_08-02-16_ES.pdf", "defermentinschool.htm",
                    "Military Deferment", "forms/MIL_20170206.pdf", "", "defermentmilitary.htm",
                    "Parent PLUS Deferment", "forms/1845-0011_09-30-18_PPB-DR_08-02-16.pdf", "forms/Spanish/1845-0011_09-30-18_PPB-DR_08-02-16_ES.pdf", "defermentplus.htm",
                    "Temporary Total Disability Deferment", "forms/1845-0011_09-30-18_TTD-DR_08-02-16.pdf", "forms/Spanish/1845-0011_09-30-18_TTD-DR_08-02-16_ES.pdf", "defermentttd.htm",
                    "Unemployment Deferment", "forms/1845-0011_09-30-18_UE-DR_08-02-16.pdf", "forms/Spanish/1845-0011_09-30-18_UE-DR_08-02-16_ES.pdf", "defermentunemployment.htm",
                    "Graduate Fellowship Deferment", "forms/1845-0011_GFL_FINAL.pdf", "forms/Spanish/1845-0011_09-30-18_GF-DR_08-02-16_ES.pdf", "graduate_fellowship_deferment.htm",
                    "Rehabilitation Training Deferment", "forms/1845-0011_RHT_FINAL.pdf", "forms/Spanish/1845-0011_09-30-18_RT-DR_08-02-16_ES.pdf", "rehabilitation_training_deferment.htm"  
                ),
                new FormGroup("ff", "Forbearance Forms",
                    "Mandatory Forbearance SERV", "forms/1845-0018_09-30-18_M-FB_08-02-16.pdf", "forms/Spanish/1845-0018_09-30-18_M-FB_08-02-16_ES.pdf", "mandatory_forbearance_request.htm",
                    "General Forbearance Request", "forms/1845-0031_09-30-18_G-FB_08-02-16.pdf", "forms/Spanish/1845-0031_GFB_FINAL (es)_3998 - 13.pdf", "forbearancegeneral.htm",
                    "Student Loan Debt Burden Forbearance Request", "forms/1845-0018_09-30-18_SLDB-FB_08-02-16.pdf", "forms/Spanish/1845-0018_09-30-18_SLDB-FB_08-02-16_ES.pdf", "forbearanceloandebt.htm",
                    "Teacher Loan Forgiveness Forbearance Request", "forms/1845-0059_TLFForb_ExpDate_20200930v2.pdf", "forms/Spanish/forb_TLF_ALLLOANS_spanish.pdf", "forbearancetlf.htm"
                ),
                new FormGroup("fdf", "Forgiveness and Discharge Forms",
                    "False Certification Ability to Benefit Discharge Application", "forms/1845-0058_02-07-19_FC-ATB_10-08-14_v1_IAI Submission.pdf", "forms/Spanish/fcatb_spanish.pdf", "dischargeatb.htm",
                    "False Certification Disqualifying Status Discharge Application", "forms/1845-0058_02-07-19_FC-DS_10-08-14_v1_IAI Submission.pdf", "forms/Spanish/fcdq_spanish.pdf", "dischargeds.htm",
                    "False Certification Unauthorized Signature/Unauthorized Payment Discharge Application", "forms/1845-0058_02-07-19_FC-US-UP_10-08-14_v2_IAI Submission.pdf", "forms/Spanish/usup_spanish.pdf", "dischargeusup.htm",
                    "School Closure Discharge Application", "forms/1845-0058_02-07-19_SC_10-08-14_v1_IAI Submission.pdf", "forms/Spanish/scld_spanish.pdf", "dischargesc.htm",
                    "Teacher Loan Forgiveness Application", "forms/1845-0059_TLFApp_ExpDate_20200930v2.pdf", "forms/Spanish/tlfa_spanish.pdf", "dischargetlf.htm",
                    "Unpaid Refund Discharge Application", "forms/1845-0058_02-07-19_UR_10-08-14_v1_IAI Submission.pdf", "forms/Spanish/ur_spanish.pdf", "dischargeur.htm"
                )
                .Append(new Form(){ Name="Public Service Loan Forgiveness Discharge Application", EnglishSpanishOverride = "<a href='https://studentaid.ed.gov/sa/repay-loans/forgiveness-cancellation/public-service' title='Link to Studentaid.ed.gov to learn more about Public Service Loan Forgiveness.' rel='external' target='_blank'>More Information</a>"} )
                .Append(new Form(){ Name = "Total and Permanent Disability Discharge", EnglishSpanishOverride = "Please visit <a href='http://www.disabilitydischarge.com' rel='external' target='_blank'>www.disabilitydischarge.com</a> for more information about Total and Permanent Disability Discharge. ", ExplanationLink = "dischargetpd.htm"} ),
                new FormGroup("pf", "Payment Forms",
                    "Repayment Plan Selection Form (Standard, Extended and Graduated)", "forms/1845-0014_12-31-16_RPR_10-08-14_v2_IAI Submission.pdf", "", "paymentsplans.htm",
                    "Income Driven Repayment Plan Request Form", "https://studentloans.gov/myDirectLoan/ibrInstructions.action?source=15SPRRPMT", "forms/Spanish/1845-0102_IDR_FINAL COR_WITH 83C CNGS (es) 20160919 new 041217 - 14.pdf", ""
                ),
                new FormGroup("o", "Other Forms",
                    "Reaffirmation Agreement", "forms/1845-0133_REA_FINAL.pdf", "forms/Spanish/1845-0133_06-30-18_SRA_08-02-16_ES.pdf", "reaffirmation.htm",
                    "School Information Authorization", "forms/School Authorization Form_FED.pdf", "", ""
                )
            };
            return formGroups;
        }

#if DEBUG
        /// <summary>
        /// This action is only used in testing for form management.
        /// </summary>
        public ActionResult UnusedForms()
        {
            var exemptions = Directory.GetFiles(Server.MapPath("~/forms/Publications")).Select(o => "forms/publications/" + Path.GetFileName(o).ToLower());
            var root = Server.MapPath("~/");
            var formPath = Server.MapPath("~/forms");
            var explanationPath = Server.MapPath("~/explanations");
            var forms = GetFormGroups().SelectMany(o => o.Forms);
            var remainingForms = AllFilesRelative(formPath, root);
            var remainingDescriptions = AllFilesRelative(explanationPath, root);
            remainingForms.AddRange(remainingDescriptions);
            foreach (var file in remainingForms.ToArray())
            {
                var lowerFile = file.ToLower().Replace("\\", "/");
                bool hasLink = forms.Any(o => (o.EnglishLink ?? "").ToLower() == lowerFile || (o.SpanishLink ?? "").ToLower() == lowerFile || ("explanations/" + (o.ExplanationLink ?? "")).ToLower() == lowerFile);
                if (hasLink || exemptions.Contains(lowerFile))
                    remainingForms.Remove(file);
            }
            if (remainingForms.Any())
                return Content(remainingForms.Count + " forms/explanations with no links:<br />" + string.Join("<br />", remainingForms));
            else
                return Content("All forms are properly linked.");
        }
#endif

        private List<string> AllFilesRelative(string path, string root)
        {
            var result = new List<string>();
            foreach (var file in Directory.GetFiles(path))
                result.Add(file.Substring(root.Length));
            foreach (var directory in Directory.GetDirectories(path))
                result.AddRange(AllFilesRelative(directory, root));
            return result;
        }

        public ActionResult Error()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Sitemap()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(o => o.Namespace == "MyCornerstoneLoan.Controllers"))
            {
                if (typeof(Controller).IsAssignableFrom(type))
                {
                    string controllerName = type.Name.Substring(0, type.Name.Length - "Controller".Length);
                    foreach (var action in type.GetMethods().Where(o => o.ReturnType == typeof(ActionResult) && o.IsPublic))
                    {
                        string actionName = action.Name;
                        nodes.Add(new SitemapNode(Url.Action(actionName, controllerName)));
                    }
                }
            }
            return new SitemapProvider().CreateSitemap(HttpContext, nodes);
        }

        public ActionResult Navigation()
        {
            return View();
        }

        public ActionResult Repaye()
        {
            return View();
        }
	}
}