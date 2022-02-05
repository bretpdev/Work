using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace ACCURINT
{
    public class RequestFileCreator
    {
        private readonly AccurintRequestFile AccurintRequestFile;
        private readonly string ScriptId;
        private readonly ReflectionInterface RI;
        private readonly string UtId;
        private readonly string[] CompassQueues = new string[] { "IC", "IM", "IO" };
        private DataAccess DA { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private RunInfo CurrentRun { get; set; }
        private bool AssignOneLinkTasks { get; set; } = false; // Used for test runs (so that tasks are assigned rather than closed, if user wishes)
        private bool AssignCompassTasks { get; set; } = false; // Assigns tasks rather than closing them (used for test)
        public RequestFileCreator(ReflectionInterface ri, AccurintRequestFile accurintRequestFile, DataAccess da, ProcessLogRun logRun, RunInfo currentRun, string scriptId, string utId)
        {
            AccurintRequestFile = accurintRequestFile;
            ScriptId = scriptId;
            UtId = utId;
            RI = ri;
            DA = da;
            LogRun = logRun;
            CurrentRun = currentRun;
        }
        /// <summary>
        /// Creates the Request File from Queue Tasks.
        /// </summary>
        public bool CreateRequestFileFromQueueTasks(bool onlyProcessFirstSelection)
        {
            if (AccurintRequestFile.Exists)
            {
                string message = $"{AccurintRequestFile.FileName} already exists.  Should new records be added to the existing file?";
                if (!Dialog.Info.YesNo(message, ScriptId))
                {
                    LogRun.AddNotification($"Accurint request file {AccurintRequestFile.FileName} already exists. User selected to add no new records to the file", NotificationType.Other, NotificationSeverityType.Informational);
                    return false;
                }
            }

            if (!HasCompassQueueAccess())
                return false;

            if (!DA.AddNewWork(CurrentRun.RunId))
                return false;

            //Work the queues.
#if !COMPASSONLY
            List<OneLinkDemosRecord> oneLinkTasks = DA.GetUnprocessedOLRecords(CurrentRun.RunId);
            bool? oneLinkResult = WorkOneLinkTasks(oneLinkTasks, onlyProcessFirstSelection);
            if (!oneLinkResult.HasValue) // Unable to unassign tasks, stop run
                return false;
#endif
            List<UheaaDemosRecord> uheaaTasks = DA.GetUnprocessedUHRecords(CurrentRun.RunId);
            WorkUheaaTasks(uheaaTasks, onlyProcessFirstSelection);

            if (AccurintRequestFile.Exists)
            {
                //Record the number of records in the file.
                string reportFolder = EnterpriseFileSystem.GetPath("EOJ_Accurint");
                string reportFile = string.Format(@"{0}Accurint Sent\Sent to Accurint Total.{1:yyyy-MM-dd-hhmm}", reportFolder, DateTime.Now);
                try
                {
                    File.WriteAllText(reportFile, AccurintRequestFile.RecordCount.ToString());
                }
                catch (Exception ex)
                {
                    LogRun.AddNotification("Error encountered while writing record count to EOJ report file.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }
                return DA.SetRequestFileInfo(CurrentRun.RunId, DA.GetRecordsSentCount(CurrentRun.RunId));
            }
            return false;
        }

        private bool HasCompassQueueAccess()
        {
            string errorMessage = $"User id {UtId} does not have access to the following Compass queue(s): ";
            bool hasAccess = true;
            foreach (string q in CompassQueues)
            {
                RI.FastPath($"tx3z/ITX6X{q};01;;");
                if (RI.MessageCode == "80014")
                {
                    hasAccess = false;
                    errorMessage += $"{q};{01}. ";
                }
            }

            if (!hasAccess)
            {
                errorMessage += "Please add access or use a different UT ID and then re-run the script.";
                LogRun.AddNotification(errorMessage, NotificationType.EndOfJob, NotificationSeverityType.Critical);
                Dialog.Warning.Ok(errorMessage);
            }
            return hasAccess;
        }

        private bool? WorkOneLinkTasks(List<OneLinkDemosRecord> oneLinkRecords, bool onlyProcessFirstSelection)
        {
            bool result = true;
            if (DataAccessHelper.TestMode)
            {
                if (!Dialog.Question.YesNo("Do you wish for this test run to close OneLINK tasks in the session?"))
                {
                    AssignOneLinkTasks = true;
                }
            }

            if (oneLinkRecords?.Count > 0)
            {
                foreach (string workGroup in oneLinkRecords.Select(p => p.WorkGroup).Distinct().ToList())
                    result &= UnassignOneLinkTasksFromUser(workGroup, "SKP");

                if (!result)
                    return null;

                result &= WorkAcurintsQueue(onlyProcessFirstSelection, oneLinkRecords.Where(p => p.WorkGroup.ToUpper() == "ACURINTS").ToList());
                result &= WorkAcurint2Queue(onlyProcessFirstSelection, oneLinkRecords.Where(p => p.WorkGroup.ToUpper() == "ACURINT2").ToList());
            }
            return result;
        }

        private bool WorkUheaaTasks(List<UheaaDemosRecord> uheaaTasks, bool onlyProcessFirstSelection)
        {
            if (DataAccessHelper.TestMode)
            {
                if (Dialog.Question.YesNo("Do you want the script to unassign Compass tasks that were assigned to the BU manager on a previous run?"))
                {
                    foreach (string q in CompassQueues)
                    {
                        UnassignCompassManagerTasks(q);
                    }
                }

                if (!Dialog.Question.YesNo("Do you wish for this test run to close UHEAA tasks in the session? If not, the task will be reassigned rather than closed."))
                {
                    AssignCompassTasks = true;
                }
            }

            if (uheaaTasks?.Count > 0)
            {
                return WorkCompassAcurintQueues(uheaaTasks, onlyProcessFirstSelection);
            }
            return true;
        }

        private bool? EnterOneLinkTaskInSession(OneLinkDemosRecord task)
        {
            //Reassign task to user via LP8Y
            RI.FastPath($"LP8YC{task.Department};{task.WorkGroup};;{task.Ssn}");

            if (RI.AltMessageCode == "47004" || RI.AltMessageCode == "47432") // 47004 = No records found, 47432 = Invalid combination entered (which will encountered if the task is not found and there are no other tasks for that work group)
            {
                LogRun.AddNotification("Task not found in the session", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                DA.SetOLDeleted(task.DemosId);
                return null;
            }

            //Get first available task for borrower
            int row = 7;
            while (RI.AltMessageCode != "46004") // 46004 code = No More Data to Display
            {
                if (RI.CheckForText(row, 33, "A"))
                {
                    RI.PutText(row, 38, UtId, Key.F6);
                    if (RI.AltMessageCode != "49000") // 49000 code = Data Successfully Updated
                    {
                        LogRun.AddNotification($"User {UtId} encountered an issue trying to assign task {task.WorkGroup} for {task.Ssn} to itself. Success code not received.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        return false;
                    }

                    RI.FastPath($"LP9AC{task.WorkGroup};;{task.Department};Y");
                    if (!RI.CheckForText(1, 9, task.WorkGroup.ToUpper()) || !RI.CheckForText(1, 77, "TASK"))
                    {
                        LogRun.AddNotification($"User {UtId} encountered an issue trying to assign task {task.WorkGroup} for {task.Ssn} to itself. Task screen was not displayed in session.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        return false; // Task wasn't displayed
                    }

                    return true;
                }
                else if (RI.CheckForText(row, 33, " "))
                {
                    LogRun.AddNotification($"User {UtId} encountered an issue trying to assign task {task.WorkGroup} for {task.Ssn} to itself. The status field was blank in the session.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false;
                }
                else
                {
                    row++;
                    if (row == 21)
                    {
                        RI.Hit(Key.F8);
                        row = 7;
                    }
                }
            }
            return false; // Couldn't find the task
        }

        private bool UnassignOneLinkTasksFromUser(string workGroup, string department)
        {
            //Go to LP8Y for working status tasks
            RI.FastPath($"LP8YC{department};{workGroup};{UtId};;;W");
            if (RI.AltMessage != "47004" && RI.GetText(7, 33, 1) == "W") // 47004 = No Records found
                RI.PutText(7, 33, "A", Key.F6); // Unassign task UT ID has in working status

            //Go to LP8Y for available/assigned status tasks
            RI.FastPath($"LP8YC{department};{workGroup};{UtId};;;A");
            if (RI.AltMessageCode == "47004")
                return true; // No tasks to unassign

            int row = 7; // We don't have to update rows because session moves records up after one is unassigned
            while (RI.GetText(row, 33, 1) == "A")
            {
                RI.PutText(row, 38, "", Key.F6, true); // Clear out USER field in session to unassign
                if (RI.AltMessageCode != "49000" && RI.AltMessageCode != "47450") // 49000 code = Data Successfully Updated; 47450 = No current tasks founds (displayed after all unassigned)
                {
                    LogRun.AddNotification($"Unable to unassign OneLINK tasks {workGroup} from the user {UtId}. Please unassign the tasks manually and then re-run the script.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
                if (RI.AltMessageCode == "47450")
                    break;
            }

            return true;
        }

        private bool WorkAcurintsQueue(bool onlyProcessFirstSelection, List<OneLinkDemosRecord> oneLinkRecords)
        {
            bool result = true;
            foreach (OneLinkDemosRecord record in oneLinkRecords)
            {
                if (!record.TaskCompletedAt.HasValue) // Only enter task if it is not already closed. Accomodates re-runs after user manually closed task
                {
                    bool? navigationResult = EnterOneLinkTaskInSession(record);
                    if (!navigationResult.HasValue) // Task not found. Already worked.
                        continue;
                    else if (!navigationResult.Value)
                    {
                        result &= false;
                        continue;
                    }
                }

                if (record.SendToAccurint.Value && !record.AddedToRequestFileAt.HasValue)
                {
                    result &= OneLinkAddToRequestFileAndCompleteTask(record);
                }
                else if (record.SendToAccurint.Value && (!record.TaskCompletedAt.HasValue || !record.RequestCommentAdded.HasValue))
                {
                    result &= OneLinkCompleteTask(record, "KUBSS");
                }
                else if (!record.SendToAccurint.Value && (!record.TaskCompletedAt.HasValue || !record.RequestCommentAdded.HasValue))
                {
                    result &= OneLinkCompleteTask(record, "KUBNU");
                }
                else
                {
                    LogRun.AddNotification($"Error working the record {record}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    result &= false;
                }

                if (onlyProcessFirstSelection)
                    break;
            }
            return result;
        }

        private bool WorkAcurint2Queue(bool onlyProcessFirstSelection, List<OneLinkDemosRecord> oneLinkRecords)
        {
            bool result = true;
            foreach (OneLinkDemosRecord record in oneLinkRecords)
            {
                if (!record.TaskCompletedAt.HasValue)
                {
                    bool? navigationResult = EnterOneLinkTaskInSession(record);
                    if (!navigationResult.HasValue) // Task not found. Already worked.
                        continue;
                    else if (!navigationResult.Value)
                    {
                        result &= false;
                        continue;
                    }
                }

                if (!record.AddedToRequestFileAt.HasValue)
                {
                    result &= OneLinkAddToRequestFileAndCompleteTask(record);
                }
                else if (!record.TaskCompletedAt.HasValue || !record.RequestCommentAdded.HasValue)
                {
                    result &= !OneLinkCompleteTask(record, "KUBSS");
                }

                if (onlyProcessFirstSelection)
                    break;
            }
            return result;
        }

        private bool WorkCompassAcurintQueues(List<UheaaDemosRecord> uheaaTasks, bool onlyProcessFirstSelection)
        {
            bool result = true;
            foreach (UheaaDemosRecord task in uheaaTasks)
            {
                if (!task.TaskCompletedAt.HasValue) // Only enter task if it needs to be closed in session. Allows for re-runs where user manually closed task.
                {
                    bool? enteredTask = EnterUheaaTaskInSession(task);
                    if (enteredTask.HasValue && !enteredTask.Value) // Unknown error prevented session from entering task
                    {
                        result &= false;
                        continue;
                    }

                    if (!enteredTask.HasValue) // Task did not exist in session, so we don't process it 
                    {
                        LogRun.AddNotification("Task did not exist in the session", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        DA.SetUHDeleted(task.DemosId);
                        continue;
                    }
                }

                if (!task.AddedToRequestFileAt.HasValue)
                {
                    result &= CompassAddToRequestFileAndCompleteTask(task);
                }
                else if (!task.TaskCompletedAt.HasValue || !task.RequestArcId.HasValue)
                {
                    result &= !CompassCompleteTask(task);
                }

                if (onlyProcessFirstSelection)
                    break;
            }
            return result;
        }

        private bool? EnterUheaaTaskInSession(UheaaDemosRecord uheaaTask)
        {
            RI.FastPath($"TX3Z/ITX6X{uheaaTask.Queue};{uheaaTask.SubQueue};{uheaaTask.TaskControlNumber};{uheaaTask.TaskRequestArc}");

            if (RI.MessageCode == "01020")
            {
                LogRun.AddNotification($"Task not found. Screen Code:{RI.ScreenCode}. Session Message:{RI.Message}; UserId:{UtId}; task: {uheaaTask}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                DA.SetUHDeleted(uheaaTask.DemosId);
                return null; // Task not found
            }

            if (!RI.CheckForText(1, 74, "TXX71"))
            {
                LogRun.AddNotification($"Expected to be on screen TXX71, but current screen is {RI.ScreenCode}. Session Message:{RI.Message}; UserId:{UtId}; task: {uheaaTask}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }
            RI.PutText(21, 18, "01", Key.Enter, true);
            return RI.GetText(1, 4, 5) == "ITD0L"; // ITD0L = The screen the script should be on
        }

        private bool OneLinkAddToRequestFileAndCompleteTask(OneLinkDemosRecord record)
        {
            bool result;
            try
            {
                if (!AddDemosToRequestFile(GetOneLinkSessionDemos(record.Ssn), record.DemosId))
                {
                    LogRun.AddNotification($"Error adding record to Accurint request file. Record: {record}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
                result = OneLinkCompleteTask(record, "KUBSS");
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"Exception occurred for task {record}. Exception: {ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                string message = string.Format("The record could not be added to the {0} request file.  Resolve the problem and run the process again.", AccurintRequestFile.FileName);
                message += "  When you run the script again, click Yes to continue when prompted to verify that records should be added to the existing file.";
                Dialog.Warning.Ok(message);
                return false;
            }
            return result;
        }

        private SystemBorrowerDemographics GetOneLinkSessionDemos(string ssn)
        {
            SystemBorrowerDemographics demos = RI.GetDemographicsFromLP22(ssn);
            if (demos != null)
                demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            return demos;
        }

        private bool OneLinkCompleteTask(OneLinkDemosRecord oneLinkTask, string actionCode)
        {
            bool successfullyCommentedAndClosed = true;

            // Add the system comment if it wasn't added already
            if (!oneLinkTask.RequestCommentAdded.HasValue || oneLinkTask.RequestCommentAdded.Value == false)
            {
                if (!RI.AddCommentInLP50(oneLinkTask.Ssn, "AM", "10", actionCode, ScriptId, ScriptId))
                {
                    string message = $"Error adding Action Code: {actionCode} to LP50 for account {oneLinkTask.AccountNumber}; Session Message: {RI.GetText(22, 3, 70)}";
                    LogRun.AddNotification($"{message}. Task being worked: {oneLinkTask}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    successfullyCommentedAndClosed = false;
                }
                else
                    successfullyCommentedAndClosed &= DA.SetOneLinkRequestCommentAdded(oneLinkTask.DemosId);
            }

            if (DataAccessHelper.TestMode && AssignOneLinkTasks)
            {
                UnassignOneLinkTasksFromUser(oneLinkTask.WorkGroup, oneLinkTask.Department);
                return successfullyCommentedAndClosed &= DA.SetOLTaskCompleted(oneLinkTask.DemosId);
            }

            // If task not closed in session, close it
            if (!oneLinkTask.TaskCompletedAt.HasValue)
            {
                //Complete the queue task.
                RI.FastPath("LP9AC");
                RI.Hit(ReflectionInterface.Key.F6);
                if (RI.AltMessageCode == "47460") // Code indicating no comment found on account
                {
                    RI.Hit(ReflectionInterface.Key.F2);
                    RI.Hit(ReflectionInterface.Key.F7);
                    RI.AddCommentInLP50(oneLinkTask.Ssn, "AM", "10", actionCode, ScriptId, ScriptId);
                    RI.FastPath("LP9AC");
                    RI.Hit(ReflectionInterface.Key.F6);
                }

                // Warn the user and end script if task is not completed
                if (RI.AltMessageCode != "49000") // 49000 = Success code
                {
                    LogRun.AddNotification($"Error occurred when processing {oneLinkTask.WorkGroup} for borrower {oneLinkTask.Ssn}. Could not close task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    string message = "The task was not completed.  The script cannot continue until the task is complete.  Resolve the problem and run the process again.";
                    message += "  When you run the script again, click Yes to continue when prompted to verify that records should be added to the existing file.";
                    Dialog.Warning.Ok(message);
                    DA.SetOLTaskCompleted(oneLinkTask.DemosId);
                    return false;
                }
                else
                    successfullyCommentedAndClosed &= DA.SetOLTaskCompleted(oneLinkTask.DemosId);
            }

            return successfullyCommentedAndClosed;
        }

        private bool CompassAddToRequestFileAndCompleteTask(UheaaDemosRecord uheaaTask)
        {
            bool result;
            try
            {
                if (!AddDemosToRequestFile(GetUheaaSessionDemos(uheaaTask.Ssn), uheaaTask.DemosId, false))
                {
                    LogRun.AddNotification($"Error adding UHEAA record to Accurint request file. Record: {uheaaTask}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
                result = CompassCompleteTask(uheaaTask);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification($"Exception occurred for UHEAA task {uheaaTask}. Exeption: {ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                string message = string.Format("The record could not be added to the {0} request file.  Resolve the problem and run the process again.", AccurintRequestFile.FileName);
                message += "  When you run the script again, click Yes to continue when prompted to verify that records should be added to the existing file.";
                Dialog.Warning.Ok(message + $"See ProcessLogId:{LogRun.ProcessLogId} for more details");
                return false;
            }
            return result;
        }

        private SystemBorrowerDemographics GetUheaaSessionDemos(string ssn)
        {
            SystemBorrowerDemographics demos = RI.GetDemographicsFromTx1j(ssn);
            if (demos != null)
                demos.AccountNumber = demos.AccountNumber.Replace(" ", "");
            return demos;
        }

        private bool CompassCompleteTask(UheaaDemosRecord uheaaTask)
        {
            bool successfullyCommentedAndClosed = true;
            int arcAddProcessingId;

            //Add the system comment.
            if (!uheaaTask.RequestArcId.HasValue)
            {
                arcAddProcessingId = AddArc(uheaaTask, "Account sent to Accurint");
                bool result = arcAddProcessingId > 0;
                if (!result)
                {
                    ReassignCompassTaskToManager(uheaaTask.Queue);
                    DA.SetUHDeleted(uheaaTask.DemosId);
                    LogRun.AddNotification($"Error adding KUBSS ARC to UH account {uheaaTask.AccountNumber}. The following task has been reassigned: {uheaaTask}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false;
                }
                else
                {
                    result &= AddAcpComment(uheaaTask);
                    if (result)
                        successfullyCommentedAndClosed &= DA.SetUHRequestArcId(uheaaTask.DemosId, arcAddProcessingId);
                    else
                    {
                        DA.SetUHRequestArcId(uheaaTask.DemosId, arcAddProcessingId);
                        ReassignCompassTaskToManager(uheaaTask.Queue);
                        DA.SetUHDeleted(uheaaTask.DemosId);
                        LogRun.AddNotification($"Error adding ACP comment to UH account {uheaaTask.AccountNumber}. The following task has been reassigned: {uheaaTask}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        return false;
                    }
                }
            }

            if (DataAccessHelper.TestMode && AssignCompassTasks)
            {
                ReassignCompassTaskToManager(uheaaTask.Queue);
                return successfullyCommentedAndClosed &= DA.SetUHTaskCompleted(uheaaTask.DemosId);
            }

            //Complete the queue task.
            if (!uheaaTask.TaskCompletedAt.HasValue)
            {
                RI.FastPath("TX3Z/ITX6X" + uheaaTask.Queue + ";01");
                PageHelper.Iterate(RI, (row, settings) =>
                {
                    string status = RI.GetText(row, 75, 1);
                    if (status == "W")
                    {
                        string sel = RI.GetText(row, 3, 2);
                        RI.PutText(21, 18, sel, ReflectionInterface.Key.F2);
                        if (RI.MessageCode == "03363") //loan has been deconverted
                        {
                            ReassignCompassTaskToManager(uheaaTask.Queue);
                            DA.SetUHDeleted(uheaaTask.DemosId);
                            LogRun.AddNotification($"Error closing the task for UH account {uheaaTask.AccountNumber}. The following task has been reassigned: {uheaaTask}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        }
                        else
                        {
                            RI.PutText(8, 19, "C");
                            RI.PutText(9, 19, "CNTCT");
                            RI.Hit(ReflectionInterface.Key.Enter);
                        }
                        settings.ContinueIterating = false;
                    }
                });
                if (RI.MessageCode != "01005") // 01005 = RECORD SUCCESSFULLY CHANGED
                {
                    LogRun.AddNotification($"Error occurred when processing UH task {uheaaTask}. Could not close task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    string message = "The task was not completed.  The script cannot continue until the task is complete.  Resolve the problem and run the process again.";
                    message += "  When you run the script again, click Yes to continue when prompted to verify that records should be added to the existing file.";
                    Dialog.Warning.Ok(message);
                    DA.SetUHTaskCompleted(uheaaTask.DemosId); // Set task as closed since user will manually do it
                    return false;
                }
                else
                    successfullyCommentedAndClosed &= DA.SetUHTaskCompleted(uheaaTask.DemosId);
            }

            return successfullyCommentedAndClosed;
        }

        private bool AddAcpComment(UheaaDemosRecord uheaaTask)
        {
            if (uheaaTask.EndorserSsn == null)
                RI.FastPath($"TX3Z/ATC00{uheaaTask.Ssn};");
            else
            {
                RI.FastPath($"TX3Z/ATC00{uheaaTask.Ssn}");
                RI.PutText(7, 41, uheaaTask.EndorserSsn);
            }

            RI.PutText(19, 38, "5", ReflectionInterface.Key.Enter);

            if (RI.ScreenCode == "TCX27")
            {
                RI.PutText(9, 2, "N", ReflectionInterface.Key.Enter);
                int row = 16;
                for (int i = 0; i < 4; i++, row++)
                {
                    if (PhoneIsPopulated(row))
                        RI.PutText(row, 2, "N");
                }

                if (RI.GetText(20, 4, 1).IsIn("Y", "N"))
                    RI.PutText(20, 2, "N", ReflectionInterface.Key.Enter);

                if (RI.GetText(1, 74, 5) == "TCX13")
                {
                    string skipTarget = uheaaTask.Queue != null && uheaaTask.Queue == "IO" ? "endorser" : "borrower"; // IO tasks are the only tasks that don't target the borrower (they target the endorser)
                    RI.PutText(18, 2, $"Requesting demographic information from accurint directory assistance for skip {skipTarget}", ReflectionInterface.Key.Enter);
                    return (RI.MessageCode == "01004"); // 01004= Successfully added
                }
            }
            return false;
        }

        private bool PhoneIsPopulated(int row)
        {
            return (RI.GetText(row, 4, 1) != "_" || RI.GetText(row, 9, 1).IsNumeric());
        }

        private int AddArc(UheaaDemosRecord uheaaTask, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = uheaaTask.AccountNumber,
                Arc = "KUBSS",
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ProcessOn = DateTime.Now,
                RecipientId = "",
                ScriptId = ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (result == null || !result.ArcAdded)
            {
                string message = $"There was an error adding a KUBSS ARC to borrower: {uheaaTask.AccountNumber}. Comment: {comment}; Error(s): {string.Join(",", result.Errors)}";
                Dialog.Error.Ok(message);
                return 0;
            }
            return result.ArcAddProcessingId;
        }

        private void ReassignCompassTaskToManager(string queue)
        {
            RI.FastPath("TX3Z/CTX6J;;;;;");  //blanking out anything still remaining in the fastpath
            RI.PutText(7, 42, queue);
            RI.PutText(8, 42, "01");
            RI.PutText(13, 42, UtId);
            RI.Hit(ReflectionInterface.Key.Enter);
            var managerId = DA.GetBuManager();
            RI.PutText(8, 15, managerId, ReflectionInterface.Key.Enter);
            //warn the user and end script if task is not completed
            if (RI.MessageCode != "01005")
            {
                string message = "The task was not reassigned to the BU manager successfully. Please reassign the task manually and then re-run the script.";
                message += "  When you run the script again, click Yes to continue when prompted to verify that records should be added to the existing file.";
                Dialog.Warning.Ok(message);
                throw new EndDLLException(message);
            }
        }

        /// <summary>
        /// Unassigns the tasks that were reassigned to the BU manager and assigns them to the user.
        /// </summary>
        /// <param name="queue"></param>
        private void UnassignCompassManagerTasks(string queue)
        {
            bool lastTask = false;
            RI.FastPath("TX3Z/CTX6J;;;;;");  //blanking out anything still remaining in the fastpath
            RI.PutText(7, 42, queue);
            RI.PutText(8, 42, "01");
            var managerId = DA.GetBuManager();
            string selection = "";
            RI.PutText(13, 42, "", ReflectionInterface.Key.Enter);

            while (!lastTask)
            {
                if (RI.MessageCode == "01020")
                    break;


                if (RI.ScreenCode == "TXX6O")
                {
                    lastTask = true;
                    RI.PutText(8, 15, UtId, ReflectionInterface.Key.Enter);
                    if (RI.MessageCode != "01005")
                    {
                        string message = "The task was not reassigned.  The script cannot continue until the task is complete.  Resolve the problem and run the process again.";
                        message += "  When you run the script again, click Yes to continue when prompted to verify that records should be added to the existing file.";
                        Dialog.Warning.Ok(message);
                        throw new EndDLLException(message);
                    }
                }

                if (RI.ScreenCode == "TXX6N")
                {
                    for (int row = 10; row < 20; row+=2)
                    {
                        if (selection != "")
                            break;
                        else
                            selection = "";

                        if (RI.GetText(row, 20, 7) == UtId)
                            continue;
                        else
                            selection = RI.GetText(row - 1, 3, 1);
                    
                        if (row == 18 && selection == "")
                        {
                            RI.Hit(ReflectionInterface.Key.F8);
                            row = 10; // restart loop on new page
                            if (RI.MessageCode == "90007")
                                lastTask = true;
                        }
                    }

                    if (lastTask)
                        break;
                    
                    RI.PutText(21, 18, selection, ReflectionInterface.Key.Enter);
                    RI.PutText(8, 15, UtId, ReflectionInterface.Key.Enter);
                    if (RI.MessageCode != "01005")
                    {
                        string message = "The task was not reassigned.  The script cannot continue until the task is complete.  Resolve the problem and run the process again.";
                        message += "  When you run the script again, click Yes to continue when prompted to verify that records should be added to the existing file.";
                        Dialog.Warning.Ok(message);
                        throw new EndDLLException(message);
                    }
                    RI.Hit(ReflectionInterface.Key.F12); // Go back to session selection screen and then re-enter so session refreshes
                    RI.PutText(13, 42, "", ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F12);
                    RI.Hit(ReflectionInterface.Key.Enter);
                }
            }
        }

        public bool AddDemosToRequestFile(SystemBorrowerDemographics demos, int demosId, bool isOneLink = true)
        {
            AccurintRequestFile.AddRecord(demos.Ssn, demos.LastName, demos.FirstName, demos.DateOfBirth, demos.Address1, demos.Address2, demos.City, demos.State, demos.ZipCode);
            if (isOneLink)
                return DA.SetOLRecordAddedToRequestFile(demosId) && DA.AddSentDemos(demosId, "OneLINK", demos.AccountNumber, demos.Address1, demos.Address2, demos.City, demos.State, demos.ZipCode, null, demos.IsValidAddress);
            else
                return DA.SetUHRecordAddedToRequestFile(demosId) && DA.AddSentDemos(demosId, "UHEAA", demos.AccountNumber, demos.Address1, demos.Address2, demos.City, demos.State, demos.ZipCode, null, demos.IsValidAddress);
        }

    }
}
