using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace ACURINTR.DemographicsProcessors
{
    class ComparePathAddress : ComparePathBase
    {
        public ComparePathAddress(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, GeneralRecovery recovery, ErrorReport errorReport, ProcessLogRun logRun, DataAccess da, string userId, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, recovery, errorReport, logRun, da, userId, scriptId)
        {
        }

        /// <summary>
        /// Address compare entry point.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="queueData"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
        {
            //If there's recovery info, make sure it matches what's on the screen.
            if (GRecovery.Path != GeneralRecovery.ProcessingPath.None && (task.Demographics.AccountNumber != GRecovery.AccountNumber || GRecovery.Path != GeneralRecovery.ProcessingPath.Address))
            {
                string message = $"{RI.UserId}: It looks like the script didn't finish processing the previous borrower, but that borrower is no longer in the { queueData.Queue} queue. \n ";
                    message += " Please check the log file to see where the processing was interrupted, finish out that borrower if needed, then delete the log file and restart the script. \n";
                Console.WriteLine(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
            }

            //Start at the appropriate point given the recovery value.
            string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Address);
            bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.None)
                GRecovery.Phase = GeneralRecovery.ProcessingPhase.ReviewDemographics;
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.ReviewDemographics)
            {
                if (DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateOneLink;
                else //The task is closed or reassigned in the DemographicsPassReview() function after adding an LP50 comment, so our work here is done.
                    return;

            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.UpdateOneLink)
            {
                if (borrowerExistsOnOnelink ? UpdateOneLink(queueData, task, pauseOnQueueClosingError) : UpdateCompass(queueData, task))
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.OneLinkComment;
                else
                    return;
            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.OneLinkComment)
            {
                string comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
                if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.CloseTask;
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, comment, borrowerExistsOnOnelink);
                    return;
                }
            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.CloseTask)
            {
                if (!task.IsClosed)
                    CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Error closing task.", borrowerExistsOnOnelink);
                GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateCompass;
            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.UpdateCompass)
            {
                //If the borrower does not exist on onelink we have already updated compass
                if (borrowerExistsOnOnelink && UpdateCompass(queueData, task))
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.CompassComment;
                else
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.Locate;
            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.CompassComment && borrowerExistsOnOnelink)
            {
                string comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
                AddCompassComment(queueData, task, actionCode, comment);
                GRecovery.Phase = GeneralRecovery.ProcessingPhase.Locate;
            }

            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.Locate && borrowerExistsOnOnelink)
                ProcessLocate(task);
        }

        /// <summary>
        /// Add comments to session regarding new address info
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        /// <param name="borrowerExistsOnOnelink"></param>
        /// <returns></returns>
        protected override bool DemographicsPassReview(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, bool borrowerExistsOnOnelink = true)
        {
            //Recovery constants:
            const string COMMENT_MISSING_STREET = "Added OneLINK comment for missing street";
            const string COMMENT_INVALID_ADDRESS = "Added OneLINK comment for invalid address";
            const string QUEUE_MISSING_ADDRESS = "Created queue task to review missing address";
            const string COMMENT_MISSING_ADDRESS = "Added OneLINK comment for missing address";
            const string QUEUE_SHORT_STATE = "Created queue task to review one-letter state code";
            const string COMMENT_SHORT_STATE = "Added OneLINK comment for one-letter state code";
            const string QUEUE_FOREIGN_ADDRESS = "Created queue task to review foreign address";
            const string COMMENT_FOREIGN_ADDRESS = "Added OneLINK comment for foreign address";
            const string COMMENT_ADDRESS_ON_FILE = "Added OneLINK comment for address already on file";
            const string COMMENT_ADDRESS_IN_HISTORY = "Added OneLINK comment for address invalid in history";

            if (GRecovery.Step.IsPopulated() && task.Demographics.FirstName.IsPopulated())
            {
                string comment = string.Format("Post Office has possible alternate name for borrower {0} {1}.", task.Demographics.FirstName, task.Demographics.LastName);
                if (borrowerExistsOnOnelink)
                    AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, "K4AKA", comment);
                else
                    AddCompassComment(queueData, task, "K4AKA", comment);
            }

            //Check the address for problem conditions.
            if (task.Demographics.Address1.IsNullOrEmpty())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_ADDRESS_WITHOUT_STREET;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (GRecovery.Step == COMMENT_MISSING_STREET || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_MISSING_STREET;
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason, borrowerExistsOnOnelink);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                }
                return false;
            }
            if (GRecovery.Step == COMMENT_INVALID_ADDRESS || task.Demographics.Address1.ToUpper() == "GENERAL DELIVERY" || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.ToUpper() == "GENERAL DELIVERY"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_ADDRESS;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (GRecovery.Step == COMMENT_INVALID_ADDRESS || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_INVALID_ADDRESS;
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason, borrowerExistsOnOnelink);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                }
                return false;
            }
            if (task.Demographics.City.IsNullOrEmpty() || task.Demographics.State.IsNullOrEmpty() || task.Demographics.ZipCode.IsNullOrEmpty() || System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.City, @"[^A-Za-z\s]"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (GRecovery.Step == QUEUE_MISSING_ADDRESS || CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    GRecovery.Step = QUEUE_MISSING_ADDRESS;
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (GRecovery.Step == COMMENT_MISSING_ADDRESS || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        GRecovery.Step = COMMENT_MISSING_ADDRESS;
                        if (!task.IsClosed)
                            CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason, borrowerExistsOnOnelink);
                    }
                    else
                    {
                        if (!task.IsClosed)
                            ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                    }
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                }
                return false;
            }
            if (task.Demographics.State.Length == 1)
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (GRecovery.Step == QUEUE_SHORT_STATE || CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    GRecovery.Step = QUEUE_SHORT_STATE;
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} .", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (GRecovery.Step == COMMENT_SHORT_STATE || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        GRecovery.Step = COMMENT_SHORT_STATE;
                        if (!task.IsClosed)
                            CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason, borrowerExistsOnOnelink);
                    }
                    else
                    {
                        if (!task.IsClosed)
                            ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                    }
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                }
                return false;
            }
            if (task.Demographics.State.ToUpper() == "FC" || task.Demographics.Country.IsPopulated())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
                if (GRecovery.Step == QUEUE_FOREIGN_ADDRESS || CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
                {
                    GRecovery.Step = QUEUE_FOREIGN_ADDRESS;
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (GRecovery.Step == COMMENT_FOREIGN_ADDRESS || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        GRecovery.Step = COMMENT_FOREIGN_ADDRESS;
                        if (!task.IsClosed)
                            CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, comment, borrowerExistsOnOnelink);
                    }
                    else
                    {
                        if (!task.IsClosed)
                            ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                    }
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                }
                return false;
            }
            if (task.Demographics.ZipCode.Length < 5)
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS, borrowerExistsOnOnelink);
                return false;
            }
            if (!task.Demographics.ZipCode.IsNumeric())
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, RejectAction.RejectReasons.RETURN_INVALID_ADDRESS, borrowerExistsOnOnelink);
                return false;
            }

            //Compare the address to the borrower's current address
            //and, if needed, to the borrower's address history.
            if (AddressIsForeign(task.Demographics, borrowerExistsOnOnelink))
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "Foreign demographics provided.", borrowerExistsOnOnelink);
                return false;
            }
            if (GRecovery.Step == COMMENT_ADDRESS_ON_FILE || borrowerExistsOnOnelink ? AddressMatchesOneLink(task.Demographics) : AddressMatchesCompass(task.Demographics))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (GRecovery.Step == COMMENT_ADDRESS_ON_FILE || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_ADDRESS_ON_FILE;
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason, borrowerExistsOnOnelink);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                }
                return false;
            }
            if (GRecovery.Step == COMMENT_ADDRESS_IN_HISTORY || borrowerExistsOnOnelink ? AddressIsInvalidInOneLinkWithinPastYear(task.Demographics) : CompareAddressHistory(task))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_INVALID_IN_HISTORY;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (GRecovery.Step == COMMENT_ADDRESS_IN_HISTORY || AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_ADDRESS_IN_HISTORY;
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason, borrowerExistsOnOnelink);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason, borrowerExistsOnOnelink);
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update the address in compass
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        protected override bool UpdateCompass(QueueData queueData, QueueTask task)
        {
            //Check that the address isn't too long to fit in the system.
            if (task.Demographics.Address1.Length > 29)
            {
                if (!CreateQueueTask(queueData, task, queueData.DemographicsReviewQueue, ""))
                {
                    Errors.AddRecord(task, "COMPASS not successfully updated.");
                    LogRun.AddNotification($"{RI.UserId}: AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: COMPASS not successfully updated.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    ReassignQueueTask(queueData, task, "Address line is too long", false);
                }
                return false;
            }

            //Check whether COMPASS already has this address.
            try
            {
                if (AddressMatchesCompass(task.Demographics)) { return false; }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("01019"))
                {
                    return false;
                }
                else
                {
                    throw new Exception("Encountered an unexpected condition on TX1J. See session for details.", ex);
                }
            }

            //We're going to assume AddressMatchesCompass() left us on CTX1J for the legal address.
            //Enter the source code (if it's a borrower), current date, and validity indicator.
            if (RI.CheckForText(1, 71, "TXX1R-01"))
                RI.PutText(8, 18, task.CompassSourceCode);
            RI.PutText(10, 32, DateTime.Now.ToString("MMddyy"));
            RI.PutText(11, 55, "Y");
            //Enter the address and blank out any extra text.
            RI.PutText(11, 10, task.Demographics.Address1, true);
            RI.PutText(12, 10, (task.Demographics.Address2 ?? ""), true);
            RI.PutText(13, 10, "", true);
            RI.PutText(14, 8, task.Demographics.City, true);
            RI.PutText(14, 32, task.Demographics.State);
            RI.PutText(14, 40, task.Demographics.ZipCode, true);
            if (RI.CheckForText(10, 67, "_"))
                RI.PutText(10, 67, "A");
            RI.Hit(Key.Enter);
            if (RI.CheckForText(23, 2, "01096", "01097", "01099"))
                return true;
            else
            {
                if (!CreateQueueTask(queueData, task, queueData.DemographicsReviewQueue, ""))
                {
                    Errors.AddRecord(task, "COMPASS not successfully updated.");
                    LogRun.AddNotification($"{RI.UserId}: AccountNumber: { task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: COMPASS not successfully updated.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                return false;
            }
        }

        /// <summary>
        /// Update the address in One link
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        /// <returns></returns>
        protected override bool UpdateOneLink(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
        {
            //Check that we don't already have a queue task to update this borrower's address.
            RI.FastPath(string.Format("LP50I{0};;;;;K0ADD", task.Demographics.Ssn));
            if (!RI.CheckForText(22, 3, "47004"))
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "Borrower has possible K0ADD.", true);
                return false;
            }
            //Check that the address isn't too long to fit in the system.
            if (task.Demographics.Address1.Length > 35 || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.Length > 35))
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "OneLINK not successfully updated.", true);
                return false;
            }

            //Update the borrower's address.
            RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
            RI.PutText(3, 9, task.OneLinkSourceCode);
            RI.PutText(10, 9, task.Demographics.Address1.Replace("-", " "), true);
            if (task.Demographics.Address2.IsNullOrEmpty())
            {
                RI.PutText(11, 9, "", true);
            }
            else
            {
                RI.PutText(11, 9, task.Demographics.Address2.Replace("-", " "), true);
            }
            RI.PutText(12, 9, task.Demographics.City, true);
            RI.PutText(12, 52, task.Demographics.State);
            RI.PutText(12, 60, task.Demographics.ZipCode, true);
            RI.PutText(10, 57, "Y");
            RI.Hit(Key.F6);
            if (RI.CheckForText(22, 3, "49000"))
                return true;
            else
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "OneLINK not successfully updated.", true);
                return false;
            }
        }
    }
}
