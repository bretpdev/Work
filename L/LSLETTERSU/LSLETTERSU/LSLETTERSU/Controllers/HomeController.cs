using LSLETTERSU.Models;
using LSLETTERSU.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LSLETTERSU.Controllers
{
    public class HomeController : LslettersuController
    {
        public ActionResult Index()
        {
            ProjectData pData = Session.ProjectData ?? new ProjectData();
            if (!pData.IsLoaded)
            {
                pData.Letters = Session.DA.GetLetterData();
                SetLetterNames(pData);
                pData.LetterTypes = pData.Letters.Select(p => p.LetterType).Distinct().OrderBy(p => p.ToString()).ToList();
                pData.IsLoaded = true;
            }
            Session.ProjectData = pData;
            return View("~/Views/Home/Index.cshtml", pData);
        }

        [HttpPost]
        public ActionResult Index(ProjectData formdata)
        {
            Session.PLR = Session.PLR ?? new ProcessLogRun("LSLETTERSU", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false, false, ActiveDirectoryUsers.UserName);
            InputData data = new InputData();
            data = LoadData(formdata);
            ErrorMessage errors = new ErrorMessage();
            if (data.AccountNumber.IsPopulated())
            {
                LoanServicingLetters letters = new LoanServicingLetters(data, Session.DA, Session.PLR, Session.ScriptId);
                errors = letters.Process();
            }
            else if (data.EX != null)
            {
                string message = $"There was an error processing account {formdata.AccountIdentifier}.";
                Session.PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, data.EX);
                errors.Message = $"{message}  Please contact System Support and reference Process Log ID: {Session.PLR.ProcessLogId}";
            }
            else
                errors.Message = data.ErrorMessage;
            if (errors.Message.IsPopulated())
                return View("Error", errors);
            return View("~/Views/Success/Success.cshtml", data);
        }

        [HttpPost]
        public ActionResult GetLetterOptions(string type)
        {
            ProjectData pData = Session.ProjectData;
            if (pData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var results = pData.Letters.Where(p => p.LetterType == type).Select(p => p.LetterOptions).Distinct().OrderBy(p => p.ToString()).ToList();
            pData.LetterOptions = results;
            Session.ProjectData = pData;
            results.Insert(0, "");
            return Json(results);
        }

        [HttpPost]
        public ActionResult GetManualLetters(string type, string option)
        {
            ProjectData pData = Session.ProjectData;

            if (pData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var results = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.AdditionalReason).Select(p => p.LetterName).Distinct().OrderBy(p => p.ToString()).ToList();
            pData.ManualLetters = results;
            Session.ProjectData = pData;
            results.Insert(0, "");
            return Json(results);
        }

        [HttpPost]
        public ActionResult GetLetterChoices1(string type, string option)
        {
            ProjectData pData = Session.ProjectData;

            if (pData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var results = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option).Select(p => p.LetterChoices).Distinct().OrderBy(p => p.ToString()).ToList();
            pData.LetterChoices1 = results;
            Session.ProjectData = pData;
            results.Insert(0, "");
            return Json(results);
        }

        [HttpPost]
        public ActionResult GetLetterChoices2(string type, string option, string choice1)
        {
            ProjectData pData = Session.ProjectData;

            if (pData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var results = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option).Select(p => p.LetterChoices).Distinct().OrderBy(p => p.ToString()).ToList();
            results.Remove(results.Where(p => p.StartsWith(choice1)).FirstOrDefault());
            pData.LetterChoices2 = results;
            Session.ProjectData = pData;
            results.Insert(0, "");
            return Json(results);
        }

        [HttpPost]
        public ActionResult GetLetterChoices3(string type, string option, string choice1, string choice2)
        {
            ProjectData pData = Session.ProjectData;

            if (pData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var results = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option).Select(p => p.LetterChoices).Distinct().OrderBy(p => p.ToString()).ToList();
            results.Remove(results.Where(p => p.StartsWith(choice1)).FirstOrDefault());
            results.Remove(results.Where(p => p.StartsWith(choice2)).FirstOrDefault());
            pData.LetterChoices3 = results;
            Session.ProjectData = pData;
            results.Insert(0, "");
            return Json(results);
        }

        [HttpPost]
        public ActionResult GetLetterChoices4(string type, string option, string choice1, string choice2, string choice3)
        {
            ProjectData pData = Session.ProjectData;

            if (pData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var results = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option).Select(p => p.LetterChoices).Distinct().OrderBy(p => p.ToString()).ToList();
            results.Remove(results.Where(p => p.StartsWith(choice1)).FirstOrDefault());
            results.Remove(results.Where(p => p.StartsWith(choice2)).FirstOrDefault());
            results.Remove(results.Where(p => p.StartsWith(choice3)).FirstOrDefault());
            pData.LetterChoices4 = results;
            Session.ProjectData = pData;
            results.Insert(0, "");
            return Json(results);
        }

        [HttpPost]
        public ActionResult GetLetterChoices5(string type, string option, string choice1, string choice2, string choice3, string choice4)
        {
            ProjectData pData = Session.ProjectData;

            if (pData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var results = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option).Select(p => p.LetterChoices).Distinct().OrderBy(p => p.ToString()).ToList();
            results.Remove(results.Where(p => p.StartsWith(choice1)).FirstOrDefault());
            results.Remove(results.Where(p => p.StartsWith(choice2)).FirstOrDefault());
            results.Remove(results.Where(p => p.StartsWith(choice3)).FirstOrDefault());
            results.Remove(results.Where(p => p.StartsWith(choice4)).FirstOrDefault());
            pData.LetterChoices5 = results;
            Session.ProjectData = pData;
            results.Insert(0, "");
            return Json(results);
        }

        [HttpPost]
        public ActionResult CheckDischarge(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().DischargeAmount;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckSchoolName(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().SchoolName;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckSchoolClosure(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().SchoolClosureDate;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckLastDate(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().LastDateAttendance;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckDefForb(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().DefForbType;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckDefForbDate(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().DefForbEndDate;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckLoanTerm(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().LoanTermEndDate;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualDenial(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().AdditionalReason;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckSchoolYear(string type, string option, string choice)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (choice.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterChoices.StartsWith(choice)).FirstOrDefault().SchoolYear;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualDischarge(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().DischargeAmount;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualSchoolName(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().SchoolName;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualSchoolClosure(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().SchoolClosureDate;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualLastDate(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().LastDateAttendance;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualDefForb(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().DefForbType;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualDefForbDate(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().DefForbEndDate;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualLoanTerm(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().LoanTermEndDate;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualMDenial(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().AdditionalReason;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult CheckManualSchoolYear(string type, string option, string letterName)
        {
            if (Session.ProjectData == null)
            {
                ErrorMessage errors = new ErrorMessage();
                errors.Message = "Your session has expired. Please start over.";
                return View("Error", errors);
            }

            var result = false;
            if (letterName.IsPopulated())
            {
                ProjectData pData = Session.ProjectData;
                result = pData.Letters.Where(p => p.LetterType == type && p.LetterOptions == option && p.LetterName == letterName).FirstOrDefault().SchoolYear;
            }
            return Json(new { returnValue = result });
        }

        [HttpPost]
        public ActionResult GetBorrowerDemo(string account)
        {
            string accountNumber = account.Length > 0 ? Session.DA.GetAccountNumber(account) : "";
            if (accountNumber.IsPopulated() && accountNumber.Length > 8)
            {
                SystemBorrowerDemographics demos = Session.DA.GetDemos(accountNumber);
                if (demos != null)
                {
                    string suffix = demos.Suffix ?? "";
                    return Json(new { returnValue = $"{demos.FirstName} {demos.LastName} {suffix};{demos.Address1};{demos.City}, {(demos.State.IsPopulated() ? demos.State : demos.Country)}  {(demos.State.IsPopulated() ? (demos.ZipCode.Length == 9 ? demos.ZipCode.Insert(5, "-") : demos.ZipCode) : demos.ZipCode)};{demos.Address2}" });
                }
                else
                    return Json(new { returnValue = "Borrower Not Found" });
            }
            return account.Length > 8 ? Json(new { returnValue = "Borrower Not Found" }) : Json(new { returnValue = "" });
        }


        /// <summary>
        /// Gets the letter name from BSYS, not just the letter ID
        /// </summary>
        public void SetLetterNames(ProjectData pData)
        {
            foreach (LetterData letter in pData.Letters.DistinctBy(p => p.LetterId))
            {
                string letterName = Session.DA.GetLetterName(letter.LetterId);
                foreach (LetterData l in pData.Letters.Where(p => p.LetterId == letter.LetterId))
                    l.LetterName = letterName;
            }
        }

        /// <summary>
        /// Loads all the deferment and forbearance to a list to be added to the DefForb drop down
        /// </summary>
        /// <returns>List of deferment and forbearances available</returns>
        private List<string> GetDefermentForbearanceTypes()
        {
            ProjectData pData = Session.ProjectData;
            List<string> combinedDefForb = new List<string>();

            List<string> deferments = pData.Letters.Where(p => p.LetterType.ToLower().IsIn("deferment")).Select(p => p.LetterOptions).Distinct().ToList();
            List<string> forbearances = pData.Letters.Where(p => p.LetterType.ToLower().IsIn("forbearance")).Select(p => p.LetterOptions).Distinct().ToList();
            foreach (string item in deferments)
                combinedDefForb.Add($"{item} Deferment");
            foreach (string item in forbearances)
                combinedDefForb.Add($"{item} Forbearance");

            combinedDefForb.Insert(0, "");
            return combinedDefForb;
        }

        private InputData LoadData(ProjectData formdata)
        {
            try
            {
                ProjectData pData = Session.ProjectData;
                int id = 0;
                if (formdata.ManualLetters.FirstOrDefault().IsPopulated())
                    id = pData.Letters.Where(p => p.LetterType == formdata.LetterTypes.FirstOrDefault() && p.LetterOptions == formdata.LetterOptions.FirstOrDefault() && p.LetterName.StartsWith(formdata.ManualLetters.FirstOrDefault())).FirstOrDefault().LoanServicingLettersId;
                else
                    id = pData.Letters.Where(p => p.LetterType == formdata.LetterTypes.FirstOrDefault() && p.LetterOptions == formdata.LetterOptions.FirstOrDefault() && p.LetterChoices.StartsWith(formdata.LetterChoices1.FirstOrDefault())).FirstOrDefault().LoanServicingLettersId;
                if (formdata.LetterChoices1.FirstOrDefault().IsNullOrEmpty() && formdata.ManualDenialReason.IsNullOrEmpty())
                    return new InputData() { ErrorMessage = "There was an error determining a denial reason. If choosing a manual letter, please provide a manual denial reason." };
                List<string> denialReasons = AddDenialReasons(formdata);
                if (denialReasons.Sum(p => p.Length) > 1216)
                    return new InputData() { ErrorMessage = "The manual denial text is too long to fit on the letter and in the comment section of the ARC when added to the letter choices. Please revise the manual denial text." };
                return new InputData()
                {
                    AccountNumber = formdata.AccountIdentifier?.Trim(),
                    AmountForDischarge = (formdata.DischargeAmount ?? "").ToString()?.Trim(),
                    SchoolName = formdata.SchoolName?.Trim(),
                    LastDateOfAttendance = formdata.LastDate,
                    SchoolClosureDate = formdata.SchoolClosure,
                    DefermentForbearanceType = formdata.DefForb.FirstOrDefault()?.Trim(),
                    DefForbEndDate = formdata.DefForbDate,
                    LoanTermEndDate = formdata.LoanTermEndDate,
                    BeginYear = formdata.BeginYear?.Trim(),
                    EndYear = formdata.EndYear?.Trim(),
                    LoanServicingLettersId = id,
                    LetterType = formdata.LetterTypes.FirstOrDefault()?.Trim(),
                    LetterOption = formdata.LetterOptions.FirstOrDefault()?.Trim(),
                    DenialReasons = denialReasons,
                    Letters = pData.Letters
                };
            }
            catch (Exception ex)
            {
                return new InputData() { EX = ex };
            }
        }

        private List<string> AddDenialReasons(ProjectData formdata)
        {
            List<string> denialReasons = new List<string>();
            if (formdata.LetterChoices1.FirstOrDefault().IsPopulated())
                denialReasons.Add(formdata.LetterChoices1.FirstOrDefault().Replace("- ", "").Replace("\"", "").Trim());
            if (formdata.LetterChoices2.FirstOrDefault().IsPopulated())
                denialReasons.Add(formdata.LetterChoices2.FirstOrDefault().Replace("- ", "").Replace("\"", "").Trim());
            if (formdata.LetterChoices3.FirstOrDefault().IsPopulated())
                denialReasons.Add(formdata.LetterChoices3.FirstOrDefault().Replace("- ", "").Replace("\"", "").Trim());
            if (formdata.LetterChoices4.FirstOrDefault().IsPopulated())
                denialReasons.Add(formdata.LetterChoices4.FirstOrDefault().Replace("- ", "").Replace("\"", "").Trim());
            if (formdata.LetterChoices5.FirstOrDefault().IsPopulated())
                denialReasons.Add(formdata.LetterChoices5.FirstOrDefault().Replace("- ", "").Replace("\"", "").Trim());
            if (formdata.ManualDenialReason.IsPopulated())
                denialReasons.Add(formdata.ManualDenialReason.Trim());
            return denialReasons;
        }

        public ActionResult NoAccess()
        {
            return View();
        }
    }
}