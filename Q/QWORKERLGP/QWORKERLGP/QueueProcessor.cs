using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static QWORKERLGP.ProcessingResults;
using static System.Console;

namespace QWORKERLGP
{
    class QueueProcessor
    {
        private readonly object LogLock = new object();
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private const string ErrorUser = "UT00559";
        public QueueProcessor()
        {
            LogRun = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, Program.ApplicationArgs.ShowPrompts, false, true, DataAccessHelper.Database.ProcessLogs);
            DA = new DataAccess(LogRun.LDA);
        }

        public int Process()
        {
            List<ProcessingResult> returnReasons = new List<ProcessingResult>() { ProcessingResult.Success }; // Start as a successful run 
            WriteLine("Cleaning up unprocessed tasks from previous run.");
            
            DA.CleanUpRecords(); //Mark records as un-processed if they have not been completed.

            try
            {
                Parallel.For(0, Program.ApplicationArgs.NumberOfThreads, index =>
                {
                    ReflectionInterface ri = new ReflectionInterface();
                    WriteLine($"Logging into the session for ThreadId:{Thread.CurrentThread.ManagedThreadId}");

                    var user = BatchProcessingLoginHelper.Login(LogRun, ri, Program.ScriptId, "BatchUheaa");

                    if(user == null)
                    {
                        ri.CloseSession();
                        return;
                    }

                    WriteLine($"Starting to process tasks with UT ID {user.UserName} on thread ThreadId:{Thread.CurrentThread.ManagedThreadId}");
                    returnReasons.Add(ProcessQueue(ri, user.UserName, Thread.CurrentThread.ManagedThreadId));
                    WriteLine($"Finished processings tasks with UT ID {user.UserName} on thread ThreadId:{Thread.CurrentThread.ManagedThreadId}");
                    ri.CloseSession();
                });
            }
            catch(Exception ex)
            {
                LogRun.AddNotification($"There was an error processing the {Program.ScriptId} script. EX: {ex}", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return Program.FAILURE;
            }
            finally
            {
                LogRun.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();
            }

            WriteLine("Finished processing tasks. Ending script run.");

            return Program.SUCCESS; //Could update this to return the results of the individual threads if necessary later
        }

        private ProcessingResult ProcessQueue(ReflectionInterface ri, string userId, int threadNum)
        {
            QueueData queueData;
            var result = ProcessingResult.Success;
            while((queueData = DA.GetNextQueue()) != null) //Retrieve next queue task to close and check result
            {
                LogToConsole(queueData, threadNum);
                var processingResult = AssignQueue(ri, queueData, userId);
                if (processingResult == ProcessingResult.Success)
                {
                    ri.FastPath($"LP9AC{queueData.WorkGroupId};;{queueData.Department};;;");
                    if (!ri.CheckForText(1, 71, "QUEUE TASK")) //Data Successfully Updated
                    {
                        result = ProcessingResult.KnownIssue;
                        LogError($"Unable to find task on LP9AC. Encountered session Message:{GetSessionMessage(ri)}; UserId:{userId}", queueData);
                        DA.UpdateProcessedAt(queueData.QueueId, true);
                        continue;
                    }


                    bool failedToFindAssignedTask = false;
                    while(!ri.CheckForText(6,57,queueData.Ssn))
                    {
                        ri.Hit(ReflectionInterface.Key.F8);
                        if(ri.CheckForText(22,3,"46004")) //No More Data to Display
                        {
                            ErrorAssignQueue(ri, queueData, userId);
                            LogError($"Failed to find assigned task on LP9AC. Session Message:{GetSessionMessage(ri)}; UserId:{userId}", queueData);
                            DA.UpdateProcessedAt(queueData.QueueId, true);
                            failedToFindAssignedTask = true;
                            break;
                        }
                    }

                    //We failed to find the task on LP9AC, continue to next record
                    if (failedToFindAssignedTask)
                    {
                        continue;
                    }

                    //Add comment
                    if (!queueData.TaskComment.IsNullOrEmpty())
                    {                       
                        var commentAdded = AddLP50Comment(ri, queueData);
                        if (!commentAdded)
                        {
                            ErrorAssignQueue(ri, queueData, userId);
                            LogError($"Failed to add Comment in LP50. Session Message:{GetSessionMessage(ri)}; UserId:{userId}", queueData);
                            DA.UpdateProcessedAt(queueData.QueueId, true);
                            continue;
                        }
                        ri.Hit(ReflectionInterface.Key.F12);
                        Thread.Sleep(5000); //Needed because sometime there is lag in onelink
                    }

                    if(ri.CheckForText(23,20,"SET1"))
                    {
                        ri.Hit(ReflectionInterface.Key.F2);
                    }
                    ri.Hit(ReflectionInterface.Key.F6);
                    if(!ri.CheckForText(22, 3, "49000")) //Data Successfully Updated
                    {
                        ErrorAssignQueue(ri, queueData, userId);
                        result = ProcessingResult.UnknownIssue; //We count this as a script failure since the cause of the closure failure is unknown and needs to be investigated
                        LogError($"Unable to close task in session. Verify task is closed properly. Encountered session Message:{GetSessionMessage(ri)}; UserId:{userId}", queueData);
                        DA.UpdateProcessedAt(queueData.QueueId, true);
                        continue;
                    }

                    LogToConsole(queueData, threadNum, true);
                    DA.UpdateProcessedAt(queueData.QueueId, false);
                }
                else
                {
                    ErrorAssignQueue(ri, queueData, userId);

                    bool hadError = false;
                    if (processingResult == ProcessingResult.UnknownIssue)
                    {
                        result = ProcessingResult.UnknownIssue;
                        hadError = true;
                    }
                    DA.UpdateProcessedAt(queueData.QueueId, hadError);
                    DA.UpdateWasFound(queueData.QueueId, false);
                    continue;
                } 
            }
            return result;
        }

        private void ErrorAssignQueue(ReflectionInterface ri, QueueData queueData, string userId)
        {
            //Try to assign the queue to Rose for review
            var errorAssignment = AssignQueue(ri, queueData, ErrorUser);
            if (errorAssignment != ProcessingResult.Success)
            {
                LogError($"Unable to assign task for review. Verify task is taken care of. Encountered session Message:{GetSessionMessage(ri)}; UserId:{userId}, ErrorUserId:{ErrorUser}", queueData);
            }
            else
            {
                LogError($"Queue Assigned for manual review to UserId:{ErrorUser}", queueData);
            }
        }

        private ProcessingResult AssignQueue(ReflectionInterface ri, QueueData queueData, string userId)
        {
            ri.FastPath("LP8YC");
            ri.PutText(6, 37, queueData.Department);
            ri.PutText(8, 37, queueData.WorkGroupId);
            ri.PutText(12, 37, queueData.Ssn, ReflectionInterface.Key.Enter);

            //Known issues
            if(ri.CheckForText(22,3,"47004")) //No records found for entered search criteria
            {
                LogError($"Queue task not found; Session Message:{ ri.GetText(22, 3, 76).Trim() }", queueData, 0);
                return ProcessingResult.KnownIssue;
            }      
            if (!ri.CheckForText(22, 3, "46011")) //Make desired data changes and press enter
            {
                LogError($"Unable to make changes; Session Message:{ ri.GetText(22, 3, 76).Trim() }", queueData, 1);
                return ProcessingResult.UnknownIssue;
            }

            //Found Queue
            ri.PutText(7, 38, userId);
            ri.Hit(ReflectionInterface.Key.F6);

            if (ri.CheckForText(22, 3, "40281")) //User Not Authorized Due to Tier Or Queue Type Access
            {
                LogError($"User: {userId} User Not Authorized Due to Tier Or Queue Type Access", queueData);
                return ProcessingResult.UnknownIssue;
            }
            if (!ri.CheckForText(22, 3, "49000") && !ri.CheckForText(22,3,"49007")) //Data Successfully Updated
            {
                LogError($"Unable to make changes; Session Message:{ ri.GetText(22, 3, 76).Trim() }", queueData, 0);
                return ProcessingResult.UnknownIssue;
            }

            return ProcessingResult.Success;
        }

        private bool AddLP50Comment(ReflectionInterface ri, QueueData queueData)
        {
            ri.Hit(ReflectionInterface.Key.F2);
            ri.Hit(ReflectionInterface.Key.F7);

            ri.PutText(9, 20, queueData.ActionCode, ReflectionInterface.Key.Enter);

            if (!ri.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                return false;

            ri.PutText(7, 2, queueData.ActivityType);
            ri.PutText(7, 5, queueData.ActivityContactType);

            var comment = queueData.TaskComment ?? "";
            if (comment.Length + Program.ScriptId.Length < 70)
            {
                comment = string.Format("{0}. {{ {1} }}", comment, Program.ScriptId);
                ri.PutText(13, 2, "", ReflectionInterface.Key.EndKey);
                ri.PutText(14, 2, "", ReflectionInterface.Key.EndKey);
                ri.PutText(15, 2, "", ReflectionInterface.Key.EndKey);
                ri.PutText(16, 2, "", ReflectionInterface.Key.EndKey);
                ri.PutText(17, 2, "", ReflectionInterface.Key.EndKey);
                ri.PutText(18, 2, "", ReflectionInterface.Key.EndKey);
                ri.PutText(13, 2, comment, ReflectionInterface.Key.F6);
            }
            else
            {
                if (comment.Length + Program.ScriptId.Length > 585)
                    throw new Exception("The requested comment will not fit on LP50");

                ri.PutText(13, 2, comment.SafeSubString(0, 75));

                for (int segmentStart = 75; segmentStart <= comment.Length - 1; segmentStart += 75)
                    ri.EnterText(comment.SafeSubString(segmentStart, 75));

                ri.EnterText(string.Format(" {{ {0} }}", Program.ScriptId));

                ri.Hit(ReflectionInterface.Key.F6);
            }

            return ri.CheckForText(22, 3, "48003");
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
                LogRun.AddNotification($"{message}, {queueInfo}", NotificationType.ErrorReport, nst);
            }
        }

        private string GetSessionMessage(ReflectionInterface ri)
        {
            return ri.GetText(22, 3, 76).Trim();
        }
    }
}
