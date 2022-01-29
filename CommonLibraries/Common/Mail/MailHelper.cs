using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Uheaa.Common.Mail
{
    public static class MailHelper
    {
        public static string UserEmail
        {
            get
            {
                return Environment.UserName + "@utahsbr.edu";
            }
        }
        public const string Host = "mail.utahsbr.edu";
        public static void SendMail(string from, string to, string subject, string body, string cc, string bcc, string[] attachmentPaths)
        {
            SendMail(from, new string[] { to }, subject, body, cc, bcc, attachmentPaths);
        }
        public static void SendMail(string from, string[] to, string subject, string body)
        {
            SendMail(from, to, subject, body, null, null, new string[] { });
        }
        public static void SendMail(string from, string[] to, string subject, string body, string cc, string bcc, string[] attachmentPaths)
        {
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient(Host);

            message.From = new MailAddress(from);
            foreach (string address in to)
                message.To.Add(new MailAddress(address));
            if (!string.IsNullOrEmpty(cc))
                message.CC.Add(cc);
            if (!string.IsNullOrEmpty(bcc))
                message.Bcc.Add(bcc);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            foreach (string path in attachmentPaths)
                message.Attachments.Add(new Attachment(path));

            client.Send(message);
        }
    }
}
