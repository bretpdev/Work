using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace RTNEMLINVF
{
    public class ReturnEmailInvalidation
    {
        public ExchangeService Service { get; set; }
        public ReflectionInterface RI { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public string ScriptId { get; set; }

        public ReturnEmailInvalidation(ProcessLogRun logRun, string scriptId)
        {
            LogRun = logRun;
            ScriptId = scriptId;
            DA = new DataAccess(logRun);
        }

        public ReturnEmailInvalidation() { }

        public int Process()
        {
            try
            {
                Service = new ExchangeService();
                Service.UseDefaultCredentials = true;
                Service.Url = new Uri("https://owa.utahsbr.edu/EWS/Exchange.asmx");
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Failed creating Exchange Service", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return 1;
            }

            LoadEmailsToDb();
            InvalidateMail();

            return 0;
        }

        /// <summary>
        /// Finds all the undeliverable emails for each mailbox and adds them to the database
        /// </summary>
        private void LoadEmailsToDb()
        {
            List<string> Addresses = DA.GetEmailAddresses();

            foreach (string emailAddress in Addresses)
            {
                Console.WriteLine($"Searching for invalid emails in {emailAddress}");
                int offset = 0;
                int pageSize = 100;
                FindItemsResults<Item> result;
                bool moreItems = true;
                ItemView view = new ItemView(pageSize, offset, OffsetBasePoint.Beginning);

                while (moreItems)
                {
                    FolderId sharedInbox = new FolderId(WellKnownFolderName.Inbox, emailAddress);
                    result = GetResults(sharedInbox, view);
                    if (result != null)
                    {
                        moreItems = result.MoreAvailable;
                        foreach (EmailMessage message in result)
                        {
                            try
                            {
                                message.Load();
                            }
                            catch (Exception ex)
                            {
                                LogRun.AddNotification("There was an error loading the email found on the exchange server to the database.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                                Console.WriteLine(ex.ToString());
                            }
                            //Run this outside the Try/Catch so you don't catch errors below that are not from loading the message
                            if (message.ReceivedBy != null && message.ReceivedBy.Address.ToLower() == emailAddress.ToLower() && message.Subject != null && message.Subject.ToLower().Contains("undeliverable"))
                                LoadDatabase(message);
                        }
                        if (moreItems)
                            view.Offset += pageSize;
                    }
                    else
                        break;
                }
            }
        }

        /// <summary>
        /// Gets 100 emails from the shared inbox
        /// </summary>
        private FindItemsResults<Item> GetResults(FolderId sharedInbox, ItemView view)
        {
            try
            {
                return Service.FindItems(sharedInbox, view);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"The Batch Scripts account does not have access to the {sharedInbox} email address.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return null;
            }
        }

        /// <summary>
        /// Gets a list of borrowers for the region being run and adds the data to the database.
        /// </summary>
        private void LoadDatabase(EmailMessage message)
        {
            LoadUheaa(message);
        }

        /// <summary>
        /// Adds the record to ULS and deletes the message from the exchange server
        /// </summary>
        private void LoadUheaa(EmailMessage message)
        {
            bool borrowerLocated = false;
            List<Borrower> bors = DA.GetSsnFromEmailUdw(message.ToRecipients.FirstOrDefault().Address); // Gets multiple UDW data
            if (bors != null)
            {
                if (bors.Count > 0)
                {
                    Console.WriteLine($"Loading Email Address: {message.ToRecipients.FirstOrDefault().Address} to database.");
                    foreach (Borrower bor in bors)
                    {
                        if (bor.Ssn.IsPopulated())
                        {
                            if (DA.AddEmailToTable(message, bor))
                                //The message only needs to be deleted once, since there is only one
                                //we delete the message when processing is complete
                                if (bor == bors.Last())
                                {
                                    DeleteMail(message);
                                    borrowerLocated = true;
                                }
                        }
                        else
                            LogRun.AddNotification($"SSN was not found for Email Address: {message.ToRecipients.FirstOrDefault().Address}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    }
                }
            }

            List<Borrower> borrower = DA.GetSsnFromEmailOdw(message.ToRecipients.FirstOrDefault().Address); // Gets single ODW data
            if (borrower != null)
            {
                if (borrower.Count > 0)
                {
                    Console.WriteLine($"Loading Email Address: {message.ToRecipients.FirstOrDefault().Address} to database.");
                    foreach (Borrower bor in borrower)
                    {
                        if (bor.Ssn.IsPopulated())
                        {
                            if (DA.AddEmailToTable(message, bor))
                                //The message only needs to be deleted once, since there is only one
                                //we delete the message when processing is complete
                                if (bor == borrower.Last() && !borrowerLocated)
                                {
                                    DeleteMail(message);
                                    borrowerLocated = true;
                                }
                        }
                        else
                            LogRun.AddNotification($"SSN was not found for Email Address: {message.ToRecipients.FirstOrDefault().Address}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    }
                }

                if (!borrowerLocated)
                    DeleteMail(message);
            }
        }

        /// <summary>
        /// Pulls all the emails out of the database and processes them.
        /// </summary>
        private void InvalidateMail()
        {
            RI = new ReflectionInterface();
            BatchProcessingLoginHelper.Login(LogRun, RI, ScriptId, "BatchUheaa");
            List<InvalidEmail> emails = DA.GetInvalidEmail();

            //Maintain a record of all arcs added on this run
            Dictionary<UserByRegion, int> ArcsAdded = new Dictionary<UserByRegion, int>(new UserByRegionComparer());

            foreach (InvalidEmail email in emails)
            {
                Console.WriteLine($"Invalidating Email Address: {email.EmailAddress}");
                try
                {
                    if (email.EmailType == "O")
                    {
                        if (InvalidateOnelinkMail(email))
                        {
                            //Check if the user already has a onlink arc on this run
                            var user = new UserByRegion(email.Ssn, true);
                            if (ArcsAdded.ContainsKey(user))
                            {
                                //If they have a onelink arc, update that record to point to the existing arc
                                DA.SetArcAddId(email.InvalidEmailId, ArcsAdded[user]);
                            }
                            else
                            {
                                //Add a new onelink arc if one does not exist
                                int? arc = AddArc(email, true);
                                if (arc.HasValue)
                                {
                                    ArcsAdded.Add(user, arc.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (InvalidateCompassMail(email))
                        {
                            //Check if the user already has a compass arc on this run
                            var user = new UserByRegion(email.Ssn, false);
                            if (ArcsAdded.ContainsKey(user))
                            {
                                //If they have a compass arc, update that record to point to the existing arc
                                DA.SetArcAddId(email.InvalidEmailId, ArcsAdded[user]);
                            }
                            else
                            {
                                //Add a new compass arc if one does not exist
                                int? arc = AddArc(email, false);
                                if (arc.HasValue)
                                {
                                    ArcsAdded.Add(user, arc.Value);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogRun.AddNotification($"There was an error processing InvalidEmailId:{email.InvalidEmailId}", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                }
            }
            RI.CloseSession();
        }

        private bool InvalidateCompassMail(InvalidEmail email)
        {
            if (!email.InvalidatedAt.HasValue)
            {
                if (RI.InvalidateCompassEmail(email.Ssn, email.EmailType))
                {
                    DA.SetInvalidatedAt(email.InvalidEmailId);
                    Console.WriteLine($"Email: {email.EmailAddress}, Email Type: {email.EmailType} invalidate for Borrower: {email.Ssn}");
                    return true;
                }
                else
                {
                    LogRun.AddNotification($"Error invalidating InvalidEmailId: {email.InvalidEmailId}, Email Address: {email.EmailAddress}, Email Type: {email.EmailType} for borrower: {email.Ssn} in the Compass region", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    DA.DeleteRecord(email.InvalidEmailId);
                }
            }
            return false;
        }

        private bool InvalidateOnelinkMail(InvalidEmail email)
        {
            if (!email.InvalidatedAt.HasValue)
            {
                if (RI.InvalidateOnelinkEmail(email.Ssn))
                {
                    DA.SetInvalidatedAt(email.InvalidEmailId);
                    Console.WriteLine($"Email: {email.EmailAddress}, Email Type: {email.EmailType} invalidate for Borrower: {email.Ssn}");
                    return true;
                }
                else
                    LogRun.AddNotification($"Error invalidating InvalidEmailId: {email.InvalidEmailId}, Email Address: {email.EmailAddress}, Email Type: {email.EmailType} for borrower: {email.Ssn} in the Compass region", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            return false;
        }

        private int? AddArc(InvalidEmail email, bool isOnelink)
        {
            if (email.ArcAddProcessingId == 0)
            {
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = email.Ssn,
                    Arc = isOnelink ? "DD101" : "BADEM",
                    ArcTypeSelected = isOnelink ? ArcData.ArcType.OneLINK : ArcData.ArcType.Atd22AllLoans,
                    Comment = "Email address invalidated due to a returned email",
                    ScriptId = ScriptId,
                    ActivityContact = isOnelink ? "04" : "",
                    ActivityType = isOnelink ? "EM" : ""
                };

                ArcAddResults result = arc.AddArc();
                if (result.ArcAdded)
                {
                    DA.SetArcAddId(email.InvalidEmailId, result.ArcAddProcessingId);
                    return result.ArcAddProcessingId;
                }
                else
                {
                    LogRun.AddNotification($"Error adding BADEM ARC to borrower: {email.Ssn}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return null;
                }
            }
            return null;
        }

        private void DeleteMail(EmailMessage message)
        {
            try
            {
                if (!DataAccessHelper.TestMode)
                    message.Delete(DeleteMode.MoveToDeletedItems);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error deleting the invalid email from the exchange server", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }
    }
}