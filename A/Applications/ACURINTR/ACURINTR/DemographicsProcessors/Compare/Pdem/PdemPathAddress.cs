using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class PdemPathAddress : ComparePathAddress
    {
        public PdemPathAddress(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userid, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, logData, userid, scriptId)
        {
        }
        /// <summary>
        /// Compare the address in the seesion to our object address.
        /// </summary>
        public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
        {
            bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);

            string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Address);
            if (!DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
                return; //The DemographicsPassReview() method takes care of LP50 and creating a user review task if needed, so our work here is done.
            if (borrowerExistsOnOnelink)
            {
                if (!UpdateOneLink(queueData, task, pauseOnQueueClosingError))
                {
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                    return;
                }
            }
            string comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
            if (!AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
            {
                CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return;
            }
            if (UpdateCompass(queueData, task))
            {
                comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
                AddCompassComment(queueData, task, actionCode, comment);
            }
            else
                ProcessLocate(task);
        }

        /// <summary>
        /// Leave comments in session for bad data from our input.
        /// </summary>
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

            if (task.Demographics.Ssn.Substring(0, 1) == "P")
            {
                ReassignQueueTask(queueData, task, "Task is for a reference");
                return false;
            }

            //Check the address for problem conditions.
            if (task.Demographics.Address1.IsNullOrEmpty())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_ADDRESS_WITHOUT_STREET;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                bool result = borrowerExistsOnOnelink ?
                    AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                    AddCompassComment(queueData, task, actionCode, comment);
                if (!result)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");

                return false;
            }
            if (task.Demographics.Address1.ToUpper() == "GENERAL DELIVERY" || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.ToUpper() == "GENERAL DELIVERY"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_ADDRESS;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                bool result = borrowerExistsOnOnelink ?
                    AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                    AddCompassComment(queueData, task, actionCode, comment);
                if (!result)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");

                return false;
            }
            if (task.Demographics.City.IsNullOrEmpty() || task.Demographics.State.IsNullOrEmpty() || task.Demographics.ZipCode.IsNullOrEmpty() || System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.City, @"[^A-Za-z\s]"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    bool result = borrowerExistsOnOnelink ?
                        AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                        AddCompassComment(queueData, task, actionCode, comment);
                    if (!result)
                        CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                }
                else
                {
                    ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, "Could not create user queue task."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                return false;
            }
            if (task.Demographics.State.Length == 1)
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_ADDRESS;
                if (CreateQueueTask(queueData, task, DemographicsReviewQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    bool result = borrowerExistsOnOnelink ?
                        AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                        AddCompassComment(queueData, task, actionCode, comment);
                    if (!result)
                        CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                }
                else
                {
                    ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, "Could not create user queue task."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                return false;
            }
            if (task.Demographics.State.ToUpper() == "FC")
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
                if (CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                    string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                    bool result = borrowerExistsOnOnelink ?
                        AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                        AddCompassComment(queueData, task, actionCode, comment);
                    if (!result)
                        CreateQueueTask(queueData, task, ForeignDemographicsQueue, "");
                }
                else
                {
                    ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, "Could not create user queue task."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                return false;
            }
            if (task.Demographics.ZipCode.Length < 5)
            {
                CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }
            if (!task.Demographics.ZipCode.IsNumeric())
            {
                CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }

            //Compare the address to the borrower's current address
            //and, if needed, to the borrower's address history.
            if (AddressIsForeign(task.Demographics, borrowerExistsOnOnelink))
            {
                CreateQueueTask(queueData, task, ForeignDemographicsQueue, "");
                return false;
            }
            if (borrowerExistsOnOnelink ? AddressMatchesOneLink(task.Demographics) : AddressMatchesCompass(task.Demographics))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                bool result = borrowerExistsOnOnelink ?
                    AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                    AddCompassComment(queueData, task, actionCode, comment);
                if (!result)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }
            if (borrowerExistsOnOnelink ? AddressIsInvalidInOneLinkWithinPastYear(task.Demographics) : CompareAddressHistory(task))
            {
                string rejectReason = RejectAction.RejectReasons.ADDRESS_INVALID_IN_HISTORY;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
                string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
                bool result = borrowerExistsOnOnelink ?
                    AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                    AddCompassComment(queueData, task, actionCode, comment);
                if (!result)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update one link address from out QueueTask data.
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
                return false;
            //Check that the address isn't too long to fit in the system.
            if (task.Demographics.Address1.Length > 35 || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.Length > 35))
                return false;

            //Update the borrower's address.
            RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
            RI.PutText(3, 9, task.OneLinkSourceCode);
            RI.PutText(11, 9, task.Demographics.Address1.Replace("-", " "), true);
            if (task.Demographics.Address2.IsNullOrEmpty())
                RI.PutText(11, 9, "", true);
            else
                RI.PutText(10, 9, task.Demographics.Address2.Replace("-", " "), true);
            RI.PutText(12, 9, task.Demographics.City, true);
            RI.PutText(12, 52, task.Demographics.State);
            RI.PutText(12, 60, task.Demographics.ZipCode, true);
            RI.PutText(10, 57, "Y");
            RI.Hit(Key.F6);
            return RI.CheckForText(22, 3, "49000");
        }
    }
}
