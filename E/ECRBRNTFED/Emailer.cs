using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ECRBRNTFED
{
    class Emailer
    {
        private EndOfJobReport Eoj { get; set; }
        private const string Total = "Total number of emails to be sent";
        private const string Sent = "Total number of email sent";
        private const string Error = "Total number of errors";

        public Emailer()
        {
            Eoj = new EndOfJobReport("Ecorr Borrowers Notification-FED", "EOJ_BU35", new List<string>() { Total, Sent, Error });
        }

        /// <summary>
        /// Gets all the email information and send emails
        /// </summary>
        /// <returns>0 if no errors 1 if errors</returns>
        public int Process()
        {
           
            List<EmailData> emails = EmailData.GetEmailData().DistinctBy(p => new { p.AccountNumber, p.EmailAddress, p.EmailSubjectLine}).ToList();
          
            Eoj[Total] = new Count(emails.Count);
            ReaderWriterLockSlim eojLock = new ReaderWriterLockSlim();

            string htmlFile = Path.Combine(EnterpriseFileSystem.GetPath("EcorrNotificationEmail"), "NEBEMLFED.html");
            string emailBody = string.Empty;
            using (StreamReader sr = new StreamReader(htmlFile))
            {
                emailBody = sr.ReadToEnd();
            }
            Parallel.ForEach(emails, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, email =>
                {
                    Repeater.TryRepeatedly(() => EmailHelper.SendMailBatch(DataAccessHelper.TestMode, email.EmailAddress, "noreply@mycornerstoneloan.org", email.EmailSubjectLine, emailBody, "", "", "", EmailHelper.EmailImportance.Normal, true));
                    
                    eojLock.EnterWriteLock();
                    Repeater.TryRepeatedly(() => email.UpdateEmailedIndicator());
                    Eoj[Sent]++;
                    eojLock.ExitWriteLock();

                });


            Eoj.Publish();
            return 0;
        }
    }
}
