using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SSCHRPTFED
{
	public class SchoolInfo
	{
		public string SchoolName { get; set; }
		public string SchoolCode { get; set; }
		public string BranchCode { get; set; }
		public string ContactNameSchool { get; set; }
		public string EmailAddressSchool { get; set; }
		public string ContactName3rdParty { get; set; }
		public string EmailAddress3rdParty { get; set; }
		public string Recipient { get; set; }

        public static string GenerateErrorNotificationString(SchoolInfo info)
        {
            string message = "There was an error sending the email for the follow school: ";
            foreach (PropertyInfo pi in info.GetType().GetProperties())
            {
                if (pi.GetValue(info, null) != null)
                    message += pi.Name + ": " + pi.GetValue(info, null);
            }

            return message;
        }
	}
}
