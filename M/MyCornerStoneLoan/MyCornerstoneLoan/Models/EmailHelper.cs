using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MyCornerstoneLoan.Models
{
	public static class EmailHelper
    { 
		private static void Send(string fromDisplayName, string to, string cc, string subject, string body)
		{
			SmtpClient client = new SmtpClient("mail.utahsbr.edu") {
				Timeout = 20000
			};
			MailMessage email = new MailMessage {
				Subject = subject,
				Body = body
			};
			email.From = new MailAddress("no-reply@mycornerstoneloan.org", fromDisplayName);
			email.To.Add(to);
			if (!String.IsNullOrEmpty(cc))
				email.CC.Add(cc);
			client.Send(email);
		}
	}
}