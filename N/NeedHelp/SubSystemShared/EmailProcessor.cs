//TODO: This class is not needed in projects that reference Q. Use Common.SendMail() instead.

using System;
using System.Threading;
using OSSMTP;

namespace SubSystemShared
{
	public class EmailProcessor
	{
		public enum EmailImportanceLevel
		{
			High,
			Low,
			Normal
		}

		public static void SendMail(bool testMode, string recipient, string sender, string subject, string body, string cc, string bcc, EmailImportanceLevel importance, bool setTestTextIfInTestMode, bool formatAsHtml)
		{
			SMTPSession Email = new SMTPSession();

			//set server and time out (default = 10 secs, set to 20 secs)
			Email.Server = Properties.Resources.SmtpServer;
			Email.Timeout = 20000;

			//add test text and recipient
			if (setTestTextIfInTestMode && testMode)
			{
				//Old code: mto = string.Format("{0},{1}@utahsbr.edu",mto, Environment.UserName);
				subject = string.Format("{0} -- THIS IS A TEST", subject);
				body = string.Format("THIS IS A TEST{0}{0}{1}", Environment.NewLine, body);
			}

			//create message
			//Old code:
			//if (mFrom.Length == 0)
			//{
			//    mFrom = Environment.UserName + "@utahsbr.edu";
			//}
			Email.MailFrom = sender;
			Email.SendTo = recipient;
			Email.CC = cc;
			Email.BCC = bcc;
			Email.MessageSubject = subject;
			if (formatAsHtml)
			{
				Email.MessageHTML = body;
			}
			else
			{
				Email.MessageText = body;
			}
			switch (importance)
			{
				case EmailImportanceLevel.High:
					Email.Importance = importance_level.ImportanceHigh;
					break;
				case EmailImportanceLevel.Low:
					Email.Importance = importance_level.ImportanceLow;
					break;
				default:
					Email.Importance = importance_level.ImportanceNormal;
					break;
			}

			//wait up to five seconds for the email to successfully be sent
			Email.SendEmail();
			for (int seconds = 0; seconds < 5 && Email.Status != "SMTP connection closed"; seconds++) { Thread.Sleep(1000); }
			if (Email.Status != "SMTP connection closed")
			{
				string message = "Your message was not sent for the following reason:  ";
				message += Email.Status;
				message += "  Please contact Process Automation for assistance.";
				throw new EmailException(message);
			}
		}//SendMail()
	}//class
}//namespace
