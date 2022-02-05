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

namespace ENRQTSKLLR
{
    public class EnrollmentTaskKiller
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public ReflectionInterface RI { get; set; }
        private readonly List<string> EnrollmentQueues = new List<string>() { "NEWSTNME", "NEWMILST", "NMMISINV" };

        public EnrollmentTaskKiller(ReflectionInterface ri, ProcessLogRun logRun)
        {
            LogRun = logRun;
            RI = ri;
            DA = new DataAccess(LogRun);
        }

        /// <summary>
        /// Notifies user of script's purpose and kicks off enrollment queue
        /// processing.
        /// </summary>
        public bool Process()
        {
            return ProcessEnrollmentQueues();
        }

        /// <summary>
        /// Entry point for queue processing. Utilizes child
        /// methods to add tasks to the processing table,
        /// pull those down from the dB, and then process them.
        /// </summary>
        private bool ProcessEnrollmentQueues()
        {
            Console.WriteLine("Pulling tasks down to add to processing table");
            if (!AddNewWork())
            {
                string error = $"The script encountered an error trying to add records to the database.";
                LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine($"{error} Please see PL # {LogRun.ProcessLogId}");
                return false;
            }
            List<EnrollmentTask> enrollmentTasks = DA.GetUnprocessedRecords();
            LogRun.AddNotification($"{enrollmentTasks?.Count ?? 0} unprocessed tasks found", NotificationType.Other, NotificationSeverityType.Informational);
            Console.WriteLine($"{enrollmentTasks?.Count ?? 0} unprocessed tasks found");

            bool processingWasSuccessful = true;
            if (TasksFound(enrollmentTasks))
                processingWasSuccessful = ProcessQueues(enrollmentTasks);

            return processingWasSuccessful;
        }

        /// <summary>
        /// Calls a stored procedure to add enrollment tasks in the CT30 table to
        /// this script's processing table.
        /// </summary>
        private bool AddNewWork()
        {
            int? recordsInserted = DA.InsertProcessingQueueRecords();
            LogRun.AddNotification($"Added {recordsInserted ?? 0} records to the enrqtskllr.ProcessingQueue table", NotificationType.Other, NotificationSeverityType.Informational);
            return recordsInserted.HasValue;
        }

        /// <summary>
        /// Indicates whether there are any tasks for the script to work.
        /// </summary>
        private bool TasksFound(List<EnrollmentTask> enrollmentTasks)
        {
            if (enrollmentTasks != null && enrollmentTasks.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Iterates through tasks, ordered by their queue, and utilizes
        /// helper methods to assign each task before processing it.
        /// </summary>
        private bool ProcessQueues(List<EnrollmentTask> enrollmentTasks)
        {
            if (UserAssignedTaskAlready("A") || UserAssignedTaskAlready("W")) // User can't run script if they already are assigned a task 
            {
                string error = $"User {RI.UserId} was already assigned one or more SCR tasks. Please unassign the task(s) and then try re-running the script.";
                LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine(error);
                return false;
            }

            int errors = 0;
            foreach (EnrollmentTask task in enrollmentTasks.OrderBy(p => p.QueueName).ToList())
            {
                Console.WriteLine($"Assigning enrollment task to user. ProcessingQueueId: {task.ProcessingQueueId}");
                if (!AssignTask(task.Ssn, task.QueueName, task.ProcessingQueueId.Value))
                {
                    errors++;
                    continue;
                }

                Console.WriteLine($"Processing enrollment task. ProcessingQueueId: {task.ProcessingQueueId}");

                RI.FastPath($"LP9AC{task.QueueName};;;Y;;");
                Thread.Sleep(2000);

                bool arcResult = AddArc(task);
                RI.WaitForText(22, 3, "48003", 2);

                if (!arcResult)
                    errors++;
                else if (!ProcessTask(task))
                {
                    errors++;
                    Console.WriteLine($"Unassigning enrollment task from user. ProcessingQueueId: {task.ProcessingQueueId}");
                    if (!UnassignTaskThatFailed(task))
                        errors++;
                }
            }

            if (errors > 0)
                return false;

            Console.WriteLine("Finished processing all enrollment tasks");
            return true;
        }

       

        /// <summary>
        /// Indicates whether the user is already assigned an enrollment task.
        /// If so, returns true. Else, logs the error, notifies the user, and then
        /// returns false.
        /// </summary>
        private bool UserAssignedTaskAlready(string status)
        {
            RI.FastPath($"LP8YI");
            RI.PutText(6, 37, "SCR");
            RI.PutText(16, 37, status);
            RI.PutText(10, 37, RI.UserId, ReflectionInterface.Key.Enter);

            if (RI.AltMessageCode != "47004")
            {
                string queues = String.Join(", ", EnrollmentQueues);
                LogRun.AddNotification($"User {RI.UserId} assigned a related queue task. Cannot run the script without reassigning the task. Please reassign the tasks from the following queues for the user: {queues}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Console.WriteLine($"User {RI.UserId} is already assigned a related queue task. Please make sure to re-run the script AFTER reassigning the following queue task(s): {queues}.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Driver method that results in the task being opened, checked, and
        /// closed in the session. If successfully opened, an ARC will be dropped 
        /// on the account. If successfully closed, the ProcessedAt field will be
        /// set in the DB.
        /// </summary>
        private bool ProcessTask(EnrollmentTask enrollmentTask)
        {
            while (!RI.CheckForText(22, 3, "47420", "47423", "47450", "46004"))
            {
                RI.FastPath($"LP9AC{enrollmentTask.QueueName};;;Y;;");
                if (!IsMatchingTask(enrollmentTask))
                    return false;

                RI.Hit(ReflectionInterface.Key.F6);
                if (RI.AltMessageCode == "49000" || RI.AltMessageCode == "48003") // 49000 = "DATA SUCCESSFULLY UPDATED"
                {
                    if (DA.SetRecordProcessed(enrollmentTask.ProcessingQueueId.Value))
                        return true;
                    else
                    {
                        string error = $"Error setting the processed field for ProcessingQueueId {enrollmentTask.ProcessingQueueId.Value}";
                        LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        Console.WriteLine(error);
                        return false;
                    }
                }
                else
                {
                    LogRun.AddNotification($"Unable to close task for borrower {enrollmentTask.Ssn} and queue {enrollmentTask.QueueName}. Was expecting success code 49000 but got {RI.AltMessageCode}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine($"The script encountered an error trying to complete the queue task {enrollmentTask.QueueName} for borrower {enrollmentTask.Ssn}. Unable to close the task. See PL # {LogRun.ProcessLogId} for more details.");
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the task matches the expected one
        /// (aka has the right borrower associated with it).
        /// </summary>
        private bool IsMatchingTask(EnrollmentTask enrollmentTask)
        {
            string ssn = RI.GetText(17, 70, 9);
            if (ssn != enrollmentTask.Ssn)
            {
                LogRun.AddNotification($"Session navigation error encountered. Expected borrower {enrollmentTask.Ssn} but got borrower {ssn}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Console.WriteLine($"The script encountered an error trying to process the queue task {enrollmentTask.QueueName} for borrower {enrollmentTask.Ssn}. Please see PL # {LogRun.ProcessLogId}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Assigns the task to the user. This allows for the task to pull
        /// up on LP9AC as the first result. If assignment fails, error
        /// is logged and user is notified.
        /// </summary>
        private bool AssignTask(string ssn, string queueName, int processingQueueId)
        {
            RI.FastPath($"LP8YC");
            if (!IsOnQueueSelectionScreen(ssn, queueName))
                return false;

            RI.PutText(6, 37, "SCR");
            RI.PutText(8, 37, queueName);
            RI.PutText(12, 37, ssn, ReflectionInterface.Key.Enter);
            if (!IsOnQueueTaskScreen(ssn, queueName))
                return false;

            if (RI.GetText(7, 33, 1) == "C" || RI.GetText(7, 33, 1) == "X") // Already closed or cancelled, so no need for script to process
            {
                DA.SetRecordProcessed(processingQueueId);
                DA.SetArcAddedAt(processingQueueId); // No ARC is necessary, so set the field in the processing table
                return false;
            }

            RI.PutText(7, 33, "A"); // TODO: Ask BA if we should only grab task if it is not assigned to user, or if we should grab it even then. Emailed Tawny, she emailed BU--we are awaiting reply.
            RI.PutText(7, 38, RI.UserId, ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6); // Post update to assign task
            bool result = RI.AltMessageCode == "49000" || RI.GetText(21, 3, 5) == "49000"; // 49000 = "DATA SUCCESSFULLY UPDATED"
            if (!result)
            {
                LogRun.AddNotification($"Error assigning task for borrower {ssn} for the queue {queueName}. Expected message code 49000 but instead got {RI.GetText(21, 3, 78) ?? ""}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine($"There was an error trying to assign the queue task {queueName} for borrower {ssn} to the user {RI.UserId}. Task not worked.");
            }
            return result;
        }

        /// <summary>
        /// In the event that a task fails, this method will unassign that task
        /// from the current batch id. This allows for the script to continue
        /// working other tasks (otherwise it wouldn't know which to work
        /// since multiple would be assigned to it).
        /// </summary>
        /// <param name="task"></param>
        private bool UnassignTaskThatFailed(EnrollmentTask task)
        {
            bool result = true;
            RI.FastPath($"LP8YCSCR;{task.QueueName};{RI.UserId};{task.Ssn}");

            if (!IsOnQueueTaskScreen(task.Ssn, task.QueueName))
                result = false;

            if (RI.GetText(7, 33, 1) == "C" || RI.GetText(7, 33, 1) == "X") // Already closed or cancelled, so no need for script to process
            {
                DA.SetRecordProcessed(task.ProcessingQueueId.Value);
                DA.SetArcAddedAt(task.ProcessingQueueId.Value); // No ARC is necessary, so set the field in the processing table
                return true;
            }

            RI.PutText(7, 38, "", ReflectionInterface.Key.Enter, true);
            RI.Hit(ReflectionInterface.Key.F6); // Post update to assign task

            result &= RI.AltMessageCode == "47450" || RI.GetText(21, 3, 5) == "47450"; // 47450 = "NO CURRENT TASKS FOUND"
            if (!result)
            {
                LogRun.AddNotification($"After a task failed to close there was an error unassigning the task for borrower {task.Ssn} for the queue {task.QueueName}. Expected message code 49000 but instead got {RI.GetText(21, 3, 78) ?? ""}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine($"There was an error trying to unassign the queue task {task.QueueName} for borrower {task.Ssn} to the user {RI.UserId}. Task failed to close and failed to reassign.");
            }
            return result;
        }

        /// <summary>
        /// Adds an ARC and relevant comment to the account
        /// that the enrollment task is on. Returns false if 
        /// it hit an error, else true.
        /// </summary>
        private bool AddArc(EnrollmentTask enrollmentTask)
        {
            if (enrollmentTask.ArcAddedAt.HasValue)
                return true;

            string arc = "MXSCR";
            string comment = $"{enrollmentTask.QueueName} task closed.  The enrollment process is not an acceptable mechanism for reporting name changes.";
            bool result = RI.AddCommentInLP50(enrollmentTask.Ssn, "MS", "99", arc, comment, Program.ScriptId);
            if (!result)
            {
                LogArcGenerationError(arc, enrollmentTask, comment);
                return false;
            }
            else
            {
                if (!DA.SetArcAddedAt(enrollmentTask.ProcessingQueueId.Value))
                {
                    string error = $"Unable to set the ArcAddedAt field for ProcessingQueueId {enrollmentTask.ProcessingQueueId}";
                    LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine(error);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Builds the ArcData object with the pertinent info
        /// for the enrollment task.
        /// </summary>
        private ArcData ConstructArc(EnrollmentTask enrollmentTask, string comment)
        {
            return new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = enrollmentTask.Ssn,
                ActivityContact = "99",
                ActivityType = "MS",
                Arc = "MXSCR",
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ScriptId = Program.ScriptId
            };
        }

        /// <summary>
        /// Adds a Process Log notification that the ARC was not added. 
        /// </summary>
        private void LogArcGenerationError(string arc, EnrollmentTask enrollmentTask, string comment)
        {
            string message = $"Unable to add OneLINK {arc} for the SSN associated with the account {enrollmentTask.AccountNumber}.";
            LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            Console.WriteLine(message);
        }

        /// <summary>
        /// Indicates if the script is on the correct queue selection
        /// screen. If not, error is logged and user is notified.
        /// </summary>
        private bool IsOnQueueSelectionScreen(string ssn, string queueName)
        {
            if (RI.GetText(1, 60, 21) != "QUEUE STATS SELECTION")
            {
                string error = $"Error assigning task for borrower {ssn} for the queue {queueName}. Expected to be on the QUEUE STATS SELECTION screen but was instead on the screen {RI.GetText(1, 60, 21)}.";
                LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine(error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Indicates if the script is on the correct queue task detail page
        /// and if the code on the screen indicates that the task is ready for
        /// updates. If not, error is logged and user is notified.
        /// </summary>
        private bool IsOnQueueTaskScreen(string ssn, string queueName, bool assigning = true)
        {
            if (RI.GetText(1, 64, 17) != "QUEUE TASK DETAIL" || RI.AltMessageCode != "46011") // 46011 = "MAKE DESIRED DATA CHANGES AND PRESS ENTER"
            {
                string scriptAction = assigning == true ? "assigning" : "unassigning";
                string error = $"Error {scriptAction} task for borrower {ssn} for the queue {queueName}. Expected to be on the QUEUE TASK DETAIL screen but was instead on the screen {RI.GetText(1, 64, 17)}.";
                LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Console.WriteLine(error);
                return false;
            }
            return true;
        }
    }
}
