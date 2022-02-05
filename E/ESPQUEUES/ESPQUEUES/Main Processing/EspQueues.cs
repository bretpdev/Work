using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.DataAccess.ArcData;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ESPQUEUES
{
    public class EspQueues
    {
        private SessionHelper Session { get; set; }
        private ReflectionInterface RI { get; set; }
        private ProcessLogRun PLR { get; set; }
        public DataAccess DA { get; set; }
        public string UserId { get; set; }
        public const string DR_QUEUE_ARC = "ERBEQ";
        public const string ED_QUEUE_ARC = "ERBDQ";
        public const string NOTE_ARC = "ENOTE";

        public EspQueues(ReflectionInterface ri)
        {
            RI = ri;
            Session = new SessionHelper(RI, Program.ScriptId);
            PLR = ri.LogRun;
            DA = new DataAccess(PLR.LDA);
            UserId = ri.UserId;
        }

        public int Run()
        {
            if (Program.ShowPrompts)
            {
                string startupMessage = "This script works ESP tasks in COMPASS RB queues. Click OK to continue, or Cancel to quit.";
                if (MessageBox.Show(startupMessage, Program.ScriptId, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                {
                    AddNotification("User cancelled script run before any tasks started processing.", NotificationType.Other, NotificationSeverityType.Informational);
                    return Program.SUCCESS;
                }
            }

            DA.LoadTasksIntoProcessingQueue();
            List<QueueTask> unprocessedTasks = DA.GetUnprocessedTasks();
            Console.WriteLine($"There are {unprocessedTasks.Count} unprocessed tasks for the script to work.");
            bool wasSuccessfullyProcessed = true;

            if (Program.AccountNumber.IsNullOrEmpty())
            {
                foreach (QueueTask task in unprocessedTasks)
                {
                    wasSuccessfullyProcessed &= WorkTask(task);
                }
            }
            else if (Program.AccountNumber.Length == 10)
            {
                Console.WriteLine($"Using provided override account {Program.AccountNumber} for run.");
                wasSuccessfullyProcessed &= WorkTask(unprocessedTasks.Where(p => p.AccountNumber == Program.AccountNumber).First());
            }

            string finishMessage = "Processing Complete.";
            if (!wasSuccessfullyProcessed)
                finishMessage = "Processing is complete. However, errors were encountered. Please review Process Logger.";

            if (Program.ShowPrompts)
                MessageBox.Show(finishMessage, Program.ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                Console.WriteLine(finishMessage);

            return wasSuccessfullyProcessed ? Program.SUCCESS : Program.ERROR;
        }

        /// <summary>
        /// Adds record to the DR queue to be worked when session message not recognized
        /// </summary>
        private bool LogUnknownTaskInfo(QueueTask task)
        {
            if (Program.ShowPrompts)
            {
                string infoMessage = $"Info message not found. Info message: {task.Message}.";
                MessageBox.Show(infoMessage, Program.ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return AddDrReviewTask(task, "INFO MESSAGE NOT RECOGNIZED.", ArcType.Atd22AllLoans);
        }

        /// <summary>
        /// Checks for an AnticipatedDisbursement.
        /// </summary>
        /// <returns>True if a borrower has an upcoming disbursement, False if they do not.</returns>
        private bool AnticipatedDisbursementExists()
        {
            //Check for an anticipated disbursement.
            int[] startingColumns = { 19, 35, 52, 69 };
            while (!RI.CheckForText(23, 2, "90007"))
            {
                foreach (int column in startingColumns)
                {
                    if (RI.CheckForText(10, column, "      "))
                        return false;
                    else
                    {
                        bool disbursementAmountIsCancelled = (RI.GetText(13, column, 10) == RI.GetText(16, column, 10));
                        if (RI.CheckForText(10, column, "ANTCPD") && !disbursementAmountIsCancelled) { return true; }
                    }
                }
                RI.Hit(Key.F8);
            }
            return false;
        }

        /// <summary>
        /// Gets the lender notified date from LG29
        /// </summary>
        private void ApplyLenderNotifiedDateFromLG29(string ssn, IEnumerable<Loan> loans)
        {
            RI.FastPath("LG29I" + ssn);
            if (RI.CheckForText(1, 49, "STUDENT ENROLLMENT STATUS SELECT"))
            {
                for (int row = 9; !RI.CheckForText(22, 3, "46004"); row++)
                {
                    if (RI.CheckForText(row, 6, " "))
                    {
                        RI.Hit(Key.F8);
                        row = 8;
                        continue;
                    }
                    string currColl = RI.GetText(row, 14, 8);
                    string enrStat = RI.GetText(row, 25, 1);
                    DateTime effDate = RI.GetText(row, 29, 8).ToDateFormat().ToDate();
                    DateTime egd = RI.GetText(row, 40, 8).ToDateFormat().ToDate();
                    DateTime certDate = RI.GetText(row, 51, 8).ToDateFormat().ToDate();
                    List<Loan> matchingLoans = loans.Where(p => p.OneLinkSchoolCode == currColl && p.EnrollmentStatus == enrStat && p.EnrollmentStatusEffectiveDate == effDate && p.ExpectedGraduationDate == egd && p.CertificationDate == certDate).ToList();
                    if (matchingLoans?.Count > 0)
                    {
                        DateTime lenderNotif = Loan.NoDate;
                        if (DateTime.TryParse(RI.GetText(row, 62, 8).ToDateFormat(), out lenderNotif)) //MMddyyyy 
                        {
                            foreach (Loan ln in matchingLoans)
                                ln.LenderNotifiedDate = lenderNotif;
                        }
                    }
                }
            }
            else
            {
                DateTime lenderNotif = Loan.NoDate;
                if (DateTime.TryParse(RI.GetText(13, 35, 8).ToDateFormat(), out lenderNotif)) //MMddyyyy
                    loans.First().LenderNotifiedDate = lenderNotif;
            }
        }

        /// <summary>
        /// Gets separation history from Ts01
        /// </summary>
        private void ApplySeparationHistoryInfoFromTS01(string ssn, IEnumerable<Loan> loansWithoutSeparationInfo)
        {
            RI.FastPath("TX3Z/ITS01" + ssn);
            RI.Hit(Key.F10);
            for (int row = 9; !RI.CheckForText(23, 2, "90007"); row++)
            {
                if (RI.CheckForText(row, 6, " "))
                {
                    RI.Hit(Key.F8);
                    row = 8;
                    continue;
                }
                string loanType = RI.GetText(row, 33, 6);
                DateTime firstDisbursementDate = DateTime.Parse(RI.GetText(row, 40, 8)); //MM/dd/yy
                Loan loan = loansWithoutSeparationInfo.SingleOrDefault(p => p.Program == loanType && p.FirstDisbursementDate == firstDisbursementDate);
                if (loan != null)
                {
                    loan.SeparationDate = DateTime.Parse(RI.GetText(row, 6, 8)); //MM/dd/yy
                    loan.SeparationReason = RI.GetText(row, 54, 2);
                    loan.CompassNotifiedDate = RI.GetText(row, 62, 8).IsNullOrEmpty() ? new DateTime() : DateTime.Parse(RI.GetText(row, 62, 8)); //MM/dd/yy
                    loan.CompassCertifiedDate = DateTime.Parse(RI.GetText(row, 71, 8)); //MM/dd/yy
                    loan.CompassSchoolCode = RI.GetText(row, 24, 8);
                    if (RI.CheckForText(row, 49, "YES")) { loan.WasDeferredOrForbeared = true; }
                }
            }
        }

        /// <summary>
        /// Checks onelink for LINKING and LINKCONC tasks.
        /// </summary>
        /// <returns>True if A task exists.  False if it does not.</returns>
        private bool CheckForLinkTask(string ssn)
        {
            string[] LINK_TASKS = { "LINKING", "LINKCONC" };
            bool linkTaskExists = false;

            RI.FastPath("LP9OI" + ssn);
            if (RI.CheckForText(1, 61, "OPEN ACTIVITY DETAIL"))
                linkTaskExists = RI.CheckForText(3, 21, LINK_TASKS);
            else if (RI.CheckForText(1, 58, "OPEN ACTIVITY SELECTION"))
            {
                for (int row = 8; !RI.CheckForText(22, 3, "46004"); row++)
                {
                    if (RI.CheckForText(row, 11, LINK_TASKS))
                    {
                        linkTaskExists = true;
                        break;
                    }
                    if (RI.CheckForText(row, 7, " "))
                    {
                        RI.Hit(Key.F8);
                        row = 7;
                    }
                }
            }
            return linkTaskExists;
        }

        /// <summary>
        /// Adds ERBEQ ARC to create a new task for d/f eligibility review
        /// </summary>
        private bool CreateDefermentElgibilityReviewTasks(QueueTask task)
        {
            return AddDrReviewTask(task, "account sent to queue to be reviewed for deferment/forbearance eligibility and/or changes");
        }

        /// <summary>
        /// Adds ERBDQ ARC to add a new queue task for deferment review
        /// </summary>
        private void CreateDefermentReviewTasks(QueueTask task, IEnumerable<Loan> deferredLoans)
        {
            const string ERB_COMMENT_FORMAT = "enr status = {0}; esd = {1:MM/dd/yyyy}; egd = {2:MM/dd/yyyy}; certdate = {3:MM/dd/yyyy}; review for deferment eligibility";
            Loan lastLoan = deferredLoans.Last();
            object[] ERB_COMMENT_ARGS = { lastLoan.EnrollmentStatus, lastLoan.EnrollmentStatus.ToDateNullable(), lastLoan.ExpectedGraduationDate, lastLoan.CertificationDate };
            List<int> loanSequences = deferredLoans.Select(p => p.Sequence).ToList();
            AddArc(task, ED_QUEUE_ARC, string.Format(ERB_COMMENT_FORMAT, ERB_COMMENT_ARGS), ArcType.Atd22ByLoan, loanSequences);
            AddArc(task, NOTE_ARC, "sent to ED queue for deferment review", ArcType.Atd22ByLoan, loanSequences);
        }

        private bool CreateGracePeriodReviewTasks(QueueTask task)
        {
            ArcType arcType = ArcType.Atd22ByBalance;
            bool result = AddArc(task, DR_QUEUE_ARC, task.ToString() + $" {{{{{Program.ScriptId}}}}}", arcType);
            result &= AddArc(task, NOTE_ARC, "account sent to DR queue for review of grace period" + $" {{{{{Program.ScriptId}}}}}", arcType);
            if (!task.ReassignedAt.HasValue)
            {
                task.ReassignedAt = DateTime.Now;
                DA.SetReassignedAt(task);
            }
            return result && task.ReassignedAt.HasValue && task.ArcAddProcessingId.HasValue;
        }

        private bool CreateMilitaryStatusReviewTasks(QueueTask task)
        {
            IEnumerable<Loan> loans = Session.GetCompassLoansFromTS26(task.BorrowerSsn);
            if (!HasLoansToWork(task, loans, "Unable to perform military status review."))
                return false;

            ApplySeparationHistoryInfoFromTS01(task.BorrowerSsn, loans);
            if (new string[] { "01", "02", "08" }.Contains(task.SeparationReason))
            {
                foreach (Loan loan in loans)
                {
                    DateTime ninetyDaysAfterEnrollment = loan.EnrollmentStatusEffectiveDate.AddDays(90);
                    DateTime ninetyDaysBeforeEnrollment = loan.EnrollmentStatusEffectiveDate.AddDays(-90);
                    bool separationDateIsWithinNinetyDaysOfEnrollment = (task.SeparationDate >= ninetyDaysBeforeEnrollment || task.SeparationDate <= ninetyDaysAfterEnrollment);
                    if (separationDateIsWithinNinetyDaysOfEnrollment && loan.SeparationReason == "ML") { loan.CommentIndicator = Loan.CommentStatus.Needed; }
                }
            }
            foreach (Loan loan in loans) { loan.CommentIndicator = (loan.CommentIndicator == Loan.CommentStatus.None ? Loan.CommentStatus.Needed : Loan.CommentStatus.None); }
            AddArc(task, "ERBMQ", task.ToString(), ArcType.Atd22ByLoan, loans.SequenceNumbersMarkedForComments());
            DA.SetReassignedAt(task);
            AddArc(task, NOTE_ARC, "account sent to MB queue for review of military status vs enrollment status", ArcType.Atd22ByLoan, loans.SequenceNumbersMarkedForComments());

            string comment = "borrower withdrew from school due to call to active duty.  military status already on account";
            AddArc(task, NOTE_ARC, comment, ArcType.Atd22ByLoan, loans.SequenceNumbersMarkedForComments()); // TODO: See if BA really wants double ENOTEs dropped on the account. Seems unnecessary.

            return task.ReassignedAt.HasValue && task.ArcAddProcessingId.HasValue;
        }

        private bool CreateMissingDataReviewTasks(QueueTask task)
        {
            return AddDrReviewTask(task, "account sent to DR queue for review of missing data");
        }

        private bool CreateSchoolCodeReviewTasks(QueueTask task)
        {
            return AddDrReviewTask(task, "account sent to DR queue for review of school code");
        }

        private void CreateWglReviewTasks(QueueTask task, IEnumerable<Loan> loans, string separationReason)
        {
            foreach (Loan loan in loans) { loan.CommentIndicator = Loan.CommentStatus.None; }
            IEnumerable<Loan> wglLoans = loans.NonSeparatedWglLoans(separationReason);
            RI.FastPath("TX3Z/ITS2H" + task.BorrowerSsn);
            if (RI.CheckForText(1, 72, "TSX2I")) //Selection screen.
            {
                int row = 8;
                while (!RI.CheckForText(23, 2, "90007"))
                {
                    Loan loan = wglLoans.SingleOrDefault(p => RI.CheckForText(row, 5, p.FirstDisbursementDate.ToString("MM/dd/yy")) && RI.CheckForText(row, 18, p.Program));
                    if (loan != null)
                    {
                        RI.PutText(21, 12, RI.GetText(row, 2, 2), Key.Enter, true);
                        if (AnticipatedDisbursementExists()) { loan.CommentIndicator = Loan.CommentStatus.Needed; }
                        RI.Hit(Key.F12);
                    }
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        RI.Hit(Key.F8);
                        row = 8;
                    }
                }
            }
            else //Target screen.
            {
                string loanType = RI.GetText(5, 12, 6);
                DateTime firstDisbursementDate = DateTime.Parse(RI.GetText(6, 17, 8));
                Loan wglLoan = wglLoans.FirstOrDefault();
                if (wglLoan != null && wglLoan.Program == loanType && wglLoan.FirstDisbursementDate == firstDisbursementDate && AnticipatedDisbursementExists())
                    wglLoan.CommentIndicator = Loan.CommentStatus.Needed;
            }

            //Add an activity record if any loans are flagged for a comment.
            List<int> loanSequences = loans.SequenceNumbersMarkedForComments();
            if (loanSequences.Count > 0)
            {
                AddArc(task, "ERBAQ", "borrower is L, W, or G and has future disbursements scheduled.  Please review for cancellation.", ArcType.Atd22ByLoan, loanSequences);
            }
        }

        private bool EnrollmentReview(QueueTask task, IEnumerable<Loan> loans)
        {
            IEnumerable<Loan> consolLoans = loans.ConsolLoans();
            if (consolLoans.Count() > 0)
            {
                Loan lastLoan = consolLoans.Last();
                List<int> loanSequences = consolLoans.Select(p => p.Sequence).ToList();
                const string COMMENT_FORMAT = "enr status = {0}; esd = {1:MM/dd/yyyy}; egd = {2:MM/dd/yyyy}; certdate = {3:MM/dd/yyyy}; review for deferment eligibility";
                object[] COMMENT_ARGS = { lastLoan.EnrollmentStatus, lastLoan.EnrollmentStatusEffectiveDate, lastLoan.ExpectedGraduationDate, lastLoan.CertificationDate };
                AddArc(task, ED_QUEUE_ARC, string.Format(COMMENT_FORMAT, COMMENT_ARGS), ArcType.Atd22ByLoan, loanSequences);
                AddArc(task, NOTE_ARC, "sent to ED queue for deferment review", ArcType.Atd22ByLoan, loanSequences);
            }

            loans = IntersectEnrollmentInfoFromLG46(task.BorrowerSsn, loans, task.ToString()); //Add loan data from OneLINK.
            if (loans.Count() == 0) { return true; }
            ApplyLenderNotifiedDateFromLG29(task.BorrowerSsn, loans);

            IEnumerable<Loan> loansWithoutLenderNotifiedDate = loans.Where(p => p.LenderNotifiedDate == Loan.NoDate).ToList();
            if (loansWithoutLenderNotifiedDate.Count() > 0)   //See if any loans had no lender notified date on LG29. Get the date from the user if needed.
            {
                return AddDrReviewTask(task, $"unable to determine notify dt"); // Task was reassigned due to missing lender notif dt
            }

            IEnumerable<Loan> loansWithoutSeparationInfo = loans.Where(p => p.SeparationDate == Loan.NoDate).ToList();
            if (loansWithoutSeparationInfo.Count() > 0)  //Add separation info for any loans that don't have it.
                ApplySeparationHistoryInfoFromTS01(task.BorrowerSsn, loansWithoutSeparationInfo);

            IEnumerable<Loan> deferredLoans = loans.Where(p => p.WasDeferredOrForbeared && p.HasCurrentDefermentOrForbearance).ToList();
            if (deferredLoans.Count() > 0)  //Create queue tasks for loans in deferment/forbearance.
                CreateDefermentReviewTasks(task, deferredLoans);

            if (!task.ProcessingStepId.HasValue)
                task.ProcessingStepId = 0;

            if (task.ProcessingStepId.Value < (int)ProcessingSteps.ProcessingStep.UpdateSeparationDate)
            { //Update the separation date in COMPASS if needed.
                UpdateCompassEnrollmentInfo(task, loans);
                task.ProcessingStepId++;
                DA.SetProcessingStepId(task.ProcessingStepId.Value, task.ProcessingQueueId);
            }

            if (task.ProcessingStepId.Value < (int)ProcessingSteps.ProcessingStep.ReviewWglStatusLoans)  //See if there are any loans with W, G, or L status to be reviewed.
            {
                CreateWglReviewTasks(task, loans, task.SeparationReason);
                task.ProcessingStepId++;
                DA.SetProcessingStepId(task.ProcessingStepId.Value, task.ProcessingQueueId);
            }

            if (task.ProcessingStepId.Value < (int)ProcessingSteps.ProcessingStep.CompleteStTasks)  //Complete all ST tasks for the borrower.
            {
                if (!Program.SkipTaskClose)
                    Session.CompleteStTasks(task.BorrowerSsn);
                task.ProcessingStepId++;
                DA.SetProcessingStepId(task.ProcessingStepId.Value, task.ProcessingQueueId);
            }

            if (task.ProcessingStepId.Value < (int)ProcessingSteps.ProcessingStep.AddPlusLoansComment) //Review the loans and add a comment for PLUS loans if any are found.
            {
                if (loans.Any(p => p.Program == "PLUS" || p.Program == "PLUSGB"))
                {
                    string plusComment = "PLUS borrower is attending school.  Please review for deferment eligibility";
                    AddArc(task, ED_QUEUE_ARC, plusComment, ArcType.Atd22ByLoanProgram, null, "PLUS", "PLUSGB");
                }
                task.ProcessingStepId++;
                DA.SetProcessingStepId(task.ProcessingStepId.Value, task.ProcessingQueueId);
            }

            return (task.ProcessingStepId.Value >= (int)ProcessingSteps.ProcessingStep.AddPlusLoansComment);
        }

        /// <summary>
        /// Returns the subset of passed-in loans that have enrollment info on LG46,
        /// with the enrollment info reflected in the returned objects.
        /// </summary>
        private IEnumerable<Loan> IntersectEnrollmentInfoFromLG46(string ssn, IEnumerable<Loan> loans, string taskDetails)
        {
            loans = Session.GetLg46Data(ssn, loans);

            List<int> loanSequences = loans.ConsolLoans().LoansMissingEnrollmentInfo().Select(p => p.Sequence).ToList(); //Add an activity record if any consol loans are missing information.
            if (loanSequences.Count > 0) { RI.Atd22ByLoan(ssn, NOTE_ARC, "consolidation loan not found in OneLINK.  no current enrollment for this borrower.", "", loanSequences, Program.ScriptId, false); }

            loanSequences = loans.NonConsolLoans().LoansMissingEnrollmentInfo().Select(p => p.Sequence).ToList(); //Add an activity record if any non-consol loans are missing information.
            if (loanSequences.Count > 0)
            {
                RI.Atd22ByLoan(ssn, DR_QUEUE_ARC, taskDetails, "", loanSequences, Program.ScriptId, false);
                RI.Atd22ByLoan(ssn, NOTE_ARC, "sent to DR queue for review of missing loan on LG46", "", loanSequences, Program.ScriptId, false);
            }

            loans = loans.LoansNotMissingEnrollmentInfo(); //Remove loans with missing information.
            return loans;
        }

        /// <summary>
        /// Checks to see if the borrowers loans have a payoff amt
        /// </summary>
        /// <returns>True if the borrowers loans have a payoff amount.  False if they do not.</returns>
        private bool LoanHasPayoffAmountOnTS2O(string ssn, IEnumerable<Loan> loans, DateTime payoffDate)
        {
            RI.FastPath("TX3Z/ITS2O" + ssn);
            RI.PutText(7, 26, payoffDate.ToString("MMddyy"));
            RI.PutText(9, 16, "X"); //SELECT ALL
            RI.PutText(9, 54, "N"); //INCLUDE OUTSTANDING LATE FEES
            RI.Hit(Key.Enter);
            IEnumerable<Loan> nonPlusLoans = loans.Where(p => p.Program != "PLUS" && p.GuarantyDate < new DateTime(1993, 7, 1)).ToList();
            int row = 13;
            while (!RI.CheckForText(23, 2, "90007"))
            {
                DateTime firstDisbursementDate = DateTime.Parse(RI.GetText(row, 27, 8));
                string loanProgram = RI.GetText(row, 43, 6);
                double payoffAmount = double.Parse(RI.GetText(row, 58, 10).Replace(",", ""));
                if (nonPlusLoans.Any(p => p.FirstDisbursementDate == firstDisbursementDate && p.Program == loanProgram) && payoffAmount > 0)
                    return true;
                else
                {
                    row++;
                    if (!RI.CheckForText(row, 2, "_"))
                    {
                        RI.Hit(Key.F8);
                        row = 13;
                    }
                }
            }//while
            return false;
        }

        private bool PlusReview(QueueTask task)
        {
            IEnumerable<Loan> loans = Session.GetCompassLoansFromTS26(task.BorrowerSsn);
            if (!HasLoansToWork(task, loans, "Unable to find loans while doing plus review."))
                return false;

            //Get the earliest disbursement date.
            DateTime earliestDisbursementDate = loans.Where(p => p.Program.StartsWith("PLUS")).Select(p => p.FirstDisbursementDate).Min();
            //Add comments, tasks.
            bool disbursementDateIsWithinRange = (earliestDisbursementDate >= new DateTime(1987, 7, 1) && earliestDisbursementDate < new DateTime(1993, 7, 1));
            if (!disbursementDateIsWithinRange) { disbursementDateIsWithinRange = (earliestDisbursementDate >= new DateTime(1993, 7, 1) && LoanHasPayoffAmountOnTS2O(task.BorrowerSsn, loans, earliestDisbursementDate)); }
            if (disbursementDateIsWithinRange)
            {
                if (!task.ArcAddProcessingId.HasValue)
                {
                    string originalMessage = task.Message;
                    task.Message = "student enrollment received.  PLUS borrower eligible for deferment";
                    AddArc(task, ED_QUEUE_ARC, task.ToString(), ArcType.Atd22ByLoanProgram, null, "PLUS");
                    task.Message = originalMessage;
                    string comment = "student enrollment received.  eligible for deferment.  sent to queue for deferment to be added";
                    AddArc(task, NOTE_ARC, comment, ArcType.Atd22ByLoanProgram, null, "PLUS");
                    DA.SetReassignedAt(task);
                }
            }
            else
            {
                if (!task.ArcAddProcessingId.HasValue)
                {
                    string originalMessage = task.Message;
                    task.Message = "student enrollment received.  review loans on NSLDS to determine eligibility for deferment";
                    AddArc(task, ED_QUEUE_ARC, task.ToString(), ArcType.Atd22ByLoanProgram, null, "PLUS");
                    task.Message = originalMessage;
                    string comment = "student enrollment received.  eligible for deferment.  sent to queue to be reviewed for deferment eligibility";
                    AddArc(task, NOTE_ARC, comment, ArcType.Atd22ByLoanProgram, null, "PLUS");
                    DA.SetReassignedAt(task);
                }
            }

            return task.ReassignedAt.HasValue && task.ArcAddProcessingId.HasValue; // Indicates we have properly reassigned task by creating a review ARC
        }

        private bool ProcessAccordingToTaskComment(QueueTask task)
        {
            if (task.Status == QueueTask.ProcessingMethod.CreateDefermentReviewTasks)
                return CreateDefermentElgibilityReviewTasks(task);
            else if (task.Status == QueueTask.ProcessingMethod.ReviewAccount)
                return ReviewAccount(task);
            else if (task.Status == QueueTask.ProcessingMethod.CreateSchoolCodeReviewTasks)
                return CreateSchoolCodeReviewTasks(task);
            else if (task.Status == QueueTask.ProcessingMethod.CreateMilitaryStatusReviewTasks)
                return CreateMilitaryStatusReviewTasks(task);
            else if (task.Status == QueueTask.ProcessingMethod.CreateMissingDataReviewTasks)
                return CreateMissingDataReviewTasks(task);
            else if (task.Status == QueueTask.ProcessingMethod.ReviewDateDiscrepancy)
                return ReviewDateDiscrepancy(task);
            else if (task.Status == QueueTask.ProcessingMethod.PlusReview)
                return PlusReview(task);
            else if (task.Status == QueueTask.ProcessingMethod.ReviewDefermentEligibility)
                return ReviewDefermentEligibility(task);
            else if (task.Status == QueueTask.ProcessingMethod.PromptForManualUpdate)
                return PromptForManualUpdate(task);
            else if (task.Status == QueueTask.ProcessingMethod.CreateGracePeriodReviewTasks)
                return CreateGracePeriodReviewTasks(task);
            else if (task.Status == QueueTask.ProcessingMethod.ManualReview)
                return AddDrReviewTask(task, "Sent to DR queue for manual review.", ArcType.Atd22AllLoans);
            else //Assuming this was adding to errors...
                return LogUnknownTaskInfo(task);

        }

        private bool PromptForManualUpdate(QueueTask task)
        {
            return AddDrReviewTask(task, $"Task needs manual update. Review the borrower's enrollment history on the clearinghouse system or NSLDS to determine the appropriate status, update OneLINK and COMPASS manually", ArcType.Atd22AllLoans);
        }

        private bool ReviewAccount(QueueTask task)
        {
            if (task.BorrowerSsn != task.StudentSsn)
                return PlusReview(task);
            else
            {
                IEnumerable<Loan> loans = Session.GetCompassLoansFromTS26(task.BorrowerSsn);
                return EnrollmentReview(task, loans);
            }
        }

        private bool ReviewDateDiscrepancy(QueueTask task)
        {
            bool processResult = true;

            //Process enrollment for loans where lender notified date > guaranty date.
            IEnumerable<Loan> loansWithLaterLenderDate = Session.GetCompassLoansFromTS26(task.BorrowerSsn).Where(p => task.LenderNotifiedDate > p.GuarantyDate);
            //Tag loans to process (task notice date > loan guar date).
            foreach (Loan loan in loansWithLaterLenderDate) { loan.CommentIndicator = Loan.CommentStatus.Needed; }
            if (loansWithLaterLenderDate?.Count() > 0)
            {
                if (task.BorrowerSsn == task.StudentSsn)
                    processResult = EnrollmentReview(task, loansWithLaterLenderDate);
                else
                    processResult = PlusReview(task); // TODO: Ask BA if this is right. Seems odd to do review on all loans and not just ones with date discrepancy
            }

            foreach (Loan loan in loansWithLaterLenderDate) { loan.CommentIndicator = Loan.CommentStatus.None; } // Reset comment status so non-consol comments will only be on non-consol loans
            //Add comments for loans where lender notified date <= guaranty date.
            IEnumerable<Loan> loansWithEarlierLenderDate = Session.GetCompassLoansFromTS26(task.BorrowerSsn).Where(p => task.LenderNotifiedDate <= p.GuarantyDate);
            foreach (Loan loan in loansWithEarlierLenderDate.NonConsolLoans()) { loan.CommentIndicator = Loan.CommentStatus.Needed; }

            if (loansWithEarlierLenderDate?.Count() > 0)
                processResult &= AddArc(task, NOTE_ARC, "enrollment notice is prior to guaranty. no update", ArcType.Atd22ByLoan, loansWithEarlierLenderDate.SequenceNumbersMarkedForComments());
            else if (loansWithLaterLenderDate == null || loansWithLaterLenderDate.Count() == 0)
            {
                return AddDrReviewTask(task, "Task reviewed for enrollment notice date discrepancy, but no loans with an enrollment date discrepancy were found. Please review.");
            }
            return processResult;
        }

        private bool ReviewDefermentEligibility(QueueTask task)
        {
            bool processResult = true;

            //Review loans where the enrollment status effective date <= repayment start date.
            IEnumerable<Loan> loansLessRepayStart = Session.GetCompassLoansFromTS26(task.BorrowerSsn).Where(p => p.EnrollmentStatus.ToDateNullable() <= p.RepaymentStartDate);
            foreach (Loan loan in loansLessRepayStart) { loan.CommentIndicator = Loan.CommentStatus.Needed; }
            if (loansLessRepayStart?.Count() > 0)
            {
                if (task.BorrowerSsn == task.StudentSsn)
                    processResult = EnrollmentReview(task, loansLessRepayStart);
                else
                    processResult = PlusReview(task); // TODO: Ask BA if this is right. Seems odd to do review on all loans and not just ones with date discrepancy
            }

            foreach (Loan loan in loansLessRepayStart) { loan.CommentIndicator = Loan.CommentStatus.None; } // Reset comment status

            //Add comments for loans where the enrollment status effective date > the repayment start date.
            IEnumerable<Loan> loansGreaterRepayStart = loansLessRepayStart.Where(p => p.EnrollmentStatus.ToDateNullable() > p.RepaymentStartDate);
            foreach (Loan loan in loansGreaterRepayStart) { loan.CommentIndicator = Loan.CommentStatus.Needed; }
            if (loansGreaterRepayStart?.Count() > 0)
            {
                processResult &= AddArc(task, ED_QUEUE_ARC, task.ToString(), ArcType.Atd22AllLoans);
                processResult &= AddArc(task, NOTE_ARC, "account sent to queue to be reviewed for deferment/forbearance eligibility and/or changes", ArcType.Atd22AllLoans);
            }
            else if (loansLessRepayStart == null || loansLessRepayStart.Count() == 0)
            {
                return AddDrReviewTask(task, "Task reviewed for def elig, but no loans to review were found.");
            }
            return processResult;
        }

        private void UpdateCompassEnrollmentInfo(QueueTask task, IEnumerable<Loan> loans)
        {
            //Group the loans by enrollment info, and update COMPASS or add comments.
            for (Loan firstUnmarkedLoan = loans.FirstOrDefault(p => p.CommentIndicator == Loan.CommentStatus.None); firstUnmarkedLoan != null; firstUnmarkedLoan = loans.FirstOrDefault(p => p.CommentIndicator == Loan.CommentStatus.None))
            {
                //Mark the loan for comments, along with any others that have matching enrollment info.
                firstUnmarkedLoan.CommentIndicator = Loan.CommentStatus.Needed;
                foreach (Loan matchingEnrollmentLoan in loans.Where(p => p.EnrollmentEquals(firstUnmarkedLoan))) { matchingEnrollmentLoan.CommentIndicator = Loan.CommentStatus.Needed; }

                //Find the first loan in the group that isn't deferred.
                Loan firstActiveLoan = loans.FirstOrDefault(p => p.CommentIndicator == Loan.CommentStatus.Needed && !p.HasCurrentDefermentOrForbearance);
                if (firstActiveLoan != null)
                {
                    //Review the loan data and determine if COMPASS needs to be updated.
                    DateTime newSeparationDate = Loan.NoDate;
                    switch (firstActiveLoan.EnrollmentStatus)
                    {
                        case "F":
                        case "H":
                        case "A":
                            newSeparationDate = task.SeparationDate;
                            break;
                        case "W":
                        case "G":
                        case "L":
                            newSeparationDate = task.SeparationDate;
                            break;
                    }
                    string abbreviatedSeparationReason = firstActiveLoan.GetAbbreviatedSeparationReason(task.SeparationReason);
                    if (newSeparationDate != Loan.NoDate && (newSeparationDate != firstActiveLoan.SeparationDate || abbreviatedSeparationReason != firstActiveLoan.EnrollmentStatus))
                        UpdateSeparationDateOnTS01(task, loans, firstActiveLoan);
                    else
                        AddArc(task, NOTE_ARC, "reviewed enrollment for borrower; matches onelink.no update performed", ArcType.Atd22ByLoan, loans.SequenceNumbersMarkedForComments());
                }

                //Mark all loans in this group as having been processed.
                foreach (Loan loan in loans.Where(p => p.CommentIndicator == Loan.CommentStatus.Needed)) { loan.CommentIndicator = Loan.CommentStatus.Added; }
            }
        }

        private void UpdateSeparationDateOnTS01(QueueTask task, IEnumerable<Loan> loans, Loan firstUpdateLoan)
        {
            //Add an activity record.
            string comment = ScriptHelper.GenerateTs01Comment(firstUpdateLoan, task.SeparationReason);
            AddArc(task, NOTE_ARC, comment, ArcType.Atd22ByLoan, loans.SequenceNumbersMarkedForComments());

            bool? encounteredError01710 = Session.UpdateTs01(task.BorrowerSsn, task.Message, loans, firstUpdateLoan);

            if (!encounteredError01710.HasValue)
                PLR.AddNotification($"The task {task} encountered an error on TS01 while trying to update the sep date.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            else if (encounteredError01710.Value) //Add task if error code 01710 was displayed.
                AddArc(task, ED_QUEUE_ARC, "defer/forb needs to be inactivated; sep date changed", ArcType.Atd22ByLoan, loans.SequenceNumbersMarkedForComments());
        }

        /// <summary>
        /// Works the given task in the session.
        /// </summary>
        private bool WorkTask(QueueTask task)
        {
            bool processResult = true;
            RI.FastPath($"TX3Z/ITX6X{task.Queue};{task.SubQueue};{task.TaskControlNumber};{task.RequestArc}");
            if (RI.MessageCode == "01020" || !RI.CheckForText(8, 75, "U"))
            {
                DA.SetProcessedAt(task);
                AddNotification($"Task did not exist in an unassigned status on ITX6X for ProcessingQueueId {task.ProcessingQueueId}. Encountered session message: {RI.Message}\n The task has been marked as processed in the espqueues.ProcessingQueue table.", NotificationType.Other, NotificationSeverityType.Informational);
                return true;
            }

            task.Populate(RI); //Get information from the first task.
            if (task.Status == QueueTask.ProcessingMethod.FailedToParseTaskInfo)
            {
                AddDrReviewTask(task, "Unable to parse task message for ESP info. Task assigned to DR queue for review.");
                AddNotification($"Unable to parse task message for ESP info. Task assigned to DR queue for review. Task: {task}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return true;
            }

            RI.PutText(21, 18, "01", Key.Enter); // Enter task

            bool hasLinkTask = CheckForLinkTask(task.BorrowerSsn);

            if (hasLinkTask)
                return AddDrReviewTask(task, "Account has a OL task of LINKING or LINKCONC. Task being sent to DR queue for review");
            else
            {
                if (Session.GetCompassLoansFromTS26(task.BorrowerSsn).Any(p => p.Program == "PLUS" || p.Program == "PLUSGB"))
                {
                    if (processResult = AddArc(task, ED_QUEUE_ARC, task.ToString(), ArcType.Atd22ByLoanProgram, null, "PLUS", "PLUSGB"))
                        DA.SetReassignedAt(task);
                    processResult &= AddArc(task, NOTE_ARC, "Task sent to ED queue for review.", ArcType.Atd22ByLoanProgram, null, "PLUS", "PLUSGB");
                }
                else
                {
                    if (CheckLp22(task.BorrowerSsn))//Check to see if the borrower exists on LP22. 
                    {
                        if (!ProcessAccordingToTaskComment(task)) //Process the task if the borrower SSN was not found in the list of borrowers processed today.
                            return false;
                    }
                    else
                        processResult &= AddDrReviewTask(task, "Sent to DR queue for review, borrower not in OneLINK [ESP]");
                }

                if (Session.CloseTask(task.SubQueue)) //Complete the task if it wasn't reassigned.
                    DA.SetProcessedAt(task);
                else
                {
                    AddNotification($"Unable to close task. Created {DR_QUEUE_ARC} review task. Task: {task}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return AddDrReviewTask(task, "Sent to DR queue for review, task not closed in session");
                }
            }

            return processResult;
        }

        private bool AddArc(QueueTask task, string arc, string comment, ArcType type, List<int> loanSequences = null, params string[] loanPrograms)
        {
            if (arc != NOTE_ARC)
            {
                if (DA.HasOpenQueueTask(task.BorrowerSsn, arc)) // See if account already has the task this ARC generates, if so: don't drop ARC
                {
                    AddNotification($"Did not add ARC {arc} for Account {task.AccountNumber} because there is already an associated task on the account.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return true;
                }
            }

            ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                Arc = arc,
                ArcTypeSelected = type,
                AccountNumber = task.AccountNumber,
                Comment = comment,
                LoanSequences = loanSequences ?? new List<int>(),
                LoanPrograms = loanPrograms?.Length > 0 ? loanPrograms.ToList() : new List<string>(),
                IsEndorser = false,
                IsReference = false,
                RecipientId = task.BorrowerSsn,
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arcData.AddArc();
            if (result != null && result.ArcAdded && result.ArcAddProcessingId > 0) // Update any ProcessingQueue record with the ARC that has been added
            {
                if (task.ArcAddProcessingId == null) // Some accounts will receive an ENOTE in addition to the main ARC dropped on the account. We record the first one in the ProcessingQueue table
                {
                    DA.SetArcAddId(task.ProcessingQueueId, result.ArcAddProcessingId);
                    task.ArcAddProcessingId = result.ArcAddProcessingId;
                }
                return true;
            }
            else
            {
                AddNotification($"Error adding ARC {arc} for Account {task.AccountNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                return false;
            }
        }

        /// <summary>
        /// Adds a DR review task to the account.
        /// </summary>
        private bool AddDrReviewTask(QueueTask task, string comment, ArcType arcType = ArcType.Atd22ByBalance)
        {
            bool result = AddArc(task, DR_QUEUE_ARC, task?.ToString() + $" {{{{{Program.ScriptId}}}}}", arcType);
            result &= AddArc(task, NOTE_ARC, comment + $" {{{{{Program.ScriptId}}}}}", arcType);
            if (!task.ReassignedAt.HasValue)
            {
                task.ReassignedAt = DateTime.Now;
                DA.SetReassignedAt(task);
            }
            return result && task.ReassignedAt.HasValue && task.ArcAddProcessingId.HasValue;
        }

        /// <summary>
        /// Checks LP22 to see if the borrower exists on oneLink
        /// </summary>
        /// <returns>True if the borrower exists on LP22</returns>
        private bool CheckLp22(string ssn)
        {
            RI.FastPath("LP22I*");
            RI.PutText(4, 33, ssn, Key.Enter, true);
            return RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS");
        }//WorkRbQueue()

        private void AddNotification(string message, NotificationType notificationType, NotificationSeverityType severityType, Exception ex = null)
        {
            if (ex == null)
            {
                PLR.AddNotification(message, notificationType, severityType);
                Console.Write(message);
            }
            else
            {
                PLR.AddNotification(message, notificationType, severityType, ex);
                Console.Write($"{message}\n Exception encountered: {ex.Message}");
            }
        }

        private bool HasLoansToWork(QueueTask task, IEnumerable<Loan> loans, string message)
        {
            if (loans?.Count() > 0)
                return true;

            AddNotification($"Unable to find loans in session for account {task.AccountNumber}. {message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            return false;
        }
    }
}
