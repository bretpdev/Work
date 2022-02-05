using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using static BATCHESP.EspEnrollmentResults;

namespace BATCHESP
{
    public class BATCHESP
    {
        private DataAccessHelper.Region region;
        private string batchLoginType;
        private bool skipWorkAdd;
        private bool skipTaskClose;
        private string overrideAccountIdentifiers;
        private string overrideQueues;
        private bool skipTaskAssign;
        private int numberOfThreads;
        private bool hasQueueAccess = true;
        private BatchProcessingHelper BLH;
        private ProcessLogRun PLR;
        private DataAccess DA;
        private ReflectionInterface RI;
        private NonSelectionHelper NSH;
        private TaskHelper TH;
        private DefermentForbearanceHelper DFH;
        public int threadId { get; set; }

        public BATCHESP(DataAccessHelper.Region region, string batchLoginType, Args parsedArgs, ProcessLogRun plr, int threadId)
        {
            this.region = region;
            this.batchLoginType = batchLoginType;
            this.skipWorkAdd = parsedArgs.SkipWorkAdd;
            this.skipTaskClose = parsedArgs.SkipTaskClose;
            this.overrideAccountIdentifiers = parsedArgs.AccountIdentifiers;
            this.overrideQueues = parsedArgs.SubQueues;
            this.skipTaskAssign = parsedArgs.SkipTaskAssign;
            this.numberOfThreads = parsedArgs.NumberOfThreads;
            this.PLR = plr;
            this.threadId = threadId;
            this.DA = new DataAccess(PLR.ProcessLogId, region);
            DataAccessHelper.CurrentRegion = region;
        }

        /// <summary>
        /// Initial load of the ESP tasks to the ESP processing tables.
        /// </summary>
        public LoadResults LoadTasks()
        {
            if (!skipWorkAdd)
            {
                Console.WriteLine("Adding new work to queue...");
                if (!DA.AddNewWorkToTables())
                {
                    PLR.AddNotification("Error adding new work, please retry.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return new LoadResults(0, null, false);
                }
            }

            var work = IdentifyTasksToWork();
            SetThreadCount(work);

            return new LoadResults(numberOfThreads, work, true);
        }

        /// <summary>
        /// Logs into the Session and sets default values to prep for processing run.
        /// </summary>
        public void Login()
        {
            // Helper classes, queues, etc. only needed for a processing run and not for the load run (hence why not in ctor)
            this.NSH = new NonSelectionHelper(this.DA);
            RI = new ReflectionInterface();
            BLH = BatchProcessingLoginHelper.Login(PLR, RI, "BATCHESP", "BatchUheaa");
            skipTaskAssign &= DataAccessHelper.TestMode;
            hasQueueAccess = CheckQueueAccess(BLH.UserName);

            TH = new TaskHelper(PLR, DA, region, RI, BLH.UserName, skipTaskClose, skipTaskAssign);
            DFH = new DefermentForbearanceHelper(PLR, RI, TH, NSH);
            TH.SetDefermentForbearanceHelper(DFH);
        }

        /// <summary>
        /// Determines which tasks to work, based on passed in args.
        /// </summary>
        public List<EspEnrollment> IdentifyTasksToWork()
        {
            var work = DA.GetUnprocessedEspEnrollments();
            LogMessage("Found {0} ESP Enrollments to process.", work.Count);
            if (!string.IsNullOrEmpty(overrideAccountIdentifiers))
            {
                var identifiers = overrideAccountIdentifiers.Split(',');
                work = work.Where(o => o.AccountNumber.IsIn(identifiers) || o.BorrowerSsn.IsIn(identifiers)).ToList();
                if (DataAccessHelper.TestMode && work.Count < identifiers.Length)
                {
                    foreach (var ident in identifiers)
                        DA.Test_MarkBorrowerUnprocessed(ident);
                    work = DA.GetUnprocessedEspEnrollments().Where(o => o.AccountNumber.IsIn(identifiers) || o.BorrowerSsn.IsIn(identifiers)).ToList();
                }
                LogMessage("Limiting work to account identifiers {0}; {1} ESP Enrollment to process.", overrideAccountIdentifiers, work.Count);
            }
            if (!string.IsNullOrEmpty(overrideQueues))
            {
                var subqueues = overrideQueues.Split(',');
                work = work.Where(o => o.SubQueue.IsIn(subqueues)).ToList();
                LogMessage("Limiting work to sub queues {0}.  {1} ESP Enrollments to process.", overrideQueues, work.Count);
            }
            return work;
        }

        /// <summary>
        /// Calculates the number of threads to use depending on the ESP task volume and passed in args.
        /// </summary>
        public void SetThreadCount(List<EspEnrollment> work)
        {
            if (numberOfThreads <= 0)
                numberOfThreads = ((work.Count() / 10) > 20) ? 20 : ((work.Count() / 10) < 1) ? 1 : (work.Count() / 10);
            else if (numberOfThreads > 20) // In case user-passed arg explicitly set thread count greater than twenty
                numberOfThreads = 20;
            Console.WriteLine($"Thread count set to: {numberOfThreads}");
        }

        private bool CheckQueueAccess(string username)
        {
            List<QueueAndSubQueue> queues = DA.GetAccessQueues();
            foreach (var q in queues)
            {
                RI.FastPath("tx3z/ITX6X" + q.Queue + ";" + q.SubQueue + ";;");
                if (RI.MessageCode == "80014")
                {
                    PLR.AddNotification(string.Format("User id {0} does not have access to {1};{2}.", username, q.Queue, q.SubQueue), NotificationType.EndOfJob, NotificationSeverityType.Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gathers ESP info and then works task.
        /// </summary>
        public bool? ProcessTask(EspEnrollment esp)
        {
            if (!hasQueueAccess) // We won't continue processing with this batch id since it doesn't have access to queues
                return null;

            bool processingResult = false;
            Console.WriteLine($"Gathering DB data for task control number {esp.TaskControlNumber}, account {esp.AccountNumber}.");
            var ts26s = DA.GetUnprocessedLoanInformation(esp.BorrowerSsn);
            var dfs = DA.GetUnprocessedDefermentForbearances(esp.BorrowerSsn);
            var ts01s = DA.GetUnprocessedTs01Enrollment(esp.BorrowerSsn);
            var ts2hs = DA.GetUnprocessedTs2hPendingDisbursements(esp.BorrowerSsn);
            var pplus = DA.GetUnprocessedParentPlusLoanDetails(esp.BorrowerSsn);
            List<Ts26LoanInformation> unfilteredTs26 = new List<Ts26LoanInformation>();
            unfilteredTs26.AddRange(ts26s);
            List<Tuple<List<Ts26LoanInformation>, List<TsayDefermentForbearance>, List<Ts01Enrollment>, List<ParentPlusLoanDetailsInformation>>> processedLoansInformation = new List<Tuple<List<Ts26LoanInformation>, List<TsayDefermentForbearance>, List<Ts01Enrollment>, List<ParentPlusLoanDetailsInformation>>>();
            bool borrowerProcessed = true;
            bool bypassUpdateDeterminations = false;
            bool taskProcessedByAgent = false;
            List<int> loansToExcludeFromDfChecks = new List<int>();

            try
            {
                if (esp.RequiresReview) //Handles tasks that were only partially worked when a COMException crash interrupted a previous script run
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Task not fully worked by script.  Session crashed mid process. Please review.");
                    TH.CreateSupervisorTask();
                    return SetProcessed(esp, dfs, ts01s, ts2hs, pplus, unfilteredTs26);
                }

                LoadLoanStatuses(esp, ts26s);
                GetTargetLoanSequences(esp, ref ts26s, dfs, ts01s, ts2hs, pplus, ref borrowerProcessed, ref bypassUpdateDeterminations);
                if (TH.TaskExistsInSession(esp))
                {
                    foreach (var ts26 in ts26s)
                    {
                        var matchingTs26s = new List<Ts26LoanInformation>();
                        matchingTs26s.Add(ts26);
                        var loanSeqs = matchingTs26s.Select(o => o.LoanSequence).Where(o => o.HasValue).Select(o => o.Value).ToArray();
                        List<TsayDefermentForbearance> matchingDfs;
                        List<Ts01Enrollment> matchingTs01s;
                        List<Ts2hPendingDisbursement> matchingTs2hs;
                        List<ParentPlusLoanDetailsInformation> matchingPplus;
                        SetTargetLoanSequences(dfs, ts01s, ts2hs, pplus, loanSeqs, out matchingDfs, out matchingTs01s, out matchingTs2hs, out matchingPplus);
                        processedLoansInformation.Add(Tuple.Create(matchingTs26s, matchingDfs, matchingTs01s, matchingPplus));

                        Console.WriteLine($"Task with the control number {esp.TaskControlNumber} was found.  Enrollment status is now being assessed.");
                        var result = ProcessEspEnrollment(esp, matchingTs26s, matchingDfs, matchingTs01s, matchingTs2hs, matchingPplus, loansToExcludeFromDfChecks, bypassUpdateDeterminations);
                        if (result == EspEnrollmentResult.FailureContinueProcessing)
                            borrowerProcessed = false;
                        else if (result == EspEnrollmentResult.FailureEndProcessing)
                        {
                            borrowerProcessed = false;
                            break;
                        }
                    }
                }
                else
                {
                    borrowerProcessed = true;
                    taskProcessedByAgent = true;
                }
                if (!ts26s.Any() && !taskProcessedByAgent)
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, $"No TS26 data found for target loans. Possible PIF account.");
                    borrowerProcessed = true;
                }
                if (!taskProcessedByAgent)
                    TH.CommitClosedTasks(esp, processedLoansInformation, loansToExcludeFromDfChecks);
            }

            catch (System.Runtime.InteropServices.COMException comEx)
            {
                LogMessage($"COMException encountered in Session, unable to continue processing. Task being worked: {esp} \r\n Exception text: {comEx}");
                borrowerProcessed = false;
                DA.SetTaskAsRequiresReview(esp.EspEnrollmentId);
                return null;
            }
            catch (Exception ex)
            {
                LogMessage("Error processing ESP record {0}.  \r\n{1}", esp, ex);
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Unknown Error, see PLD#" + PLR.ProcessLogId);
                TH.CreateSupervisorTask();
            }
            finally
            {
                if (borrowerProcessed)
                {
                    processingResult = SetProcessed(esp, dfs, ts01s, ts2hs, pplus, unfilteredTs26);
                }
            }

            return processingResult;
        }

        /// <summary>
        /// Sets the relevant records to processed in the various ESP processing tables.
        /// </summary>
        private bool SetProcessed(EspEnrollment esp, List<TsayDefermentForbearance> dfs, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus, List<Ts26LoanInformation> unfilteredTs26)
        {
            bool result = true;
            foreach (var ts01 in ts01s)
                result &= DA.SetTs01EnrollmentAsProcessed(ts01.Ts01EnrollmentId);
            foreach (var df in dfs)
                result &= DA.SetTsayDefermentForbearanceAsProcessed(df.TsayDefermentForbearanceId);
            foreach (var ts26 in unfilteredTs26)
                result &= DA.SetTs26LoanInformationAsProcessed(ts26.Ts26LoanInformationId);
            foreach (var ts2h in ts2hs)
                result &= DA.SetTs2hPendingDisbursementAsProcessed(ts2h.Ts2hPendingDisbursementId);
            foreach (var plus in pplus)
                result &= DA.SetParentPlusLoanDetailInformationAsProcessed(plus.ParentPlusLoanDetailsId);
            return result && DA.SetEspEnrollmentAsProcessed(esp.EspEnrollmentId);
        }

        private string GetListOfLoanSequences(List<Ts26LoanInformation> loans)
        {
            string loanList = "";
            foreach (var loan in loans.OrderBy(p => p.LoanSequence).ToList())
            {
                loanList += $"{loan.LoanSequence}, ";
            }

            if (loans?.Count() > 0)
                return loanList.Remove(loanList.Length - 2, 2);
            
            return loanList;
        }

        /// <summary>
        /// Identifies which loan sequences need to be processed.
        /// Sets the ts26s list to only be populated with the loans to be processed.
        /// Handles accounts with DLPLUS and PLUS loans.
        /// </summary>
        private void GetTargetLoanSequences(EspEnrollment esp, ref List<Ts26LoanInformation> ts26s, List<TsayDefermentForbearance> dfs, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus, ref bool borrowerProcessed, ref bool bypassUpdateDeterminations)
        {
            bool hasPlusLoan = (pplus != null && pplus.Any()) ? true : false;
            bool updatingPplus = (esp.BorrowerSsn != esp.StudentSsn && hasPlusLoan) ? true : false;
            DateTime? separationDateIfNull = (esp.Esp_SeparationDate == null) ? esp.EnrollmentBeginDate : esp.Esp_SeparationDate;

            if (updatingPplus)
            {
                List<int?> targetLoanSequences = pplus.Where(p => p.StudentSsn == esp.StudentSsn).Select(q => (int?)q.LoanSequence).ToList();
                ts26s = ts26s.Where(p => targetLoanSequences.Contains(p.LoanSequence)).ToList();
            }
            else if (hasPlusLoan)
            {
                if (esp.BorrowerSsn == esp.StudentSsn)
                {
                    var dfsOnPlusLoans = dfs.Where(o => o.LoanSequence.IsIn(pplus.Select(p => p.LoanSequence).ToArray())).ToList();
                    if (!dfsOnPlusLoans.Any()) // No def/forb for plus loan(s)
                    {
                        if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.OnLeave, StatusCode.LessThanHalfTime)) // If this status, bypass processing of DLPLUS and PLUS loans
                        {
                            ts26s = ts26s.Where(p => !p.LoanSequence.IsIn(pplus.Select(o => (int?)o.LoanSequence).ToArray())).ToList();
                        }
                    }
                    else // Has def/forb for plus loan(s)
                    {
                        if (esp.Esp_Status.IsIn(StatusCode.HalfTime, StatusCode.FullTime, StatusCode.ThreeQuartersTime) && separationDateIfNull <= dfs.Select(p => p.EndDate).Max())
                        {
                            var loansWithFutureDfs = dfs.Where(df => df.LoanSequence.IsIn(pplus.Select(p => p.LoanSequence).ToArray()) &&
                                 df.EndDate >= (separationDateIfNull)).Select(df => (int?)df.LoanSequence).ToArray();
                            if (loansWithFutureDfs.Any())
                            {
                                var targetLoanSeqs = ts26s.Where(p => !p.LoanSequence.IsIn(loansWithFutureDfs)).Select(o => o.LoanSequence).Where(o => o.HasValue).Select(o => o.Value).ToArray();
                                ts26s = ts26s.Where(p => !p.LoanSequence.IsIn(loansWithFutureDfs)).ToList();
                            }
                        }
                        else if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime))
                        {
                            var loansWithD45OrD46 = dfs.Where(p => p.Type == "D45" || p.Type == "D46").Select(o => (int?)o.LoanSequence).ToArray();
                            ts26s = ts26s.Where(p => !p.LoanSequence.IsIn(loansWithD45OrD46)).ToList();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the lists of Deferment/Forbearance info (dfs), enrollment info (ts01s), future disbursements (ts2hs), and parent plus loan info (pplus) 
        /// to a filtered version of themselves, wherein only loan sequences needing to be processed are not filtered out.
        /// </summary>
        private static void SetTargetLoanSequences(List<TsayDefermentForbearance> dfs, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus, int[] loanSeqs, out List<TsayDefermentForbearance> matchingDfs, out List<Ts01Enrollment> matchingTs01s, out List<Ts2hPendingDisbursement> matchingTs2hs, out List<ParentPlusLoanDetailsInformation> matchingPplus)
        {
            matchingDfs = dfs.Where(o => o.LoanSequence.IsIn(loanSeqs)).ToList();
            matchingTs01s = ts01s.Where(o => o.LoanSequence.IsIn(loanSeqs)).ToList();
            matchingTs2hs = ts2hs.Where(o => o.LoanSequence.IsIn(loanSeqs)).ToList();
            matchingPplus = pplus.Where(o => o.LoanSequence.IsIn(loanSeqs)).ToList();
        }

        private void LoadLoanStatuses(EspEnrollment esp, List<Ts26LoanInformation> loans)
        {
            RI.FastPath("tx3z/its26" + esp.BorrowerSsn);
            if (RI.ScreenCode == "TSX29")
                foreach (var ts26 in loans)
                    ts26.LoanStatus = RI.GetText(3, 10, 31).Trim();
            else
                PageHelper.Iterate(RI, (row, settings) =>
                {
                    int? sel = RI.GetText(row, 2, 2).ToIntNullable();
                    if (sel.HasValue)
                    {
                        var seq = RI.GetText(row, 14, 4).ToIntNullable();
                        RI.PutText(21, 12, sel.Value.ToString(), Key.Enter, true);
                        var matchingLoans = loans.Where(o => o.LoanSequence == seq);
                        foreach (var matchingLoan in matchingLoans)
                            matchingLoan.LoanStatus = RI.GetText(3, 10, 31).Trim();
                        RI.Hit(Key.F12);
                    }
                    else
                        settings.ContinueIterating = false;
                });
        }

        /// <summary>
        /// Process the given enrollment.
        /// </summary>
        private EspEnrollmentResult ProcessEspEnrollment(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<TsayDefermentForbearance> dfs, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus, List<int> loansToExcludeFromDfChecks, bool bypassUpdateDeterminations)
        {
            if (ts26s.Any(ts => ts.LoanProgramType.IsIn("SUBSPC", "UNSPC", "DLSSPL", "DLUSPL", "DLSCPN", "SPCNSL")))
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Borrower has a spousal loan");
                return EspEnrollmentResult.FailureEndProcessing;
            }
            else if (esp.Esp_Status.IsIn(StatusCode.NeverEnrolled, StatusCode.Deceased))
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Reporting never enrolled/death, pls rvw");
                return EspEnrollmentResult.FailureEndProcessing;
            }
            else
            {
#if DEBUG
                Console.WriteLine("Doing initial screen-scraping for TS26 and TSAY to capture pre-processing snapshot of loan and deferment details.");
                var ts26Scrapes = Ts26ScrapedLoanInformation.ScrapeTs26(RI, esp.BorrowerSsn);
                foreach (var ts26Scrape in ts26Scrapes.Where(o => o.LoanSequence == ts26s.SingleOrDefault().LoanSequence))
                    DA.AddTs26ScrapedLoanInformation(ts26Scrape.BorrowerSsn, ts26Scrape.LoanSequence, ts26Scrape.LoanStatus, ts26Scrape.RepaymentStartDate);

                var tsayScrapes = TsayScrapedLoanInformation.ScrapeTsay(RI, esp.BorrowerSsn);
                foreach (var tsayScrape in tsayScrapes.Where(o => o.LoanSequence == ts26s.SingleOrDefault().LoanSequence))
                {
                    DA.AddTsayScrapedLoanInformation(tsayScrape.BorrowerSsn, tsayScrape.LoanSequence, tsayScrape.LoanProgramType, tsayScrape.BeginDate, tsayScrape.EndDate, tsayScrape.CertificationDate, tsayScrape.DisbursementDate, tsayScrape.DeferSchool, tsayScrape.ApprovalStatus, tsayScrape.DfType, tsayScrape.AppliedDate, "ProcessEspEnrollment");
                    DA.SetTsayScrapedLoanInformationAsProcessed(tsayScrape.BorrowerSsn, tsayScrape.LoanSequence);
                }
#endif

                bool isPlusLoan = ts26s.Any() && ts26s.First().LoanProgramType.IsIn("DLPLUS", "PLUS") ? true : false;
                bool updatingPplus = (esp.BorrowerSsn != esp.StudentSsn && isPlusLoan) ? true : false;
                bool requiresReview = false, failureContinueProcessing = false;
                bool hasSchoolDefer = dfs.Where(p => p.Type == "D45" || p.Type == "D46").Any();
                DetermineIfRequiresReview(ref requiresReview, ref failureContinueProcessing, esp, ts26s, dfs, ts01s, pplus, loansToExcludeFromDfChecks); // Checks underlying loans and disbursement dates

                DateTime? separationDateIfNull = (esp.Esp_SeparationDate == null) ? esp.EnrollmentBeginDate : esp.Esp_SeparationDate;
                if (!updatingPplus && isPlusLoan && dfs.Any() && separationDateIfNull > dfs.First().EndDate && hasSchoolDefer)
                    bypassUpdateDeterminations = true;

                if (failureContinueProcessing)
                    return EspEnrollmentResult.FailureContinueProcessing;
                else if (requiresReview)
                    return EspEnrollmentResult.Success; // This ESP task was successfully passed off to be reviewed by supervisor or for def/forb review

                if (updatingPplus || ts26s.SingleOrDefault().LoanProgramType.IsIn("DLPLGB", "PLUSGB"))
                    UpdateDeterminations_Plus(esp, dfs, ts26s, ts01s, ts2hs, pplus);
                else
                    UpdateDeterminations(esp, dfs, ts26s, ts01s, ts2hs, pplus, bypassUpdateDeterminations);
            }
            return EspEnrollmentResult.Success;
        }

        /// <summary>
        /// Checks if any loans require a review based off of loan type and disbursement dates.
        /// Review will either be by a supervisor or a deferment/forbearance review.
        /// </summary>
        private void DetermineIfRequiresReview(ref bool requiresReview, ref bool failureContinueProcessing, EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<TsayDefermentForbearance> dfs, List<Ts01Enrollment> ts01s, List<ParentPlusLoanDetailsInformation> pplus, List<int> loansToExcludeFromDfChecks)
        {
            //Call sproc to see if loan is split; if so, don't update enrollment and don't look at bridge defer or alignment forb modules later
            if (DA.IsASplitConsolLoan(esp.BorrowerSsn, ts26s.FirstOrDefault().LoanSequence.Value))
            {
                requiresReview = true;
                loansToExcludeFromDfChecks.Add(ts26s.FirstOrDefault().LoanSequence.Value);
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, $"Consolidated split loan encountered for loan sequence: {ts26s.FirstOrDefault().LoanSequence.Value}.");
            }
            else if ((esp.Esp_Status == StatusCode.HalfTime || esp.Esp_Status == StatusCode.ThreeQuartersTime))
            {
                if (ts26s.Any(ts => ts.LoanProgramType.IsIn("CNSLDN", "SUBCNS", "SUBSPC", "UNCNS", "UNSPC", "DLCNSL", "DLSCNS", "DLUCNS", "DLUSPL", "DLPCNS") && ts.DisbursementDate < DateTime.Parse("07/01/1993")))
                {
                    requiresReview = true;
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Need to review underlying loans (ineligible for HT if underlying loan disb prior July 1987)");
                }
                else if (ts26s.Any(ts => ts.DisbursementDate < DateTime.Parse("07/01/1987")))
                {
                    var matchingTypes = dfs.Where(df => df.BeginDate <= DateTime.Now && df.EndDate >= DateTime.Now);
                    if (matchingTypes.Select(o => o.Type).Distinct().Count() > 1)
                    {
                        failureContinueProcessing = true;
                        PLR.AddNotification(string.Format("Expected only one type of DF for EspEnrollmentId {0}, but found the following: {1}", esp.EspEnrollmentId, string.Join(",", matchingTypes)), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        return;
                    }
                    else if (!matchingTypes.Any())
                    {
                        failureContinueProcessing = true;
                        PLR.AddNotification(string.Format("Expected only one type of DF for EspEnrollmentId {0}, but found none.", esp.EspEnrollmentId), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        return;
                    }
                    var matchingType = matchingTypes.First();
                    requiresReview = true;
                    DFH.DefermentForbearanceChange(esp, matchingType, ts26s, ts01s, pplus, null, esp.EnrollmentBeginDate);
                    TH.CloseTask(esp, ts26s, dfs, pplus, "Borrower has loans prior to 07/01/1987 and is not eligible for HT Dfr", false);
                }
            }
            else if (esp.Esp_Status == StatusCode.OnLeave)
            {
                requiresReview = true;
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Review brwr LOA");
                Console.WriteLine($"Sent task with EspEnrollmentId {esp.EspEnrollmentId} to supervisor. Borrower has DLPLUS / PLUS loans and an ESP status of \"5\".");
            }
        }


        /// <summary>
        /// Make updates based on dates.
        /// </summary>
        private void UpdateDeterminations(EspEnrollment esp, List<TsayDefermentForbearance> dfs, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus, bool bypassUpdateDeterminations)
        {
            Console.WriteLine($"Determining necessary updates for task number {esp.TaskControlNumber}");
            if (bypassUpdateDeterminations)
            {
                DFH.DefermentForbearanceAdd(esp, null, null, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, esp.Esp_SeparationDate);
                TH.CloseTask(esp, ts26s, dfs, pplus, "added schl defers per brwr enrllmnt", false);
                return;
            }


            if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime))
            {
                if (ts26s.Any(ts26 => ts26.TermBeg.HasValue && ts26.TermBeg.Value.AddYears(-1) <= esp.EnrollmentBeginDate && esp.EnrollmentBeginDate <= ts26.TermBeg))
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Possible Disb issue");
                    return;
                }
                else if (ts26s.Any(ts26 => ts26.TermBeg.HasValue && esp.EnrollmentBeginDate < ts26.TermBeg.Value.AddYears(-1)))
                {
                    TH.CloseTask(esp, ts26s, dfs, pplus, "Enrollment data was for period prior to loan term", false);
                    return;
                }
                else if (ts26s.Any(o => o.RehabRepurch.IsIn("N", "R", "S") && o.EffectAddDate > esp.EnrollmentBeginDate))
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Enrollment is for period before rehab or repurch date");
                    return;
                }
                else if (ts2hs.Any(o => o.DisbType == "Anticipated"))
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Pending Disb and LTHT, GRAD, or WTHD");
                    return;
                }
                else if (esp.Esp_SeparationDate > DateTime.Today || esp.Esp_SeparationDate == null)
                    esp.Esp_SeparationDate = esp.EnrollmentBeginDate;
            }
            if (!dfs.Any())
            {
                if (UpdateDeterminations_NoTsay(esp, ts26s, ts01s, dfs, ts2hs, pplus))
                    return;
            }
            else if (UpdateDeterminations_WithTsay(esp, dfs, ts26s, ts01s, ts2hs, pplus))
                return;

            if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime) && ts01s.Any())
            {
                if (ts01s.AllAndAny(o => o.SchoolCode != esp.SchoolCode && o.SeparationDate > DateTime.Now.Date) || dfs.AllAndAny(o => o.DeferSchool != esp.SchoolCode && o.EndDate > DateTime.Now.Date))
                {
                    TH.CloseTask(esp, ts26s, dfs, pplus, "Diff sch exiting, not covered by curr enroll", false);
                    return;
                }
                else if (ts01s.AllAndAny(o => o.SchoolCode != esp.SchoolCode && o.SeparationDate < DateTime.Now.Date) && ts26s.AllAndAny(o => o.GraceEndDate < DateTime.Now.Date))
                {
                    if (ts01s.Any(o => esp.Esp_SeparationDate <= o.SeparationDate))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Diff schl exiting", false);
                        return;
                    }
                    else
                    {
                        TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Diff School exiting, review NSLDS");
                        return;
                    }
                }
                else if (ts01s.AllAndAny(o => o.SchoolCode != esp.SchoolCode && o.SeparationDate < DateTime.Now.Date && o.SeparationDate < esp.EnrollmentBeginDate) && ts26s.AllAndAny(o => o.GraceEndDate > DateTime.Now.Date))
                {
                    if (ts01s.Any(o => esp.Esp_SeparationDate <= o.SeparationDate))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Diff schl exiting", false);
                        return;
                    }
                    else
                    {
                        TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Diff school exiting, review NSLDS");
                        return;
                    }
                }
                else if (dfs.Any(o => o.BeginDate > esp.EnrollmentBeginDate))
                {
                    if (dfs.Any(o => o.CertificationDate > esp.Esp_CertificationDate))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Old Enroll Info, Update not applicable", false);
                        return;
                    }
                    else if (dfs.Any(o => o.CertificationDate <= esp.Esp_CertificationDate && o.DeferSchool == esp.SchoolCode))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "same school, newer cert has GR, WI, LT before defer begin", false);
                        return;
                    }
                    else if (dfs.Any(o => o.CertificationDate <= esp.Esp_CertificationDate && o.DeferSchool != esp.SchoolCode))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "diff school, newer cert has GR, WI, LT before defer begin", false);
                        return;
                    }

                }
            }

            if (!dfs.Any())
            {
                if (ts01s.Any(o => o.DateCertified > esp.Esp_CertificationDate))
                {
                    TH.CloseTask(esp, ts26s, null, pplus, "System has more recent information than incoming enrollment data", false);
                    return;
                }
                else if (ts01s.Any(o => o.DateCertified == esp.Esp_CertificationDate))
                {
                    if (ts01s.Any(o => o.SeparationDate == esp.Esp_SeparationDate))
                    {
                        TH.CloseTask(esp, ts26s, null, pplus, "System already has sep date", false);
                        return;
                    }
                    else if (ts01s.Any(o => o.SeparationDate > esp.Esp_SeparationDate))
                    {
                        TH.CloseTask(esp, ts26s, null, pplus, "Borrower covered by higher sep date", false);
                        return;
                    }
                }
                if (ts26s.Any(o => o.RepaymentStartDate <= DateTime.Now.Date))
                {
                    if (ts26s.Any(o => o.RepaymentStartDate > esp.EnrollmentBeginDate))
                    {
                        if (DFH.UpdateEnrollment(esp, ts26s, ts01s, dfs, pplus))
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming enrollment", false);
                    }
                    else if (esp.Esp_Status.IsIn(StatusCode.Enrolled, StatusCode.HalfTime, StatusCode.FullTime, StatusCode.ThreeQuartersTime))
                    {
                        if (DFH.DefermentForbearanceAdd(esp, null, null, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, esp.Esp_SeparationDate))
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date", false);
                    }
                    else if (esp.Esp_Status == StatusCode.OnLeave)
                    {
                        HandleOnLeaveStatus(esp, dfs, ts26s, pplus);
                        return;
                    }
                    else
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Incoming enrollment not applicable", false);
                        return;
                    }
                }
                else
                {
                    if (esp.Esp_Status.IsIn(StatusCode.Enrolled, StatusCode.HalfTime, StatusCode.FullTime, StatusCode.ThreeQuartersTime))
                    {
                        if (DFH.UpdateEnrollment(esp, ts26s, ts01s, dfs, pplus))
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date", false);
                    }
                    else if (esp.Esp_Status == StatusCode.OnLeave)
                    {
                        HandleOnLeaveStatus(esp, dfs, ts26s, pplus);
                    }
                    else if (ts26s.Any(o => o.RepaymentStartDate > esp.EnrollmentBeginDate))
                    {
                        if (ts01s.AllAndAny(o => o.SchoolCode == esp.SchoolCode))
                        {
                            if (DFH.UpdateEnrollment(esp, ts26s, ts01s, dfs, pplus))
                                TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date", false);
                        }
                        else
                        {
                            if (ts01s.Any(o => o.SeparationDate >= esp.Esp_SeparationDate))
                            {
                                TH.CloseTask(esp, ts26s, dfs, pplus, "Incoming enroll not applicable");
                                return;
                            }
                            else if (ts01s.Any(o => o.SeparationDate < esp.Esp_SeparationDate))
                            {
                                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Review for possible sep date change");
                                return;
                            }
                        }
                    }
                    else
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Incoming enrollment not applicable", false);
                        return;
                    }
                }
            }
            else
            {
                var df = dfs.First();
                string dfType = DetermineDeferType(esp);
                if (dfs.Count(o => o.BeginDate == df.BeginDate && o.EndDate == df.EndDate && o.CertificationDate == df.CertificationDate && o.Type == df.Type) != dfs.Count())
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "differing tsay data across loans");
                    return;
                }
                if (df.CertificationDate > esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate)
                    TH.CloseTask(esp, ts26s, dfs, pplus, "ESP Cert is before DF Cert and ESP Begin is before DF Begin", false);
                else if (df.CertificationDate > esp.Esp_CertificationDate && df.BeginDate <= esp.EnrollmentBeginDate)
                    TH.CloseTask(esp, ts26s, dfs, pplus, "System has more recent information than incoming enroll data", false);
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate == esp.EnrollmentBeginDate && df.EndDate == esp.Esp_SeparationDate)
                    TH.CloseTask(esp, ts26s, dfs, pplus, "System already matches incoming enroll data", false);
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate == esp.EnrollmentBeginDate && df.EndDate != esp.Esp_SeparationDate)
                {
                    if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, null, esp.Esp_SeparationDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming sep date");
                }
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate)
                {
                    if (ts26s.Any(ts => ts.RepaymentStartDate <= esp.EnrollmentBeginDate))
                    {
                        bool noErrors = true;
                        foreach (var ts26 in ts26s)
                        {
                            if (dfs.AllAndAny(o => o.DeferSchool == esp.SchoolCode))
                            {
                                noErrors &= DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, esp.Esp_SeparationDate);
                            }
                            else if (dfs.AllAndAny(o => o.DeferSchool != esp.SchoolCode))
                            {
                                noErrors &= DFH.DefermentForbearanceAdd(esp, dfType, df, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, esp.Esp_SeparationDate);
                            }
                        }

                        if (noErrors)
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming sep date");
                    }
                    else
                    {
                        if (esp.Esp_Status.IsIn(StatusCode.Enrolled, StatusCode.HalfTime, StatusCode.FullTime, StatusCode.ThreeQuartersTime))
                        {
                            if (DFH.UpdateEnrollment(esp, ts26s, ts01s, dfs, pplus))
                                TH.CloseTask(esp, ts26s, dfs, pplus, "Updt to matching incoming sep dt", false);
                        }
                        else if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.OnLeave, StatusCode.LessThanHalfTime))
                        {
                            TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Cert Dates equal, ESP begins before DF, and ESP begins before Repay Start.");
                        }
                    }
                }
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate < esp.EnrollmentBeginDate)
                {
                    if (DFH.DefermentForbearanceAdd(esp, dfType, df, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, df.EndDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming sep date");
                }
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate <= esp.EnrollmentBeginDate && df.EndDate == esp.Esp_SeparationDate)
                    TH.CloseTask(esp, ts26s, dfs, pplus, "System already matches incoming enrollment data", false);
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate <= esp.EnrollmentBeginDate && df.EndDate != esp.Esp_SeparationDate)
                {
                    if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, null, esp.Esp_SeparationDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date");
                }
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate && df.EndDate == esp.Esp_SeparationDate)
                {
                    if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, df.EndDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date");
                }
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate && df.EndDate != esp.Esp_SeparationDate)
                {
                    if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, esp.Esp_SeparationDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date");
                }
            }
        }

        /// <summary>
        /// Determines whether to close a task or send it to a sup when it is an ESP task with a status of "Onleave"
        /// </summary>
        private void HandleOnLeaveStatus(EspEnrollment esp, List<TsayDefermentForbearance> dfs, List<Ts26LoanInformation> ts26s, List<ParentPlusLoanDetailsInformation> pplus)
        {
            if (ts26s.Any(o => o.RepaymentStartDate < esp.EnrollmentBeginDate))
                TH.CloseTask(esp, ts26s, dfs, pplus, "Incoming enroll not applicable", false);
            else
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "LOA please review");
        }

        /// <summary>
        /// Returns the deferment type to be used for a loan, given the borrower's enrollment status.
        /// </summary>
        private string DetermineDeferType(EspEnrollment esp)
        {
            string dfType = null;
            if (esp.Esp_Status == StatusCode.FullTime)
                dfType = "D15";
            else if (esp.Esp_Status.IsIn(StatusCode.HalfTime, StatusCode.ThreeQuartersTime))
                dfType = "D18";
            return dfType;
        }

        /// <summary>
        /// Determines what updates, if any, need to be made based on dates of task, enrollment, deferments, etc.
        /// Used only for Parent Plus scenarios.  If a deferment is added to an account, adds a D45.
        /// </summary>
        private void UpdateDeterminations_Plus(EspEnrollment esp, List<TsayDefermentForbearance> dfs, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus)
        {
            Console.WriteLine($"Determining necessary updates based on Parent Plus scenario for task number {esp.TaskControlNumber}");
            string dfType = "D45";
            DateTime? maxDisb = ts2hs.Where(ln => ln.LoanSequence == ts26s.First().LoanSequence.Value).Max(l => l.DisbursementDate);
            DateTime? startDate = (esp.EnrollmentBeginDate < maxDisb) ? maxDisb : esp.EnrollmentBeginDate;
            if (ts26s.SingleOrDefault().LoanProgramType.IsIn("DLPLGB", "PLUSGB"))
                dfType = null; // The DefermentForbearanceAdd method will update this to either D15/D18 depending on enrollment status

            // If PLUS/DLPLUS loans don't have a defer requested, send to sup
            if (pplus.Any(o => ts26s.Where(p => p.LoanProgramType.IsIn("PLUS", "DLPLUS")).Select(p => p.LoanSequence).ToArray().Contains(o.LoanSequence) && !o.DefermentRequested))
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Review PPlus for defer elig.");
                return;
            }

            if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime))
            {
                if (ts26s.Any(ts26 => ts26.TermBeg.HasValue && ts26.TermBeg.Value.AddYears(-1) <= esp.EnrollmentBeginDate && esp.EnrollmentBeginDate <= ts26.TermBeg))
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Possible Disb issue");
                    return;
                }
                else if (ts26s.Any(ts26 => ts26.TermBeg.HasValue && esp.EnrollmentBeginDate < ts26.TermBeg.Value.AddYears(-1)))
                {
                    TH.CloseTask(esp, ts26s, dfs, pplus, "enrollment data was for period prior to loan term", false);
                    return;
                }
                else if (ts26s.Any(o => o.RehabRepurch.IsIn("N", "R", "S") && o.EffectAddDate > esp.EnrollmentBeginDate))
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Enrollment is for period before rehab or repurch date");
                    return;
                }
                else if (ts2hs.Any(o => o.DisbType == "Anticipated"))
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Pending Disb and LTHT, GRAD, or WTHD");
                    return;
                }
                if (esp.Esp_SeparationDate > DateTime.Today || esp.Esp_SeparationDate == null)
                    esp.Esp_SeparationDate = esp.EnrollmentBeginDate;
            }
            if (!dfs.Any())
            {
                if (UpdateDeterminations_NoTsay_Plus(esp, ts26s, ts01s, dfs, ts2hs, pplus))
                    return;
            }
            else if (UpdateDeterminations_WithTsay_Plus(esp, dfs, ts26s, ts01s, ts2hs, pplus))
                return;

            if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime))
            {
                if (dfs.AllAndAny(o => o.DeferSchool != esp.SchoolCode && o.EndDate > esp.Esp_SeparationDate))
                {
                    TH.CloseTask(esp, ts26s, dfs, pplus, "Diff sch exiting", false);
                    return;
                }
                else if (dfs.AllAndAny(o => o.DeferSchool != esp.SchoolCode && o.EndDate < esp.Esp_SeparationDate))
                {
                    TH.CloseTask(esp, ts26s, dfs, pplus, "Diff School exiting, review NSLDS");
                    return;
                }
                else if (dfs.AllAndAny(o => esp.EnrollmentBeginDate < o.BeginDate))
                {
                    if (dfs.AllAndAny(o => esp.Esp_CertificationDate < o.CertificationDate))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Old enroll info, update not applicable");
                        return;
                    }
                    else if (dfs.AllAndAny(o => esp.SchoolCode == o.DeferSchool))
                    {
                        TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Same school, newer cert has GR, WI, LT before defer begin");
                        return;
                    }
                }
            }

            if (!dfs.Any())
            {
                if (ts26s.AllAndAny(o => maxDisb <= DateTime.Now.Date))
                {
                    if (esp.Esp_Status.IsIn(StatusCode.Enrolled, StatusCode.HalfTime, StatusCode.FullTime, StatusCode.ThreeQuartersTime))
                    {
                        if (DFH.DefermentForbearanceAdd(esp, dfType, null, ts26s, ts01s, pplus, startDate, esp.Esp_SeparationDate))
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming enrollment", false);
                    }
                    else if (esp.Esp_Status == StatusCode.OnLeave)
                    {
                        TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "LOA please review");
                        return;
                    }
                    else if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime))
                    {
                        TH.CloseTask(esp, ts26s, null, pplus, "Incoming enrollment not applicable", false);
                        return;
                    }
                }
                else
                {
                    if (esp.Esp_Status.IsIn(StatusCode.Enrolled, StatusCode.HalfTime, StatusCode.FullTime, StatusCode.ThreeQuartersTime))
                    {
                        if (DFH.DefermentForbearanceAdd(esp, dfType, null, ts26s, ts01s, pplus, startDate, esp.Esp_SeparationDate))
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date", false);
                    }
                    else if (esp.Esp_Status == StatusCode.OnLeave)
                    {
                        TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "LOA please review");
                    }
                    else
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Incoming enrollment not applicable", false);
                    }
                }
            }
            else
            {
                var df = dfs.First();
                if (dfs.Count(o => o.BeginDate == df.BeginDate && o.EndDate == df.EndDate && o.CertificationDate == df.CertificationDate && o.Type == df.Type) != dfs.Count())
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "differing tsay data across loans");
                    return;
                }
                if (df.CertificationDate > esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate)
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "ESP Cert is before DF Cert and ESP Begin is before DF Begin");
                else if (df.CertificationDate > esp.Esp_CertificationDate && df.BeginDate <= esp.EnrollmentBeginDate)
                    TH.CloseTask(esp, ts26s, dfs, pplus, "System has more recent information than incoming enroll data", false);
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate == esp.EnrollmentBeginDate && df.EndDate == esp.Esp_SeparationDate)
                    TH.CloseTask(esp, ts26s, dfs, pplus, "System already matches incoming enroll data", false);
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate == esp.EnrollmentBeginDate && df.EndDate != esp.Esp_SeparationDate)
                {
                    if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, null, esp.Esp_SeparationDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming sep date");
                }
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate)
                {
                    if (ts26s.Any(ts => maxDisb <= esp.EnrollmentBeginDate))
                    {
                        if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, esp.Esp_SeparationDate))
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming enrollment");
                    }
                    else if (esp.Esp_SeparationDate != df.EndDate)
                    {
                        if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, startDate, esp.Esp_SeparationDate))
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming enrollment");
                    }
                    else if (esp.Esp_SeparationDate == df.EndDate)
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "No update - equal sep and cert dts", false);
                    }
                }
                else if (df.CertificationDate == esp.Esp_CertificationDate && df.BeginDate < esp.EnrollmentBeginDate)
                {
                    if (DFH.DefermentForbearanceAdd(esp, dfType, df, ts26s, ts01s, pplus, esp.EnrollmentBeginDate, MaxDate(df.EndDate, esp.Esp_SeparationDate)))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming sep date");
                }
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate <= esp.EnrollmentBeginDate && df.EndDate == esp.Esp_SeparationDate)
                    TH.CloseTask(esp, ts26s, dfs, pplus, "System already matches incoming enrollment data", false);
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate <= esp.EnrollmentBeginDate && df.EndDate != esp.Esp_SeparationDate)
                {
                    if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, null, esp.Esp_SeparationDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date");
                }
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate && df.EndDate == esp.Esp_SeparationDate)
                {
                    if (esp.EnrollmentBeginDate > maxDisb)
                        TH.CloseTask(esp, ts26s, dfs, pplus, "No updt, incoming SEP = TSAY end");
                    else if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, startDate, df.EndDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming begin date");
                }
                else if (df.CertificationDate < esp.Esp_CertificationDate && df.BeginDate > esp.EnrollmentBeginDate && df.EndDate != esp.Esp_SeparationDate)
                {
                    if (DFH.DefermentForbearanceChange(esp, df, ts26s, ts01s, pplus, startDate, esp.Esp_SeparationDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Updated system to match incoming separation date");
                }
            }
        }

        /// <summary>
        /// Returns the max of the two dates if both have values.
        /// Otherwise, returns the one with a value (or null if for some reason the ESP task has 
        /// no value for SeparationDate, which shouldn't happen per BA).
        /// </summary>
        private DateTime? MaxDate(DateTime? endDate, DateTime? esp_SeparationDate)
        {
            if (endDate.HasValue && esp_SeparationDate.HasValue)
                return endDate > esp_SeparationDate ? endDate : esp_SeparationDate;
            else if (endDate.HasValue)
                return endDate;
            else
                return esp_SeparationDate;
        }

        /// <summary>
        /// Determines if ESP withdraw occurred or if the student is attending less than half time.
        /// Returns true if so; false otherwise.
        /// </summary>
        private bool UpdateDeterminations_NoTsay(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<TsayDefermentForbearance> dfs, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus)
        {
            if (esp.Esp_Status == StatusCode.Graduated)
            {
                if (ts01s.Any(o => o.SchoolCode != esp.SchoolCode))
                {
                    if (ts01s.AllAndAny(o => esp.Esp_SeparationDate <= o.SeparationDate))
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Diff sch grad covered by current enroll", false);
                    else
                        TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Diff sch grad not covered by curr enroll");
                    return true;
                }
                else if (ts01s.Any(o => o.SchoolCode == esp.SchoolCode))
                {
                    if (ts01s.Any(o => o.SeparationReason == StatusCode.LessThanHalfTime))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Graduated after dropping LTHT", false);
                        return true;
                    }
                    else if (ts01s.Any(o => o.SeparationReason == StatusCode.Withdrew))
                    {
                        if (ts01s.Any(o => esp.EnrollmentBeginDate.HasValue && o.SeparationDate.HasValue && (o.SeparationDate.Value.DateDiffInDays(esp.EnrollmentBeginDate.Value) < 30)))
                        {
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Graduated after withdrawing", false);
                            return true;
                        }
                        else if (ts01s.Any(o => esp.EnrollmentBeginDate.HasValue && o.SeparationDate.HasValue && (o.SeparationDate.Value.DateDiffInDays(esp.EnrollmentBeginDate.Value) >= 30)))
                        {
                            TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Rvw, grad after w/d");
                            return true;
                        }
                    }
                    else if (ts01s.Any(o => esp.EnrollmentBeginDate?.Year == o.SeparationDate?.Year && esp.EnrollmentBeginDate?.Month == o.SeparationDate?.Month)
                        || ts01s.Any(o => esp.Esp_SeparationDate?.Year == o.SeparationDate?.Year && esp.Esp_SeparationDate?.Month == o.SeparationDate?.Month))
                    {
                        if (ts26s.AllAndAny(o => o.RepaymentStartDate < DateTime.Now.Date))
                        {
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Graduated, same month and year on record", false);
                            return true;
                        }
                    }
                }
            }

            else if (ts01s.AllAndAny(o => o.SeparationDate < esp.Esp_SeparationDate)
                 && ts01s.AllAndAny(o => o.DateCertified < esp.Esp_CertificationDate))
            {
                if (ts01s.Any(o => o.SchoolCode == esp.SchoolCode))
                {
                    if (esp.Esp_Status == StatusCode.Withdrew)
                    {
                        if (ts01s.Any(o => o.SeparationReason == StatusCode.LessThanHalfTime))
                        {
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Withdrew after LTHT", false);
                            return true;
                        }
                        else if (ts01s.Any(o => o.SeparationReason.IsIn(StatusCode.Graduated, StatusCode.Withdrew)))
                        {
                            if (ts01s.Any(o => o.SeparationDate.HasValue && esp.EnrollmentBeginDate.HasValue && o.SeparationDate.Value.DateDiffInDays(esp.EnrollmentBeginDate.Value) < 30))
                            {
                                TH.CloseTask(esp, ts26s, dfs, pplus, "Grad or Withdrew after Withdrew", false);
                                return true;
                            }
                            else if (ts01s.Any(o => o.SeparationDate.HasValue && esp.EnrollmentBeginDate.HasValue && o.SeparationDate.Value.DateDiffInDays(esp.EnrollmentBeginDate.Value) >= 30))
                            {
                                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Rvw, w/d after grad or w/d");
                                return true;
                            }
                        }
                    }
                }

                if (esp.Esp_Status.IsIn(StatusCode.LessThanHalfTime) && ts01s.Any(o => o.SeparationReason.IsIn(StatusCode.LessThanHalfTime)))
                {
                    TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "LTHT after LTHT");
                    return true;
                }
            }
            if (esp.Esp_Status == StatusCode.LessThanHalfTime)
            {
                if (ts01s.Any(o => o.SeparationReason.IsIn(StatusCode.Graduated, StatusCode.Withdrew)))
                {
                    if (ts01s.AllAndAny(o => o.SeparationDate < esp.Esp_SeparationDate)
                     && ts01s.AllAndAny(o => o.DateCertified < esp.Esp_CertificationDate))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "LTHT after Grad or Withdrew", false);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if ESP withdraw occurred or if the student is attending less than half time.
        /// Returns true if so; false otherwise.
        /// Used for Parent Plus scenarios.
        /// </summary>
        private bool UpdateDeterminations_NoTsay_Plus(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<TsayDefermentForbearance> dfs, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus)
        {
            if (esp.Esp_Status == StatusCode.Graduated || esp.Esp_Status == StatusCode.Withdrew)
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "W/d but no dfr exists");
                return true;
            }
            else if (esp.Esp_Status == StatusCode.LessThanHalfTime)
            {
                TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "No update, student attending ltht");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if school graduation already covered by the current enrollment on the account.
        /// Returns true if so; false otherwise.
        /// </summary>
        private bool UpdateDeterminations_WithTsay(EspEnrollment esp, List<TsayDefermentForbearance> dfs, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus)
        {
            if (esp.Esp_Status == StatusCode.Graduated)
            {
                if (!dfs.Any(o => o.DeferSchool == esp.SchoolCode))
                {
                    if (dfs.AllAndAny(o => esp.Esp_SeparationDate <= o.EndDate))
                    {
                        TH.SupervisorCloseTask(esp, ts26s, dfs, pplus, "Diff sch grad not covered by curr enroll");
                        return true;
                    }
                    else
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Diff sch grad covered by current enroll", false);
                        return true;
                    }
                }
                else
                {
                    if (dfs.Any(o => esp.EnrollmentBeginDate?.Year == o.EndDate?.Year && esp.EnrollmentBeginDate?.Month == o.EndDate?.Month)
                        || dfs.Any(o => esp.Esp_SeparationDate?.Year == o.EndDate?.Year && esp.Esp_SeparationDate?.Month == o.EndDate?.Month))
                    {
                        if (ts26s.AllAndAny(o => o.RepaymentStartDate < DateTime.Now.Date))
                        {
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Graduated, same month and year on record", false);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if school graduation already covered by the current enrollment on the account.
        /// Returns true if so; false otherwise.
        /// Used for Parent Plus loan scenarios.  Looks further back in enrollment history than 
        /// the regular UpdateDeterminations_WithTsay() method.
        /// </summary>
        private bool UpdateDeterminations_WithTsay_Plus(EspEnrollment esp, List<TsayDefermentForbearance> dfs, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<Ts2hPendingDisbursement> ts2hs, List<ParentPlusLoanDetailsInformation> pplus)
        {
            if (esp.Esp_Status == StatusCode.Graduated)
            {
                if (!dfs.Any(o => o.DeferSchool == esp.SchoolCode))
                {
                    if (dfs.AllAndAny(o => esp.Esp_SeparationDate <= o.EndDate))
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Diff sch grad not covered by current enroll", false);
                        return true;
                    }
                    else
                    {
                        TH.CloseTask(esp, ts26s, dfs, pplus, "Diff sch grad covered by curr enroll", false);
                        return true;
                    }
                }
                else
                {
                    if (dfs.Any(o => esp.EnrollmentBeginDate?.Year == o.EndDate?.Year && esp.EnrollmentBeginDate?.Month == o.EndDate?.Month)
                        || dfs.Any(o => esp.Esp_SeparationDate?.Year == o.EndDate?.Year && esp.Esp_SeparationDate?.Month == o.EndDate?.Month))
                    {
                        if (dfs.Any(o => o.EndDate < DateTime.Now.AddMonths(-6)))
                        {
                            TH.CloseTask(esp, ts26s, dfs, pplus, "Graduated, same month and year on record", false);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void EndLoginHelper(int? threadId)
        {
            Thread.Sleep(5); //Sleeping in order to avoid issue where Session opens, there is no work to be done, and then tries to close immediately (without a sleep, this creates hanging processes)
            Console.WriteLine($"Closing the Session for thread: {threadId}.");
            this.RI.CloseSession();
            Console.WriteLine($"Closing DB connections for thread: {threadId}.");
            if (BLH != null)
                BatchProcessingHelper.CloseConnection(BLH);
        }

        private void LogMessage(string message, params object[] args)
        {
            message = string.Format(message, args);
            PLR.AddNotification(message, NotificationType.EndOfJob, NotificationSeverityType.Informational);
        }
    }
}
