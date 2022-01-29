using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using ACURINTR.DemographicsParsers;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	class PdemProcessor : ProcessorBase
	{
        public PdemProcessor(ReflectionInterface ri, string userId, string scriptId, ProcessLogData logData)
            : base(ri, userId, scriptId, logData) { }
		/// <summary>
		/// PdemProcessor entry point.
		/// </summary>
		public override void Process(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
		{
			GeneralObj = new General(RI, UserId, ScriptId, LogData);
			if (queueData.System == QueueData.Systems.COMPASS)
				ProcessCompassQueue(queueData, assemblyName, pauseOnQueueClosingError);
			else
				ProcessOneLinkQueue(queueData, assemblyName, pauseOnQueueClosingError);
		}

		protected override void ProcessApplicablePath(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
		{
			//PDEM records will only involve either address or phone (never both in the same record),
			//so process the appropriate path based on the task demographics.
			if (task.HasAddress)
			{
                PdemPathAddress addressProcessor = new PdemPathAddress(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				addressProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			else if (task.HasHomePhone || task.HasAltPhone)
			{
				PdemPathPhone phoneProcessor = new PdemPathPhone(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
		}

        /// <summary>
        /// Mark queue tasks as completed
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="assemblyName"></param>
        /// <param name="pauseOnQueueClosingError"></param>
		protected override void ProcessCompassQueue(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
		{
			//Create the parser object specified by the QueueData.
			string fullyQualifiedParser = string.Format("{0}.DemographicsParsers.{1}", assemblyName, queueData.Parser);
			IDemographicsParser parser = (IDemographicsParser)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { base.RI }, null, new Object[] { }).Unwrap();

            //<<Step 5>>
			//Loop through the queue until all tasks are handled.
			for (RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department)); !RI.CheckForText(23, 2, "01020"); RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department)))
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

					if (RI.CheckForText(row, 75, "U", "W"))
					{
						RI.PutText(21, 18, RI.GetText(row, 3, 2), Key.Enter, true);
						foundTask = RI.CheckForText(1, 72, "TXX1T");
						break;
					}
				}
				if (!foundTask)
					return;
				if (RI.CheckForText(23, 2, "01758"))
				{
					//No pending PDEM records. Close or reassign the task and go on to the next one.
					if (!GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError))
					{
                        //<<Step 6>>
						QueueTask pdem = parser.Parse();
						string managerId = DataAccess.LoanServicingManagerId(LogData);
						string comment = string.Format("Error closing {0} task.", queueData.Queue);
						GeneralObj.ReassignQueueTask(queueData, pdem, UserId, managerId, comment);
					}
					continue;
				}

				//Read each record from the queue task.
				List<QueueTask> pdemList = new List<QueueTask>();
				while (RI.CheckForText(23, 2, "01022"))
				{
                    //<<Step 6>>
					QueueTask pdem = parser.Parse();
					//The PDEM has the SSN but not the account number. The recovery table needs an account number,
					//so set it to the SSN for now, and we'll update it from TX1J after reading all PDEMs.
					pdem.Demographics.AccountNumber = pdem.Demographics.Ssn;
					//We also need a page number, which COMPASS PDEMs don't have. Use a counter instead.
					pdem.PageNumber = pdemList.Count + 1;
					//Add this PDEM to our list and reject it in the system. The next one will automatically appear.
					pdemList.Add(pdem);
					RI.Hit(Key.F4);
				}
				if (!RI.CheckForText(23, 2, "01867"))
				{
					//Reassign the task and move on to the next one.
					QueueTask pdem = pdemList.FirstOrDefault();
					if (pdem != null)
					{
						pdem = parser.Parse();
						RI.FastPath("TX3Z/ITX1J;" + pdem.Demographics.Ssn);
						pdem.Demographics.AccountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
					}
					if (!GeneralObj.ReassignQueueTask(queueData, pdem, UserId, DataAccess.LoanServicingManagerId(LogData), "Error closing task."))
					{
						throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
					}
					continue;
				}

				//The PDEM screen doesn't have the account number, so get it from TX1J and add it to the tasks.
				RI.FastPath("TX3Z/ITX1J;" + pdemList.First().Demographics.Ssn);
				string accountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
				foreach (QueueTask pdem in pdemList) 
					pdem.Demographics.AccountNumber = accountNumber;

                
				//Sort the demographics by verified date and process each one. <<Step 7>>
				pdemList = pdemList.OrderBy(p => p.PdemVerificationDate).ToList();
				foreach (QueueTask pdem in pdemList)
				{
					ProcessApplicablePath(queueData, pdem, pauseOnQueueClosingError);
				}
			}
		}

		/// <summary>
		/// Close out queue tasks in one link
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="assemblyName"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		protected override void ProcessOneLinkQueue(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
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
			IDemographicsParser parser = (IDemographicsParser)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { base.RI }, null, new Object[] { }).Unwrap();

			//Loop through the queue until all tasks are handled.
			while (RI.CheckForText(1, 77, "TASK"))
			{
				//Get the account number and SSN from the open task.
				string accountNumber = "";
				string ssn = "";
				switch (RI.GetText(1, 71, 5))
				{
					case "GROUP":
						accountNumber = RI.GetText(10, 8, 12).Replace(" ", "");
						ssn = RI.GetText(5, 70, 9);
						break;
					case "QUEUE":
						accountNumber = RI.GetText(17, 52, 12).Replace(" ", "");
						ssn = RI.GetText(17, 70, 9);
						break;
				}
				//Go to LP5F to get the PDEMs.
				RI.FastPath("LP5FC" + ssn);
				if (RI.CheckForText(22, 3, "48012"))
				{
					//Add an LP50 comment for "No pending PDEMs to review," close the task, and move on to the next one.
					RejectAction action = DataAccess.GetRejectActions(RejectAction.Sources.ONELINK_PENDING_PDEM).Where(p => p.RejectReason == RejectAction.RejectReasons.NO_PENDING_PDEM_RECORDS_TO_REVIEW).Single();
					GeneralObj.AddOneLinkComment(ssn, "AM", "10", action.ActionCodeAddress, action.RejectReason);
					if (!GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError))
					{
						//Reassign the task if it didn't close.
						RI.FastPath(string.Format("LP8YC{0};{1};{2};;;W", queueData.Department, queueData.Queue, UserId));
						RI.PutText(7, 33, "A");
						RI.PutText(7, 38, DataAccess.LoanServicingManagerId(LogData));
						RI.Hit(Key.F6);
					}
					RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
					continue;
				}

				//Read each record from the queue task.
				List<QueueTask> pdemList = new List<QueueTask>();
                try
                {
                    bool taskIsInvalid = false;
                    while (!RI.CheckForText(22, 3, "46004"))
                    {
                        QueueTask pdem = parser.Parse();
                        pdem.Demographics.AccountNumber = accountNumber;
                        //The database can't deal with dates before 1/1/1900, so check the verification date.
                        if (pdem.PdemVerificationDate < new DateTime(1900, 1, 1))
                        {
                            taskIsInvalid = true;
                            //Close this task and create a user review task.
                            while (!RI.CheckForText(22, 3, "46004"))
                            {
								RI.Hit(Key.F4);
								RI.Hit(Key.F6);
								RI.Hit(Key.F6);
								RI.Hit(Key.F8);
                            }
                            GeneralObj.CreateQueueTask(queueData, pdem, queueData.DemographicsReviewQueue, "");
                            break;
                        }
                        //Add this PDEM to our list and reject it in the system.
                        pdemList.Add(pdem);
						RI.Hit(Key.F4);
						RI.Hit(Key.F6);
						RI.Hit(Key.F6);
						RI.Hit(Key.F8);
                    }
                    if (taskIsInvalid)
                    {
                        GeneralObj.AddOneLinkComment(ssn, "AM", "10", "KGNRL", "Pending PDEM review in process");
                        GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);
						RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
                        continue;
                    }
                }
                catch (QueueTaskException ex)
                {
                    //Close the PDEM records, close the task, and move on to the next one.
                    while (!RI.CheckForText(22, 3, "46004"))
                    {
						RI.Hit(Key.F4);
						RI.Hit(Key.F6);
						RI.Hit(Key.F6);
						RI.Hit(Key.F8);
                    }
					RejectAction action = DataAccess.GetRejectActions(RejectAction.Sources.ONELINK_PENDING_PDEM).Where(p => p.RejectReason == RejectAction.RejectReasons.DEMOGRAPHICS_IS_INVALID).Single();
                    string comment = string.Format("{0} {1}", RejectAction.Sources.ONELINK_PENDING_PDEM, action.RejectReason);
                    GeneralObj.AddOneLinkComment(ex.AccountNumber, "AM", "10", action.ActionCodeAddress, comment);
                    GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);
                    RI.AddQueueTaskInLP9O(ex.AccountNumber, "KSKPREQ", null, ex.Message);  // Account number is the SSN
					RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
                    continue;
                }
                catch (ParseException)
                {
                    //Close the PDEM records, add an LP50 comment, close the task, and move on to the next one.
                    while (!RI.CheckForText(22, 3, "46004"))
                    {
						RI.Hit(Key.F4);
						RI.Hit(Key.F6);
						RI.Hit(Key.F6);
						RI.Hit(Key.F8);
                    }
					RejectAction action = DataAccess.GetRejectActions(RejectAction.Sources.ONELINK_PENDING_PDEM).Where(p => p.RejectReason == RejectAction.RejectReasons.DEMOGRAPHICS_IS_INVALID).Single();
                    string comment = string.Format("{0} {1}", RejectAction.Sources.ONELINK_PENDING_PDEM, action.RejectReason);
                    GeneralObj.AddOneLinkComment(ssn, "AM", "10", action.ActionCodeAddress, comment);
                    GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);
					RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
                    continue;
                }

				//Add an activity record and close the task.
				GeneralObj.AddOneLinkComment(ssn, "AM", "10", "KGNRL", "Pending PDEM review in process");
				GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);

				//Sort the demographics by verified date and process each one.
				pdemList = pdemList.OrderBy(p => p.PdemVerificationDate).ToList();
				foreach (QueueTask pdem in pdemList)
				{
					ProcessApplicablePath(queueData, pdem, pauseOnQueueClosingError);
				}
#if DEBUG
                return; //So task doesnt close out
#endif
				RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
			}
		}
	}
}
