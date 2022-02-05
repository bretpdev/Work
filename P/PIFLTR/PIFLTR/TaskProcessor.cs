using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace PIFLTR
{
    class TaskProcessor
    {
        private List<Borrower> BorrowersToProcess { get; set; }
        private List<CoBorrower> CoborrowersToProcess;
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }

        public TaskProcessor(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(plr);

            BorrowersToProcess = new List<Borrower>();
            CoborrowersToProcess = new List<CoBorrower>();
        }

        /// <summary>
        /// Driver method to prepare tasks to be worked.  Helper methods close canceled tasks in addition to gathering tasks, addresses, and loans sequences for a given borrower.
        /// </summary>
        /// <returns>result</returns>
        public bool RunProcess()
        {
            bool result = true;
            try
            {
                Console.WriteLine("Loading tasks into ULS.pifltr.ProcessingQueue.  This may take a minute.");
                List<TaskData> tasks = DA.GetUnprocessedTasks().OrderBy(p => p.AccountNumber).ThenBy(c => c.CoBorrowerSsn).ThenBy(l => l.IsConsolPif).ToList(); //TODO: Test

                GetTasksToProcess(tasks.Where(p => !p.IsCanceled).ToList());

                result &= ProcessBorrowers();
                result &= ProcessCoBorrowers();
                result &= CloseTasks(tasks);
            }
            catch (Exception ex)
            {
                PLR.AddNotification($"PIFLTR was not able to run successfully. Please contact a member of System Support.", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
            return result;
        }

        /// <summary>
        /// Builds lists of borrowers and co-borrowers that need to be processed. Adds all the relevant tasks to each.
        /// </summary>
        /// <param name="tasks">Loan sequence specific tasks for a borrowers various paid-in-full loans.</param>
        private void GetTasksToProcess(List<TaskData> tasks)
        {
            Borrower bor = new Borrower();
            CoBorrower coBor = new CoBorrower();

            foreach (TaskData task in tasks)
            {
                if (bor.AccountNumber.IsPopulated() && bor.AccountNumber != task.AccountNumber)
                    BorrowersToProcess.Add(bor);
                if (coBor.CoBorrowerSsn.IsPopulated() && coBor.CoBorrowerSsn != task.CoBorrowerSsn)
                {
                    CoborrowersToProcess.Add(coBor);
                    coBor = new CoBorrower();
                }
                if (bor.AccountNumber == task.AccountNumber)
                {
                    bor.Tasks.Add(task);
                    if (task.CoBorrowerSsn.IsPopulated() && task.CoBorrowerSsn == coBor.CoBorrowerSsn) //If not a new coborrower, just add task
                        coBor.Tasks.Add(task);
                    else if (task.CoBorrowerSsn.IsPopulated() && task.CoBorrowerSsn != coBor.CoBorrowerSsn) //If a new coborrower, create new coborrower obj
                        coBor = CreateCoBorrower(task);
                }
                else
                {
                    Console.WriteLine($"Gathering demographics for borrower {task.Ssn}.");
                    bor = new Borrower();
                    bor = DA.GetBorrowerDemographics(task.AccountNumber); //In future, put PLR notification?
                    if (task.CoBorrowerSsn.IsPopulated())
                        coBor = CreateCoBorrower(task);
                    bor.Tasks.Add(task);
                }
            }
            BorrowersToProcess.Add(bor); //This adds the last borrower to the list
            if (coBor.CoBorrowerSsn.IsPopulated()) //Now adding last coborrower, if applicable
                CoborrowersToProcess.Add(coBor);
        }

        /// <summary>
        /// Constructs coborrower object by pulling back demos from database.
        /// </summary>
        /// <returns>Coborrower object</returns>
        private CoBorrower CreateCoBorrower(TaskData task)
        {
            CoBorrower coBor = new CoBorrower();
            Console.WriteLine($"Gathering demographics for co-borrower {task.CoBorrowerSsn}.");
            coBor = DA.GetCoBorrowerDemographics(task.CoBorrowerSsn);
            coBor.BorrowerAccountNumber = task.AccountNumber;
            coBor.BorrowerSsn = task.Ssn;
            if (coBor == null)
            {
                string errorMessage = $"The coborrower {task.CoBorrowerSsn} demographics were not returned from the database. Letter not sent. Please verify that they have a valid legal address.";
                Console.WriteLine(errorMessage);
                PLR.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return null;
            }

            coBor.Tasks.Add(task);

            return coBor;
        }

        /// <summary>
        /// Determines cost code for borrower, makes a temp borrower specific to task types (consol/non-consol), then passes that to the InsertAndUpdatePrintJob() method.
        /// </summary>
        private bool ProcessBorrowers()
        {
            bool result = true;
            foreach (Borrower bor in BorrowersToProcess)
            {

                List<TaskData> TaskDataTemp = new List<TaskData>();
                TaskDataTemp.AddRange(bor.Tasks);

                // Process borrower consol tasks
                if (bor.Tasks.Any(p => p.IsConsolPif))
                {
                    bor.CostCenter = DetermineUCode(bor.Tasks.Where(p => p.IsConsolPif).ToList());
                    bor.Tasks = new List<TaskData>(bor.Tasks.Where(l => l.IsConsolPif).ToList());
                    result &= InsertAndUpdatePrintJob(bor);
                }

                bor.Tasks = TaskDataTemp; //Reset back to original task list to include non-consol tasks, if applicable

                // Process borrower non-consol tasks
                if (bor.Tasks.Any(p => !p.IsConsolPif))
                {
                    bor.CostCenter = DetermineUCode(bor.Tasks.Where(p => !p.IsConsolPif).ToList());
                    bor.Tasks = new List<TaskData>(bor.Tasks.Where(l => !l.IsConsolPif).ToList());
                    result &= InsertAndUpdatePrintJob(bor);
                }
            }
            return result;
        }

        /// <summary>
        /// If a task doesn't have a PrintProcessingId, inserts into PrintProcessing via the helper method InsertPrintProcessingData().
        /// </summary>
        /// <param name="b">The temp borrower with only consol/non-consol tasks.</param>
        private bool InsertAndUpdatePrintJob(Borrower b)
        {
            bool result = true;
            if (b.Tasks.Any(p => p.PrintProcessingId == null))
            {
                int? printProcessingId = InsertPrintProcessingData(b);
                if (printProcessingId != null)
                    result &= UpdatePrintProcessing(printProcessingId, b.Tasks, false); //Updates PrintProcessingId in pifltr.ProcessingQueue table
                else
                    result = false; // Failed to insert into PrintProcessing table
            }
            return result;
        }

        /// <summary>
        /// Determines cost code for coborrower, makes a temp coborrower specific to task types (consol/non-consol), then passes that to the InsertAndUpdateCoBorPrintJob() method.
        /// </summary>
        private bool ProcessCoBorrowers()
        {
            bool result = true;
            foreach (CoBorrower coBor in CoborrowersToProcess)
            {
                List<TaskData> TaskDataTemp = new List<TaskData>();
                TaskDataTemp.AddRange(coBor.Tasks);

                // Process coborrower consol tasks
                if (coBor.Tasks.Any(p => p.IsConsolPif))
                {
                    coBor.CostCenter = DetermineUCode(coBor.Tasks.Where(p => p.IsConsolPif).ToList());
                    coBor.Tasks = new List<TaskData>(coBor.Tasks.Where(l => l.IsConsolPif).ToList());
                    result &= InsertAndUpdateCoBorPrintJob(coBor);
                }

                coBor.Tasks = TaskDataTemp; //Reset back to original task list to include non-consol tasks, if applicable

                // Process coborrower non-consol tasks
                if (coBor.Tasks.Any(p => !p.IsConsolPif))
                {
                    coBor.CostCenter = DetermineUCode(coBor.Tasks.Where(p => !p.IsConsolPif).ToList());
                    coBor.Tasks = new List<TaskData>(coBor.Tasks.Where(l => !l.IsConsolPif).ToList());
                    result &= InsertAndUpdateCoBorPrintJob(coBor);
                }
            }
            return result;
        }

        /// <summary>
        /// If a task doesn't have a PrintProcessingId, inserts into CoBwrPrintProcessing via the helper method InsertCoBorrowerPrintProcessingData().
        /// </summary>
        /// <param name="cb">The temp coborrower with only consol/non-consol tasks.</param>
        private bool InsertAndUpdateCoBorPrintJob(CoBorrower cb)
        {
            bool result = true;
            if (cb.Tasks.Any(p => p.CoBwrPrintProcessingId == null))
            {
                int? coBwrPrintProcessingId = InsertCoBorrowerPrintProcessingData(cb, cb.Tasks.FirstOrDefault().Ssn);
                if (coBwrPrintProcessingId != null)
                    result &= UpdatePrintProcessing(coBwrPrintProcessingId, cb.Tasks, true); //Updates CoBwrPrintProcessingId in pifltr.ProcessingQueue table
                else
                    result = false; // Failed to insert coborrower record into PrintProcessing
            }
            return result;
        }

        /// <summary>
        /// Inserts all print related data to the PrintProcessing table to be printed 
        /// </summary>
        /// <param name="task">a Queuedata object to be written to the PrintProcessing table </param>
        /// <returns> a bool indicating if the insert was successful</returns>
        public int? InsertPrintProcessingData(Borrower borr)
        {
            borr.GetLetterData();
            string letterId = borr.Tasks.Any(p => p.IsConsolPif) ? Program.ConsolLetterId : Program.PifLetterId;
            string letterData = borr.Tasks.Any(p => p.IsConsolPif) ? borr.ConsolPifLetterData.ToString() : borr.LetterData.ToString();
            int? printProcessingId = null;

            var result = Repeater.TryRepeatedly(() => printProcessingId = EcorrProcessing.AddRecordToPrintProcessing(Program.ScriptId, letterId, letterData, borr.AccountNumber, borr.CostCenter));

            if (!result.Successful)
            {
                foreach (var ex in result.CaughtExceptions)
                {
                    PLR.AddNotification($"Error while inserting into PrintProcessing for borrower account {borr.AccountNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }
            }

            if (printProcessingId.HasValue)
                Console.WriteLine($"Inserted PrintProcessingId {printProcessingId} for borrower {borr.BorrowerSsn}");

            return printProcessingId;
        }

        /// <summary>
        /// Inserts all print related data to the CoBwrPrintProcessing table to be printed 
        /// </summary>
        /// <param name="task">a Queuedata object to be written to the CoBwrPrintProcessing table </param>
        /// <returns> a bool indicating if the insert was successful</returns>
        public int? InsertCoBorrowerPrintProcessingData(CoBorrower coBorr, string borrowerSsn)
        {
            coBorr.GetLetterData();
            string letterId = coBorr.Tasks.Any(p => p.IsConsolPif) ? Program.ConsolLetterId : Program.PifLetterId;
            string letterData = coBorr.Tasks.Any(p => p.IsConsolPif) ? coBorr.ConsolLetterData.ToString() : coBorr.PifLetterData.ToString();
            int? coBwrPrintProcessingId = null;

            var result = Repeater.TryRepeatedly(() => coBwrPrintProcessingId = EcorrProcessing.AddCoBwrRecordToPrintProcessing(Program.ScriptId, letterId, letterData, coBorr.CoBorrowerAccount, coBorr.CostCenter, borrowerSsn));

            if (!result.Successful)
            {
                foreach (var ex in result.CaughtExceptions)
                {
                    PLR.AddNotification($"Error while inserting into PrintProcessing for coborrower account {coBorr.CoBorrowerAccount}", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                }
            }

            if (coBwrPrintProcessingId.HasValue)
                Console.WriteLine($"Inserted PrintProcessingId {coBwrPrintProcessingId} for coborrower {coBorr.CoBorrowerSsn}");

            return coBwrPrintProcessingId;
        }

        /// <summary>
        /// Updates PrintProcessingId/CoBwrPrintPRocessingId in the pifltr.ProcessingQueue table
        /// </summary>
        private bool UpdatePrintProcessing(int? printProcessingId, List<TaskData> tasks, bool isCoborrower)
        {
            bool result = true;
            if (isCoborrower)
            {
                foreach (TaskData task in tasks.Where(p => p.CoBwrPrintProcessingId == null))
                {
                    task.CoBwrPrintProcessingId = printProcessingId;
                    result &= DA.UpdateCoBwrPrintProcessingId(task);
                }
            }
            else
            {
                foreach (TaskData task in tasks.Where(p => p.PrintProcessingId == null))
                {
                    task.PrintProcessingId = printProcessingId;
                    result &= DA.UpdatePrintProcessingId(task);
                }
            }
            return result;
        }

        /// <summary>
        /// Determines and sets cost center for borrower object.
        /// </summary>
        private string DetermineUCode(List<TaskData> tasks)
        {
            List<string> uCodes = DA.GetUcodes();
            List<string> borrCostCenterCodes = new List<string>();
            foreach (TaskData task in tasks)
            {
                Console.WriteLine($"Identifying cost code for the task {task.TaskControlNumber}");
                string costCenter = DA.GetBorrowerCostCenterCode(task);
                borrCostCenterCodes.Add(costCenter.Trim());

                if (uCodes.Intersect(borrCostCenterCodes).Count() > 0)
                    task.CostCode = "MA2324";
                else
                    task.CostCode = "MA2327";
            }

            if (uCodes.Intersect(borrCostCenterCodes).Count() > 0)
                return "MA2324";
            else
                return "MA2327";
        }

        /// <summary>
        /// Closes all tasks by sending them to QueueCompleter
        /// </summary>
        private bool CloseTasks(List<TaskData> tasksProcessed)
        {
            bool result = true;
            Console.WriteLine("Adding tasks to queue completer tables.");

            // Get tasks that printed for borrower and, if applicable, coborrower as well
            List<TaskData> tasksToClose = tasksProcessed.Where(p => (string.IsNullOrWhiteSpace(p.CoBorrowerSsn) && p.PrintProcessingId.HasValue) || (!string.IsNullOrWhiteSpace(p.CoBorrowerSsn) && p.PrintProcessingId.HasValue && p.CoBwrPrintProcessingId.HasValue)).ToList();
            foreach (TaskData task in tasksToClose)
            {
                if (DA.UpdateQueueCompleter(task))
                {
                    Console.WriteLine($"Updated Queue Completer table for task control number {task.TaskControlNumber}");
                    if (SetProcessedAt(task))
                        Console.WriteLine($"Marked task {task.TaskControlNumber} as processed");
                    else
                        result = false;
                }
                else
                    PLR.AddNotification($"Error adding task: {task.TaskControlNumber} to Queue Completer table.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            return result;
        }

        /// <summary>
        /// Sets the ProcessedAt field in the pifltr.ProcessingQueue table.
        /// </summary>
        private bool SetProcessedAt(TaskData task)
        {
            string errorMessage = string.Empty;
            if ((task.PrintProcessingId == null && task.IsCanceled == false))
                errorMessage += $"Did not set the following task as processed: {task.TaskControlNumber}.  An expected PrintProcessingId for the borrower {task.Ssn} was not found.\n";
            else if ((task.CoBorrowerSsn.IsPopulated() && task.CoBwrPrintProcessingId == null && task.IsCanceled == false))
                errorMessage += $"Did not set the following task as processed: {task.TaskControlNumber}.  An expected CoBwrPrintProcessingId for the co-borrower {task.CoBorrowerSsn} was not found.\n";
            else if (!DA.SetProcessedAt(task))
                errorMessage += $"Error setting processed at for task: {task.TaskControlNumber}.\n";

            if (!errorMessage.IsNullOrEmpty())
            {
                PLR.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }

            return true;
        }
    }
}