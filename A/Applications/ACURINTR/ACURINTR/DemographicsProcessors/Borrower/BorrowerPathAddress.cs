using System;
using System.Text.RegularExpressions;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	class BorrowerPathAddress : ComparePathAddress
	{
		public BorrowerPathAddress(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData,string userId, string scriptId)
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
			const string COMMENT_MISSING_STREET = "Added OneLINK comment for missing street";
			const string COMMENT_INVALID_ADDRESS = "Added OneLINK comment for invalid address";
			const string QUEUE_MISSING_ADDRESS = "Created queue task to review missing address";
			const string COMMENT_MISSING_ADDRESS = "Added OneLINK comment for missing address";
			const string QUEUE_SHORT_STATE = "Created queue task to review one-letter state code";
			const string COMMENT_SHORT_STATE = "Added OneLINK comment for one-letter state code";
			const string QUEUE_FOREIGN_ADDRESS = "Created queue task to review foreign address";
			const string COMMENT_FOREIGN_ADDRESS = "Added OneLINK comment for foreign address";

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
            if (task.Demographics.Country.IsPopulated())
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

			if (task.Demographics.State.ToUpper() == "FC")
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

			return true;
		}

		/// <summary>
		/// Create a queue task in compass from the passed in values.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <returns>true is sucessful</returns>
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

			//Go to CTX1J for the legal address.
			RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", task.Demographics.Ssn));

            if (RI.CheckForText(23, 2, "01019")) 
				return false;
			if (RI.CheckForText(23, 2, "01080"))
			{
				RI.FastPath(string.Format("TX3Z/CTX1JS;{0}", task.Demographics.Ssn));
				if (RI.CheckForText(23, 2, "01019")) 
					return false;
				if (RI.CheckForText(23, 2, "01222"))
					RI.FastPath(string.Format("TX3Z/CTX1JE;{0}", task.Demographics.Ssn));
			}
			if (!RI.CheckForText(1, 71, "TXX"))
			{
				throw new Exception(RI.GetText(23, 2, 77));
			}
			//Go to the legal address if it's not already displayed.
			RI.Hit(Key.F6);
			RI.Hit(Key.F6);
			if (!RI.CheckForText(10, 14, "L")) 
				RI.PutText(10, 14, "L", Key.Enter);

			//Enter the source code, current date, and validity indicator.
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
	}
}
