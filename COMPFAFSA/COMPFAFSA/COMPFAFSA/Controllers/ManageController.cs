using COMPFAFSA.DataAccess;
using COMPFAFSA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common;

namespace COMPFAFSA.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.NavHighlight = "UserManage";

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            //var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = true,
            };
            return View(model);
        }

        public ActionResult ManageUsers()
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] == true)
            {
                var model = DataAccess.GetExistingUsers();

                return View(model);
            }
            else
            {
                return View("NoAccess");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] == true)
            {
                return View(new CreateUser()); // Passing dummy object for the purpose of creating the form
            }
            else
            {
                return View("NoAccess");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateUser model)
        {
            if ((bool?)Session["Admin"] == true)
            {
                if (ModelState.IsValid)
                {
                    var result = DataAccess.Register(model.EmailAddress, StringToSecureString(model.Password), model.FullName, model.Admin);
                    if (result == UsersDataAccess.RegistrationResult.Success)
                    {
                        return View("CreateSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Account already exists for email address.");
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View("NoAccess");
            }

        }

        [HttpGet]
        public ActionResult DeleteUser(int id, string fullName, string emailAddress)
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            return View(new DeleteUser() { UserId = id, FullName = fullName, EmailAddress = emailAddress });
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserPost(int id)
        {
            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            var success = DataAccess.DeleteUser(id, User.Identity.Name);
            if (success)
            {
                return View("DeleteSuccess");
            }
            else
            {
                return View("DeleteFailure");
            }
        }

        [HttpGet]
        public ActionResult UnlockUser(int id, string fullName, string emailAddress)
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            return View(new UnlockUser() { UserId = id, FullName = fullName, EmailAddress = emailAddress });
        }

        [HttpPost]
        [ActionName("UnlockUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UnlockUserPost(int id)
        {
            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            var success = DataAccess.UnlockUser(id);
            if (success)
            {
                return View("UnlockSuccess");
            }
            else
            {
                return View("UnlockFailure");
            }

        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            ViewBag.NavHighlight = "UserManage";

            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dataAccess = DataAccess;
            var result = dataAccess.ChangePassword(HttpContext.GetOwinContext().Authentication.User.Identity.Name/*Email*/, StringToSecureString(model.NewPassword), StringToSecureString(model.OldPassword), StringToSecureString(model.ConfirmPassword));
            if (result == UsersDataAccess.ChangePasswordResult.Success)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            else
            {
                List<string> errors = new List<string>();
                if (result == UsersDataAccess.ChangePasswordResult.OldPasswordDoesNotMatch)
                {
                    errors.Add("Old password does not match.");
                }
                else if (result == UsersDataAccess.ChangePasswordResult.ConfirmPasswordDoesNotMatch)
                {
                    errors.Add("Confirm password does not match new password.");
                }
                else
                {
                    errors.Add("Error updating password.");
                }

                AddErrors(errors);
                return View(model);
            }
            //var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            //if (result.Succeeded)
            //{
            //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            //    if (user != null)
            //    {
            //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            //    }
            //    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            //}
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ResetPassword(int id)
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            return View(new ResetPasswordViewModel() { UserId = id, NewPassword = "", ConfirmPassword = "" });
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dataAccess = DataAccess;
            var result = dataAccess.ResetPassword(model.UserId, StringToSecureString(model.NewPassword), StringToSecureString(model.ConfirmPassword));
            if (result == UsersDataAccess.ChangePasswordResult.Success)
            {
                return RedirectToAction("ManageUsers", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            else
            {
                List<string> errors = new List<string>();
                if (result == UsersDataAccess.ChangePasswordResult.ConfirmPasswordDoesNotMatch)
                {
                    errors.Add("Confirm password does not match new password.");
                }
                else
                {
                    errors.Add("Error updating password.");
                }

                AddErrors(errors);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult ManageUserSchools(int id)
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            var userName = DataAccess.GetUserName(id);
            var userSchools = DataAccess.GetUserSchools(id);
            ViewBag.UserId = id;
            ViewBag.UserName = userName;

            return View(userSchools);
        }

        [HttpGet]
        public ActionResult DeleteUserSchool(int userId, int schoolId, string school)
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            return View(new UserSchool() { UserId = userId, SchoolId = schoolId, School = school });
        }

        [HttpPost]
        [ActionName("DeleteUserSchool")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserSchoolPost(int userId, int schoolId)
        {
            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            var success = DataAccess.DeleteUserSchool(userId, schoolId);
            if (success)
            {
                return RedirectToAction("DeleteUserSchoolSuccess", new { userId = userId });
            }
            else
            {
                return RedirectToAction("DeleteUserSchoolFailure", new { userId = userId });
            }
        }

        public ActionResult DeleteUserSchoolSuccess(int userId)
        {
            return View(new User { UserId = userId });
        }

        public ActionResult DeleteUserSchoolFailure(int userId)
        {
            return View(new User { UserId = userId });
        }

        [HttpGet]
        public ActionResult AddUserSchool(int userId)
        {
            ViewBag.NavHighlight = "Admin";

            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            var userName = DataAccess.GetUserName(userId);
            var schools = DataAccess.GetSchools();
            var userSchools = DataAccess.GetUserSchools(userId);
            if (schools != null && schools.Count() > 0 && userSchools != null && userSchools.Count() > 0)
                schools.RemoveAll(s => userSchools.Select(p => p.School).ToList().Contains(s));

            Session["manageschools"] = schools;

            ViewBag.Schools = schools.Select(p => new SelectListItem() { Text = string.Copy(p), Value = string.Copy(p) });
            ViewBag.UserName = userName;
            

            return View(new UserSchool() { UserId = userId });
        }

        [HttpPost]
        [ActionName("AddUserSchool")]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserSchoolPost(int userId, string school)
        {
            if ((bool?)Session["Admin"] != true)
            {
                return View("NoAccess");
            }

            var matchingSchool = ((List<string>)Session["manageschools"]).FirstOrDefault(p => p == school);
            if (matchingSchool.IsNullOrEmpty())
            {
                return View("AddUserSchoolFailure");
            }
            school = matchingSchool;

            var success = DataAccess.AddUserSchool(userId, school);
            if (success)
            {
                return RedirectToAction("AddUserSchoolSuccess", new { userId = userId });
            }
            else
            {
                return RedirectToAction("AddUserSchoolFailure", new { userId = userId });
            }
        }

        public ActionResult AddUserSchoolSuccess(int userId)
        {
            return View(new User { UserId = userId });
        }

        public ActionResult AddUserSchoolFailure(int userId)
        {
            return View(new User { UserId = userId });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            if (disposing && _dataAccess != null)
            {
                _dataAccess.Dispose();
                _dataAccess = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        #endregion
    }
}