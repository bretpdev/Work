using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static QUECOMPLET.ProcessingResults;
using static System.Console;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace QUECOMPLET
{
    class QueueProcessor
    {
        private readonly object LogLock = new object();
        private ProcessLogRun PLR { get; set; }

        private DataAccess DA { get; set; }
        public QueueProcessor()
        {
            PLR = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, Program.ApplicationArgs.ShowPrompts, false, true);
            DA = new DataAccess(PLR.LDA);
        }

        public int StartProcessing()
        {
            List<ProcessingResult> returnReasons = new List<ProcessingResult>() { ProcessingResult.Success }; // Start as a successful run 
            WriteLine("Cleaning up unprocessed tasks from previous run.");
            DA.CleanUpRecords();//Mark records as un-processed if they have not been completed.
            int tasks = DA.GetProcessingCount();
            try
            {
                if (tasks > 0)//Do not do anything if there is nothing to process.
                {
                    WriteLine($"Spinning up {Program.ApplicationArgs.NumberOfThreads} thread(s) to handle {tasks} task(s).");
                    BatchProcessingHelper userids;
                    Parallel.For(0, Program.ApplicationArgs.NumberOfThreads, index =>
                    {
                        ReflectionInterface ri = new ReflectionInterface();
                        WriteLine($"Logging into the session for ThreadId:{Thread.CurrentThread.ManagedThreadId}");

                        userids = BatchProcessingLoginHelper.Login(PLR, ri, "QUECOMPLET", "BatchUheaa");

                        if (userids == null)
                        {
                            ri.CloseSession();
                            return;
                        }

                        WriteLine($"Starting to process tasks with UT ID {userids.UserName} on thread ThreadId:{Thread.CurrentThread.ManagedThreadId}");
                        returnReasons.Add(BeginProcessing(ri, userids.UserName, Thread.CurrentThread.ManagedThreadId));
                        WriteLine($"Finished processings tasks with UT ID {userids.UserName} on thread ThreadId:{Thread.CurrentThread.ManagedThreadId}");
                        ri.CloseSession();
                    });
                }
            }
            catch (Exception ex)
            {
                PLR.AddNotification($"There was an error processing the QUECOMPLET script. EX: {ex}", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return 1;
            }

            WriteLine("Finished processing tasks. Ending script run.");
            PLR.LogEnd();

            return 0;
        }

        private ProcessingResult BeginProcessing(ReflectionInterface ri, string userId, int threadNum)
        {
            QueueData queueInfo;
            ProcessingResult result = ProcessingResult.Success;
            while ((queueInfo = DA.GetNextQueue()) != null) //Retrieve next queue task to close and check result
            {
                LogToConsole(queueInfo, threadNum);
                ProcessingResult processingResult;
                if ((processingResult = AssignQueue(ri, queueInfo, userId)) != ProcessingResult.Success) //Assign queue task to Batch ID and check result
                {
                    bool hadError = false;
                    if (processingResult == ProcessingResult.UnknownIssue)
                    {
                        result = ProcessingResult.UnknownIssue;
                        hadError = true;
                    }
                    DA.UpdateProcessed(queueInfo.QueueId, hadError);
                    DA.UpdateWasFound(0, queueInfo.QueueId);
                    continue;
                }
                ri.FastPath($"TX3Z/ITX6X{queueInfo.Queue};{queueInfo.SubQueue};{queueInfo.TaskControlNumber}");
                if (!ri.CheckForText(1, 74, "TXX71"))
                {
                    LogError($"Expected to be on screen TXX71, but current screen is {ri.ScreenCode}. Session Message:{ri.Message}; UserId:{userId}", queueInfo, 1);
                    DA.UpdateWasFound(0, queueInfo.QueueId);
                    DA.UpdateProcessed(queueInfo.QueueId, true);
                    continue;
                }

                DA.UpdateWasFound(1, queueInfo.QueueId);
                ri.PutText(21, 18, "01", F2, true);

                if (ri.ScreenCode != "TXX6S")
                {
                    LogError($"Expected to be on screen TXX6S, but current screen is {ri.ScreenCode}. Session Message:{ri.Message}; UserId:{userId}", queueInfo);
                    DA.UpdateProcessed(queueInfo.QueueId, true);
                    continue;
                }

                ri.PutText(8, 19, queueInfo.TaskStatus);
                ri.PutText(9, 19, queueInfo.ActionResponse, Enter);

                if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev && Program.ApplicationArgs.NumberOfThreads == 1 && Program.ApplicationArgs.PauseForTest) // Allows BA to have script pause at this juncture for testing purposes
                    ri.PauseForInsert();

                if (ri.MessageCode != "01005") // "01005" is the code used for successfully closing the task
                {
                    result = ProcessingResult.UnknownIssue; //We count this as a script failure since the cause of the closure failure is unknown and needs to be investigated
                    LogError($"Unable to close task in session. Verify task is closed properly. Encountered session Message:{ri.Message}; UserId:{userId}", queueInfo);
                    DA.UpdateProcessed(queueInfo.QueueId, true);
                    continue;
                }

                LogToConsole(queueInfo, threadNum, true);
                DA.UpdateProcessed(queueInfo.QueueId, false);
            }

            return result;
        }
        /// <summary>
        /// Assigns the current Queue task to the user
        /// </summary>
        private ProcessingResult AssignQueue(ReflectionInterface ri, QueueData queueInfo, string userId)
        {
            ri.FastPath("TX3Z/CTX6J");
            ri.PutText(7, 42, queueInfo.Queue, true);
            ri.PutText(8, 42, queueInfo.SubQueue, true);
            ri.PutText(9, 42, queueInfo.TaskControlNumber, true);
            ri.PutText(9, 76, queueInfo.WC_TYP_NUM_CTL_TSK, Enter, true);

            if (ri.MessageCode == "01029") // 01029 = Record updated/deleted by another user. Refresh page and then try.
                ri.Hit(F5);

            // The check below handles when tasks don't exist in session, usually due to already being closed by an agent or other process.
            if (ri.MessageCode == "01020") // 01020 = task not found (For reference, 01022 = task successfully found)
            {
                LogError($"Queue task not found; Session Message:{ri.Message}; UserId:{userId}", queueInfo, 0);
                return ProcessingResult.KnownIssue;
            }

            // Multiple results found, narrow to result with matching ARC.
            if (ri.ScreenCode == "TXX6N")
            {
                if (string.IsNullOrWhiteSpace(queueInfo.ARC))
                {
                    LogError($"Multiple queue tasks found on TXX6N, but no specific ARC was provided for queue.", queueInfo, 0);
                    return ProcessingResult.KnownIssue;
                }
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 9;
                settings.RowIncrementValue = 2;
                PageHelper.Iterate(ri, row =>
                {
                    var arc = ri.GetText(row, 7, 5);
                    if (arc == queueInfo.ARC)
                    {
                        var sel = ri.GetText(row, 2, 2);
                        ri.PutText(21, 18, sel, Enter, true);
                        settings.ContinueIterating = false;
                    }
                }, settings);
            }

            if (ri.ScreenCode != "TXX6O") // TXX60 = Screen we input UT# to assign task
            {
                LogError($"Expected to be on screen TXX6O, but current screen is {ri.ScreenCode}. Session Message:{ri.Message}; UserId:{userId}", queueInfo, 0);
                return ProcessingResult.KnownIssue;
            }

            ri.PutText(8, 15, userId, Enter, true);

            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev && Program.ApplicationArgs.NumberOfThreads == 1 && Program.ApplicationArgs.PauseForTest) // Allows BA to have script pause at this juncture for testing purposes
                ri.PauseForInsert();

            if (ri.MessageCode != "01005") // 01005 = Successfully assigned task
            {
                LogError($"Unable to assign Queue; Session Message:{ri.Message}; UserId:{userId}; AssignedTo: {ri.GetText(8, 15, 7)}", queueInfo);
                return ProcessingResult.UnknownIssue;
            }

            return ProcessingResult.Success;
        }

        private void LogToConsole(QueueData queueInfo, int threadNum, bool wasClosed = false)
        {
            WriteLine($"ThreadId: {threadNum}; {(wasClosed ? "Closing task" : "Processing")}: {queueInfo}");
        }

        private void LogError(string message, QueueData queueInfo, int severity = 1)
        {
            NotificationSeverityType nst = (severity == 1) ? NotificationSeverityType.Critical : NotificationSeverityType.Informational;
            lock (LogLock)
            {
                PLR.AddNotification($"{message}, {queueInfo}", NotificationType.ErrorReport, nst);
            }
        }
    }
}