using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Uheaa.Common
{
    public static class EmailHelper
    {
        public enum EmailImportance
        {
            High,
            Normal,
            Low
        }

        /// <summary>
        /// Sends an e-mail message using SMTP
        /// </summary>
        /// <param name="to">Email address of who to send the email to.  Comma delimit multiple entries.</param>
        /// <param name="from">Who the email is being sent from.</param>
        /// <param name="subject">The email subject line.</param>
        /// <param name="body">The email body text.</param>
        /// <param name="cc">Recipients to carbon copy.  Comma delimit multiple entries.</param>
        /// <param name="bcc">Recipients to blind copy.  Comma delimit multiple entries.</param>
        /// <param name="attachmentFilePath">Attachments for email.  Comma delimit multiple entries.  Send blank string if none are desired.</param>
        /// <param name="importance">The importance level assigned to the email.</param>
        /// <param name="sendAsHtml">Indicator as to whether the email should be sent as HTML or text.</param>
        public static void SendMail(bool testMode, string to, string from, string subject, string body, string cc,  EmailImportance importance, bool sendAsHtml)
        {
            //UNDONE if you need to attach a file to an email please talk to Jarom
            SmtpClient client = new SmtpClient("mail.utahsbr.edu")
            {
                Timeout = 20000
            };
            MailMessage message = new MailMessage();
            if (importance == EmailImportance.Low)
                message.Priority = MailPriority.Low;
            else if (importance == EmailImportance.High)
                message.Priority = MailPriority.High;
            else if (importance == EmailImportance.Normal)
                message.Priority = MailPriority.Normal;
            if (testMode)
            {

                subject += "-- THIS IS A TEST --";
                body = string.Format("Normally this email would be sent to the following:{0}To:{1}{0}CC:{2}{0}", sendAsHtml ? "<br />" : "\r\n", to, cc) + body;
                body += "\r\n\r\nTHIS IS A TEST \r\n";
                to = Environment.UserName + "@utahsbr.edu";
                cc = "";
            }
            from = string.IsNullOrEmpty(from) ? Environment.UserName + "@utahsbr.edu" : from;
            message.From = new MailAddress(from);

            foreach (string email in to.Split(';').Where(p => !p.IsNullOrEmpty()))
                message.To.Add(email);

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = sendAsHtml;
            foreach (string email in cc.Split(';').Where(p => !p.IsNullOrEmpty()))
                message.CC.Add(email);

            client.Send(message);
        }

        /// <summary>
        /// Sends an e-mail message using SMTP
        /// </summary>
        /// <param name="to">Email address of who to send the email to.  Comma delimit multiple entries.</param>
        /// <param name="from">Who the email is being sent from.</param>
        /// <param name="subject">The email subject line.</param>
        /// <param name="body">The email body text.</param>
        /// <param name="cc">Recipients to carbon copy.  Comma delimit multiple entries.</param>
        /// <param name="bcc">Recipients to blind copy.  Comma delimit multiple entries.</param>
        /// <param name="attachmentFilePath">Attachments for email.  Comma delimit multiple entries.  Send blank string if none are desired.</param>
        /// <param name="importance">The importance level assigned to the email.</param>
        /// <param name="sendAsHtml">Indicator as to whether the email should be sent as HTML or text.</param>
        public static void SendMailBatch(bool testMode, string to, string from, string subject, string body, string cc, string bcc,  EmailImportance importance, bool sendAsHtml)
        {
            //UNDONE if you need to attach a file to an email please talk to Jarom
            SmtpClient client = new SmtpClient("smtp2.utahsbr.edu")
            {
                Timeout = 20000
            };
            MailMessage message = new MailMessage();
            if (importance == EmailImportance.Low)
                message.Priority = MailPriority.Low;
            else if (importance == EmailImportance.High)
                message.Priority = MailPriority.High;
            else if (importance == EmailImportance.Normal)
                message.Priority = MailPriority.Normal;
            if (testMode)
            {
                subject += "-- THIS IS A TEST --";
                body = string.Format("Normally this email would be sent to the following:{0}To:{1}{0}CC:{2}{0}BCC:{3}{0}", sendAsHtml ? "<br />" : "\r\n",to, cc, bcc) + body;
                body += "THIS IS A TEST \r\n";
                to = Environment.UserName + "@utahsbr.edu";
                cc = "";
                bcc = "";
            }
            from = string.IsNullOrEmpty(from) ? Environment.UserName + "@utahsbr.edu" : from;
            message.From = new MailAddress(from);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = sendAsHtml;
            foreach (string email in cc.Split(';').Where(p => !p.IsNullOrEmpty()))
                message.CC.Add(email);
            foreach (String email in bcc.Split(';').Where(p => !p.IsNullOrEmpty()))
                message.Bcc.Add(email);


            client.Send(message);
        }//end SendMail
    }
}
