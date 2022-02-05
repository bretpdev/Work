using System;
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
	class BorrowerPathEmail : ComparePathBase
	{
		public BorrowerPathEmail(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, GeneralRecovery recovery, ErrorReport errorReport, ProcessLogRun logRun, DataAccess da, string userId, string scriptId)
			: base(ri, demographicsReviewQueue, foreignDemographicsQueue, recovery, errorReport, logRun, da, userId, scriptId)
		{
		}

		public override void Process(QueueTask task, QueueData queueData, bool pauseOnQueueClosingError)
		{
			//Recovery constants:
			const string COMMENT_INVALID_ADDRESS = "Added OneLINK comment for invalid address";
			const string QUEUE_INVALID_ADDRESS = "Created queue task for invalid address";

			//If there's recovery info, make sure it matches what's on the screen.
			if (GRecovery.Path != GeneralRecovery.ProcessingPath.None && (task.Demographics.AccountNumber != GRecovery.AccountNumber || GRecovery.Path != GeneralRecovery.ProcessingPath.Email))
			{
                string message = $"{RI.UserId}: It looks like the script didn't finish processing the previous borrower, but that borrower is no longer in the {queueData.Queue} queue. \n ";
					message += "Please check the log file to see where the processing was interrupted, finish out that borrower if needed, then delete the log file and restart the script. \n";
				Console.WriteLine(message);
				LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
				throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary
			}

			//Start at the appropriate point given the recovery value.
			string actionCode = task.GetRejectActionCode(RejectAction.RejectReasons.NONE, QueueTask.DemographicType.Email);
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.None)
				GRecovery.Phase = GeneralRecovery.ProcessingPhase.ReviewDemographics;
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.ReviewDemographics)
			{
				if (DemographicsPassReview(queueData, task, pauseOnQueueClosingError))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateOneLink;
				else
				{
					string rejectReason = RejectAction.RejectReasons.DEMOGRAPHICS_IS_INVALID;
					string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", task.Demographics.EmailAddress, "}");
					if (GRecovery.Step == COMMENT_INVALID_ADDRESS || GRecovery.Step == QUEUE_INVALID_ADDRESS || AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
					{
						if (GRecovery.Step.IsNullOrEmpty())
							GRecovery.Step = COMMENT_INVALID_ADDRESS;
						if (GRecovery.Step == QUEUE_INVALID_ADDRESS || CreateQueueTask(queueData, task, queueData.DemographicsReviewQueue, ""))
							GRecovery.Step = QUEUE_INVALID_ADDRESS;
						if (!task.IsClosed)
							CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, rejectReason, true);
					}
					else
					{
						if (!task.IsClosed) 
							ReassignQueueTask(queueData, task, rejectReason, true);
					}
					return;
				}
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.UpdateOneLink)
			{
				if (UpdateOneLink(queueData, task, pauseOnQueueClosingError))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.OneLinkComment;
				else
					return;
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.OneLinkComment)
			{
				string comment = string.Format("Accept demos from {0} {1}{2}{3}.", task.DemographicsSource, "{", task.Demographics.EmailAddress, "}");
				if (AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.CloseTask;
				else
				{
					if (!task.IsClosed) 
						ReassignQueueTask(queueData, task, comment, true);
					return;
				}
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.CloseTask)
			{
				if (!task.IsClosed) 
					CloseOrReassignQueueTask(queueData, task, pauseOnQueueClosingError, "Error closing task.", true);
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
				string comment = string.Format("Accept demos from {0} {1}{2}{3}.", task.DemographicsSource, "{", task.Demographics.EmailAddress, "}");
				AddCompassComment(queueData, task, actionCode, comment);
				GRecovery.Phase = GeneralRecovery.ProcessingPhase.Locate;
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.Locate)
				ProcessLocate(task);
		}

		/// <summary>
		/// Checks the email address associated with task to be valid.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		/// <param name="borrowerExistsOnOnelink"></param>
		/// <returns></returns>
		protected override bool DemographicsPassReview(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, bool borrowerExistsOnOnelink = true)
		{
			//"Practical" regular expression for e-mail format, from http://www.regular-expressions.info/email.html
			const string emailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
			return System.Text.RegularExpressions.Regex.IsMatch(task.Demographics.EmailAddress, emailPattern, RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// Updates the email in compass
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <returns></returns>
		protected override bool UpdateCompass(QueueData queueData, QueueTask task)
		{
			//Hit up TX1J to get the borrower's address.
			RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", task.Demographics.Ssn));
			if (RI.CheckForText(23, 2, "01080"))
			{
				RI.FastPath(string.Format("TX3Z/CTX1JS;{0}", task.Demographics.Ssn));
				if (RI.CheckForText(23, 2, "01019"))
					RI.FastPath(string.Format("TX3Z/CTX1JE;{0}", task.Demographics.Ssn));
			}
			if (RI.CheckForText(23, 2, "01019"))
				return false;

			//Go to the e-mail screen.
			RI.Hit(Key.F2);
			RI.Hit(Key.F10);

			//Enter the source code, address type, current date, and validity indicator.
			RI.PutText(9, 20, task.CompassSourceCode);
			RI.PutText(11, 17, DateTime.Now.ToString("MMddyy"));
			RI.PutText(12, 14, "Y");

			//Blank out all e-mail lines and enter the new e-mail address.
			RI.PutText(14, 10, "", true);
			RI.PutText(15, 10, "", true);
			RI.PutText(16, 10, "", true);
			RI.PutText(17, 10, "", true);
			RI.PutText(18, 10, "", true);
			RI.PutText(14, 10, task.Demographics.EmailAddress);
			RI.Hit(Key.Enter);
			if (RI.CheckForText(23, 2, "01005", "01004"))
				return true;
			else
			{
				if (!CreateQueueTask(queueData, task, queueData.DemographicsReviewQueue, ""))
				{
					Errors.AddRecord(task, "COMPASS not successfully updated.");
					LogRun.AddNotification($"{RI.UserId}: AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: { task.OriginalDemographicsText} ErrorReason: COMPASS not successfully updated.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
				}
				return false;
			}
		}

		/// <summary>
		/// Updates the email in One link.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		/// <returns></returns>
		protected override bool UpdateOneLink(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
		{
			RI.FastPath(string.Format("LP22C{0}", task.Demographics.Ssn));
			RI.PutText(3, 9, task.OneLinkSourceCode);
			RI.PutText(19, 9, task.Demographics.EmailAddress, true);
			RI.PutText(18, 56, "Y");
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
