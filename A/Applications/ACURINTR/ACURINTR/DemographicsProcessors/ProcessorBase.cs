using System;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using ACURINTR.DemographicsParsers;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	abstract class ProcessorBase
	{
		protected General GeneralObj { get; set; }
		protected ReflectionInterface RI { get; set; }
		protected string UserId { get; set; }
		protected string ScriptId { get; set; }
		protected ProcessLogData LogData { get; set; }

		public ProcessorBase(ReflectionInterface ri, string userId, string scriptId, ProcessLogData logData)
		{
			RI = ri;
			UserId = userId;
			ScriptId = scriptId;
			LogData = logData;
		}

		public abstract void Process(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError);

		protected abstract void ProcessApplicablePath(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError);

		//The following methods are designed to work appropriately for Compare and Borrower processing.
		//PDEM processing needs to override these methods to handle recovery and exceptions differently.
		protected virtual void ProcessCompassQueue(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
		{
			//Hit up the COMPASS queue task screen for the given queue/subqueue.
			RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department));

			//Create the parser object specified by the QueueData.
			string fullyQualifiedParser = string.Format("{0}.DemographicsParsers.{1}", assemblyName, queueData.Parser);
			IDemographicsParser parser = (IDemographicsParser)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { RI }, null, new Object[] { }).Unwrap();

			//Loop through the queue until all tasks are handled.
			while (!RI.CheckForText(23, 2, "01020"))
			{
				//Look through the tasks until we find one in "U" status or run out of tasks to look at.
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
						//Working status means either we're recovering from this task,
						//or we selected it the previous time through this loop (see "else if" block).
						//Go into it to get the demographics.
						RI.PutText(21, 18, RI.GetText(row, 3, 2), Key.F2, true);
						foundTask = RI.CheckForText(1, 72, "TXX6S");
					}
					else if (RI.CheckForText(row, 75, "U"))
					{
						//Select the task to put it in Working status, then back out and find the task again.
						RI.PutText(21, 18, RI.GetText(row, 3, 2), Key.Enter, true);
						RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department));
						row = 5; //Set to 3 less than the actual start row, because the loop will add 3.
					}
				}
				if (!foundTask)
					return;

				//Parse the demographics from the queue task.
				QueueTask task;
				try
				{
					task = parser.Parse();
				}
				catch (ParseException ex)
				{
					//Make a QueueTask object to pass to the ReassignQueueTask() method.
					task = new QueueTask(ex.DemographicsSource, ex.SystemSource);
					task.Demographics = new AccurintRDemographics();
					task.OriginalDemographicsText = ex.DemographicsText;
					task.Demographics.Ssn = ex.AccountNumber;
					if (task.Demographics.Ssn.IsNullOrEmpty())
						task.Demographics.Ssn = RI.GetText(17, 70, 9);
					if (task.Demographics.Ssn.IsNullOrEmpty())
						task.Demographics.Ssn = RI.GetText(12, 2, 9);

					//Reassign the task.
					string comment = "Could not decipher demographic information.";
					if (!GeneralObj.ReassignQueueTask(queueData, task, UserId, DataAccess.LoanServicingManagerId(LogData), comment))
					{
						throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
					}

					//Reset the recovery log and move on to the next task.
					RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department));
					continue;
				}

				//Tasks don't have the account number. Get it from TX1J.
				RI.FastPath("TX3Z/ITX1J;" + task.Demographics.Ssn);
				task.Demographics.AccountNumber = RI.GetText(3, 34, 12).Replace(" ", "");

				ProcessApplicablePath(queueData, task, pauseOnQueueClosingError);

				RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department));
			}
		}

		/// <summary>
		/// Work through one link queue tasks.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="assemblyName"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		protected virtual void ProcessOneLinkQueue(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
		{

			//Check that there are tasks to process.
			RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
			if (RI.CheckForText(22, 3, "47420", "47423", "47450"))
				return;
			if (!RI.CheckForText(1, 9, queueData.Queue))
			{
				string message = "You have another task open. Please complete that task first.";
				Console.WriteLine(message);
				ProcessLogger.AddNotification(LogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly());
				throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
			}

			//Create the parser object specified by the QueueData.
			string fullyQualifiedParser = string.Format("{0}.DemographicsParsers.{1}", assemblyName, queueData.Parser);
			IDemographicsParser parser = (IDemographicsParser)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { RI }, null, new Object[] { }).Unwrap();

			//Loop through the queue until all tasks are handled.
			QueueTask task;
			while (RI.CheckForText(1, 77, "TASK"))
			{
				try
				{
					task = parser.Parse();
				}
				catch (QueueTaskException ex)
				{
					//Add an activity record and close the task.
					SystemCode postOfficeCodes = DataAccess.SystemCodes().Where(p => p.Source == SystemCode.Sources.POST_OFFICE).Single();
					GeneralObj.AddOneLinkComment(ex.AccountNumber, postOfficeCodes.ActivityType, postOfficeCodes.ContactType, "KULSK", ex.Message);
					GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);

					//Reset the recovery log and move on to the next task.
					RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
					continue;
				}
				catch (ParseException ex)
				{
					//Make a QueueTask object to pass to the ReassignQueueTask() method.
					task = new QueueTask(ex.DemographicsSource, ex.SystemSource);
					task.OriginalDemographicsText = ex.DemographicsText;
					task.Demographics = new AccurintRDemographics();
					task.Demographics.Ssn = RI.GetText(17, 70, 9);

					//Reassign the task.
					string comment = "Could not decipher demographic information.";
					if (!GeneralObj.ReassignQueueTask(queueData, task, UserId, DataAccess.LoanServicingManagerId(LogData), comment))
					{
						throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
					}

					RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
					continue;
				}

				//See if the first or last name is set, which means the post office returned a possible alternate name.
				if (task.Demographics.FirstName.IsPopulated() || task.Demographics.LastName.IsPopulated())
				{
					string comment = string.Format("Post Office has possible alternate name for borrower {0} {1}", task.Demographics.FirstName, task.Demographics.LastName);
					if (!GeneralObj.AddOneLinkComment(task.Demographics.Ssn, "ET", "90", "K4AKA", comment))
					{
						ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.Demographics.AccountNumber, queueData.DemographicsReviewQueue, task.OriginalDemographicsText, "K4AKA comment not added to OneLINK."), NotificationType.ErrorReport, NotificationSeverityType.Critical);
					}
				}

				//Get the account number if we're dealing with a target SSN.
				if (task.Demographics.AccountNumber.IsNullOrEmpty())
					task.Demographics.AccountNumber = RI.GetDemographicsFromLP22(task.Demographics.Ssn).AccountNumber;

				ProcessApplicablePath(queueData, task, pauseOnQueueClosingError);

				RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
			}
		}
	}
}
