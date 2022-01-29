using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ACURINTR.DemographicsParsers;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
    class PdemProcessor : ProcessorBase
    {
        //Shadow the inherited ErrorReport and Recovery variables with ones specific to PDEM.
        private new PdemErrorReport ErrorReport;
        private readonly PdemRecovery Recovery;

        public PdemProcessor(ReflectionInterface ri, string userId, string scriptId, RecoveryLog recovery, ProcessLogRun logRun)
            : base(ri, userId, scriptId, recovery, logRun)
        {
            Recovery = new PdemRecovery(recovery, DA);
        }

        /// <summary>
        /// PdemProcessor entry point.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="assemblyName"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        public override void Process(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
        {
            ErrorReport = new PdemErrorReport(queueData.Queue, base.LogRun.ProcessLogId);
            GeneralObj = new General(RI, ErrorReport, UserId, ScriptId, LogRun, DA);
            if (Recovery.Queue.IsNullOrEmpty())
                Recovery.Queue = queueData.Queue;
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
                if (Recovery.Path == GeneralRecovery.ProcessingPath.None) { Recovery.Path = GeneralRecovery.ProcessingPath.Address; }
                PdemPathAddress addressProcessor = new PdemPathAddress(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, Recovery, ErrorReport, LogRun, DA, UserId, ScriptId);
                addressProcessor.Process(task, queueData, pauseOnQueueClosingError);
            }
            else if (task.HasHomePhone || task.HasAltPhone)
            {
                if (Recovery.Path == GeneralRecovery.ProcessingPath.None) { Recovery.Path = GeneralRecovery.ProcessingPath.HomePhone; }
                PdemPathPhone phoneProcessor = new PdemPathPhone(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, Recovery, ErrorReport, LogRun, DA, UserId, ScriptId);
                phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
            }
            Recovery.Delete(task);
        }

        /// <summary>
        /// Mark queue tasks as completed
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="assemblyName"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        protected override void ProcessCompassQueue(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
        {
            //Take care of recovery if needed.
            if (Recovery.AccountNumber.IsPopulated())
            {
                List<QueueTask> pdemList = DA.GetRecordsFromRecovery().OrderBy(p => p.PdemVerificationDate).ToList();
                if (pdemList.Count > 0)
                {
                    //The recovery records are stored either without an SSN or without an account number
                    //(in the case that the script didn't get as far as retrieving the account number),
                    //so hit up TX1J to get both and update each task accordingly.
                    RI.FastPath("TX3Z/ITX1J;" + pdemList.First().Demographics.AccountNumber);
                    string accountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
                    string ssn = RI.GetText(3, 12, 11).Replace(" ", "");

                    //Finish processing the recovery tasks.
                    foreach (QueueTask pdem in pdemList)
                    {
                        pdem.Demographics.AccountNumber = accountNumber;
                        pdem.Demographics.Ssn = ssn;
                        ProcessApplicablePath(queueData, pdem, pauseOnQueueClosingError);
                    }
                }
                Recovery.Delete();
                Recovery.Queue = queueData.Queue;
            }

            //Create the parser object specified by the QueueData.
            string fullyQualifiedParser = string.Format("{0}.DemographicsParsers.{1}", assemblyName, queueData.Parser);
            DemographicsParserBase parser = (DemographicsParserBase)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { base.RI, DA }, null, new Object[] { }).Unwrap();

            //<<Step 5>>
            //Loop through the queue until all tasks are handled.
            for (RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department)); !RI.CheckForText(23, 2, "01020"); RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department)))
            {
                string accountNumber = "";
                try
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
                    }//for
                    if (!foundTask)
                        return;
                    if (RI.CheckForText(23, 2, "01758"))
                    {
                        //No pending PDEM records. Close or reassign the task and go on to the next one.
                        if (!GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError))
                        {
                            //<<Step 6>>
                            QueueTask pdem = parser.Parse();
                            string managerId = DA.LppManagerId();
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
                        //Add each PDEM to the database immediately, because the task will close itself once they're all rejected.
                        DA.AddRecoveryRecord(pdem);
                        //We need to set the recovery file's account number, but can't use the SSN, so just make it a flag.
                        if (Recovery.AccountNumber.IsNullOrEmpty())
                            Recovery.AccountNumber = "1";
                        //Add this PDEM to our list and reject it in the system. The next one will automatically appear.
                        pdemList.Add(pdem);
                        RI.Hit(Key.F4);
                    }
                    if (!RI.CheckForText(23, 2, "01867"))
                    {
                        //Reassign the task and move on to the next one.
                        QueueTask pdem;
                        if (pdemList.Count > 0)
                        {
                            pdem = pdemList.First();
                        }
                        else
                        {
                            pdem = parser.Parse();
                            RI.FastPath("TX3Z/ITX1J;" + pdem.Demographics.Ssn);
                            pdem.Demographics.AccountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
                        }
                        if (!GeneralObj.ReassignQueueTask(queueData, pdem, UserId, DA.LppManagerId(), "Error closing task."))
                        {
                            Recovery.Delete();
                            throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
                        }
                        Recovery.Delete();
                        Recovery.Queue = queueData.Queue;
                        continue;
                    }

                    //The PDEM screen doesn't have the account number, so get it from TX1J and add it to the tasks.
                    RI.FastPath("TX3Z/ITX1J;" + pdemList.First().Demographics.Ssn);
                    accountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
                    DA.UpdatePdemRecoveryAccountNumber(pdemList.First().Demographics.Ssn, accountNumber);
                    Recovery.AccountNumber = accountNumber;
                    foreach (QueueTask pdem in pdemList)
                        pdem.Demographics.AccountNumber = accountNumber;


                    //Sort the demographics by verified date and process each one. <<Step 7>>
                    pdemList = pdemList.OrderBy(p => p.PdemVerificationDate).ToList();
                    foreach (QueueTask pdem in pdemList)
                    {
                        ProcessApplicablePath(queueData, pdem, pauseOnQueueClosingError);
                    }

                    //Clear the recovery log and move on to the next task.
                    Recovery.Delete();
                    Recovery.Queue = queueData.Queue;
                }
                catch (Exception ex)
                {
                    string message = $"{RI.UserId}: There was an error processing Queue: {queueData.Queue} for borrower: {accountNumber}";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
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
            //Take care of recovery if needed.
            if (Recovery.AccountNumber.IsPopulated())
            {
                List<QueueTask> pdemList = DA.GetRecordsFromRecovery().OrderBy(p => p.PdemVerificationDate).ToList();
                if (pdemList.Count > 0)
                {
                    //The recovery records are stored without an SSN, so get it from LP22.
                    string ssn = RI.GetDemographicsFromLP22(pdemList.First().Demographics.AccountNumber).Ssn;

                    //Finish processing the recovery tasks.
                    foreach (QueueTask pdem in pdemList)
                    {
                        pdem.Demographics.Ssn = ssn;
                        ProcessApplicablePath(queueData, pdem, pauseOnQueueClosingError);
                    }
                }
                Recovery.Delete();
                Recovery.Queue = queueData.Queue;
            }

            //Check that there are tasks to process.
            RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
            if (RI.CheckForText(22, 3, "47420", "47423", "47450"))
                return;
            //Create the parser object specified by the QueueData.
            string fullyQualifiedParser = string.Format("{0}.DemographicsParsers.{1}", assemblyName, queueData.Parser);
            DemographicsParserBase parser = (DemographicsParserBase)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { base.RI, DA }, null, new Object[] { }).Unwrap();
            if (!RI.CheckForText(1, 9, queueData.Queue))
            {
                string managerId = DA.LgpManagerId();
                string message = $"{RI.UserId}: You have another task open. An attempt will be made to reassign this task to {managerId}.";
                var pdem = parser.Parse();
                bool success = GeneralObj.ReassignQueueTask(queueData, pdem, UserId, managerId, message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                if (success)
                    RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
                if (!success && !RI.CheckForText(1, 9, queueData.Queue))
                {
                    message = $"Unable to reassign currently opened task";
                    Console.WriteLine(message);
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    throw new EarlyTerminationException();
                }
            }


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
                    RejectAction action = DA.GetRejectActions(RejectAction.Sources.ONELINK_PENDING_PDEM).Where(p => p.RejectReason == RejectAction.RejectReasons.NO_PENDING_PDEM_RECORDS_TO_REVIEW).Single();
                    GeneralObj.AddOneLinkComment(ssn, "AM", "10", action.ActionCodeAddress, action.RejectReason);
                    if (!GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError))
                    {
                        //Reassign the task if it didn't close.
                        RI.FastPath(string.Format("LP8YC{0};{1};{2};;;W", queueData.Department, queueData.Queue, UserId));
                        RI.PutText(7, 33, "A");
                        RI.PutText(7, 38, DA.LgpManagerId());
                        RI.Hit(Key.F6);
                    }
                    Recovery.Queue = queueData.Queue;
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
                        //Add each PDEM to the database immediately, because the task will close itself once they're all rejected.
                        DA.AddRecoveryRecord(pdem);
                        if (Recovery.AccountNumber.IsNullOrEmpty())
                            Recovery.AccountNumber = accountNumber;
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
                    RejectAction action = DA.GetRejectActions(RejectAction.Sources.ONELINK_PENDING_PDEM).Where(p => p.RejectReason == RejectAction.RejectReasons.DEMOGRAPHICS_IS_INVALID).Single();
                    string comment = string.Format("{0} {1}", RejectAction.Sources.ONELINK_PENDING_PDEM, action.RejectReason);
                    GeneralObj.AddOneLinkComment(ex.AccountNumber, "AM", "10", action.ActionCodeAddress, comment);
                    GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);
                    RI.AddQueueTaskInLP9O(ex.AccountNumber, "KSKPREQ", null, ex.Message);  // Account number is the SSN
                    Recovery.Delete();
                    Recovery.Queue = queueData.Queue;
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
                    RejectAction action = DA.GetRejectActions(RejectAction.Sources.ONELINK_PENDING_PDEM).Where(p => p.RejectReason == RejectAction.RejectReasons.DEMOGRAPHICS_IS_INVALID).Single();
                    string comment = string.Format("{0} {1}", RejectAction.Sources.ONELINK_PENDING_PDEM, action.RejectReason);
                    GeneralObj.AddOneLinkComment(ssn, "AM", "10", action.ActionCodeAddress, comment);
                    GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);
                    Recovery.Delete();
                    Recovery.Queue = queueData.Queue;
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
                //Reset the recovery log and move on to the next task.
                Recovery.Queue = queueData.Queue;
#if DEBUG
                return; //So task doesnt close out
#endif
                RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
            }
        }
    }
}
