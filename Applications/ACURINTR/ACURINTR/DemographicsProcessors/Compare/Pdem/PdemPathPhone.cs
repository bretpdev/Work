using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using System.Collections.Generic;
using Uheaa.Common;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class PdemPathPhone : ComparePathPhone
    {
        public PdemPathPhone(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userId, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, logData, userId, scriptId)
        {
        }

        /// <summary>
        /// Compare the object phone number to the session
        /// </summary>
        /// <param name="task"></param>
        /// <param name="queueData"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
        {
            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);
            bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);

            if (borrowerExistsOnOnelink)
            {
                if (!UpdateOneLink(queueData, task, pauseOnQueueClosingError))
                {
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                    return;
                }
            }
            if (!borrowerExistsOnOnelink)
            {
                SystemBorrowerDemographics demo = RI.GetDemographicsFromTx1j(task.Demographics.Ssn);
                RI.Hit(Key.F12);
                RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                RI.Hit(Key.F6, 3);


                string primaryPhoneValid = RI.GetText(17, 54, 1);
                RI.PutText(16, 14, "A", Key.Enter);
                string altPhoneValid = RI.GetText(17, 54, 1);

                if (primaryPhoneValid == "N" || (primaryPhoneValid == "Y" && altPhoneValid == "Y"))
                    phoneType = PhoneType.Home;
                else if (altPhoneValid != "Y")
                    phoneType = PhoneType.Alternate;
            }

            if (UpdateCompass(queueData, task))
            {
                string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} phone number {1}{2}{3} updated as {4} phone.", task.DemographicsSource, "{", phoneNumber, "}", (phoneType == PhoneType.Home ? "home" : "alternate"));
                AddCompassComment(queueData, task, actionCode, comment);
            }
            else
                ProcessLocate(task);
        }

        /// <summary>
        /// Add comments to session if problems arise from object phone number
        /// </summary>

        protected override bool DemographicsPassReview(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, bool borrowerExistsOnOnelink)
        {
            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);

            if (task.Demographics.Ssn.Substring(0, 1) == "P")
            {
                ReassignQueueTask(queueData, task, "Task is for a reference");
                return false;
            }

            //Check the phone number for error conditions.
            if (phoneNumber.Length > 10 || phoneNumber.StartsWith("011"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
                if (CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
                {
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                    string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                    bool success = false;
                    success = borrowerExistsOnOnelink ?
                        AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) :
                        AddCompassComment(queueData, task, actionCode, comment);
                    if (!success)
                        CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                }
                else
                {
                    ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, "Could not create user queue task."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                return false;
            }

            if (phoneNumber.Length < 10)
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                bool results = borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment);
                if (!results)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }

            if (!phoneNumber.IsNumeric())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                bool result = borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment);
                if (!result)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }

            if (phoneNumber == "8015551212")
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                bool result = borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment);
                if (!result)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }

            if (PhoneNumberIsForeign(task.Demographics.Ssn, borrowerExistsOnOnelink))
            {
                CreateQueueTask(queueData, task, ForeignDemographicsQueue, "Current demographics foreign.");
                return false;
            }

            if (PhoneNumberMatchesOneLink(task.Demographics.Ssn, phoneNumber))
            {
                string rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                bool result = borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment);
                if (!result)
                    CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }

            bool phoneHistInvalid = false;
            if (!borrowerExistsOnOnelink)
            {
                foreach (PhoneType type in new List<PhoneType>() { PhoneType.Home, PhoneType.Alternate, PhoneType.Work })
                {
                    int? value = CompareHistoryPhone(task.Demographics.Ssn, phoneNumber, type);
                    if (value.HasValue)
                    {
                        phoneHistInvalid = value.Value == 1;
                        if (phoneHistInvalid)
                            break;
                    }
                }
            }
            if (borrowerExistsOnOnelink ? PhoneNumberIsInvalidInOneLinkWithinPastYear(task.Demographics.Ssn, phoneNumber) : phoneHistInvalid)
            {
                string rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_INVALID_IN_HISTORY;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                bool result = borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment);
                if (!result) ;
                CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update the phone number in the session
        /// </summary>
        protected override bool UpdateOneLink(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
        {
            //Recovery constants:
            const string SUCCESS = "Phone number changed in OneLINK";
            const string FAILURE = "Phone number blocked in OneLINK by parameter card";

            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);

            RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
            RI.PutText(3, 9, task.OneLinkSourceCode);
            string consentIndicator = string.Empty;
            //determine consent for Compass PDEM tasks
            if (queueData.Queue == "1E")
            {
                if (task.Demographics.MblIndicator == "L" && (task.Demographics.ConsentIndicator == "Y" || task.Demographics.ConsentIndicator == "N")) { consentIndicator = "L"; }
                else if (task.Demographics.MblIndicator == "M" && task.Demographics.ConsentIndicator == "Y") { consentIndicator = "P"; }
                else if (task.Demographics.MblIndicator == "M" && task.Demographics.ConsentIndicator == "N") { consentIndicator = "N"; }
                else if (task.Demographics.MblIndicator == "U" && (task.Demographics.ConsentIndicator == "Y" || task.Demographics.ConsentIndicator == "N")) { consentIndicator = "T"; }
            }
            else
                consentIndicator = "T";
            if (RI.CheckForText(13, 38, "N") || (RI.CheckForText(13, 38, "Y") && RI.CheckForText(14, 38, "Y")))
            {
                phoneType = PhoneType.Home;
                RI.PutText(13, 12, phoneNumber);
                RI.PutText(13, 38, "Y");
                RI.PutText(13, 68, consentIndicator);
            }
            else if (RI.CheckForText(14, 38, "N"))
            {
                phoneType = PhoneType.Alternate;
                RI.PutText(14, 12, phoneNumber);
                RI.PutText(14, 38, "Y");
                RI.PutText(14, 68, consentIndicator);
            }
            RI.Hit(Key.F6);
            bool success = false;
            switch (RI.GetText(22, 3, 5))
            {
                case "49000":
                    //Set the recovery step, which will determine what comment to add below.
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
                    return false;
            }

            string rejectReason;
            string actionCode;
            if (success)
            {
                rejectReason = RejectAction.RejectReasons.NONE;
                actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("Accept demos from {0} {1}{2}{3}.", task.DemographicsSource, "{", phoneNumber, "}");
                return AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment);
            }
            else
            {
                rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_BLOCKED_BY_PARAMETER_CARD;
                actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} returned phone number blocked by parameter card.", task.DemographicsSource);
                AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment);
                return false;
            }
            return false;
        }
    }
}
