using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.FileLoad;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace EMAILBTCF
{
    public class EmailBatchScript
    {
        ProcessLogRun PLR;
        DataAccess DA;
        const string ScriptId = "EMAILBTCF";
        public void Main(int? filterCampaignId)
        {
            this.PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            this.DA = new DataAccess(new LogDataAccess(DataAccessHelper.CurrentMode, PLR.ProcessLogId, false, false));

            List<EmailCampaign> campaigns = this.DA.GetAllCampaigns();
            if (filterCampaignId.HasValue)
                campaigns = campaigns.Where(o => o.EmailCampaignId == filterCampaignId.Value).ToList();
            WorkCampaigns(campaigns);
            this.PLR.LogEnd();
        }

        public void WorkCampaigns(List<EmailCampaign> campaigns)
        {
            foreach (EmailCampaign campaign in campaigns)
            {
                if (CheckCampaignFile(campaign)) //Template file exists?
                    LoadNewEmailRecords(campaign);

                int completedCDEmails = HandleCampaignDataEmails(campaign);
                Console.WriteLine("Processed {0} campaign data email records for campaign {1}", completedCDEmails, campaign.EmailCampaignId);
            }
        }

        public void LoadNewEmailRecords(EmailCampaign campaign)
        {
            string file = FileSystemHelper.DeleteOldFilesReturnMostCurrent(EnterpriseFileSystem.FtpFolder, campaign.SasFile);
            if (CheckFile(file, campaign.SasFile))
            {
                if (campaign.WorkLastLoaded < DateTime.Now.Date || !campaign.WorkLastLoaded.HasValue)
                {
                    List<CampaignData> pendingBorrowers = ReadFile(file);
                    if (pendingBorrowers != null)
                    {
                        DA.UploadCampaignData(campaign.EmailCampaignId, pendingBorrowers.Select(o => new CampaignDataForUpload(o)).ToList());
                        File.Delete(file);
                    }
                }
            }
            else if (campaign.SasFile.IsNullOrEmpty()) //Empty source files dont need an "already loaded" message
                return;
            else
                PLR.AddNotification(string.Format("Already loaded file {0} today, skipping load portion until tomorrow.", campaign.SasFile), NotificationType.EndOfJob, NotificationSeverityType.Warning);
        }


        public bool CheckExtraFields(EmailCampaign campaign, string email)
        {
            List<string> missingFields = new List<string>();
            while (email.Contains("[[["))
            {
                missingFields.Add(email.Substring(email.IndexOf("[[[")+3, email.IndexOf("]]]") - (email.IndexOf("[[[") + 3) ));
                email = email.Replace("[[[" + missingFields.Last() + "]]]", ""); //Remove the bad merge field to not double count it
            }

            if (missingFields.Any())
            {
                string error = string.Format("Merge fields exist in html document, but not in the database mapping tables for email campaign {0}. The email will not be sent. Update mapping for email and rerun. The following fields were found: \r\n", campaign.EmailCampaignId);
                foreach (string field in missingFields)
                {
                    error += field + "\r\n";
                }

                PLR.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return true;
            }

            return false;
        }

        public int HandleCampaignDataEmails(EmailCampaign campaign)
        {
            int numberOfEmailsSent = 0;
            var pendingWork = DA.GetCampaignData(campaign.EmailCampaignId);

            if (!pendingWork.Any()) //Active campaign, but doesnt have any pending work
                return 0;

            List<MergeField> mergeFields;
            if (!pendingWork.First().LineData.IsNullOrEmpty())
                mergeFields = DA.GetMergeFieldMapping(campaign.EmailCampaignId);
            else
                mergeFields = new List<MergeField>();

            Console.WriteLine("Found {0} pending records for campaign {1}", pendingWork.Count, campaign.EmailCampaignId);
            foreach (CampaignData borrower in pendingWork)
            {
                //We need a pause for 5 seconds so that we do not overload the email server.
                if (numberOfEmailsSent % 5000 == 0)
                    Thread.Sleep(5000);

                bool sentEmail = false;
                if (borrower.EmailSentAt.HasValue)
                    sentEmail = true;
                else
                    sentEmail = ProcessEmail(borrower, campaign, numberOfEmailsSent == 0, mergeFields);
                
                if (sentEmail)
                {
                    numberOfEmailsSent++;
                    if (!borrower.EmailSentAt.HasValue)
                        DA.MarkCampaignDataEmailSent(borrower.CampaignDataId);
                    if (!borrower.ArcProcessedAt.HasValue)
                    {
                        int? arcAddProcessingId = null;
                        if (!campaign.Arc.IsNullOrEmpty())
                            arcAddProcessingId = AddArc(borrower, campaign);
                        DA.MarkCampaignDataArcProcessed(borrower.CampaignDataId, arcAddProcessingId);
                    }
                }
            }
            return numberOfEmailsSent;
        }

        public bool ProcessEmail(CampaignData borrower, EmailCampaign campaign, bool sendSsCampaignEmail, List<MergeField> mergeFields)
        {
            string email = GetEmailText(campaign);
            email = email.Replace("[[[Account Number]]]", "XXXXXX" + borrower.AccountNumber.Substring(5));
            email = email.Replace("[[[Name]]]", string.Format("{0} {1}", borrower.FirstName, borrower.LastName).Trim());
            if (mergeFields.Any())
            {
                List<string> splitLine = borrower.LineData.SplitAndRemoveQuotes(",");
                foreach (MergeField field in mergeFields)
                {
                    if (email.Contains("[[[" + field.MergeFieldName + "]]]"))
                        email = email.Replace("[[[" + field.MergeFieldName + "]]]", splitLine[field.LineDataIndex]);
                    else
                    {
                        PLR.AddNotification(string.Format("Merge field [[[{0}]]] not found in email html, but existed in the database mapping for email campaign {1}. The email will not be sent. Update mapping for email and rerun.", field.MergeFieldName, campaign.EmailCampaignId), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        return false;
                    }
                }
            }

            if (CheckExtraFields(campaign, email))
                return false; //Has extra merge fields and should not be processed


            try
            {
                if (sendSsCampaignEmail)
                {
                    string subject = "[Batch E-mail: New campaign starting] " + campaign.SubjectLine;
                    EmailHelper.SendMailBatch(DataAccessHelper.TestMode, "sshelp@utahsbr.edu", campaign.SendingAddress, campaign.SubjectLine, email, "", "", "", Uheaa.Common.EmailHelper.EmailImportance.Normal, true);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("{3}; There was an error sending the email {0}, to the account {1} for the following email address {2} to sshelp@utahsbr.edu.", campaign.LetterId, borrower.AccountNumber, borrower.Recipient, DateTime.Now.ToString());
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }

            try
            {
                EmailHelper.SendMailBatch(DataAccessHelper.TestMode, borrower.Recipient, campaign.SendingAddress, campaign.SubjectLine, email, "", "", "", Uheaa.Common.EmailHelper.EmailImportance.Normal, true);
            }
            catch (Exception ex)
            {
                string message = string.Format("{3}; There was an error sending the email {0}, to the account {1} for the following email address {2}.", campaign.LetterId, borrower.AccountNumber, borrower.Recipient, DateTime.Now.ToString());
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
            return true;
        }

        public int? AddArc(CampaignData borrower, EmailCampaign campaign)
        {
            ArcData ad = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = borrower.AccountNumber,
                Arc = campaign.Arc,
                Comment = campaign.CommentText,
                ScriptId = ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };
            var results = ad.AddArc();
            if (!results.ArcAdded)
            {
                string message = string.Format("{0}; Unable to add ARC: {0}, to the following Account {1}.  Comment Text {2}", campaign.Arc, borrower.AccountNumber, campaign.CommentText, DateTime.Now.ToString());
                PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return null;
            }
            return results.ArcAddProcessingId;
        }
        
        public string GetEmailText(EmailCampaign campaign)
        {
            string doc = Path.Combine(EnterpriseFileSystem.GetPath("EmailCampaigns"), campaign.LetterId);
            return File.ReadAllText(doc);
        }

        public bool CheckCampaignFile(EmailCampaign campaign)
        {
            string doc = Path.Combine(EnterpriseFileSystem.GetPath("EmailCampaigns"), campaign.LetterId);
            if (!File.Exists(doc))
            {
                PLR.AddNotification(string.Format("Unable to process Campaign Id {0}, could not find file {1}.", campaign.EmailCampaignId, doc), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks to see if the exists, and is empty.
        /// </summary>
        /// <param name="file">File to check.</param>
        /// <returns>True if the file exists and is not empty.</returns>
        public bool CheckFile(string file, string filePattern)
        {
            //We did not find any files to process.
            if (file.IsNullOrEmpty() && filePattern != "")
            {
                string message = string.Format("{0}; The following SAS file was missing {1}", DateTime.Now.ToString(), filePattern);
                PLR.AddNotification(message, NotificationType.NoFile, NotificationSeverityType.Informational);
                return false;
            }
            //Empty File
            if (!file.IsNullOrEmpty())
            {
                if (new FileInfo(file).Length == 0)
                {
                    File.Delete(file);
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        public List<CampaignData> ReadFile(string file)
        {
            List<CampaignData> fileData = new List<CampaignData>();
            DataTable dt = FileSystemHelper.CreateDataTableFromFile(file);
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    fileData.Add(new CampaignData
                    {
                        Recipient = dr.Field<string>("DX_ADR_EML"),
                        AccountNumber = dr.Field<string>("DF_SPE_ACC_ID"),
                        FirstName = dr.Field<string>("DM_PRS_1"),
                        LastName = dr.Field<string>("DM_PRS_LST")
                    });
                }
                catch (ArgumentException ex)
                {
                    string message = string.Format("{0}; The follow file was in the incorrect format: {1}", DateTime.Now.ToString(), file);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    return null;
                }
            }

            return fileData;
        }
    }
}