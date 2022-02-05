using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace COMPCB
{
    /// <summary>
    /// This script completes tasks in the Compass JH queue to request information from credit bureaus for skip tracing
    /// </summary>
    public class TaskProcessor
    {
        private string ScriptId { get; set; }
        private ReflectionInterface RI { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private string UserId { get; set; }
        private string ReassignmentId { get; set; }
        public TaskProcessor(string scriptId, ReflectionInterface ri, ProcessLogRun logRun)
        {
            ScriptId = scriptId;
            RI = ri;
            LogRun = logRun;
            UserId = RI.UserId;
            DA = new DataAccess(LogRun);
            ReassignmentId = DA.GetManagerId(DA.GetBusinessUnitForScript(ScriptId).Name);
        }

        /// <summary>
        /// Entry method that utilizes helper methods to
        /// gather new tasks and then work those tasks.
        /// </summary>
        public bool Run()
        {
            List<TaskInfo> tasksToWork = GetTasksToWork();
            if (tasksToWork == null)
                return false;

            return ProcessTasks(tasksToWork);
        }

        /// <summary>
        /// Iterates through all tasks to have them worked one at a time.
        /// </summary>
        private bool ProcessTasks(List<TaskInfo> tasksToWork)
        {
            Console.WriteLine("Processing tasks now.");
            bool overallProcessingResult = true;
            foreach (TaskInfo task in tasksToWork)
            {
                bool taskProcessingResult = ProcessTask(task);
                overallProcessingResult &= taskProcessingResult;
                if (!taskProcessingResult && task.ProcessingAttempts >= 3)
                    ReassignTask(task);
            }

            return overallProcessingResult;
        }

        /// <summary>
        /// Indicates whether a task is processed successfully
        /// after ushering task through helper methods to 
        /// enter the task in the session, work the task,
        /// and then leave a comment. Returns true if successful,
        /// false otherwise.
        /// </summary>
        private bool ProcessTask(TaskInfo task)
        {
            DA.UpdateProcessingAttempts(task.ProcessingQueueId);
            task.ProcessingAttempts++;
            if (!EnterTaskInSession(task) || !WorkTaskInSession(task) || !AddArcForWorkedTask(task))
                return false;

            return true;
        }

        /// <summary>
        /// Navigates via the session into the task. Checks that proper screen
        /// is navigated to. Returns true if on desired screen, false otherwise.
        /// </summary>
        private bool EnterTaskInSession(TaskInfo task)
        {
            Console.WriteLine($"Processing task with control number {task.TaskControlNumber}");
            RI.FastPath($"TX3Z/ITX6XJH;01;{task.TaskControlNumber}");
            if (RI.MessageCode == "01020")
            {
                LogRun.AddNotification($"Task {task.TaskControlNumber} not found in the session. This is attempt # {task.ProcessingAttempts}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                if (task.ProcessingAttempts >= 3 && !DA.SetDeletedAt(task.ProcessingQueueId)) //Remove task from queue to work if tried 3 times and still doesn't exist in session
                {
                    string errorMessage = $"Unable to mark record with task control number {task.TaskControlNumber} as deleted after the task was not found in the session";
                    LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine($"{errorMessage}. Please see PL # {LogRun.ProcessLogId} for more details.");
                    return false;
                }
                return false;
            }
            else if (RI.ScreenCode != "TXX7")
            {
                LogSessionNavigationError(task.TaskControlNumber, "TXX7");
                return false;
            }

            bool isForeginAddress = task.IsForeignAddress.HasValue && task.IsForeignAddress.Value;
            RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
            if (RI.ScreenCode != "TDX0B")
            {
                LogSessionNavigationError(task.TaskControlNumber, "TDX0B");
                return false;
            }

            if (isForeginAddress)
            {
                RI.FastPath($"TX3Z/ITX6XJH;01;{task.TaskControlNumber}");
                RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
                if (isForeginAddress && RI.ScreenCode != "TXX6S")
                {
                    LogSessionNavigationError(task.TaskControlNumber, "TXX6S");
                    return false;
                }
            }
            Console.WriteLine($"Entered task with the control number {task.TaskControlNumber} in the session.");
            return true;
        }

        /// <summary>
        /// Updates the necessary fields with the applicable credit bureau codes
        /// if a domestic address. Otherwise closes the task if it is a foregin address.
        /// Upon updating the fields, the domestic-address task is automatically closed in the session.
        /// Returns true if successfully worked & closed, false otherwise.
        /// </summary>
        private bool WorkTaskInSession(TaskInfo task)
        {
            if (task.IsForeignAddress.HasValue && task.IsForeignAddress.Value) // For tasks with a bwr/endr w/ a foregin address, close task
            {
                RI.PutText(8, 19, "C");
                RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);

                if (RI.MessageCode != "01005") //01005 = Successfully closed
                {
                    string errorMessage = $"Error closing record on TXX6S screen. Encountered session message: {RI.Message}";
                    LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine($"{errorMessage}. Please see PL # {LogRun.ProcessLogId} for more details.");
                    return false;
                }
                Console.WriteLine($"Closed task with foregin address successfully. Finished processing task control number: {task.TaskControlNumber}");
            }
            else // For domestic addresses, complete credit bureau info request
            {
                RI.PutText(10, 44, "CB", ReflectionInterface.Key.Enter);
                if (RI.ScreenCode != "TDX0I")
                {
                    LogSessionNavigationError(task.TaskControlNumber, "TDX0I");
                    return false;
                }
                RI.PutText(13, 44, "1");
                RI.PutText(13, 58, "S", ReflectionInterface.Key.Enter);

                if (RI.MessageCode != "01004")
                {
                    string errorMessage = $"Error adding record on TDX0I screen. Encountered session message: {RI.Message}";
                    LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine($"{errorMessage}. Please see PL # {LogRun.ProcessLogId} for more details.");
                    return false;
                }
                Console.WriteLine($"Updated fields on TDX0I successfully for task control number: {task.TaskControlNumber}");
            }
            if (!DA.SetProcessedAt(task.ProcessingQueueId))
            {
                LogRun.AddNotification($"Unable set the ULS.compcb.ProcessingQueue.ProcessedAt field while working ProcessingQueueId {task.ProcessingQueueId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds ARC to account corresponding to whether it is an endorser or borrower
        /// task and whether the recipient has a foreign address or not. 
        /// Returns true if ARC successfully added or if recipient has domestic address.
        /// False otherwise.
        /// </summary>
        private bool AddArcForWorkedTask(TaskInfo task)
        {
            if (task.IsForeignAddress.HasValue && task.IsForeignAddress.Value) // If foreign address, drop review ARC
            {
                ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    Arc = (task.IsEndorserTask.HasValue && task.IsEndorserTask.Value) ? "S3AR8" : "S3AR7",
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                    AccountNumber = task.BorrowerAccountNumber,
                    Comment = $"{{{ScriptId}}} / {UserId}",
                    IsEndorser = task.IsEndorserTask.HasValue && task.IsEndorserTask.Value,
                    IsReference = false,
                    RecipientId = task.EndorserSsn ?? task.BorrowerSsn,
                    ScriptId = ScriptId
                };
                ArcAddResults result = arcData.AddArc();
                if (result == null || !result.ArcAdded)
                {
                    LogRun.AddNotification($"Error adding ARC {arcData.Arc} for account {task.BorrowerAccountNumber}. Intended ARC comment: {arcData.Comment}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine($"Failed to add ARC {arcData.Arc} for account {task.BorrowerAccountNumber}");
                    return false;
                }
                else
                {
                    task.ArcAddProcessingId = result.ArcAddProcessingId;
                    if (!DA.SetArcAddProcessingId(task))
                    {
                        LogRun.AddNotification($"Unable to add the ARC {arcData.Arc} for account {task.BorrowerAccountNumber}. See ProcessingQueueId {task.ProcessingQueueId} for the relevant record in the processing table.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Adds a ProcessLogger record and writes out to the console
        /// when a session navigation error is encountered.
        /// </summary>
        private void LogSessionNavigationError(string taskControlNumber, string expectedScreen)
        {
            LogRun.AddNotification($"For task {taskControlNumber} an unexpected screen {RI.ScreenCode} was encountered. Expected {expectedScreen}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            Console.WriteLine($"Session navigation issue encountered. Please see PL # {LogRun.ProcessLogId} for more details.");
        }

        /// <summary>
        /// Adds new tasks to the processing table, retrieves those tasks,
        /// updates the IsForeignAddress flag for any task tied to a 
        /// foreign address. Returns the tasks that are to be worked by the script.
        /// </summary>
        private List<TaskInfo> GetTasksToWork()
        {
            Console.WriteLine("Gathering tasks from the DB.");
            bool loadResult = DA.LoadNewWork(); // Adds new JH queue tasks to ProcessingQueue table
            if (!loadResult)
                return null;

            List<TaskInfo> tasksToWork = DA.GetUnprocessedWork();
            Console.WriteLine($"{tasksToWork?.Count()} JH tasks found to work");
            List<TaskInfo> tasksWithoutTargetDemos = new List<TaskInfo>();
            if (tasksToWork != null && tasksToWork.Any())
            {
                List<string> stateCodes = DA.GetDomesticStateCodes();
                foreach (TaskInfo task in tasksToWork)
                {
                    string targetSsnForTask = (task.IsEndorserTask.HasValue && task.IsEndorserTask.Value) ? task.EndorserSsn : task.BorrowerSsn;
                    try
                    {
                        var demos = RI.GetDemographicsFromTx1j(targetSsnForTask);
                        if (!stateCodes.Contains(demos.State))
                        {
                            task.IsForeignAddress = true;
                            DA.UpdateForeignAddressIndicator(task.ProcessingQueueId);
                        }
                    }
                    catch (DemographicException ex)
                    {
                        LogRun.AddNotification($"For task {task.TaskControlNumber} the account identifier of {targetSsnForTask} could not be found in the session. Exception message: {ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        tasksWithoutTargetDemos.Add(task);
                        DA.UpdateProcessingAttempts(task.ProcessingQueueId);
                        if (task.ProcessingAttempts >= 3)
                        {
                            if (!DA.SetDeletedAt(task.ProcessingQueueId))
                            {
                                string errorMessage = $"Unable to mark record with task control number {task.TaskControlNumber} as deleted after the demographics failed to be retrieved and the task failed to be processed three times";
                                LogRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                Console.WriteLine($"{errorMessage}. Please see PL # {LogRun.ProcessLogId} for more details.");
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Finished verifying whether addresses associated with tasks are foreign or domestic");
            return tasksToWork.Where(p => !tasksWithoutTargetDemos.Contains(p)).ToList();
        }

        /// <summary>
        /// Reassigns a task that couldn't be worked by the script.
        /// Only invoked after the script attempts to work the task
        /// three times.
        /// </summary>
        /// <param name="task"></param>
        private void ReassignTask(TaskInfo task)
        {
            if (!RI.ReAssignQueueTask("JH", "01", UserId, ReassignmentId).Trim().Contains("01005")) // If session message for reassignment attempt doesn't have 01005 (success code)
                LogRun.AddNotification($"Unable to reassign task control number {task.TaskControlNumber} from user {UserId} to {ReassignmentId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }
    }
}
