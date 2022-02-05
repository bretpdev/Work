using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using System.Threading;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EMAILBATCH
{
    public class EMAILBATCH
    {
        private readonly string CampaignDirectory;
        private int ReturnValue { get; set; }
        public EMAILBATCH()
        {
            CampaignDirectory = EnterpriseFileSystem.GetPath("EmailCampaigns");
        }

        public int Process()
        {
            var emails = Program.DA.GetAll();
            Parallel.ForEach<EmailProcessingData>(emails, new ParallelOptions() { MaxDegreeOfParallelism =1 }, record =>
            {
                Console.WriteLine("About to process EmailProcessingId:{0}", record.EmailProcessingId);
                ProcessRecord(record);
            });

            return ReturnValue;
        }

        private string Replace(string body, string field, string value)
        {
            return body.Replace("[[[" + field + "]]]", value);
        }

        private HashSet<string> batchEmailsSent = new HashSet<string>();
        private void ProcessRecord(EmailProcessingData record)
        {
            string mergeFile = Path.Combine(CampaignDirectory, record.HTMLFile);
            string mergeText = File.ReadAllText(mergeFile);
            if (!ValidateEmail(mergeFile, mergeText, record.EmailProcessingId))
                return;

            MergedEmailData data = GetEmailData(record, mergeText);

            if (!ValidateBorrowersEmail(record, data))
                return;

            SendEmail(record, data);
            AddComment(record, data);
        }

        private void AddComment(EmailProcessingData record, MergedEmailData data)
        {
            //do activity comments if needed
            if (record.ArcNeeded && !record.ArcAddProcessingId.HasValue)
            {
                var arc = new ArcData(DataAccessHelper.CurrentRegion);
                arc.Arc = record.Arc;
                arc.Comment = record.Comment;
                arc.ScriptId = Program.ScriptId;
                arc.AccountNumber = data.AccountNumber;
                arc.ArcTypeSelected = ArcData.ArcType.Atd22AllLoans;

                if (record.ActivityContact != null) //onelink
                {
                    string ssn = Program.DA.GetSSNFromAccountNumber(record.AccountNumber);
                    arc.ArcTypeSelected = ArcData.ArcType.OneLINK;
                    arc.ActivityContact = record.ActivityContact;
                    arc.ActivityType = record.ActivityType;
                    arc.AccountNumber = ssn.IsNullOrEmpty() ? record.AccountNumber : ssn;
                }

                var result = arc.AddArc();
                if (!result.ArcAdded)
                {
                    string message = string.Format("Unable to add ARC:{0}; for EmailProcessingId:{1};", record.Arc, record.AccountNumber);
                    var id = Program.PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, result.Ex);
                    Program.DA.AddProcessNotificationId(record.EmailProcessingId, id);
                }
                else
                    Program.DA.UpdateArcAddedAt(record.EmailProcessingId, result.ArcAddProcessingId);
            }
        }

        private void SendEmail(EmailProcessingData record, MergedEmailData data)
        {
            try
            {
                if (!record.EmailSentAt.HasValue)
                {
                    //Before sending the first real e-mail, send a sample copy to Systems Support.
                    if (!batchEmailsSent.Contains(record.HTMLFile))
                    {
                        batchEmailsSent.Add(record.HTMLFile);
                        string subject = "[Batch E-mail: New campaign starting] " + record.SubjectLine;
                        EmailHelper.SendMailBatch(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", record.FromAddress, subject, data.Body, "", "", "", EmailHelper.EmailImportance.Normal, true);
                    }

                    EmailHelper.SendMailBatch(DataAccessHelper.TestMode, data.Recipient, record.FromAddress, record.SubjectLine, data.Body, "", "", "", EmailHelper.EmailImportance.Normal, true);
                    Program.DA.UpdateEmailProcessedAt(record.EmailProcessingId);
                }
            }
            catch (Exception ex)
            {
                var id = Program.PLR.AddNotification(string.Format("Unable to send email for EmailProcessingId:{0}", record.EmailProcessingId), NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Program.DA.AddProcessNotificationId(record.EmailProcessingId, id);
            }
        }

        private bool ValidateBorrowersEmail(EmailProcessingData record, MergedEmailData data)
        {
            //Check to see if the email is valid
            if (!System.Text.RegularExpressions.Regex.IsMatch(record.Recipient, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                string message = string.Format("The email {0} given for account number {1} is invalid.  EmailProcessingId:{2}", record.Recipient, record.AccountNumber, record.EmailProcessingId);
                var id = Program.PLR.AddNotification(message, NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
                Program.DA.AddProcessNotificationId(record.EmailProcessingId, id);
                ReturnValue = 1;
                return false;
            }

            return true;
        }

        private MergedEmailData GetEmailData(EmailProcessingData record, string mergeText)
        {
            MergedEmailData data = new MergedEmailData() { AccountNumber = record.AccountNumber, Recipient = record.Recipient };
            data.Body = mergeText;
            data.Body = Replace(data.Body, "RECIPIENT", data.Recipient);
            data.Body = Replace(data.Body, "ACCOUNTNUMBER", data.AccountNumber);
            data.Body = Replace(data.Body, "NAME", record.Name);
            return data;
        }

        private bool ValidateEmail(string mergeFile, string mergeText, int emailProcessingId)
        {
            if (!mergeText.ToLower().Contains("<html") || !mergeText.ToLower().Contains("</html>"))
            {
                var id = Program.PLR.AddNotification("Couldn't find an opening and closing <html> tag in document " + mergeFile, NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
                Program.DA.AddProcessNotificationId(emailProcessingId, id);
                ReturnValue = 1;
                return false;
            }

            return true;
        }
    }
}
