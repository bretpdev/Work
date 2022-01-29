using System;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class BorrowerPathPhone : ComparePathPhone
    {
        public BorrowerPathPhone(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userId, string scriptId)
            : base(ri, demographicsReviewQueue, foreignDemographicsQueue, logData, userId, scriptId)
        {
        }

        /// <summary>
        /// Borrower processing just needs to override any methods that compare the task demographics to the system demographics.
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

            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);

            //Check the phone number for error conditions.
            if (phoneNumber.Length > 10 || phoneNumber.StartsWith("011"))
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

            if (phoneNumber == "8015551212")
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

            return true;
        }

		/// <summary>
		/// Updates the phone number in compass
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <returns></returns>
        protected override bool UpdateCompass(QueueData queueData, QueueTask task)
        {
            //Determine which phone number we're dealing with.
            string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);

            //Go to CTX1J for the appropriate phone type.
			RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", task.Demographics.Ssn));
            if (RI.CheckForText(23, 2, "01080"))
            {
				RI.FastPath(string.Format("TX3Z/CTX1JS;{0}", task.Demographics.Ssn));
                if (RI.CheckForText(23, 2, "01019"))
					RI.FastPath(string.Format("TX3Z/CTX1JE;{0}", task.Demographics.Ssn));
            }
            if (!RI.CheckForText(1, 71, "TXX1R"))
            {
				throw new Exception(RI.GetText(23, 2, 77));
            }

            //Go to the correct phone type if it's not already displayed.
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
            string phoneTypeCode = "H";
            if (phoneNumber == task.Demographics.PrimaryPhone)
                phoneType = PhoneType.Home;
            else
                phoneType = PhoneType.Alternate;
            switch (phoneType)
            {
                case PhoneType.Alternate:
                    phoneTypeCode = "A";
                    break;
                case PhoneType.Work:
                    phoneTypeCode = "W";
                    break;
                case PhoneType.Mobile:
                    phoneTypeCode = "M";
                    break;
            }
			if (!RI.CheckForText(16, 14, phoneTypeCode)) 
				RI.PutText(16, 14, phoneTypeCode, Key.Enter);

            //Update the phone information.
			RI.PutText(16, 20, "U");
			RI.PutText(16, 30, "N");
			RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
			RI.PutText(17, 14, phoneNumber);
			RI.PutText(17, 54, "Y");
			if (phoneType == PhoneType.Home) 
				RI.PutText(17, 67, "", true);
			RI.PutText(19, 14, task.CompassSourceCode);
            if (RI.CheckForText(16, 78, "_"))
				RI.PutText(16, 78, "A");
			RI.Hit(Key.Enter);
            if (RI.CheckForText(23, 2, "01097", "01100"))
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
    }
}
