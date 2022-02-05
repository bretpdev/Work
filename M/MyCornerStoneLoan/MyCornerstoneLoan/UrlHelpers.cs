using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCornerstoneLoan
{
	public static class UrlHelpers
	{
		public static string  AccountAccessUrl
		{
			get
			{
				if (HttpContext.Current.Request.Browser.IsMobileDevice)
					return "https://m.mycornerstoneloan.org/authentication/signIn.html";
				return "https://myaccount.mycornerstoneloan.org/accountAccess/index.cfm";
			}
		}

		public static string CreateAccountLink
		{
			get
			{
				return "https://myaccount.mycornerstoneloan.org/authentication/personalInfo.html";
			}
		}
	}
}