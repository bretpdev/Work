using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MyCornerstoneLoan.Models
{
	public static class Configuration
	{
		public static string ContactEmail
		{
			get
			{
				return (string)ConfigurationManager.AppSettings["ContactEmail"];
			}
		}

		public static string WebinarEmail
		{
			get
			{
				return (string)ConfigurationManager.AppSettings["WebinarEmail"];
			}
		}
	}
}