using System;
using System.Diagnostics;
using System.Linq;
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
    class ComparePathPhone : ComparePathBase
    {
        protected PhoneType phoneType;

        public ComparePathPhone(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, GeneralRecovery recovery, ErrorReport errorReport, ProcessLogRun logRun, DataAccess da, string userId, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, recovery, errorReport, logRun, da, userId, scriptId)
        {
        }

		/// <summary>
		/// Entry point for phone number comparison in ComparePathPhone
		/// </summary>
		/// <param name="task"></param>
		/// <param name="queueData"></param>
		/// <param name="pauseOnQueueClosingError"></param>
        public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
        {
            //If there's recovery info, make sure it matches the open task.
            if (GRecovery.Path != GeneralRecovery.ProcessingPath.None && (task.Demographics.AccountNumber != GRecovery.AccountNumber || (GRecovery.Path != GeneralRecovery.ProcessingPath.HomePhone && GRecovery.Path != GeneralRecovery.ProcessingPath.AltPhone)))
            {
                string message = $"{RI.UserId}: It looks like the script didn't finish processing the previous borrower, but that borrower is no longer in the {queueData.Queue} queue. \n ";
                message += " Please check the log file to see where the processing was interrupted, finish out that borrower if needed, then delete the log file and restart the script. \n";
				Console.WriteLine(message);
				LogRun.AddNotification( message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
               	throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
            }

            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);
            bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);

            //Start at the appropriate point given the recovery value.
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.None) 
				GRecovery.Phase = GeneralRecovery.ProcessingPhase.ReviewDemographics;
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.ReviewDemographics)
            {
                if (DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateOneLink;
                else //The task is closed or reassigned in the DemographicsPassReview() function after adding an LP50 comment, so our work here is done.
                    return;
            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.UpdateOneLink || GRecovery.Phase == GeneralRecovery.ProcessingPhase.OneLinkComment)
            {
                if (borrowerExistsOnOnelink ? UpdateOneLink(queueData, task, pauseOnQueueClosingError) : UpdateCompass(queueData, task))
                {
                    if (borrowerExistsOnOnelink)
                        GRecovery.Phase = GeneralRecovery.ProcessingPhase.CloseTask;
                    else
                        GRecovery.Phase = GeneralRecovery.ProcessingPhase.CompassComment;
                }
				else //The UpdateOneLink() function also adds an LP50 comment and handles closing or reassigning the task as needed, so our work here is done.
                {
                    if (!borrowerExistsOnOnelink)
                        CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Unable to close task.", borrowerExistsOnOnelink);
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
                if (UpdateCompass(queueData, task))
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.CompassComment;
                else
                    GRecovery.Phase = GeneralRecovery.ProcessingPhase.Locate;
            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.CompassComment)
            {
                string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} phone number {1}{2}{3} updated as {4} phone.", task.DemographicsSource, "{", phoneNumber, "}", (phoneType == PhoneType.Home ? "home" : "alternate"));
                AddCompassComment(queueData, task, actionCode, comment);
                GRecovery.Phase = GeneralRecovery.ProcessingPhase.Locate;
            }
            if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.Locate && borrowerExistsOnOnelink)
                ProcessLocate(task);
            else if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.Locate && !task.IsClosed)
                CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Error closing task.", borrowerExistsOnOnelink);
        }

		/// <summary>
		/// Add notes in session for issues with the new phone number
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
            const string COMMENT_PHONE_NUMBER_IN_HISTORY = "Added OneLINK comment for phone number invalid in history";

            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);

            //Check the phone number for error conditions.
            if (phoneNumber.Length > 10 || phoneNumber.StartsWith("011"))
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
                if (GRecovery.Step == QUEUE_FOREIGN_PHONE_NUMBER || CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
                {
                    GRecovery.Step = QUEUE_FOREIGN_PHONE_NUMBER;
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                    string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                    if (GRecovery.Step == COMMENT_FOREIGN_PHONE_NUMBER || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                    {
                        GRecovery.Step = COMMENT_FOREIGN_PHONE_NUMBER;
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

            if (phoneNumber.Length < 10)
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                if (GRecovery.Step == COMMENT_INCOMPLETE_PHONE_NUMBER || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_INCOMPLETE_PHONE_NUMBER;
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

            if (!phoneNumber.IsNumeric())
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                if (GRecovery.Step == COMMENT_NON_NUMERIC_PHONE_NUMBER || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_NON_NUMERIC_PHONE_NUMBER;
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

            if (phoneNumber == "8015551212")
            {
                string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                if (GRecovery.Step == COMMENT_INVALID_PHONE_NUMBER || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_INVALID_PHONE_NUMBER;
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

            if (PhoneNumberIsForeign(task.Demographics.Ssn, borrowerExistsOnOnelink))
            {
                if (!task.IsClosed) 
					ReassignQueueTask(queueData, task, "Current demographics foreign.", borrowerExistsOnOnelink);
                return false;
            }

            if (borrowerExistsOnOnelink ? PhoneNumberMatchesOneLink(task.Demographics.Ssn, phoneNumber) : PhoneNumberMatchesCompass(task.Demographics.Ssn, phoneNumber, PhoneType.Home))
            {
                string rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_SAME_AS_ON_FILE;
                string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                if (GRecovery.Step == COMMENT_PHONE_NUMBER_ON_FILE || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_PHONE_NUMBER_ON_FILE;
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
                if (GRecovery.Step == COMMENT_PHONE_NUMBER_IN_HISTORY || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
                {
                    GRecovery.Step = COMMENT_PHONE_NUMBER_IN_HISTORY;
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
		/// update the phone number in compass to match the new one.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <returns></returns>
        protected override bool UpdateCompass(QueueData queueData, QueueTask task)
        {
            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);

            if (queueData.Queue != "1E")
            {
                if (task.HasHomePhone)
                    phoneType = PhoneType.Home;
                else if (task.HasAltPhone)
                    phoneType = PhoneType.Alternate;
            }
            

            try
            {
                if (PhoneNumberMatchesCompass(task.Demographics.Ssn, phoneNumber, phoneType)) 
                {
                    string rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_SAME_AS_ON_FILE;
                    string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                    string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
                    AddCompassComment(queueData, task, actionCode, comment);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("01019"))
                {
                    return false;
                }
                else
                {
                    throw new Exception("Encountered an unexpected condition on TX1J. See session for details.");
                }
            }

            //Update the phone information.
            if (queueData.Queue != "1E")
            {
                if (phoneNumber == task.Demographics.PrimaryPhone)
                    phoneType = PhoneType.Home;
                else
                    phoneType = PhoneType.Alternate;
            }
            UpdatePhoneNumber(phoneNumber, phoneType, task.CompassSourceCode);
            if (RI.CheckForText(23, 2, "01097", "01100"))
				return true;
            
            else if (RI.CheckForText(23, 2, "06536"))
            {
                //add the phone number to the home phone field instead
                RI.PutText(16, 14, "H", Key.Enter);
                UpdatePhoneNumber(phoneNumber, PhoneType.Home, task.CompassSourceCode);
                return true;
            }
            else
            {
				if (!CreateQueueTask(queueData, task, queueData.DemographicsReviewQueue, ""))
				{
					Errors.AddRecord(task, "COMPASS not successfully updated.");
					LogRun.AddNotification($"AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: COMPASS not successfully updated.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
				}
                return false;
            }
        }

        /// <summary>
		/// Update the phone information.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="phoneType"></param>
        /// <param name="sourceCode"></param>
        private void UpdatePhoneNumber(string phoneNumber, PhoneType phoneType, string sourceCode)
        {
            if (phoneType == PhoneType.Home)
				RI.PutText(16, 14, "H", ReflectionInterface.Key.Enter);
            else
				RI.PutText(16, 14, "A", ReflectionInterface.Key.Enter);
			RI.PutText(16, 20, "U");
			RI.PutText(16, 30, "N");
			RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
			RI.PutText(17, 14, phoneNumber);
			RI.PutText(17, 54, "Y");
			RI.PutText(19, 14, sourceCode);
			RI.Hit(Key.Enter);
        }

		/// <summary>
		/// update the phone information in one link.
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
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);

            if (GRecovery.Phase != GeneralRecovery.ProcessingPhase.OneLinkComment)
            {
				RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
				RI.PutText(3, 9, task.OneLinkSourceCode);

                if (RI.CheckForText(13, 38, "N") || (RI.CheckForText(13, 38, "Y") && RI.CheckForText(14, 38, "Y")))
                {
                    phoneType = PhoneType.Home;
					RI.PutText(13, 12, phoneNumber);
					RI.PutText(13, 38, "Y");
					RI.PutText(13, 68, "T");
                }
                else if (RI.CheckForText(14, 38, "N"))
                {
                    phoneType = PhoneType.Alternate;
					RI.PutText(14, 12, phoneNumber);
					RI.PutText(14, 38, "Y");
					RI.PutText(14, 68, "T");
                }

				RI.Hit(Key.F6);
				switch (RI.GetText(22, 3, 5))
                {
                    case "49000":
                        //Set the recovery step, which will determine what comment to add below.
                        GRecovery.Step = SUCCESS;
						break;
					case "UPDAT":
						if (RI.CheckForText(22, 3, "UPDATE ERROR: ALTPH") || RI.CheckForText(22, 3, "UPDATE ERRORS: ALTPH"))
							GRecovery.Step = SUCCESS;
						else
						{
							GRecovery.Step = FAILURE;
							LogRun.AddNotification($"{RI.UserId}: Error updating the phone number '{phoneNumber}' for borrower '{task.Demographics.AccountNumber}' with source '{task.OneLinkSourceCode}'.  Error Message: {RI.Message}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
						}
						break;
                    case "40163":
                    case "48081":
                        //Set the recovery step, which will determine what comment to add below.
                        GRecovery.Step = FAILURE;
                        break;
                    default:
                        if (!task.IsClosed) 
							ReassignQueueTask(queueData, task, "OneLINK not successfully updated.", true);
                        return false;
                }
            }

            string rejectReason;
            string actionCode;
            switch (GRecovery.Step)
            {
                case SUCCESS:
                    rejectReason = RejectAction.RejectReasons.NONE;
                    actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                    string comment = string.Format("Accept demos from {0} {1}{2}{3}.", task.DemographicsSource, "{", phoneNumber, "}");
                    if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                        return true;
                    else
                    {
                        if (!task.IsClosed) 
							ReassignQueueTask(queueData, task, comment, true);
                        return false;
                    }
                case FAILURE:
                    rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_BLOCKED_BY_PARAMETER_CARD;
                    actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
                    comment = string.Format("{0} returned phone number blocked by parameter card.", task.DemographicsSource);
                    if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
                    {
                        if (!task.IsClosed) 
							CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, comment, true);
                    }
                    else
                    {
                        if (!task.IsClosed) 
							ReassignQueueTask(queueData, task, rejectReason, true);
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}