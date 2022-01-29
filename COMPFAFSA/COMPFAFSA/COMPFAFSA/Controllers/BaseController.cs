using COMPFAFSA.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace COMPFAFSA.Controllers
{
    public abstract class BaseController : Controller
    {
        protected UsersDataAccess _dataAccess;

        public UsersDataAccess DataAccess
        {
            get
            {
                return _dataAccess ?? HttpContext.GetOwinContext().Get<UsersDataAccess>();
            }
            private set
            {
                _dataAccess = value;
            }
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected SecureString StringToSecureString(string str)
        {
            SecureString secure = new SecureString();
            foreach(char c in str)
            {
                secure.AppendChar(c);
            }
            return secure;
        }

        // Used for XSRF protection when adding external logins
        protected const string XsrfKey = "XsrfId";

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        protected void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _dataAccess != null)
            {
                _dataAccess.Dispose();
                _dataAccess = null;
            }

            base.Dispose(disposing);
        }
    }
}