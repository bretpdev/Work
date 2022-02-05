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
		public PdemPathAddress(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, PdemRecovery recovery, PdemErrorReport errorReport, ProcessLogRun logRun, DataAccess da, string userid, string scriptId)
			: base(ri, demographicsReviewQueue, foreignDemographicsQueue, recovery, errorReport, logRun, da, userid, scriptId)
		{
		}
		/// <summary>
		/// Compare the address in the seesion to our object address.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="queueData"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
		{
			//If there's recovery info, make sure it matches what's on the screen.
			if (GRecovery.Path != GeneralRecovery.ProcessingPath.None && (task.Demographics.AccountNumber != GRecovery.AccountNumber || GRecovery.Path != GeneralRecovery.ProcessingPath.Address))
			{
                string message = $"{RI.UserId}: It looks like the script didn't finish processing the previous borrower, but that borrower is no longer in the {queueData.Queue} queue. \n ";
				message += "Please check the log file to see where the processing was interrupted, finish out that borrower if needed, then delete the log file and restart the script. \n";
				Console.WriteLine(message);
				LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
				throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
			}

			bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);

			//Start at the appropriate point given the recovery value.
			string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Address);
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.None) { GRecovery.Phase = GeneralRecovery.ProcessingPhase.ReviewDemographics; }
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.ReviewDemographics)
			{
				if (DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateOneLink;
				else //The DemographicsPassReview() method takes care of LP50 and creating a user review task if needed, so our work here is done.
					return;
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.UpdateOneLink)
			{
				if (!borrowerExistsOnOnelink)
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateCompass;
				else if (UpdateOneLink(queueData, task, pauseOnQueueClosingError))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.OneLinkComment;
				else
				{
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
					return;
				}
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.OneLinkComment)
			{
				string comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
				if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateCompass;
				else
				{
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
					return;
				}
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
				string comment = string.Format("Accept demos from {0} {1}.", task.DemographicsSource, General.FormatAddressForComments(task.Demographics));
				AddCompassComment(queueData, task, actionCode, comment);
				GRecovery.Phase = GeneralRecovery.ProcessingPhase.Locate;
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.Locate && borrowerExistsOnOnelink)
				ProcessLocate(task);
		}

		/// <summary>
		/// Leave comments in session for bad data from our input.
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

			if (task.Demographics.Ssn.Substring(0, 1) == "P")
			{
				ReassignQueueTask(queueData, task, "Task is for a reference", borrowerExistsOnOnelink);
				return false;
			}

			//Check the address for problem conditions.
			if (task.Demographics.Address1.IsNullOrEmpty())
			{
				string rejectReason = RejectAction.RejectReasons.RETURN_ADDRESS_WITHOUT_STREET;
				string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
				string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
				if (GRecovery.Step == COMMENT_MISSING_STREET || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_MISSING_STREET;
				else
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");

				return false;
			}
			if (GRecovery.Step == COMMENT_INVALID_ADDRESS || task.Demographics.Address1.ToUpper() == "GENERAL DELIVERY" || (task.Demographics.Address2.IsPopulated() && task.Demographics.Address2.ToUpper() == "GENERAL DELIVERY"))
			{
				string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_ADDRESS;
				string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
				string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
				if (GRecovery.Step == COMMENT_INVALID_ADDRESS || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_INVALID_ADDRESS;
				else
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");

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
						GRecovery.Step = COMMENT_MISSING_ADDRESS;
					else
						CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
				}
				else
				{
					Errors.AddRecord(task, "Could not create user queue task.");
					LogRun.AddNotification($"{RI.UserId}: AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: Could not create user queue task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
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
					string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
					if (GRecovery.Step == COMMENT_SHORT_STATE || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
						GRecovery.Step = COMMENT_SHORT_STATE;
					else
						CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
				}
				else
				{
					Errors.AddRecord(task, "Could not create user queue task.");
					LogRun.AddNotification($"{RI.UserId}: AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: { task.OriginalDemographicsText} ErrorReason: Could not create user queue task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
				}
				return false;
			}
			if (task.Demographics.State.ToUpper() == "FC")
			{
				string rejectReason = RejectAction.RejectReasons.RETURN_FOREIGN_DEMOGRAPHICS;
				if (GRecovery.Step == QUEUE_FOREIGN_ADDRESS || CreateQueueTask(queueData, task, ForeignDemographicsQueue, ""))
				{
					GRecovery.Step = QUEUE_FOREIGN_ADDRESS;
					string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
					string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
					if (GRecovery.Step == COMMENT_FOREIGN_ADDRESS || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
						GRecovery.Step = COMMENT_FOREIGN_ADDRESS;
					else
						CreateQueueTask(queueData, task, ForeignDemographicsQueue, "");
				}
				else
				{
					Errors.AddRecord(task, "Could not create user queue task.");
					LogRun.AddNotification($"{RI.UserId}: AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: Could not create user queue task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
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
			if (GRecovery.Step == COMMENT_ADDRESS_ON_FILE || borrowerExistsOnOnelink ? AddressMatchesOneLink(task.Demographics) : AddressMatchesCompass(task.Demographics))
			{
				string rejectReason = RejectAction.RejectReasons.ADDRESS_SAME_AS_ON_FILE;
				string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
				string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
				if (GRecovery.Step == COMMENT_ADDRESS_ON_FILE || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_ADDRESS_ON_FILE;
				else
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
				return false;
			}
			if (GRecovery.Step == COMMENT_ADDRESS_IN_HISTORY || borrowerExistsOnOnelink ? AddressIsInvalidInOneLinkWithinPastYear(task.Demographics) : CompareAddressHistory(task))
			{
				string rejectReason = RejectAction.RejectReasons.ADDRESS_INVALID_IN_HISTORY;
				string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Address);
				string comment = string.Format("{0} {1}.", rejectReason, General.FormatAddressForComments(task.Demographics));
				if (GRecovery.Step == COMMENT_ADDRESS_IN_HISTORY || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_ADDRESS_IN_HISTORY;
				else
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
