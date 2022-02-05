using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Threading;

namespace Monitor
{
    class R0QueueProcessor
    {
        const string queue = "R0";
        const string subqueue = "01";
        const string R001 = queue + ";" + subqueue + ";";
        const string itx6xFastPath = "tx3z/itx6x" + R001;
        StandardArgs args;
        string utId;
        bool skipTaskClose;
        List<string> overrideIdentifiers;
        HashSet<R0Task> WorkedTasks = new HashSet<R0Task>();
        public R0QueueProcessor(StandardArgs args, string utid, bool skipTaskClose, List<string> overrideIdentifiers)
        {
            this.utId = utid;
            this.args = args;
            this.skipTaskClose = skipTaskClose;
            this.overrideIdentifiers = overrideIdentifiers;
        }

        public bool Working { get; set; }
        public bool Work()
        {
            int runHistoryId = args.DA.StartRunHistory();
            bool didAnyWork = false;
            Working = true;
            while (Working)
            {
                UnselectExistingTask();
                var selectedTask = SelectTask();
                if (selectedTask == null) //nothing more to work
                {
                    Working = false;
                    break;
                }
                var taskWorker = new R0TaskProcessor(args, utId, selectedTask);
                TaskEoj eoj = new TaskEoj();
                eoj.Task = selectedTask;
                eoj.R0CreateDate = selectedTask.DateRequested;
                if (overrideIdentifiers.Any())
                    if (!selectedTask.Ssn.IsIn(overrideIdentifiers.ToArray()) && !selectedTask.AccountNumber.IsIn(overrideIdentifiers.ToArray()))
                    {
                        WorkedTasks.Add(selectedTask);
                        continue;
                    }
                if (selectedTask.IsAbend)
                {
                    eoj.EojType = EojReport.ForwardedSkipped;
                    eoj.CancelReason = "SYSTEM ABEND";
                }
                else if (WorkedTasks.Any(o => o.Ssn == selectedTask.Ssn))
                {
                    eoj.EojType = EojReport.Cancelled;
                    eoj.CancelReason = string.Format("Already processed a task for {0}, cancelling unexpected duplicate task {1}.", selectedTask.AccountNumber, selectedTask);
                }
                else if (selectedTask.Ssn.Trim().Length != 9)
                {
                    eoj.EojType = EojReport.Cancelled;
                    eoj.CancelReason = string.Format("Unable to work task {0} due to no loans found.  Cancelling task.", selectedTask);
                }
                else if (args.DA.BorrowerGetRecentMonitorSkippedTask(selectedTask.AccountNumber) != null)
                {
                    var arc = args.DA.BorrowerGetRecentMonitorSkippedTask(selectedTask.AccountNumber);
                    eoj.EojType = EojReport.ExemptConditionSkipped;
                    eoj.CancelReason = string.Format("Skipping task {0}, task previously reviewed, awaiting ARC Add Processing: {1}, {2}, {3}.", selectedTask, arc.CreatedAt.ToShortDateString(), arc.Arc, arc.Comment);
                }
                else if (string.IsNullOrWhiteSpace(selectedTask.Ssn))
                {
                    eoj.EojType = EojReport.Cancelled;
                    eoj.CancelReason = string.Format("Cancelling task {0}, could not find task SSN.");
                }
                else if (selectedTask.IsInvalid)
                {
                    eoj.EojType = EojReport.Cancelled;
                    eoj.CancelReason = args.RI.Message;
                }
                else
                {
                    eoj = taskWorker.ProcessTask();
                }
#if !DEBUG
                SetTaskStatus(selectedTask, eoj.GetTaskResult(), runHistoryId, true);
#else
                WorkedTasks.Add(selectedTask);
#endif
                args.DA.AddEojItem(runHistoryId, eoj, args.MS.MaxIncrease);
                didAnyWork = true;
            }
            UnselectExistingTask();
            args.MS.LastRecoveryPage = null;
            args.DA.SaveMonitorSettings(args.MS);
            args.DA.EndRunHistory(runHistoryId);
            return didAnyWork;
        }

        public bool CheckAccess()
        {
            UnselectExistingTask();
            if (args.RI.MessageCode == "80014")
                return false; //user has bad access
            return true;
        }

        private void EOJ(string message, params object[] parameters)
        {
            message = string.Format(message, parameters);
            string prefix = "FED: ";
            Console.WriteLine(prefix + message);
            args.PLR.AddNotification(message, NotificationType.EndOfJob, NotificationSeverityType.Informational);
        }

        private R0Task SelectTask()
        {
            args.RI.FastPath(itx6xFastPath);
            var iterSettings = PageHelper.IterationSettings.Default();
            iterSettings.MinRow = 8;
            iterSettings.MaxRow = 17;
            iterSettings.RowIncrementValue = 3;
            R0Task foundTask = null;
            PageHelper.Iterate(args.RI, (row, s) =>
            {
                if (args.MS.LastRecoveryPage.HasValue && args.MS.LastRecoveryPage > s.CurrentPage)
                    return; //catch up to recovery
                args.MS.LastRecoveryPage = s.CurrentPage;
                args.DA.SaveMonitorSettings(args.MS);
                bool isBeingWorked = args.RI.CheckForText(row, 75, "W");
                if (!isBeingWorked)
                {
                    foundTask = new R0Task();
                    foundTask.TaskControl = args.RI.GetText(row, 6, 20);
                    foundTask.ActionRequest = args.RI.GetText(row, 27, 5);
                    foundTask.DateRequested = args.RI.GetText(row, 47, 10).ToDate();
                    foundTask.MonitorReason = args.RI.GetText(row + 1, 2, 78);

                    if (WorkedTasks.Any(o => o.Equals(foundTask))) //don't work a task we already tried working or skipped during this session
                    {
                        foundTask = null;
                    }
                    else
                    {
                        string selection = args.RI.GetText(row, 3, 2);
                        args.RI.PutText(21, 18, selection, ReflectionInterface.Key.Enter);

                        if (args.RI.CheckForText(19, 41, "ABNORMALLY TERMINATED"))
                        {
                            foundTask.IsAbend = true;
                        }
                        else if (args.RI.MessageCode == "50108")
                        {
                            foundTask.IsInvalid = true;
                            foundTask.Ssn = "";
                            foundTask.AccountNumber = "";
                        }
                        else
                        {
                            foundTask.Ssn = args.RI.GetText(1, 9, 9);
                            foundTask.AccountNumber = args.RI.GetText(6, 10, 12).Replace(" ", "");
                        }
                        s.ContinueIterating = false;
                    }
                }
            }, iterSettings);

            return foundTask;
        }

        private void UnselectExistingTask()
        {
            args.RI.FastPath("tx3z/ctx6j;;;;");
            if (args.RI.ScreenCode == "TXX6K")
            {
                args.RI.PutText(7, 42, queue);
                args.RI.PutText(8, 42, subqueue);
                args.RI.PutText(13, 42, utId);
                args.RI.Hit(ReflectionInterface.Key.Enter);
            }
            if (args.RI.ScreenCode == "TXX6N")
                args.RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
            if (args.RI.ScreenCode == "TXX6O")
            {
                args.RI.PutText(8, 15, "", true);
                args.RI.Hit(ReflectionInterface.Key.Enter);
            }
        }

        private void SetTaskStatus(R0Task task, TaskResult result, int runHistoryId, bool cancelDuplicates)
        {
            var tasks = new List<R0Task>();
            tasks.Add(task);
            SetTaskStatus(tasks, result, runHistoryId, cancelDuplicates);
        }

        private void SetTaskStatus(IEnumerable<R0Task> tasks, TaskResult result, int runHistoryId, bool cancelDuplicates)
        {
            string ssn = tasks.FirstOrDefault().Ssn;
            if (string.IsNullOrEmpty(ssn))
            {
                ssn = tasks.FirstOrDefault().TaskControl;
            }
            List<R0Task> duplicates = new List<R0Task>();
            args.RI.FastPath(itx6xFastPath + ssn + "*");
            var iterSettings = PageHelper.IterationSettings.Default();
            iterSettings.MinRow = 8;
            iterSettings.MaxRow = 17;
            iterSettings.RowIncrementValue = 3;
            PageHelper.Iterate(args.RI, (row, s) =>
            {
                bool isBeingWorked = args.RI.CheckForText(row, 75, "W");
                var foundTask = new R0Task();
                foundTask.TaskControl = args.RI.GetText(row, 6, 20);
                foundTask.ActionRequest = args.RI.GetText(row, 27, 5);
                foundTask.DateRequested = args.RI.GetText(row, 47, 10).ToDate();
                foundTask.MonitorReason = args.RI.GetText(row + 1, 2, 78);
                var existingTask = tasks.SingleOrDefault(o => o.Equals(foundTask));
                if (existingTask != null)
                {
                    WorkedTasks.Add(existingTask);
                    if (result != TaskResult.SkipTask && !skipTaskClose)
                    {
#if !DEBUG
                        string selection = args.RI.GetText(row, 3, 2).PadLeft(2, '0');
                        args.RI.PutText(21, 18, selection, ReflectionInterface.Key.F2);
                        args.RI.PutText(8, 19, result.ToInfo().Status.ToString(), ReflectionInterface.Key.Enter);
                        if (args.RI.MessageCode != "01005")
                        {
                            args.RI.PutText(9, 19, result.ToInfo().ActionResponse, ReflectionInterface.Key.Enter);
                            if (args.RI.MessageCode != "01005")
                            {
                                string message = "Unable to " + result.ToInfo().Verb + " Task - Queue: R0, Subqueue: 01, Task Control Number: {0}, Action Request: {1}";
                                message = string.Format(message, existingTask.TaskControl, existingTask.ActionRequest);
                                args.PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);

                                args.RI.FastPath("tx3z/ctx6j");
                                args.RI.PutText(7, 42, "R0");
                                args.RI.PutText(8, 42, "01");
                                args.RI.PutText(9, 42, existingTask.TaskControl);
                                args.RI.PutText(10, 42, existingTask.ActionRequest);
                                args.RI.PutText(9, 76, "D");
                                args.RI.PutText(13, 42, utId, ReflectionInterface.Key.Enter);
                                args.RI.Hit(ReflectionInterface.Key.EndKey);
                                args.RI.PutText(13, 42, args.DA.GetReassignmentUserId(), ReflectionInterface.Key.Enter);
                                return;
                            }
                        }
                        args.RI.Hit(ReflectionInterface.Key.F12);

#endif
                    }

                    if (result == TaskResult.CompleteTask)
                        EOJ("Completed task {0}", foundTask);
                    else if (result == TaskResult.CancelTask)
                        EOJ("Cancelled task {0}", foundTask);
                    else if (result == TaskResult.SkipTask)
                        EOJ("Skipped task {0}", foundTask);
                }
                else if (cancelDuplicates)
                {
                    foundTask.Ssn = foundTask.TaskControl.Substring(0, 9);
                    if (!string.IsNullOrWhiteSpace(foundTask.Ssn))
                        duplicates.Add(foundTask);
                }

            }, iterSettings);

            if (cancelDuplicates && duplicates.Any())
            {
                EOJ("Cancelling {0} found duplicate tasks for {1}", duplicates.Count, tasks.FirstOrDefault());
                SetTaskStatus(duplicates, TaskResult.CancelTask, runHistoryId, false);
                foreach (var dupe in duplicates)
                {
                    var eoj = new TaskEoj();
                    eoj.Task = dupe;
                    eoj.EojType = EojReport.Cancelled;
                    eoj.CancelReason = "Cancelled Duplicate";
                    eoj.R0CreateDate = dupe.DateRequested;
                    args.DA.AddEojItem(runHistoryId, eoj, args.MS.MaxIncrease);
                }
            }
        }

    }
}
