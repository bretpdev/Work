using System;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	abstract class ProcessingPathBase
	{
		protected string DemographicsReviewQueue { get; set; }
		protected string ForeignDemographicsQueue { get; set; }
		private readonly General General;
		protected ReflectionInterface RI { get; set; }
		private string ScriptId { get; set; }
		private string UserId { get; set; }
		protected ProcessLogData LogData { get; set; }

		public ProcessingPathBase(ReflectionInterface ri, string demographicsReviewQueue, string foreignDemographicsQueue, ProcessLogData logData, string userId, string scriptId)
		{
			RI = ri;
            UserId = userId;
			LogData = logData;
			ScriptId = scriptId;
			DemographicsReviewQueue = demographicsReviewQueue;
			ForeignDemographicsQueue = foreignDemographicsQueue;
			General = new General(ri, UserId, ScriptId, LogData);
		}

		/// <summary>
		/// drop a comment in a compass comment screen
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="arc"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		protected bool AddCompassComment(QueueData queueData, QueueTask task, string arc, string comment)
		{
            if(!RI.Atd22ByBalance(task.Demographics.Ssn, arc, comment,"", ScriptId, false, false))
            {
			    if(!RI.Atd37FirstLoan(task.Demographics.Ssn, arc, comment, ScriptId, UserId, null))
			    {
				    //Anything else is an error.
				    if (!CreateQueueTask(queueData, task, queueData.DemographicsReviewQueue, "Unable to add COMPASS comment"))
				    {
					    ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
				    }
                    return false;
			    }
            }
			return true;
		}

		/// <summary>
		/// drop a comment in one link
		/// </summary>
		/// <param name="ssn"></param>
		/// <param name="activityType"></param>
		/// <param name="contactType"></param>
		/// <param name="actionCode"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		protected bool AddOneLinkComment(string ssn, string activityType, string contactType, string actionCode, string comment)
		{
			return General.AddOneLinkComment(ssn, activityType, contactType, actionCode, comment);
		}

		/// <summary>
		/// If the task cannot be closed, it will be reassigned.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		/// <param name="commentWhenReassignFails"></param>
		protected void CloseOrReassignQueueTask(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError, string commentWhenReassignFails)
		{
			if (!General.CloseQueueTask(queueData, pauseOnQueueClosingError))
				ReassignQueueTask(queueData, task, commentWhenReassignFails);
			task.IsClosed = true;
		}

		/// <summary>
		/// Generate a new queue task.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="newQueue"></param>
		/// <param name="commentIntro"></param>
		/// <returns></returns>
		protected bool CreateQueueTask(QueueData queueData, QueueTask task, string newQueue, string commentIntro)
		{
			return General.CreateQueueTask(queueData, task, newQueue, commentIntro);
		}

		/// <summary>
		/// Determine if we are skipping phone, address or both.  Kill all skip tasks if the address/phone are valid.
		/// </summary>
		/// <param name="task"></param>
		protected void ProcessLocate(QueueTask task)
		{
			//Recovery constants:
			const string ADDED_COMMENT = "Added COMPASS comment for locate";

			//See if the borrower can be a locate based off the current validity indicators and dates.
			SystemBorrowerDemographics lp22Demographics = RI.GetDemographicsFromLP22(task.Demographics.Ssn);
			if ((!lp22Demographics.IsValidAddress || lp22Demographics.AddressValidityDate != DateTime.Now.ToString()) && (!lp22Demographics.IsPrimaryPhoneValid || lp22Demographics.PhoneValidityDate != DateTime.Now.ToString()))
				return;

				//See what kind of locate we have.
				string skipType = string.Empty;
				//From LP22, go to LP2J and select all historical records.
				if (RI.CheckForText(23, 20, "SET2"))
					RI.Hit(Key.F2);
				RI.Hit(Key.F5);
				RI.PutText(7, 51, "X", Key.Enter);
				//Loop through all the records that have today's date.
				for (int row = 7; RI.CheckForText(row, 34, DateTime.Now.ToString("MMddyyyy")) && !RI.CheckForText(22, 3, "46004"); row++)
				{
					if (row > 18)
					{
						RI.Hit(Key.F8);
						row = 7;
					}
					//Select the record.
					RI.PutText(21, 13, RI.GetText(row, 2, 2), Key.Enter);
					//Check the address validity indicator.
					if (lp22Demographics.IsValidAddress && lp22Demographics.AddressValidityDate == DateTime.Now.ToString() && RI.CheckForText(5, 80, "N"))
						skipType = (skipType.Length == 0 ? "A" : "B");
					//Check the phone validity indicator.
					if (lp22Demographics.IsPrimaryPhoneValid && lp22Demographics.PhoneValidityDate == DateTime.Now.ToString() && RI.CheckForText(8, 38, "N"))
						skipType = (skipType.Length == 0 ? "P" : "B");
					//We can stop looking if we've already found both.
					if (skipType == "B")
						break;
					//Go back for the next record.
					RI.Hit(Key.F12);
				}

				if (skipType.Length == 0)
					return;

				string comment = string.Format("Prev Skip Type: {0}, Locate Type: {1}", skipType, task.LocateType);
				RI.AddCommentInLP50(task.Demographics.Ssn, "SFNDM", ScriptId, "AM", "36", comment);

			//Cancel eligible queue tasks if the address and phone are both valid,
			//and the task isn't currently being worked.
			if (lp22Demographics.IsValidAddress && lp22Demographics.IsPrimaryPhoneValid)
			{
				RI.FastPath(string.Format("LP8YCSKP;;;{0}", task.Demographics.Ssn));
				if (!RI.CheckForText(22, 3, "47004"))
				{
					while (!RI.CheckForText(22, 3, "46004"))
					{
						//Mark all the eligible active queue tasks.
						for (int row = 7; !RI.CheckForText(row, 33, " "); row++)
						{
							if (RI.CheckForText(row, 33, "A") && DataAccess.LocateQueues().Contains(RI.GetText(row, 65, 8)))
							{
								RI.PutText(row, 33, "X");
							}
						}
						//Post the cancellations.
						RI.Hit(Key.F6);
						//Move on to the next page.
						RI.Hit(Key.F8);
					}
				}
			}
		}

		/// <summary>
		/// Reassign the task so that the script can continue to process.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="commentWhenReassignFails"></param>
		protected void ReassignQueueTask(QueueData queueData, QueueTask task, string commentWhenReassignFails)
		{
			if (!General.ReassignQueueTask(queueData, task, UserId, DataAccess.LoanServicingManagerId(LogData), commentWhenReassignFails))
			{
				throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
			}
			task.IsClosed = true;
		}
	}
}
