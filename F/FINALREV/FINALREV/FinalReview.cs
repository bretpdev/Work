using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace FINALREV
{
    public class FinalReview
    {
        public static string ScriptId { get; set; } = "FINALREV";
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }
        public BorrowerRecord Borrower { get; set; }
        public int TaskCount { get; set; }
        public int ERROR { get; set; } = 1;
        public int SUCCESS { get; set; } = 0;

        public enum SkipSystem
        {
            Compass,
            Onelink,
            CheckLc05,
            Both,
            None
        }

        public enum RecoveryStep
        {
            All = 0,
            KORGLNDR = 1,
            KSCHLLTR = 2,
            KENDORSR = 3,
            BRWRCALS = 4,
            ACURINT2 = 5,
            REF_REV = 6,
            LP50REC = 7,
            KRREFREC = 8
        }

        public FinalReview(ReflectionInterface ri) => RI = ri;

        public int Process(bool pauseBetweenRecords)
        {
            int hadError = SUCCESS;
            int assignedCount = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev ? 2 : 20;
            DA = new DataAccess(RI.LogRun);
            Borrower = DA.CheckUnprocessedBorrower();
            for (int i = 0; i < 2; i++)
            {
                bool addTask = true;
                bool addNext = false;
                string queue = (i == 0 ? "FINALRVW" : "KFINALRV");
                if (!UnassignTasks(queue))
                {
                    hadError = ERROR;
                    continue;
                }
                WriteLine($"Checking for account to processing in Queue: {queue}");
                RI.FastPath($"LP9AC{queue}");
                if (HasAssigned(queue))
                {
                    hadError = ERROR;
                    continue;
                }
                if (RI.CheckForText(1, 67, "WORKGROUP TASK") || RI.CheckForText(1, 71, "QUEUE TASK"))
                {
                    while (!RI.AltMessageCode.IsIn("46004", "47450"))
                    {
                        GatherQueueData(queue);
                        DA.SetProcessedAt(Borrower.BorrowerRecordId);
                        WriteLine($"Finished processing queue: {queue} for borrower: {Borrower.Demos.AccountNumber}. Attempting to gather next borrower in the queue");
                        TaskCount = DA.GetTaskCount();
                        if (TaskCount % assignedCount == 0 && Borrower.SkipSystem == SkipSystem.CheckLc05) //Don't add a task if the skip system was to check LC05
                        {
                            addTask = false; //Don't add the task because of the status
                            addNext = true; //Add on the next even though it is not the 20th
                        }
                        else if ((TaskCount % assignedCount == 0 && addTask) || addNext)
                        {
                            WriteLine("Ading KSKPREVW task to 20th account");
                            RI.AddQueueTaskInLP9O(Borrower.Demos.Ssn, "KSKPREVW");
                            addTask = true;
                            addNext = false;
                        }
                        Borrower = new BorrowerRecord();
                        CheckForPause(pauseBetweenRecords);
                        if (RI.CheckForText(1, 67, "WORKGROUP TASK") || RI.CheckForText(1, 71, "QUEUE TASK"))
                        {
                            RI.Hit(ReflectionInterface.Key.F8);
                            if (RI.AltMessageCode.IsIn("46004", "47450"))
                                break;
                        }
                        else
                        {
                            RI.FastPath($"LP9AC{queue}");
                            if (!RI.CheckForText(1, 67, "WORKGROUP TASK") && !RI.CheckForText(1, 71, "QUEUE TASK"))
                                break;
                        }
                    }
                }
            }

            new PrintSchoolLetters(DA, Borrower, RI).PrintLetters();
            return hadError;
        }

        /// <summary>
        /// Check if a pause is needed between records
        /// </summary>
        private void CheckForPause(bool pauseBetweenRecords)
        {
            if (pauseBetweenRecords)
            {
                WriteLine("The script is paused to review the record. To start script, set focus on the console window and hit enter");
                ReadKey();
            }
        }

        /// <summary>
        /// Unassigns any task that is in a working status for the queue
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public bool UnassignTasks(string queue)
        {
            WriteLine($"Checking if any records are in a working status for the {queue} queue.");
            RI.FastPath($"LP8YCSKP{queue}");
            if (RI.CheckForText(1, 61, "QUEUE TASK SELECTION") && RI.CheckForText(8, 11, queue))
                RI.PutText(21, 13, "01", ReflectionInterface.Key.Enter);
            else
                return true;
            if (RI.CheckForText(1, 64, "QUEUE TASK DETAIL"))
            {
                while (RI.AltMessageCode != "46004")
                {
                    bool hadUpdate = false;
                    for (int i = 7; i < 21; i++)
                    {
                        if (!hadUpdate && RI.CheckForText(7, 33, "A", "C")) //7,33 is the first record. If it is an A or C, then there are no records being worked
                            return true;
                        if (RI.CheckForText(i, 33, "W"))
                        {
                            hadUpdate = true;
                            WriteLine($"Unassigning borrower {RI.GetText(i, 12, 9)} from user {RI.GetText(i, 38, 7)}");
                            RI.PutText(i, 33, "A");
                            RI.Hit(ReflectionInterface.Key.EndKey);
                        }
                    }
                    if (hadUpdate)
                    {
                        RI.Hit(ReflectionInterface.Key.Enter);
                        RI.Hit(ReflectionInterface.Key.F6);
                    }
                    if (RI.AltMessageCode.IsIn("49007", "47431"))
                    {
                        string message = $"There was an error unassigning queues in the {queue} queue. Please unassign all queues and start the {ScriptId} again.";
                        WriteLine(message);
                        RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        return false;
                    }
                    RI.Hit(ReflectionInterface.Key.F8);
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if there is a queue task assigned to the UT ID. If there is, it gets a new ID and tries again up to 5 times.
        /// </summary>
        private bool HasAssigned(string queue)
        {
            List<string> invalidIds = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (!RI.CheckForText(1, 9, queue))
                {
                    invalidIds.Add(RI.UserId);
                    WriteLine($"{RI.UserId} is assigned to a task in queue: {RI.GetText(1, 9, 8)}. Attempting to get a different UT ID");
                    BatchProcessingHelper helper = BatchProcessingHelper.GetNextAvailableId(Program.ScriptId, "BatchUheaa", invalidIds);
                    RI.LogOut();
                    RI.UserId = null;
                    RI.Login(helper.UserName, DA.GetPassword(helper.UserName)[0]);
                    WriteLine($"Logging in with {RI.UserId}");
                    RI.FastPath($"LP9AC{queue}");
                    helper.Connection.Close();
                }
                else
                    return false;
            }
            RI.LogRun.AddNotification($"FINALREV tried to access the {queue} queue with 5 UT IDs, {(string.Join(";", invalidIds))}, but they all had another task assigned to them", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            return true;
        }

        private void GatherQueueData(string queue)
        {
            string bSsn;
            string ssn;
            if (queue == "FINALRVW")
            {
                bSsn = RI.GetText(9, 61, 9);
                ssn = RI.GetText(5, 70, 9);
                if (bSsn != ssn)
                    Borrower.IsEndorser = true;
            }
            else
            {
                bSsn = ssn = RI.GetText(17, 70, 9);
                Borrower.IsEndorser = false;
            }
            if (ssn != Borrower?.Ssn)
                SetupBorrower(ssn);
            Borrower.Demos = RI.GetDemographicsFromLP22(ssn);
            Borrower.Demos.AccountNumber = Borrower.Demos.AccountNumber.Replace(" ", "");
            WriteLine($"Account: {Borrower.Demos.AccountNumber} ready to process");
            DetermineSystem(bSsn);
            //Thread.Sleep(2000); //Wait for 2 seconds while the activity records load in LP9A
            AddActivityRecord(bSsn);
            CancelTask(queue);
        }

        private void SetupBorrower(string ssn)
        {
            Borrower = new BorrowerRecord()
            {
                BorrowerRecordId = DA.InsertBorrowerRecord((int)RecoveryStep.All, ssn),
                Step = RecoveryStep.All
            };
        }

        /// <summary>
        /// Determines if the borrower is in Onelink, Compass or both and call the processor for that system
        /// </summary>
        private void DetermineSystem(string borSsn)
        {
            bool checkLC05 = false;
            RI.FastPath($"LP8QI{Borrower.Demos.Ssn}");
            Borrower.StartDate = RI.GetText(5, 29, 8).ToDate();
            Borrower.SkipType = RI.GetText(5, 17, 1);
            RI.FastPath($"LG10I{borSsn}");
            SkipSystem tempSystem;
            if (RI.CheckForText(1, 52, "LOAN BWR STATUS RECAP DISPLAY"))
            {
                if (!RI.CheckForText(5, 27, "700126"))
                    Borrower.SkipSystem = SkipSystem.Onelink;
                else
                    Borrower.SkipSystem = GetSkipSystem();
            }
            else
            {
                int row = 7;
                while (!RI.CheckForText(20, 3, "46004", "47004"))
                {
                    if (!RI.CheckForText(row, 46, "700126"))
                        tempSystem = SkipSystem.Onelink;
                    else
                    {
                        RI.PutText(19, 15, RI.GetText(row, 5, 2), ReflectionInterface.Key.Enter);
                        tempSystem = GetSkipSystem();
                        RI.Hit(ReflectionInterface.Key.F12);
                    }

                    if (tempSystem == SkipSystem.CheckLc05)
                        checkLC05 = true;
                    else if (tempSystem != Borrower.SkipSystem && Borrower.SkipSystem != SkipSystem.None)
                        Borrower.SkipSystem = SkipSystem.Both;
                    else
                        Borrower.SkipSystem = tempSystem;
                    row++;
                    if (RI.GetText(row, 5, 2) == "")
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                    }
                }
            }

            if (checkLC05 || Borrower.SkipSystem == SkipSystem.CheckLc05)
            {
                RI.FastPath($"LC05I{Borrower.Demos.Ssn}");
                if (RI.CheckForText(1, 62, "DEFAULT/CLAIM RECAP"))
                    RI.PutText(21, 13, "01", ReflectionInterface.Key.Enter);
                DateTime claimDate = RI.GetText(5, 13, 8).ToDate();
                RI.Hit(ReflectionInterface.Key.F8);
                bool claimStatus = false;
                while (RI.AltMessageCode != "46004")
                {
                    if (RI.GetText(5, 13, 8).ToDate() > claimDate)
                    {
                        claimDate = RI.GetText(5, 13, 8).ToDate();
                        claimStatus = !(RI.CheckForText(4, 10, "04", "03") && RI.GetText(19, 73, 8).IsPopulated());
                    }
                    RI.Hit(ReflectionInterface.Key.F8);
                }
                tempSystem = (claimStatus ? SkipSystem.Compass : SkipSystem.Onelink);
                Borrower.SkipSystem = (tempSystem != Borrower.SkipSystem && Borrower.SkipSystem != SkipSystem.CheckLc05) ? SkipSystem.Both : tempSystem;
            }

            WriteLine($"Processing borrower: {Borrower.Demos.AccountNumber} in {(Borrower.SkipSystem == SkipSystem.Onelink ? "Onelink" : "Compass")}");
            if (Borrower.SkipSystem == SkipSystem.Onelink)
                new OneLinkProcess(RI, Borrower).Process();
            else
                new CompassProcess(RI, Borrower).Process();
        }

        /// <summary>
        /// Gets the status from LG10 and determines which system
        /// </summary>
        private SkipSystem GetSkipSystem()
        {
            string status = RI.GetText(11, 59, 2);
            if (status == "CP")
                return SkipSystem.CheckLc05;
            else if (status.IsIn("AE", "AL", "CA", "DN", "PC", "PN", "PM", "PF", "UA", "UB", "UC", "UD", "UI"))
                return SkipSystem.Onelink;
            else
                return SkipSystem.Compass;
        }

        /// <summary>
        /// Add activity records depending on the pat
        /// </summary>
        private void AddActivityRecord(string ssn)
        {
            if (Borrower.Step < RecoveryStep.LP50REC)
            {
                if (Borrower.TaskAdded && Borrower.IsEndorser)
                    AddLp50("KUEFR", "additional skip events requested", ssn);
                else if (Borrower.OLTaskNeeded && Borrower.IsEndorser)
                    AddLp50("KUEFR", "", ssn);
                else if (!Borrower.TaskAdded && !Borrower.OLTaskNeeded && Borrower.IsEndorser)
                    AddLp50("KSEFR", "", ssn);
                else if (Borrower.TaskAdded && !Borrower.IsEndorser)
                    AddLp50("KUBFR", "additional skip events requested");
                else if (Borrower.OLTaskNeeded && !Borrower.IsEndorser)
                    AddLp50("KUBFR");
                else if (!Borrower.TaskAdded && !Borrower.OLTaskNeeded && !Borrower.IsEndorser)
                    AddLp50("KSBFR");
                Borrower.UpdateStep(DA, RecoveryStep.LP50REC);
            }
        }

        /// <summary>
        /// Add the comment using the session
        /// </summary>
        private void AddLp50(string actionCode, string comment = "", string ssn = "")
        {
            WriteLine($"Adding {actionCode} in LP50 for borrower: {Borrower.Demos.AccountNumber}");
            string borSsn = ssn.IsPopulated() ? ssn : Borrower.Demos.Ssn;
            string endSsn = ssn.IsNullOrEmpty() ? Borrower.Demos.Ssn : ssn;
            RI.AddCommentInLP50(borSsn, "AM", "36", actionCode, comment, ScriptId);
        }

        /// <summary>
        /// Cancel the task if the skip table row has been deleted for the task
        /// </summary>
        private void CancelTask(string queue)
        {
            RI.FastPath($"LP9AC{queue}");
            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev)
            {
                if (Dialog.Question.YesNo("Do you want to close the task?"))
                    RI.Hit(ReflectionInterface.Key.F6);
            }
            else
                RI.Hit(ReflectionInterface.Key.F6);

            if (RI.AltMessageCode.IsIn("47461", "49230", "47460"))
            {
                RI.FastPath($"LP8YCSKP;{queue};;{Borrower.Demos.Ssn}");
                if (RI.CheckForText(1, 64, "QUEUE TASK DETAIL"))
                {
                    RI.PutText(7, 33, "A", ReflectionInterface.Key.F6); //Make the task available
                    RI.PutText(7, 33, "X", ReflectionInterface.Key.F6); //Cancel the task
                    if (RI.AltMessageCode != "49000")
                    {
                        string message = $"There was a error canceling the queue: {queue} for borrower: {Borrower.Demos.AccountNumber}.";
                        RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        WriteLine(message);
                        return;
                    }
                }
            }
            WriteLine($"Canceling task: {queue} for borrower: {Borrower.Demos.AccountNumber}");
        }
    }
}