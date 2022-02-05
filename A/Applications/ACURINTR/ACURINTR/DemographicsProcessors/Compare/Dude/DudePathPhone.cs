using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class DudePathPhone : ComparePathBase
    {
        private const string NO_PHONE = "NO PHONE";

        public DudePathPhone(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userId, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, logData, userId, scriptId)
        {
        }

        /// <summary>
        /// Overrides ComparePathBase.Process
        /// </summary>
        /// <param name="task"></param>
        /// <param name="queueData"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
        {
            bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);
            if (!DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
                return;//The task is closed or reassigned in the DemographicsPassReview() function after adding an LP50 comment, so our work here is done.
            if (!borrowerExistsOnOnelink ? UpdateOneLink(queueData, task, pauseOnQueueClosingError) : UpdateCompass(queueData, task))
                return; //The UpdateOneLink() function also adds an LP50 comment and handles closing or reassigning the task as needed, so our work here is done.
            if (!task.IsClosed)
                CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Error closing task.");
            ProcessLocate(task);
        }

        /// <summary>
        /// compare our data with the session and leave notes if new info is found.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        /// <param name="borrowerExistsOnOnelink"></param>
        /// <returns></returns>
        protected override bool DemographicsPassReview(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, bool borrowerExistsOnOnelink = true)
        {
            //Recovery constants:
            const string QUEUE_FOREIGN_PHONE_NUMBER = "Created queue task to review foreign phone number";
            const string COMMENT_FOREIGN_PHONE_NUMBER = "Added OneLINK comment for foreign phone number";
            const string COMMENT_INCOMPLETE_PHONE_NUMBER = "Added OneLINK comment for incomplete phone number";
            const string COMMENT_NON_NUMERIC_PHONE_NUMBER = "Added OneLINK comment for non-numeric phone number";
            const string COMMENT_INVALID_PHONE_NUMBER = "Added OneLINK comment for invalid phone number";
            const string COMMENT_PHONE_NUMBER_ON_FILE = "Added OneLINK comment for phone number already on file";

            //Determine which phone number we're dealing with.
            string phoneNumber = task.Demographics.PrimaryPhone;
            bool isAltPhone = false;
            if (task.HasAltPhone)
            {
                phoneNumber = task.Demographics.AlternatePhone;
                isAltPhone = true;
            }

            //No other checks apply if there is no phone number.
            if (phoneNumber.ToUpper() == NO_PHONE)
                return true;

            //Check the phone number for error conditions.
            if (phoneNumber.Length > 14 || phoneNumber.StartsWith("011"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
                if (CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                    string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
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

            if (phoneNumber.Length < 10)
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
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

            if (!phoneNumber.IsNumeric())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
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

            if (phoneNumber == "8015551212" && task.AdditionalInfo == "Y")
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
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

            if (PhoneNumberIsForeign(task.Demographics.Ssn, borrowerExistsOnOnelink))
            {
                if (!task.IsClosed)
                    ReassignQueueTask(queueData, task, "Current demographics foreign.");
                return false;
            }

            if (borrowerExistsOnOnelink ? PhoneNumberMatchesOneLink(task.Demographics.Ssn, phoneNumber, isAltPhone, task.AdditionalInfo) : PhoneNumberMatchesCompass(task.Demographics.Ssn, phoneNumber, PhoneType.Home))
            {
                string rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
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
            return true;
        }

        /// <summary>
        /// Compare our object data with data in the session.
        /// </summary>
        /// <param name="ssnOrAccountNumber"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="isAltPhone"></param>
        /// <param name="validityIndicator"></param>
        /// <returns></returns>
        private bool PhoneNumberMatchesOneLink(string ssnOrAccountNumber, string phoneNumber, bool isAltPhone, string validityIndicator)
        {
            //Hit up LP22 to get the borrower's current demographics.
            if (ssnOrAccountNumber.Length == 9)
                RI.FastPath(string.Format("LP22I{0};;;;L", ssnOrAccountNumber));
            else
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", ssnOrAccountNumber));
            //Compare the phone number in question to the corresponding phone on the system.
            if (isAltPhone)
                return (RI.CheckForText(14, 12, phoneNumber) && RI.CheckForText(15, 38, validityIndicator));
            else
                return (RI.CheckForText(13, 12, phoneNumber) && RI.CheckForText(14, 38, validityIndicator));
        }

        /// <summary>
        /// Set the session data to match our object data.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="task"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        /// <returns></returns>
        protected override bool UpdateOneLink(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
        {
            //Recovery constants:
            const string SUCCESS = "Phone number changed in OneLINK";
            const string FAILURE = "Phone number blocked in OneLINK by parameter card";

            //Determine which phone number we're dealing with.
            string phoneNumber = task.Demographics.PrimaryPhone;
            bool isAltPhone = false;
            if (task.HasAltPhone)
            {
                phoneNumber = task.Demographics.AlternatePhone;
                isAltPhone = true;
            }

            //Update the system with an invalid phone number if there is no phone.
            if (phoneNumber.ToUpper() == NO_PHONE)
            {
                phoneNumber = "8015551212";
                task.AdditionalInfo = "N";
            }


            RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
            RI.PutText(3, 9, task.OneLinkSourceCode);
            if (isAltPhone)
            {
                RI.PutText(14, 12, phoneNumber);
                RI.PutText(14, 38, task.AdditionalInfo);
                RI.PutText(14, 68, task.Demographics.ConsentIndicator);
            }
            else
            {
                RI.PutText(13, 12, phoneNumber);
                RI.PutText(13, 38, task.AdditionalInfo);
                RI.PutText(13, 68, task.Demographics.ConsentIndicator);
            }
            RI.Hit(Key.F6);
            bool success = false;
            switch (RI.GetText(22, 3, 5))
            {
                case "49000":
                    success = true;
                    break;
                case "UPDAT":
                    if (RI.CheckForText(22, 3, "UPDATE ERROR: ALTPH") || RI.CheckForText(22, 3, "UPDATE ERRORS: ALTPH"))
                        success = true;
                    else
                    {
                        success = false;
                        ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Error updating the phone number '{0}' for borrower '{1}' with source '{2}'.  Error Message: {3}", phoneNumber, task.Demographics.AccountNumber, task.OneLinkSourceCode, RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
                    }
                    break;
                case "40163":
                case "48081":
                    //Set the recovery step, which will determine what comment to add below.
                    success = false;
                    break;
                default:
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, "OneLINK not successfully updated.");
                    return false;
            }

            string rejectReason;
            string actionCode;
            if (success)
            {
                rejectReason = RejectAction.RejectReasons.NONE;
                actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("Accept demos from {0} {1}{2}{3}.", task.DemographicsSource, "{", phoneNumber, "}");
                if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                    return true;
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, comment);
                    return false;
                }
            }
            else
            {
                rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_BLOCKED_BY_PARAMETER_CARD;
                actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} returned phone number blocked by parameter card.", task.DemographicsSource);
                if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                {
                    if (!task.IsClosed)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, comment);
                }
                else
                {
                    if (!task.IsClosed)
                        ReassignQueueTask(queueData, task, rejectReason);
                }
                string compassComment = string.Format("Phone number was not added {0}{1}{2}. Please review.", "{", phoneNumber, "}");
                AddCompassComment(queueData, task, "RVWTU", compassComment);
                return false;
            }
            return false;
        }
    }
}
