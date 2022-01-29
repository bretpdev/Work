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
        public ComparePathAddress(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userId, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, logData, userId, scriptId)
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
            //Start at the appropriate point given the recovery value.
            string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Address);
            bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);
            if (!DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
                return; //The task is closed or reassigned in the DemographicsPassReview() function after adding an LP50 comment, so our work here is done.
            bool success = borrowerExistsOnOnelink ?
                UpdateOneLink(queueData, task, pauseOnQueueClosingError) :
                UpdateCompass(queueData, task);
            if (!success)
                return;

            string comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
            success = borrowerExistsOnOnelink ?
                AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                AddCompassComment(queueData, task, actionCode, comment);
            if (!success)
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, comment);
                return;
            }
            if (!task.IsClosed)
                CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Error closing task.");
            //If the borrower does not exist on onelink we have already updated compass
            if (borrowerExistsOnOnelink && UpdateCompass(queueData, task))
            {
                comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
                AddCompassComment(queueData, task, actionCode, comment);
            }
            else
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

            if (task.Demographics.FirstName.IsPopulated())
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
                if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }
            if (task.Demographics.Address1.ToUpper() == "GENERAL DELIVERY" || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.ToUpper() == "GENERAL DELIVERY"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_ADDRESS;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }
            if (task.Demographics.City.IsNullOrEmpty() || task.Demographics.State.IsNullOrEmpty() || task.Demographics.ZipCode.IsNullOrEmpty() || System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.City, @"[^A-Za-z\s]"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        if (!task.IsClosed)
                            CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                    }
                    else
                    {
                        if (!task.IsClosed)
                            ReassignQueueTask(queueData, task, rejectReason);
                    }
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }
            if (task.Demographics.State.Length == 1)
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} .", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        if (!task.IsClosed)
                            CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                    }
                    else
                    {
                        if (!task.IsClosed)
                            ReassignQueueTask(queueData, task, rejectReason);
                    }
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }
            if (task.Demographics.State.ToUpper() == "FC" || task.Demographics.Country.IsPopulated())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
                if (CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        if (!task.IsClosed)
                            CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, comment);
                    }
                    else
                    {
                        if (!task.IsClosed)
                            ReassignQueueTask(queueData, task, rejectReason);
                    }
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }
            if (task.Demographics.ZipCode.Length < 5)
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS);
                return false;
            }
            if (!task.Demographics.ZipCode.IsNumeric())
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, RejectAction.RejectReasons.RETURN_INVALID_ADDRESS);
                return false;
            }

            //Compare the address to the borrower's current address
            //and, if needed, to the borrower's address history.
            if (AddressIsForeign(task.Demographics, borrowerExistsOnOnelink))
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "Foreign demographics provided.");
                return false;
            }
            if (borrowerExistsOnOnelink ? AddressMatchesOneLink(task.Demographics) : AddressMatchesCompass(task.Demographics))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                return false;
            }
            if (borrowerExistsOnOnelink ? AddressIsInvalidInOneLinkWithinPastYear(task.Demographics) : CompareAddressHistory(task))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_INVALID_IN_HISTORY;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                {
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
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
                    ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, "COMPASS not successfully updated."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
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
            RI.Hit(Key.Enter);
            if (RI.CheckForText(23, 2, "01096", "01097"))
                return true;
            else
            {
                if (!CreateQueueTask(queueData, task, queueData.DemographicsReviewQueue, ""))
                {
                    ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, "COMPASS not successfully updated."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
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
                    ReassignQueueTask(queueData, task, "Borrower has possible K0ADD.");
                return false;
            }
            //Check that the address isn't too long to fit in the system.
            if (task.Demographics.Address1.Length > 35 || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.Length > 35))
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "OneLINK not successfully updated.");
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
                    ReassignQueueTask(queueData, task, "OneLINK not successfully updated.");
                return false;
            }
        }
    }
}
