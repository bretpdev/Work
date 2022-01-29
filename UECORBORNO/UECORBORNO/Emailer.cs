using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UECORBORNO
{
    class Emailer
    {
        private DataAccess DA { get; set; }
        private ProcessLogRun PLR { get; set; }
        public Emailer(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(PLR.ProcessLogId);
        }

        /// <summary>
        /// Gets all the email information and send emails
        /// </summary>
        /// <returns>0 if no errors 1 if errors</returns>
        public int Process()
        {
            List<EmailData> allEmails = DA.GetEmailData();
            if(!allEmails.Any())
                return 0;

            List<EmailData> groupedEmails = allEmails.DistinctBy(p => new { p.AccountNumber, p.EmailAddress, p.EmailSubjectLine }).ToList();
            Parallel.ForEach(groupedEmails, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, emailGroup =>
                {
                    //GET ALL OF THE DOCUMENTDETAILIDS FOR THE GROUP
                    emailGroup.DocumentDetailIds = allEmails.Where(q => q.AccountNumber == emailGroup.AccountNumber && q.EmailAddress == emailGroup.EmailAddress
                        && q.EmailSubjectLine == emailGroup.EmailSubjectLine).Select(p => p.EmailId).ToList();
                });

            string htmlFile = Path.Combine(EnterpriseFileSystem.GetPath("EmailCampaigns"), "NEBEMLUH.html");
            string emailBody = string.Empty;
            using (StreamReader sr = new StreamReader(htmlFile))
            {
                emailBody = sr.ReadToEnd();
            }
            Parallel.ForEach(groupedEmails, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, email =>
                {
                    //Make sure we do not send any test emails out
                    //if (DataAccessHelper.TestMode)
                    //    email.EmailAddress = string.Format("{0}@utahsbr.edu", Environment.UserName);

                    Console.WriteLine("Sending Email to {0}", email.EmailAddress);

                    var result = Repeater.TryRepeatedly(() => EmailHelper.SendMailBatch(DataAccessHelper.TestMode, email.EmailAddress, "noreply@uheaa.org", email.EmailSubjectLine, 
                        emailBody, "", "", EmailHelper.EmailImportance.Normal, true));
                    if (!result.Successful)
                    {
                        foreach (var ex in result.CaughtExceptions)
                        {
                            PLR.AddNotification(string.Format("Error sending email to borrower:{0} at email address:{1}.", email.AccountNumber, email.EmailAddress),
                                NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                        }
                    }
                    else
                    {
                        email.UpdateEmailedIndicator(DA);
                    }
                });

            return 0;
        }
    }
}
