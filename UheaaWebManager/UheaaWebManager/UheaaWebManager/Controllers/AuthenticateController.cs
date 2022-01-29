using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uheaa.Common.DataAccess;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace UheaaWebManager.Controllers
{
    public class AuthenticateController : Controller
    {
        public ActionResult Index()
        {
            string username = HttpContext.User.Identity.Name.Replace("UHEAA\\", "");
            var result = DataAccessHelper.ExecuteSingle<Guid?>("webapi.GetOrGenerateUserToken", DB.UheaaWebManagement, SqlParams.Single("AssociatedWindowsUsername", username));
            return Content(result?.ToString());
        }
    }
}