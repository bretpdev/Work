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
        protected ErrorReport ErrorReport { get; set; }
        protected General GeneralObj { get; set; }
        protected GeneralRecovery GRecovery { get; set; }
        protected ReflectionInterface RI { get; set; }
        protected string UserId { get; set; }
        protected string ScriptId { get; set; }
        protected ProcessLogRun LogRun { get; set; }
        protected DataAccess DA { get; set; }

        public ProcessorBase(ReflectionInterface ri, string userId, string scriptId, RecoveryLog recovery, ProcessLogRun logRun)
        {
            RI = ri;
            UserId = userId;
            ScriptId = scriptId;
            LogRun = logRun;
            DA = new DataAccess(logRun);
            GRecovery = new GeneralRecovery(recovery, DA);
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
            DemographicsParserBase parser = (DemographicsParserBase)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { RI, DA }, null, new Object[] { }).Unwrap();

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

                if (GRecovery.Queue.IsNullOrEmpty())
                    GRecovery.Queue = queueData.Queue;

                //Parse the demographics from the queue task.
                QueueTask task;
                try
                {
                    task = parser.Parse();
                }
                catch (ParseException ex)
                {
                    //Make a QueueTask object to pass to the ReassignQueueTask() method.
                    task = new QueueTask(ex.DemographicsSource, ex.SystemSource, DA);
                    task.Demographics = new AccurintRDemographics();
                    task.OriginalDemographicsText = ex.DemographicsText;
                    task.Demographics.Ssn = ex.AccountNumber;
                    if (task.Demographics.Ssn.IsNullOrEmpty())
                        task.Demographics.Ssn = RI.GetText(17, 70, 9);
                    if (task.Demographics.Ssn.IsNullOrEmpty())
                        task.Demographics.Ssn = RI.GetText(12, 2, 9);

                    //Reassign the task.
                    string comment = "Could not decipher demographic information.";
                    if (!GeneralObj.ReassignQueueTask(queueData, task, UserId, DA.LppManagerId(), comment))
                    {
                        GRecovery.Queue = queueData.Queue;
                        throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
                    }

                    //Reset the recovery log and move on to the next task.
                    GRecovery.Queue = queueData.Queue;
                    RI.FastPath(string.Format("TX3Z/ITX6X{0};{1}", queueData.Queue, queueData.Department));
                    continue;
                }

                //Tasks don't have the account number. Get it from TX1J.
                RI.FastPath("TX3Z/ITX1J;" + task.Demographics.Ssn);
                task.Demographics.AccountNumber = RI.GetText(3, 34, 12).Replace(" ", "");
                if (GRecovery.AccountNumber.IsNullOrEmpty())
                    GRecovery.AccountNumber = task.Demographics.AccountNumber;

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
            //If we're in recovery, there's a chance that we've already closed the queue task we were working on,
            //so don't go to LP9AC yet. Load the task from the database and finish it.
            if (GRecovery.AccountNumber.IsPopulated())
            {
                QueueTask recoveryTask = DA.GetRecordsFromRecovery().SingleOrDefault();
                if (recoveryTask != null)
                {
                    recoveryTask.Demographics.Ssn = RI.GetDemographicsFromLP22(recoveryTask.Demographics.AccountNumber).Ssn;
                    ProcessApplicablePath(queueData, recoveryTask, pauseOnQueueClosingError);
                }
            }

            //Create the parser object specified by the QueueData.
            string fullyQualifiedParser = string.Format("{0}.DemographicsParsers.{1}", assemblyName, queueData.Parser);
            DemographicsParserBase parser = (DemographicsParserBase)Activator.CreateInstance(assemblyName, fullyQualifiedParser, false, BindingFlags.Default, null, new Object[] { RI, DA }, null, new Object[] { }).Unwrap();


            //Check that there are tasks to process.
            RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
            if (RI.CheckForText(22, 3, "47420", "47423", "47450"))
                return;
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


            if (GRecovery.Queue.IsNullOrEmpty())
                GRecovery.Queue = queueData.Queue;

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
                    SystemCode postOfficeCodes = DA.SystemCodes().Where(p => p.Source == SystemCode.Sources.POST_OFFICE).Single();
                    GeneralObj.AddOneLinkComment(ex.AccountNumber, postOfficeCodes.ActivityType, postOfficeCodes.ContactType, "KULSK", ex.Message);
                    GeneralObj.CloseQueueTask(queueData, pauseOnQueueClosingError);

                    //Reset the recovery log and move on to the next task.
                    GRecovery.Queue = queueData.Queue;
                    RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
                    continue;
                }
                catch (ParseException ex)
                {
                    //Make a QueueTask object to pass to the ReassignQueueTask() method.
                    task = new QueueTask(ex.DemographicsSource, ex.SystemSource, DA);
                    task.OriginalDemographicsText = ex.DemographicsText;
                    task.Demographics = new AccurintRDemographics();
                    task.Demographics.Ssn = RI.GetText(17, 70, 9);

                    //Reassign the task.
                    string comment = "Could not decipher demographic information.";
                    if (!GeneralObj.ReassignQueueTask(queueData, task, UserId, DA.LgpManagerId(), comment))
                    {
                        GRecovery.Delete();
                        throw new EarlyTerminationException(); //TODO: refactor logic to make exitting via exception unnecessary; 
                    }

                    //Reset the recovery log and move on to the next task.
                    GRecovery.Queue = queueData.Queue;
                    RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
                    continue;
                }

                //See if the first or last name is set, which means the post office returned a possible alternate name.
                if (task.Demographics.FirstName.IsPopulated() || task.Demographics.LastName.IsPopulated())
                {
                    string comment = string.Format("Post Office has possible alternate name for borrower {0} {1}", task.Demographics.FirstName, task.Demographics.LastName);
                    if (!GeneralObj.AddOneLinkComment(task.Demographics.Ssn, "ET", "90", "K4AKA", comment))
                    {
                        ErrorReport.AddRecord(task, "K4AKA comment not added to OneLINK.");
                        LogRun.AddNotification($"{RI.UserId}: AccountNumber: {task.Demographics.AccountNumber} QueueName: {queueData.DemographicsReviewQueue} CapturedDemographics: {task.OriginalDemographicsText} ErrorReason: K4AKA comment not added to OneLINK.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }

                //Get the account number if we're dealing with a target SSN.
                if (task.Demographics.AccountNumber.IsNullOrEmpty())
                    task.Demographics.AccountNumber = RI.GetDemographicsFromLP22(task.Demographics.Ssn).AccountNumber;

                DA.AddRecoveryRecord(task);
                if (GRecovery.AccountNumber.IsNullOrEmpty())
                    GRecovery.AccountNumber = task.Demographics.AccountNumber;
                ProcessApplicablePath(queueData, task, pauseOnQueueClosingError);

                RI.FastPath(string.Format("LP9AC{0};;{1}", queueData.Queue, queueData.Department));
            }
        }
    }
}
