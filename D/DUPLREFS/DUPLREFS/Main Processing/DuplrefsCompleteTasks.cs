using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DUPLREFS
{
    public class DuplrefsCompleteTasks : ScriptBase
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        private bool UserRequestedScriptEnd { get; set; }
        private bool CloseTasks { get; set; } = true;
        public string Queue { get; set; } = DataAccessHelper.TestMode ? "XDUPREFX" : "DUPLREFS";

        public DuplrefsCompleteTasks(ReflectionInterface ri) : base(ri, "DUPLREFS", DataAccessHelper.Region.Uheaa)
        {
            LogRun = RI.LogRun ?? new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            DA = new DataAccess(LogRun);
        }

        public override void Main()
        {
            if (Dialog.Info.OkCancel($"This script assists the user to complete tasks in the {Queue} queue.  Click OK to continue or Cancel to Quit.", "Run the DUPLREFS Script?"))
                if (DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                    ProcessTasks();
            EndScriptRun();
        }


        /// <summary>
        /// Driver method for working tasks. Grabs the task
        /// from the session, enters it, and then sends it off
        /// to child method to work. Utilizes helper methods
        /// to notify user if there are no tasks to work,
        /// or if they have an unresolved task.
        /// 
        /// After each task is worked, the user is asked if 
        /// they wish to continue processing.
        /// </summary>
        private void ProcessTasks()
        {
            if (DataAccessHelper.TestMode)
                CloseTasks = Dialog.Info.YesNo("Do you want the script to close any tasks in OneLINK?", "Close Tasks?");
            int taskCount = 0;
            DA.CleanUp();
            if (!CloseTasks)
                UnassignTasks();
            while (!UserRequestedScriptEnd)
            {
                RI.FastPath($"LP9AC{Queue}");
                if (!TaskExists())
                {
                    PromptEndOfScriptRun(taskCount);
                    return;
                }
                if (HasUnresolvedTask())
                    return;

                ProcessTask();
                taskCount++;
                if (!UserRequestedScriptEnd)
                    ContinueProcessingPrompt();
            }
        }

        /// <summary>
        /// Indicates whether one or more DUPLREFS tasks exists in the session,
        /// per the LP9A screen. If a task exists, returns true; else, false.
        /// </summary>
        private bool TaskExists()
        {
            if (RI.CheckForText(22, 3, "47423") || RI.CheckForText(22, 3, "47420"))
                return false;
            return true;
        }

        /// <summary>
        /// Prompts the user when there are no more tasks in the queue.
        /// </summary>
        private void PromptEndOfScriptRun(int taskCount)
        {
            string comment = (taskCount > 0) ? $"There are no more tasks in the {Queue} queue. A total of {taskCount} tasks were worked. Processing is complete" : $"No tasks in the {Queue} queue were found. Processing is complete.";
            Dialog.Info.Ok(comment);
        }

        /// <summary>
        /// Indicates whether the user has a task that has not been completed.
        /// True if so, false otherwise.
        /// </summary>
        private bool HasUnresolvedTask()
        {
            if (!RI.CheckForText(1, 9, Queue))
            {
                Dialog.Warning.Ok($"You have an unresolved task in the {RI.GetText(1, 9, 8)} queue. You must complete the task before working the pending pdem queue. Please restart the script after that task has been resolved.", "Unresolved Task");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes a specific task that was already entered in the session.
        /// </summary>
        private void ProcessTask()
        {
            string ssn = RI.GetText(5, 70, 9);

            Borrower bwr = GetBorrower(ssn);
            if (bwr != null)
            {
                if (bwr.References == null || bwr?.References.Count() <= 1) // Only one or less ref means we don't need to process for dupe refs
                {
                    CommentAndCloseTask(bwr);
                    return;
                }
                else if (bwr?.References?.Count() > 1 && bwr.DuplicateReferences?.Count() == 0)
                {
                    CheckForDuplicateReferences(bwr);
                    DA.InsertReferenceRecords(bwr);
                }

                bool exactDupeResult = ProcessDuplicateReferences(bwr);
                if (!exactDupeResult)
                {
                    IssueProcessingErrorPrompt(exactDupeResult, bwr);
                    RI.PauseForInsert();
                    return;
                }

                bool? possibleDupeResult = ProcessPossibleDuplicateRefs(bwr);
                if (possibleDupeResult == null) // User cancelled script run
                {
                    UserRequestedScriptEnd = true;
                    return;
                }


                if (possibleDupeResult == false)
                {
                    IssueProcessingErrorPrompt(possibleDupeResult, bwr);
                    RI.PauseForInsert();
                    return;
                }
                else if (bwr.PossibleDuplicateReferences == null || bwr.PossibleDuplicateReferences?.Count == 0)
                    Dialog.Info.Ok($"The account {bwr.AccountNumber} did not have any duplicate references that required a review.", "No Possible Duplicates Found!");

                SetNonUpdatedReferencesProcessed(bwr.References.Where(p => !p.Duplicate && !p.PossibleDuplicate && !p.ProcessedAt.HasValue).ToList());
                ReviewReferences(bwr);
                CommentAndCloseTask(bwr);
            }
        }

        /// <summary>
        /// Driver method to add a comment via ArcAddProcessing and then have the task closed.
        /// </summary>
        private void CommentAndCloseTask(Borrower bwr)
        {
            RI.FastPath($"LP9AC{Queue}");
            Thread.Sleep(2000);
            AddComment(bwr, bwr.Ssn, "KGNRL", "dup ref completed");
            if (CloseTasks)
            {
                if (!CloseTask())
                    PromptAndPauseForManualReconciliation(bwr, $"Error attempting to close the task where BorrowerQueueId = {bwr.BorrowerQueueId}", NotificationSeverityType.Warning, "The script was unable to close the task. Please close the task manually and then press Insert when done.", "Manually Close Task");
            }
            else
                AssignTask();
            if (!DA.SetBorrowerProcessedAt(bwr.BorrowerQueueId))
                LogRun.AddNotification($"Unable to set BorrowerQueueId: {bwr.BorrowerQueueId} as processed in the OLS.duplrefs.BorrowerQueue table.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }

        /// <summary>
        /// Prompts user when duplicate reference processing hits a snag.
        /// Also, results in records being set as manually processed.
        /// </summary>
        private void IssueProcessingErrorPrompt(bool? result, Borrower bwr)
        {
            if (result == null)
            {
                string plrComment = $"User cancelled script run for BorrowerQueueId {bwr.BorrowerQueueId}. Prompting user to work the task manually";
                string prompt = $"You canceled the execution of the script while working on the account {bwr.AccountNumber}. Please work and complete the task manually. Hit Insert when you have completed the task.";
                PromptAndPauseForManualReconciliation(bwr, plrComment, NotificationSeverityType.Informational, prompt, "Script Run Cancellation");
            }
            else if (result == false)
            {
                string plrComment = $"An error for BorrowerQueueId: {bwr.BorrowerQueueId} occurred while trying to process duplicate references. Prompting user to work task manually.";
                string prompt = $"There was an error encountered in the session while working on the account {bwr.AccountNumber}. Please work and complete the task manually. Hit Insert when you are completed the task.";
                PromptAndPauseForManualReconciliation(bwr, plrComment, NotificationSeverityType.Critical, prompt, "Script Run Cancellation");
            }
        }

        /// <summary>
        /// Sets session field value according to how reference demos were
        /// updated. Each reference then has a KDUPL comment left on them.
        /// </summary>
        private void ReviewReferences(Borrower bwr)
        {
            bwr.References = DA.GetReferences(bwr.BorrowerQueueId); // Refresh references to gather changes made by user (e.g. RefStatus might have changed)
            foreach (Reference rfr in bwr.References.Where(p => !p.Lp2fProcessedAt.HasValue).ToList())
            {
                RI.FastPath($"LP2CC{bwr.Ssn}{rfr.RefId}");

                // These two scenarios will occur if user in-/activated a reference during their review portion
                if (rfr.RefStatus == "A" && RI.CheckForText(6, 67, "I"))
                    rfr.RefStatus = "nI";
                if (rfr.RefStatus == "I" && RI.CheckForText(6, 67, "A"))
                    rfr.RefStatus = "A";

                if ((rfr.Duplicate || rfr.PossibleDuplicate) && rfr.RefStatus == "I" && RI.CheckForText(6, 67, "I")) // We know that only active status references are considered dupes, so if it is a dupe, it had RefStatus == "A"
                    rfr.RefStatus = "nI";

                bool finished = false;
                if (rfr.RefStatus == "A")
                {
                    RI.Hit(ReflectionInterface.Key.F11);
                    NavigateToLP2F();
                    while (!finished)
                    {
                        Coordinate coord = RI.FindText("_", 6, 2);
                        if (coord == null)
                        {
                            RI.Hit(ReflectionInterface.Key.Enter);
                            RI.Hit(ReflectionInterface.Key.F8);
                            if (RI.CheckForText(22, 3, "46004"))
                                finished = true;
                        }
                        else
                        {
                            RI.PutText(coord.Row, coord.Column, "S");
                        }
                    }
                    RI.Hit(ReflectionInterface.Key.F12);
                }
                else if (rfr.RefStatus == "nI")
                {
                    RI.Hit(ReflectionInterface.Key.F11);
                    NavigateToLP2F();
                    while (!finished)
                    {
                        Coordinate coord = RI.FindText(" S ", 6, 1);
                        if (coord == null)
                        {
                            RI.Hit(ReflectionInterface.Key.Enter);
                            RI.Hit(ReflectionInterface.Key.F8);
                            if (RI.CheckForText(22, 3, "46004"))
                                finished = true;
                        }
                        else
                        {
                            RI.PutText(coord.Row, coord.Column + 1, "*");
                        }
                    }
                    RI.Hit(ReflectionInterface.Key.F12);
                }
                AddComment(rfr, rfr.RefId, "KDUPL", "");
                DA.SetReferenceLp2fProcessedAt(rfr.ReferenceQueueId);
            }
        }

        /// <summary>
        /// Verifies propert navigation in session to LP2F.
        /// </summary>
        private void NavigateToLP2F()
        {
            while (!RI.CheckForText(1, 2, "LP2F"))
            {
                Dialog.Warning.Ok("The script was not able to access LP2F to link the loans.  Click OK to correct the error and hit F11 to select LP2F.  Hit Insert when you are done.", "Unable to Access LP2F");
                RI.PauseForInsert();
            }
        }

        /// <summary>
        /// Identifies duplicate references and possible duplicate references.
        /// </summary>
        private void CheckForDuplicateReferences(Borrower bwr)
        {
            //Enhancement: Optimize this by changing to LINQ statement? Would have to be slightly different than the line below:
            //List<Reference> duplicateReferences = bor.References.GroupBy(x => new { AnonymousName = x.Name, AnonymousAddress = x.Address1.SafeSubString(0, 5), AnonymousPhone = x.PrimaryPhone.SafeSubString(0, 5) }).Where(y => y.Count() > 1).Select(z => new Reference() { Name = z.Key.AnonymousName, Address1 = z.Key.AnonymousAddress, PrimaryPhone = z.Key.AnonymousPhone }).ToList();
            for (int i = 0; i < bwr.References.Count(); i++)
            {
                for (int j = 0; j < bwr.References.Count(); j++)
                {
                    if (bwr.References[i].RefId != bwr.References[j].RefId && bwr.References[i].RefStatus == "A" && bwr.References[j].RefStatus == "A")
                    {
                        if (IsDuplicateReference(bwr.References[i], bwr.References[j]))
                        {
                            if (!bwr.DuplicateReferences.Contains(bwr.References[i]))
                            {
                                bwr.References[i].Duplicate = true;
                                bwr.DuplicateReferences.Add(bwr.References[i]);
                            }
                        }
                        else if (bwr.References[i].RefName == bwr.References[j].RefName)
                        {
                            if (!bwr.PossibleDuplicateReferences.Contains(bwr.References[i]))
                            {
                                bwr.References[i].PossibleDuplicate = true;
                                bwr.PossibleDuplicateReferences.Add(bwr.References[i]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the ref status to nI for any older references that are exact dupes of a
        /// newer reference. Uses the numeric value of the ref ID to determine which is older
        /// and newer.
        /// </summary>
        /// <param name="bwr"></param>
        private bool ProcessDuplicateReferences(Borrower bwr)
        {
            bool result = true;
            if (bwr.References == null || !bwr.References[0].ReferenceQueueId.HasValue) // We only need to pull from the DB if we didn't already do it for recovery. 
                GetReferencesFromDb(bwr);
            var unprocessedRefs = bwr.DuplicateReferences?.Where(p => !p.ProcessedAt.HasValue).ToList(); // Filter out processed refs
            List<Reference> updatedReferences = new List<Reference>();
            foreach (Reference rfr in unprocessedRefs)
            {
                // While we are only updating unprocessed references, we still want to check them against all references.
                // Hence, this is why we are not using the unprocessedRefs collection in the LINQ statement.
                if (bwr.DuplicateReferences.Any(p => IsDuplicateReference(p, rfr) && p.GetRefNumber().CompareTo(rfr.GetRefNumber()) > 0))
                {
                    rfr.RefStatus = "I";
                    rfr.DemosChanged = true;
                    rfr.ZipChanged = false;
                    rfr.ValidAddress = false;
                    rfr.ValidPhone = false;
                    if (InactivateOldReference(bwr.Ssn, rfr))
                        updatedReferences.Add(rfr);
                    else
                        result = false;
                }
                else
                {
                    rfr.DemosChanged = false;
                    rfr.ZipChanged = false;
                    DA.SetReferenceProcessedAt(rfr.ReferenceQueueId);
                }
            }
            RecordReferenceUpdatesToDb(updatedReferences);
            return result;
        }

        private bool InactivateOldReference(string borrowerSsn, Reference rfr)
        {
            bool result = true;
            RI.FastPath($"LP2CC{borrowerSsn}{rfr.RefId}");
            if (RI.CheckForText(1, 59, "REFERENCE DEMOGRAPHICS"))
            {
                RI.PutText(6, 67, "I"); // Update ref status to inactive
                RI.PutText(8, 53, "N"); // Update ref to have invalid address
                RI.PutText(13, 36, "N"); // Update ref to have invalid phone
                RI.Hit(ReflectionInterface.Key.F6);

                if (RI.AltMessageCode != "49000" && RI.AltMessageCode != "40639") // 49000 = success, 40639 = success, data cleansed by demo subroutine
                {
                    SetDuplicateReferenceManuallyWorked(rfr);
                    Dialog.Warning.Ok($"The record for reference {rfr.RefId} was not updated. Correct the error, press F6 to post the changes, and hit Insert to continue.", "Record Not Updated");
                    RI.PauseForInsert();
                }
                result = DA.SetReferenceProcessedAt(rfr.ReferenceQueueId);
                if (!result)
                    LogRun.AddNotification($"Error setting ReferenceQueueId: {rfr.ReferenceQueueId} as processed in the OLS.duplrefs.DuplicateReferences table after updating demos in the session.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            else
            {
                SetDuplicateReferenceManuallyWorked(rfr);
                Dialog.Warning.Ok($"The record for reference {rfr.RefId} was not updated. Correct the error, press F6 to post the changes, and hit Insert to continue.", "Reference Not Updated - Session Error");
                RI.PauseForInsert();
            }
            return result;
        }

        /// <summary>
        /// Helper method that indicates whether two given references
        /// are considered duplicate. True if they share the same name,
        /// same first chars of their addresses, and the same phone number.
        /// Will only hit this point if both are in an active status.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        private bool IsDuplicateReference(Reference lhs, Reference rhs)
        {
            return lhs.RefName == rhs.RefName && lhs.RefAddress1.SafeSubString(0, 5) == rhs.RefAddress1.SafeSubString(0, 5) && lhs.RefPhone == rhs.RefPhone;
        }

        /// <summary>
        /// Sorts references and presents them to user in a GUI.
        /// Passes on user updates to an auxiliary method to process
        /// those changes in the session.
        /// </summary>
        private bool? ProcessPossibleDuplicateRefs(Borrower bwr)
        {
            if (bwr.References == null || !bwr.References[0].ReferenceQueueId.HasValue) // We only need to pull from the DB if we didn't already do it for recovery. 
                GetReferencesFromDb(bwr);
            bool? processingResult = true;
            bwr.PossibleDuplicateReferences = bwr.PossibleDuplicateReferences?.Where(p => !p.ProcessedAt.HasValue).ToList(); // Filter out processed refs
            bwr.PossibleDuplicateReferences = bwr.PossibleDuplicateReferences?.OrderBy(x => x.RefName).ThenBy(y => y.RefId).ToList(); //Sort references by name and then ID
            for (int i = 0; i < bwr.PossibleDuplicateReferences?.Count();)
            {
                int duplicateCount = bwr.PossibleDuplicateReferences.Where(p => p.RefName == bwr.PossibleDuplicateReferences[i].RefName).Count();
                if (duplicateCount > 4) // Form only displays up to four references, so warn user if there's more
                {
                    Dialog.Info.Ok($"{bwr.PossibleDuplicateReferences[i].RefName} is listed too many times for the script to process the duplicates.  Click OK to pause the script and update the records manually.  Hit Insert to continue when you are done.");
                    i += duplicateCount; // Increment past the references that the user manually took care of
                    continue;
                }
                else
                {
                    UserInput userInput = new UserInput(DA, bwr);
                    userInput.PopulateFields(ref i); // Populate and provide UI for user

                    DialogResult result = userInput.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        List<Reference> updatedReferences = userInput.GetUserUpdates();
                        RecordReferenceUpdatesToDb(updatedReferences);
                        processingResult &= ProcessReferenceUpdates(bwr, updatedReferences.Where(p => p.DemosChanged.HasValue && p.DemosChanged.Value).ToList());
                        processingResult &= SetNonUpdatedReferencesProcessed(updatedReferences.Where(p => !p.DemosChanged.HasValue || (p.DemosChanged.HasValue && !p.DemosChanged.Value)).ToList());
                    }
                    else if (result == DialogResult.Cancel) // User closed the form. Return to caller and prompt user
                        return null;
                    else if (result == DialogResult.Abort) // An error occurred
                        return false;
                    userInput.Dispose();
                }
            }
            return processingResult;
        }

        private void RecordReferenceUpdatesToDb(List<Reference> updatedReferences)
        {
            foreach (Reference rfr in updatedReferences)
            {
                if (!DA.UpdateReferenceRecord(rfr))
                    LogRun.AddNotification($"Error updating the ReferenceQueueId: {rfr.ReferenceQueueId} to: {rfr}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        /// <summary>
        /// Pulls down reference data from the DB.
        /// </summary>
        private bool GetReferencesFromDb(Borrower bwr)
        {
            bwr.References = DA.GetReferences(bwr.BorrowerQueueId) ?? bwr.References;
            if (bwr.References?.Count() > 0) // References acquired from DB, now group them
            {
                bwr.DuplicateReferences = bwr.References.Where(p => p.Duplicate).ToList();
                bwr.PossibleDuplicateReferences = bwr.References.Where(p => p.PossibleDuplicate).ToList();
            }
            return bwr.References?.Count() > 0;
        }

        /// <summary>
        /// Sets a reference record on the DB to process for references
        /// that did not have any demographic updates.
        /// </summary>
        private bool SetNonUpdatedReferencesProcessed(List<Reference> references)
        {
            bool processingResult = true;
            foreach (Reference rfr in references)
            {
                processingResult &= SetNonUpdatedRefProcessed(rfr);
            }
            return processingResult;
        }

        private bool SetNonUpdatedRefProcessed(Reference rfr)
        {
            bool result = DA.SetReferenceProcessedAt(rfr.ReferenceQueueId) && DA.SetDemosChanged(rfr.ReferenceQueueId, false);
            if (!result)
                LogRun.AddNotification($"Error setting ReferenceQueueId: {rfr.ReferenceQueueId} as processed with DemosUpdated: false in the OLS.duplrefs.DuplicateReferences table.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            return result;
        }

        /// <summary>
        /// Updates the given borrower's references in the session,
        /// setting validity flags and demos fields according to the
        /// passed-in user input.
        /// </summary>
        private bool? ProcessReferenceUpdates(Borrower bor, List<Reference> updatedReferences)
        {
            bool? processingResult = true;
            foreach (Reference rfr in updatedReferences)
            {
                DA.SetDemosChanged(rfr.ReferenceQueueId, true); // Indicates the demos provided by user are different than previous demos
                RI.FastPath($"LP2CC{bor.Ssn}{rfr.RefId}");
                if (RI.CheckForText(1, 59, "REFERENCE DEMOGRAPHICS"))
                {
                    if (rfr.RefStatus == "I")
                        RI.PutText(6, 67, "I");
                    else
                    {
                        RI.PutText(8, 9, rfr.RefAddress1, true);
                        RI.PutText(9, 9, rfr.RefAddress2, true);
                        RI.PutText(10, 9, rfr.RefCity, true);
                        RI.PutText(10, 52, rfr.RefState, true);
                        RI.PutText(10, 60, rfr.RefZip, true);
                        RI.PutText(13, 16, rfr.RefPhone, true);
                        RI.PutText(9, 55, rfr.RefCountry, true);
                    }

                    if (rfr.ValidAddress.HasValue)
                    {
                        if (!rfr.ValidAddress.Value)
                            RI.PutText(8, 53, "N");
                        else if (rfr.HasDifferentDemos(bor.References.Where(p => p.ReferenceQueueId == rfr.ReferenceQueueId).Single()))
                            RI.PutText(8, 53, "Y");
                    }

                    if (rfr.ValidPhone.HasValue)
                    {
                        if (!rfr.ValidPhone.Value)
                            RI.PutText(13, 36, "N");
                        else if (rfr.HasDifferentDemos(bor.References.Where(p => p.ReferenceQueueId == rfr.ReferenceQueueId).Single()))
                            RI.PutText(13, 36, "Y");
                    }
                    RI.Hit(ReflectionInterface.Key.F6);
                    if (RI.AltMessageCode != "49000" && RI.AltMessageCode != "40639") // 49000 = success, 40639 = success, data cleansed by demo subroutine
                    {
                        SetDuplicateReferenceManuallyWorked(rfr);
                        Dialog.Warning.Ok($"The record for reference {rfr.RefId} was not updated. Correct the error, press F6 to post the changes, and hit Insert to continue.", "Record Not Updated");
                        RI.PauseForInsert();
                    }
                    bool result = DA.SetReferenceProcessedAt(rfr.ReferenceQueueId);
                    processingResult &= result;

                    if (!result)
                        LogRun.AddNotification($"Error setting ReferenceQueueId: {rfr.ReferenceQueueId} as processed in the OLS.duplrefs.DuplicateReferences table after updating demos in the session.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
                else
                {
                    SetDuplicateReferenceManuallyWorked(rfr);
                    Dialog.Warning.Ok($"The record for reference {rfr.RefId} was not updated. Correct the error, press F6 to post the changes, and hit Insert to continue.", "Reference Not Updated - Session Error");
                    RI.PauseForInsert();
                }

            }
            return processingResult;
        }

        /// <summary>
        /// Driver method to populate borrower and reference data.
        /// Determines if recovery is needed to get either.
        /// </summary>
        private Borrower GetBorrower(string ssn)
        {
            RI.FastPath("LP22I" + ssn);
            string accountNumber = RI.GetText(3, 60, 12).Replace(" ", "");
            RecoveryData recoveryData = DA.GetRecoveryData(accountNumber);

            bool useRecovery = false;
            if (recoveryData != null && recoveryData.CreatedAt > DateTime.Now.AddDays(-3)) // Allow user to choose whether to do recovery if the task was being worked within the last three days
            {
                if (recoveryData.UserId == UserId && !(useRecovery = UserSelectedRecoveryMode(recoveryData.CreatedAt))) // Ask user if they want to work in recovery mode
                    DA.InactivateRecord(recoveryData.BorrowerQueueId); // Inactivate old records if they don't want to use recovery
            }
            else if (recoveryData != null) // Automatically inactivate if the old record was not within the last three days
                DA.InactivateRecord(recoveryData.BorrowerQueueId);

            if (useRecovery)
                return RecoverBorrower(recoveryData.BorrowerQueueId, ssn);
            else
                return CreateNewBorrower(ssn, accountNumber);
        }

        /// <summary>
        /// Gets a not-fully-processed borrower from the ProcessQueue table.
        /// </summary>
        private Borrower RecoverBorrower(int? borrowerQueueId, string ssn)
        {
            Borrower bwr = DA.GetUnprocessedBorrower(borrowerQueueId);
            bwr.Ssn = ssn;
            if (RecoverReferences(bwr))
                return bwr;
            return null;
        }

        /// <summary>
        /// Gets the references associated with the passed-in borrower from
        /// the ReferenceQueue table.
        /// 
        /// Returns true if the references were successfully retrieved. Otherwise, false.
        /// </summary>
        private bool RecoverReferences(Borrower bwr)
        {
            if (GetReferencesFromDb(bwr))
                return true;
            else
                return GatherBorrowerReferences(bwr, true);
        }

        /// <summary>
        /// Inserts comments into ArcAddProcessing.
        /// </summary>
        private void AddComment(object recipient, string accountIdentifier, string arc, string comment)
        {
            if (recipient is Borrower bwr) // We add borrower level comment via session so that we can close task
            {
                if (!RI.AddCommentInLP50(accountIdentifier, "AM", "36", arc, comment, ScriptId))
                {
                    LogRun.AddNotification($"Unable to add the ARC {arc} with the comment {comment} for account {accountIdentifier}. See BorrowerQueueId {bwr.BorrowerQueueId}. Having user manually handle this issue.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    string message = $"Unable to add OneLINK {arc} for the recipient: {accountIdentifier}.";
                    Dialog.Warning.Ok($"{message} Please add the following comment manually and then press Insert after you have posted the comment: {comment}", "Error Adding Comment");
                    RI.PauseForInsert();
                }
            }
            else if (recipient is Reference rfr) // We add reference comments through AAP
            {
                ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    AccountNumber = accountIdentifier,
                    ActivityContact = "36",
                    ActivityType = "AM",
                    Arc = arc,
                    ArcTypeSelected = ArcData.ArcType.OneLINK,
                    Comment = comment,
                    IsEndorser = false,
                    IsReference = false, // TODO: See if reference needs this set to true in OL. Ask BA to verify AAP drops comment successfully.
                    ScriptId = ScriptId
                };
                ArcAddResults result = arcData.AddArc();
                if (result == null || !result.ArcAdded)
                {
                    string message = $"Unable to add OneLINK {arc} for the recipient: {accountIdentifier}.";
                    RI.LogRun.AddNotification($"{message} Prompting the user to add the comment manually.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    Dialog.Warning.Ok($"{message} Please add the following comment manually and then press Insert after you have posted the comment: {comment}", "Error Adding Comment");
                    RI.PauseForInsert();
                }
                if (!rfr.ArcAddProcessingId.HasValue && !DA.SetReferenceArcAddProcessingId(rfr.ReferenceQueueId, result.ArcAddProcessingId))
                    LogRun.AddNotification($"Unable to set the ArcAddProcessingId field to {result.ArcAddProcessingId} for ReferenceQueueId {rfr.ReferenceQueueId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        /// <summary>
        /// Finds out if user wants to use recovery mode or not.
        /// If not, the old records will be inactivated on the DB.
        /// </summary>
        private bool UserSelectedRecoveryMode(DateTime? createdAt)
        {
            bool useRecovery = Dialog.Warning.YesNo($"Records indicate that you were working on this account on {createdAt.Value:MM/dd/yyyy}. Select YES if you want to recover from where you left off or NO to start over.", "Enter Into Recovery Mode?");
            LogRun.AddNotification($"The user has selected to {(!useRecovery ? "not" : "")} run the script in recovery mode", NotificationType.Other, NotificationSeverityType.Informational);
            return useRecovery;
        }

        /// <summary>
        /// Populates borrower. Kicks off helper method to populate
        /// references.
        /// </summary>
        private Borrower CreateNewBorrower(string ssn, string accountNumber)
        {
            Borrower bwr = new Borrower()
            {
                Ssn = ssn,
                AccountNumber = accountNumber,
                UserId = UserId, // Assign ScriptBase.UserId to Borrower obj
                References = new List<Reference>()
            };

            if (GatherBorrowerReferences(bwr))
                return bwr;
            else
                return null;
        }

        /// <summary>
        /// Gets borrower references. Prompts user if an issue occurs.
        /// </summary>
        private bool GatherBorrowerReferences(Borrower bwr, bool recoveryMode = false)
        {
            if (GetReferencesFromSession(bwr))
            {
                if (!recoveryMode)
                    bwr.BorrowerQueueId = DA.InsertBorrowerRecord(bwr.AccountNumber, UserId);
                if (bwr.BorrowerQueueId.HasValue && bwr.BorrowerQueueId.Value != 0)
                    return true;
                else
                {
                    string plrComment = $"Unable to add {bwr.AccountNumber} to OLS.duplrefs.BorrowerQueue table. Informing user and having them process manually";
                    string prompt = $"There was an error trying to add the account {bwr.AccountNumber} to the database. Please manually process this borrower's references and then press Insert after you have completed the task.";
                    PromptAndPauseForManualReconciliation(bwr, plrComment, NotificationSeverityType.Warning, prompt, "Error Interacting with Databse");
                }
            }
            else
            {
                string plrComment = $"Unable to gather references for account {bwr.AccountNumber}. Informing user and having them process manually";
                string prompt = $"There was an error trying to gather references for account {bwr.AccountNumber}. Please manually process this borrower's references and then press Insert after you have completed the task.";
                PromptAndPauseForManualReconciliation(bwr, plrComment, NotificationSeverityType.Critical, prompt, "Error Gathering Reference Info");
            }
            return false;
        }

        /// <summary>
        /// Prompt user when a task needs to be manually worked.
        /// Record such on the DB.
        /// </summary>
        private void PromptAndPauseForManualReconciliation(Borrower bwr, string plrComment, NotificationSeverityType severity, string prompt, string caption)
        {
            LogRun.AddNotification(plrComment, NotificationType.ErrorReport, severity);
            SetTaskManuallyWorked(bwr);
            Dialog.Error.Ok(prompt, caption);
            RI.PauseForInsert();
        }

        /// <summary>
        /// Borrower level - sets borrower as processed and then iterate 
        /// through all the reference records associated with that borrower 
        /// to set them as manually worked.
        /// </summary>
        private bool SetTaskManuallyWorked(Borrower bwr)
        {
            if (DA.SetBorrowerProcessedAt(bwr.BorrowerQueueId))
            {
                foreach (Reference rfr in bwr.DuplicateReferences)
                    SetDuplicateReferenceManuallyWorked(rfr);
            }
            else
            {
                LogRun.AddNotification($"Error setting the record with BorrowerQueueId: {bwr.BorrowerQueueId} to processed. User manually worked task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reference level - sets references as manually worked and processed
        /// </summary>
        private bool SetDuplicateReferenceManuallyWorked(Reference rfr)
        {
            if (DA.SetManuallyWorked(rfr.ReferenceQueueId, true))
                if (DA.SetReferenceProcessedAt(rfr.ReferenceQueueId))
                    return true;
                else
                    LogRun.AddNotification($"Error setting ReferenceQueueId: {rfr.ReferenceQueueId} as processed. User manually worked task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            else
                LogRun.AddNotification($"Error setting the ManuallyWorked field to true for ReferenceQueueId: {rfr.ReferenceQueueId}. User manually worked task.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            return false;
        }

        /// <summary>
        /// Navigates to reference page and then iterates through references.
        /// Passes on the row to helper methods to check line validity and
        /// scrape info from the session.
        /// </summary>
        private bool GetReferencesFromSession(Borrower bwr)
        {
            RI.FastPath($"LP2CI{bwr.Ssn}"); // Navigate to reference selection page
            if (RI.CheckForText(1, 65, "REFERENCE SELECT"))
            {
                int row = 6;
                bool allPagesChecked = false;

                while (!allPagesChecked)
                {
                    if (LineHasReferenceRecord(row))
                    {
                        Reference reference = GetReferenceFromSession(row);
                        bwr.References.Add(reference);
                        row += 3; // Increment three to get since there are three rows between records on LP2CI
                    }
                    else
                    {
                        RI.Hit(ReflectionInterface.Key.F8); // Check next page
                        if (RI.CheckForText(22, 3, "46004"))
                            allPagesChecked = true;
                        else
                            row = 6; // Reset row for refs on new page
                    }
                }
            }
            else if (RI.CheckForText(1, 65, "NCE DEM"))
            {
                Reference reference = new Reference()
                {
                    RefId = RI.GetText(3, 14, 9),
                    RefStatus = RI.GetText(6, 67, 1) // TODO: Do we need to grab the other demo info from this? Ask BA about this screen.
                };
                bwr.References.Add(reference);
            }
            else
                return false; // Unexpected session screen
            return true;
        }

        /// <summary>
        /// Populates a reference with data from the session.
        /// </summary>
        private Reference GetReferenceFromSession(int row)
        {
            Reference reference = new Reference()
            {
                RefName = RI.GetText(row, 7, 40).Trim(),
                RefId = RI.GetText(row, 68, 9).Trim(),
                RefStatus = RI.GetText(row + 1, 27, 1).Trim(),
            };

            if (reference.RefStatus == "A") // Enter into ref and get demos
            {
                RI.PutText(21, 13, RI.GetText(row, 3, 1), ReflectionInterface.Key.Enter);
                ScrapeReferenceDemosFromSession(reference);
                RI.Hit(ReflectionInterface.Key.F12); // Return to ref selection screen
            }

            return reference;
        }

        /// <summary>
        /// Scrapes demographic fields from the session.
        /// </summary>
        private void ScrapeReferenceDemosFromSession(Reference reference)
        {
            reference.RefAddress1 = RI.GetText(8, 9, 35).Trim();
            reference.RefAddress2 = RI.GetText(9, 9, 35).Trim();
            reference.RefCity = RI.GetText(10, 9, 35).Trim();
            reference.RefState = RI.GetText(10, 52, 2).Trim();
            reference.RefZip = RI.GetText(10, 60, 9).Trim();
            reference.RefPhone = RI.GetText(13, 16, 10).Trim();
            reference.RefCountry = RI.CheckForText(9, 55, "_") ? "" : RI.GetText(9, 55, 25);
            reference.ValidAddress = RI.GetText(8, 53, 1).Trim() == "Y";
            reference.ValidPhone = RI.GetText(13, 36, 1).Trim() == "Y";
        }

        /// <summary>
        /// Indicates if a line contains a reference record by checking 
        /// if the session cursor has progressed past the records or not.
        /// </summary>
        private bool LineHasReferenceRecord(int row)
        {
            return !RI.CheckForText(row, 3, "SEL") && !RI.CheckForText(row, 3, "   ");
        }

        /// <summary>
        /// Closes a task in the session.
        /// </summary>
        private bool CloseTask()
        {
            RI.FastPath($"LP9AC{Queue}");
            RI.Hit(ReflectionInterface.Key.F6);
            return (RI.AltMessageCode == "49000" || RI.AltMessageCode == "48003" || RI.AltMessageCode == "48081"); // 49000 = "DATA SUCCESSFULLY UPDATED"; 48081 = alt success code
        }

        private void AssignTask()
        {
            RI.FastPath($"LP8YCSKP;{Queue}");
            RI.PutText(21, 13, "1", ReflectionInterface.Key.Enter);

            while (RI.AltMessageCode != "46004")
            {
                int row = 7;
                while (RI.GetText(row, 38, 8).Trim() != "")
                {
                    if (RI.GetText(row, 38, 8).Trim() == UserId && RI.GetText(row, 33, 1) == "W")
                    {
                        RI.PutText(row, 33, "A");
                        RI.PutText(row, 38, "UT00023", true); //Assigning to diff user
                        RI.Hit(ReflectionInterface.Key.Enter);
                        RI.Hit(ReflectionInterface.Key.F6); // Post assignment
                    }
                    row++;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }

        }

        private void UnassignTasks()
        {
            RI.FastPath($"LP8YCSKP;{Queue}");
            RI.PutText(21, 13, "1", ReflectionInterface.Key.Enter);

            while (RI.AltMessageCode != "46004")
            {
                int row = 7;
                while (RI.GetText(row, 38, 8).Trim() != "")
                {
                    if (DataAccessHelper.TestMode && RI.GetText(row, 38, 8).Trim() != "UT00380" && RI.GetText(row, 33, 1) == "A") //For testing purposes, unassign tasks unless they were manually assigned to this ID
                    {
                        RI.PutText(row, 38, "", ReflectionInterface.Key.EndKey); // Unassign
                        RI.Hit(ReflectionInterface.Key.Enter);
                        RI.Hit(ReflectionInterface.Key.F6); // Post assignment
                    }
                    row++;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }

        }

        /// <summary>
        /// Prompts user to see if they want to continue processing after
        /// the a task was completed.
        /// </summary>
        private void ContinueProcessingPrompt()
        {
            UserRequestedScriptEnd = !Dialog.Info.YesNo("Do you wish to process another task? Click Yes to continue processing. Click No to end the script run.", "Process Another Task?");
        }

        /// <summary>
        /// Cleans up the end of the script run. Logs the end for Process 
        /// Logger, closes the session, and closes any managed connections.
        /// </summary>
        private void EndScriptRun()
        {
            LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
        }

    }
}
