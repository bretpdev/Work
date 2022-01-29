using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using ACURINTR.DemographicsProcessors;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using Uheaa.Common.DataAccess;

namespace ACURINTR
{
	class General
	{
		private ErrorReport ErrorReport { get; set; }
		private ReflectionInterface RI { get; set; }
		private string UserId { get; set; }
		private string ScriptId { get; set; }
		private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
		/// <summary>
		/// Contains code that is used by different classes that don't share an inheritance hierarchy.
		/// </summary>
		/// <param name="ri">The instance of ReflectionInterface used by the script.</param>
		public General(ReflectionInterface ri, ErrorReport errorReport, string userId, string scriptId, ProcessLogRun logRun, DataAccess da)
		{
			RI = ri;
			ErrorReport = errorReport;
			ScriptId = scriptId;
			LogRun = logRun;
            UserId = userId;
            DA = da;
		}

		/// <summary>
		/// Adds an activity record in LP50, using the activity type and contact type from the QueueData.
		/// </summary>
		/// <param name="queueData">
		/// QueueData object containing details about the queue currently being worked.
		/// At a minimum, the ActivityType and ContactType properties are required to be set.
		/// </param>
		/// <param name="ssn">SSN for the borrower in the queue task being worked.</param>
		/// <param name="actionCode">Action code for the LP50 record.</param>
		/// <param name="comment">Text for the activity comment.</param>
		public bool AddOneLinkComment(string ssn, string activityType, string contactType, string actionCode, string comment)
		{
			//OneLINK seems to need a two-second pause in between opening the task and adding the comment.
			Thread.Sleep(2000);
			RI.FastPath(string.Format("LP50A{0};;;;;{1}", ssn, actionCode));
			RI.PutText(7, 2, activityType);
			RI.PutText(7, 5, contactType);
			RI.PutText(13, 2, comment);
			RI.Hit(Key.F6);
			return RI.CheckForText(22, 3, "48003", "48081");
		}

		/// <summary>
		/// Returns the system to the queue task screen and closes the open queue task.
		/// </summary>
		/// <param name="queueData">QueueData object holding details for the queue currently being worked.</param>
		/// <param name="pauseOnQueueClosingError">
		/// True if the script should pause and let the user adjust the system if the task does not close.
		/// </param>
        public bool CloseQueueTask(QueueData queueData, bool pauseOnQueueClosingError)
        {
            bool reallyClose = true;
            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
            {
                var result = Dialog.Info.YesNoCancel("About to close queue task.  Press YES to continue, NO to skip queue task closing and move on, and CANCEL to abort this script entirely.");
                reallyClose = result ?? true;
                if (result == null)
                    throw new EndDLLException();
            }
            if (reallyClose)
            {
                //Close the task in the appropriate system.
                if (queueData.System == QueueData.Systems.ONELINK)
                    return CloseOneLinkQueueTask(pauseOnQueueClosingError);
                else
                    return CloseCompassQueueTask(queueData, pauseOnQueueClosingError);
            }
            return true;
        }

		/// <summary>
		/// Creates a new queue task in the same system from which the current task is being worked,
		/// using the current task's source and demographics as comment text.
		/// </summary>
		/// <param name="queueData">
		/// QueueData object containing details about the queue currently being worked.
		/// At a minimum, the System property is required to be set.
		/// </param>
		/// <param name="task">
		/// QueueTask object containing the details of the queue task currently being worked.
		/// At a minimum, the Source, OriginalDemographicsText, and Demographics.Ssn properties are required to be set.
		/// </param>
		/// <param name="newQueue">The name of the queue in which to create a task.</param>
		public bool CreateQueueTask(QueueData queueData, QueueTask task, string newQueue, string commentIntro)
		{
            if (queueData.System == QueueData.Systems.ONELINK)
            {
                if (DA.ValidTask(task.Demographics.AccountNumber))
                    return CreateOneLinkTask(task, newQueue, commentIntro);
                else
                    return true;
            }
            else
                return CreateCompassTask(task, commentIntro);
		}

		/// <summary>
		/// Gets the index of the last non-blank character before the user ID or script ID (whichever comes first).
		/// If the text doesn't contain a user ID or script ID, the index of the last character is returned.
		/// </summary>
		/// <param name="commentText">The entirety of the text from which the index of the end of the comment is needed.</param>
		public static int FindEndOfCommentText(string commentText)
		{
			//Check for an empty argument.
			if (commentText.IsNullOrEmpty()) 
				return 0;

			//The comment text should end with a user ID in parentheses, but may also end
			//with a script ID in curly brackets. Find where the first such ID starts.
			int commentIndex = commentText.Length - 1;
			if (commentText.Contains("(") && commentText.Contains("{"))
			{
				//Both user ID and script ID are there. Find the start of whichever one comes first.
				if (commentText.LastIndexOf('(') < commentText.LastIndexOf('{'))
					commentIndex = commentText.LastIndexOf('(');
				else
					commentIndex = commentText.LastIndexOf('{');
			}
			else if (commentText.Contains("(")) //Only the user ID is there.	
				commentIndex = commentText.LastIndexOf('(');
			else if (commentText.Contains("{"))//Only the script ID is there.
				commentIndex = commentText.LastIndexOf('{');

			//Back the comment index pointer up past any blank spaces or underscores.
			commentIndex--;
			while (commentText[commentIndex] == ' ' && commentIndex > 0) 
				commentIndex--;
			return commentIndex;
		}

		public static string FormatAddressForComments(AccurintRDemographics demographics)
		{
			return string.Format("{0}{1}, {2}, {3}, {4}, {5}{6}", "{", demographics.Address1, demographics.Address2 ?? "", demographics.City, demographics.State, demographics.ZipCode, "}");
		}

		/// <summary>
		/// Reassigns the queue task currently being worked to another user.
		/// </summary>
		/// <param name="queueData">
		/// QueueData object containing details about the queue currently being worked.
		/// At a minimum, the System, Queue, and Department properties are required to be set.
		/// COMPASS will use the Department property for the sub-queue.
		/// </param>
		/// <param name="task">
		/// QueueTask object containing the details of the queue task currently being worked.
		/// At a minimum, the Demographics.Ssn property is required to be set.
		/// </param>
		/// <param name="fromUserId">The ID of the user currently working the task.</param>
		/// <param name="toUserId">The ID of the user to which the task should be assigned.</param>
		/// <param name="comment">
		/// The comment text to include in the task (for COMPASS queues) or activity record (for OneLINK queues).
		/// In the case of failure to reassign the task, this comment will be used in the error report.
		/// </param>
		public bool ReassignQueueTask(QueueData queueData, QueueTask task, string fromUserId, string toUserId, string comment)
		{
			//Reassign the task in the appropriate system.
			if (queueData.System == QueueData.Systems.ONELINK)
				return ReassignOneLinkTask(queueData, task, fromUserId, toUserId, comment);
			else
				return ReassignCompassTask(queueData, task, fromUserId, toUserId, comment);
		}

		private bool CloseCompassQueueTask(QueueData queueData, bool pauseOnQueueClosingError)
		{
			//Go to the COMPASS queue screen for the given queue.
			RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department));
			//Select the task that's in Working status.
			bool foundTask = false;
			for (int row = 8; !RI.CheckForText(23, 2, "90007") && !foundTask; row += 3)
			{
				if (row > 17)
				{
					RI.Hit(Key.F8);
					row = 8;
				}

				if (RI.CheckForText(row, 75, "W"))
				{
					RI.PutText(21, 18, RI.GetText(row, 3, 2), Key.F2, true);
					foundTask = RI.CheckForText(1, 72, "TXX6S");
				}
			}
			if (!foundTask)
				return false;
			//Mark the task as Complete.
			RI.PutText(8, 19, "C");
			RI.PutText(9, 19, "COMPL");
			RI.Hit(Key.Enter);
			if (RI.CheckForText(23, 2, "01644"))
				RI.PutText(9, 19, "", Key.Enter, true);
			return RI.CheckForText(23, 2, "01005");
		}

		/// <summary>
		/// Close a task
		/// </summary>
		/// <param name="pauseOnQueueClosingError"></param>
		/// <returns></returns>
		private bool CloseOneLinkQueueTask(bool pauseOnQueueClosingError)
		{
			RI.FastPath("LP9AC");
			RI.Hit(Key.F6);
			if (RI.CheckForText(22, 3, "49000"))
				return true;
			return false;
		}

		/// <summary>
		/// C reate a task in compass
		/// </summary>
		/// <param name="task"></param>
		/// <param name="commentIntro"></param>
		/// <returns></returns>
		private bool CreateCompassTask(QueueTask task, string commentIntro)
        {
            string comment = string.Format("{0} {1}", task.DemographicsSource, task.OriginalDemographicsText);
			if (commentIntro.IsPopulated())
				comment = string.Format("{0}: {1}", commentIntro, comment);
            if (!RI.Atd22AllLoans(task.Demographics.Ssn, "MUBFD", comment, null, ScriptId, false))
            {
                if (!RI.Atd37FirstLoan(task.Demographics.Ssn, "MUBFD", comment, ScriptId, UserId, null))
                    return false;
            }
            return true;
        }
		/// <summary>
		/// Create a one link task
		/// </summary>
		/// <param name="task"></param>
		/// <param name="newQueue"></param>
		/// <param name="commentIntro"></param>
		/// <returns></returns>
		private bool CreateOneLinkTask(QueueTask task, string newQueue, string commentIntro)
		{
			string comment = string.Format("{0} {1}", task.DemographicsSource, task.OriginalDemographicsText);
			if (commentIntro.IsPopulated())
				comment = string.Format("{0}: {1}", commentIntro, comment);
			RI.FastPath(string.Format("LP9OA{0};;{1}", task.Demographics.Ssn, newQueue));
			RI.PutText(11, 25, DateTime.Now.ToString("MMddyyyy"));
			RI.PutText(16, 12, comment);
			RI.Hit(Key.F6);
			return RI.CheckForText(22, 3, "48003");
		}

		/// <summary>
		/// Move a Compass task to toUserId
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="fromUserId"></param>
		/// <param name="toUserId"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		private bool ReassignCompassTask(QueueData queueData, QueueTask task, string fromUserId, string toUserId, string comment)
        {
            //Open up the task we're currently working.
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, queueData.Queue);
            RI.PutText(8, 42, queueData.Department);
			RI.PutText(9, 42, "", true);
            RI.PutText(13, 42, fromUserId);
            RI.Hit(Key.Enter);
            if (RI.MessageCode == "01020") //The task was already worked and is no longer open and cannot be re-assigned
                return true;
            //Assign it to someone else.
            RI.PutText(8, 15, toUserId);
            RI.Hit(Key.Enter);
            if (!RI.CheckForText(23, 2, "01005"))
            {
				string message = string.Format("Error working task in {0}{1}. Re-start the script to finish processing after the error has been resolved.", queueData.Queue, queueData.Department);
				Console.WriteLine(message);
				LogRun.AddNotification( message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                
				return false;
            }

            if (!RI.Atd22AllLoans(task.Demographics.Ssn, "KNOTE", comment, null, ScriptId, false))
            {
				if (!RI.Atd37FirstLoan(task.Demographics.Ssn, "KNOTE", comment, ScriptId, UserId, null))
				{
					ErrorReport.AddRecord(task, comment);
					LogRun.AddNotification($"AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: {comment}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
				}
            }

			return true;
        }

		/// <summary>
		/// Move task to toUserId
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="task"></param>
		/// <param name="fromUserId"></param>
		/// <param name="toUserId"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		private bool ReassignOneLinkTask(QueueData queueData, QueueTask task, string fromUserId, string toUserId, string comment)
		{
			//Get a user's currently working task and assign it to another user.
			RI.FastPath(string.Format("LP8YC{0};{1};{2};;;W", queueData.Department, queueData.Queue, fromUserId));
			if (RI.CheckForText(22, 3, "47004"))
			{
				string message = string.Format("No queue task found assigned to user for queue {0} , {1}", queueData.Queue, queueData.Department);
				Console.WriteLine(message);
				return false;
			}
			RI.PutText(7, 33, "A");
			RI.PutText(7, 38, toUserId);
			RI.Hit(Key.F6);
			if (!RI.CheckForText(22, 3, "47450"))
			{
				string message = $"Error working task in {queueData.Queue}{queueData.Department}. Re-start the script to finish processing after the error has been resolved.  " + RI.Message;
				Console.WriteLine(message);
				LogRun.AddNotification( message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
				return false;
			}

			if (!RI.AddCommentInLP50(task.Demographics.Ssn, "AM", "10", "KGNRL", comment, ScriptId))
			{
				ErrorReport.AddRecord(task, comment);
				LogRun.AddNotification($"AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: {comment}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
			}

			return true;
		}
	}
}
