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
		public PdemPathPhone(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, PdemRecovery recovery, PdemErrorReport errorReport, ProcessLogRun logRun, DataAccess da, string userId, string scriptId)
			: base(ri, demographicsReviewQueue, foreignDemographicsQueue, recovery, errorReport, logRun, da, userId, scriptId)
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
			//If there's recovery info, make sure it matches the open task.
			if (GRecovery.Path != GeneralRecovery.ProcessingPath.None && (task.Demographics.AccountNumber != GRecovery.AccountNumber || GRecovery.Path != GeneralRecovery.ProcessingPath.HomePhone))
			{
                string message = $"{RI.UserId}: It looks like the script didn't finish processing the previous borrower, but that borrower is no longer in the {queueData.Queue} queue. \n ";
				message += "Please check the log file to see where the processing was interrupted, finish out that borrower if needed, then delete the log file and restart the script. \n";
				Console.WriteLine(message);
				LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
				throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
			}

			//Determine which phone number we're dealing with.
			string phoneNumber = (task.HasAltPhone ? task.Demographics.AlternatePhone : task.Demographics.PrimaryPhone);
			bool borrowerExistsOnOnelink = BorrowerExistsOnOnelink(task.Demographics.Ssn);

			//Start at the appropriate point given the recovery value.
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.None) { GRecovery.Phase = GeneralRecovery.ProcessingPhase.ReviewDemographics; }
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.ReviewDemographics)
			{
				if (DemographicsPassReview(queueData, task, pauseOnQueueClosingError, borrowerExistsOnOnelink))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateOneLink;
				else //The DemographicsPassReview() method takes care of LP50 and creating a user review task if needed, so our work here is done.
					return;
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.UpdateOneLink || GRecovery.Phase == GeneralRecovery.ProcessingPhase.OneLinkComment)
			{
				if (!borrowerExistsOnOnelink)
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateCompass;
				else if (UpdateOneLink(queueData, task, pauseOnQueueClosingError))
					GRecovery.Phase = GeneralRecovery.ProcessingPhase.UpdateCompass;
				else
				{
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
					return;
				}
			}
			if (GRecovery.Phase == GeneralRecovery.ProcessingPhase.UpdateCompass)
			{
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
		}

		/// <summary>
		/// Add comments to session if problems arise from object phone number
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		/// <param name="borrowerExistsOnOnelink"></param>
		/// <returns></returns>
		protected override bool DemographicsPassReview(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, bool borrowerExistsOnOnelink)
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

			if (task.Demographics.Ssn.Substring(0, 1) == "P")
			{
				ReassignQueueTask(queueData, task, "Task is for a reference", borrowerExistsOnOnelink);
				return false;
			}

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
						GRecovery.Step = COMMENT_FOREIGN_PHONE_NUMBER;
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

			if (phoneNumber.Length < 10)
			{
				string rejectReason = RejectAction.RejectReasons.RETURN_INCOMPLETE_PHONE_NUMBER;
				string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
				string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
				if (GRecovery.Step == COMMENT_INCOMPLETE_PHONE_NUMBER || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_INCOMPLETE_PHONE_NUMBER;
				else
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
				return false;
			}

			if (!phoneNumber.IsNumeric())
			{
				string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
				string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
				string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
				if (GRecovery.Step == COMMENT_NON_NUMERIC_PHONE_NUMBER || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_NON_NUMERIC_PHONE_NUMBER;
				else
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
				return false;
			}

			if (phoneNumber == "8015551212")
			{
				string rejectReason = RejectAction.RejectReasons.RETURN_INVALID_PHONE_NUMBER;
				string actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
				string comment = string.Format("{0} {1}{2}{3}.", rejectReason, "{", phoneNumber, "}");
				if (GRecovery.Step == COMMENT_INVALID_PHONE_NUMBER || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_INVALID_PHONE_NUMBER;
				else
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
				if (GRecovery.Step == COMMENT_PHONE_NUMBER_ON_FILE || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_PHONE_NUMBER_ON_FILE;
				else
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
				if (GRecovery.Step == COMMENT_PHONE_NUMBER_IN_HISTORY || borrowerExistsOnOnelink ? AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment) : AddCompassComment(queueData, task, actionCode, comment))
					GRecovery.Step = COMMENT_PHONE_NUMBER_IN_HISTORY;
				else
					CreateQueueTask(queueData, task, DemographicsReviewQueue, "");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Update the phone number in the session
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
					return AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment);
				case FAILURE:
					rejectReason = RejectAction.RejectReasons.PHONE_NUMBER_BLOCKED_BY_PARAMETER_CARD;
					actionCode = task.GetRejectActionCode(rejectReason, QueueTask.DemographicType.Phone);
					comment = string.Format("{0} returned phone number blocked by parameter card.", task.DemographicsSource);
					AddOneLinkComment(task.Demographics.Ssn, task.ActivityType, task.ContactType, actionCode, comment);
					return false;
				default:
					//This shouldn't be reached, but it's needed for the compiler to be happy.
					return false;
			}
		}
	}
}
