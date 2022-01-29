using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using COMPFAFSA.Models;
using System.Collections.Generic;
using COMPFAFSA.DataAccess;
using Uheaa.Common;
using System.Security.Cryptography;
using static COMPFAFSA.DataAccess.UsersDataAccess;

namespace COMPFAFSA.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;

        public AccountController()
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
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {

            //redirect to secure controller if user is already logged in
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Secure");
            }

            ViewBag.NavHighlight = "Login";

            return View(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            using (model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                UsersDataAccess dataAccess = DataAccess;
                var result = dataAccess.Authenticate(model.Email, StringToSecureString(model.Password));
                if (result == UsersDataAccess.LoginResult.Success || result == UsersDataAccess.LoginResult.SuccessNeedSetPassword)
                {
                    //IF AUTHENTICATED
                    var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaims(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, model.Email),
                    new Claim(ClaimTypes.Name, model.Email)
                });
                    HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                    var isAdmin = DataAccess.GetUserIsAdmin(model.Email);
                    //Set the status of admin in the session so that we can set up admin user controls based off of this
                    Session["Admin"] = isAdmin;

                    if (result == LoginResult.SuccessNeedSetPassword)
                    {
                        //Have the user set their password if they have not
                        return RedirectToAction("ChangePassword", "Manage");
                    }
                    else if(isAdmin)
                    {
                        return RedirectToAction("ManageUsers", "Manage");
                    }    
                    else
                    {
                        return RedirectToAction("Index", "Secure");
                    }
                }
                else if (result == UsersDataAccess.LoginResult.Lockout)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
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

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                int? userId = DataAccess.GetUserId(model.Email);

                if(!userId.HasValue || userId.Value < 1)
                {
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = DataAccess.CreateResetPassword(model.Email);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = userId.Value, code = code }, protocol: Request.Url.Scheme);
                //If you want to use Dev/Live according to how we actually do it use the following. We're bypassing this for testing
                //(DataAccessHelper.Mode)System.Web.HttpContext.Current.Application["Mode"] == DataAccessHelper.Mode.Dev
                bool testMode = false;
#if DEBUG
                testMode = true;
#endif
                EmailHelper.SendMail(testMode, model.Email, "noreply@uheaa.org", "Password Reset Link For Complete Financial FAFSA", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>. This code expires in 15 minutes.", "", EmailHelper.EmailImportance.Normal, true);
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(UserResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int? userId = DataAccess.GetUserId(model.Email);
            if (!userId.HasValue || userId.Value < 1)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            //Lockout accounts as if the attempt to reset the password were an authentication attempt
            bool accountLockedOut = DataAccess.GetAccountLockedOut(model.Email);
            if(accountLockedOut)
            {
                return View("Lockout");
            }

            var resetPasswordResponse = DataAccess.GetUserHashedPasswordReset(model.Email);
            if(resetPasswordResponse != null && resetPasswordResponse.Expired)
            {
                LoginResult loginResult = DataAccess.AuthenticationFailure(model.Email);
                if (loginResult == UsersDataAccess.LoginResult.Lockout)
                {
                    return View("Lockout");
                }
                else
                {
                    return View("LinkExpired");
                }
            }
            if(resetPasswordResponse == null || model.Code != resetPasswordResponse.HashedResetPassword)
            {
                //Fail authentication
                LoginResult loginResult = DataAccess.AuthenticationFailure(model.Email);
                if (loginResult == UsersDataAccess.LoginResult.Lockout)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update password");
                    return View(model);
                }
            }

            var result = DataAccess.ResetPassword(userId.Value, StringToSecureString(model.Password), StringToSecureString(model.ConfirmPassword));
            if (result == UsersDataAccess.ChangePasswordResult.Success)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            else if(result == UsersDataAccess.ChangePasswordResult.ConfirmPasswordDoesNotMatch)
            {
                ModelState.AddModelError("", "Confirm password does not match new password");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update password");
            }

            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #region Helpers

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}