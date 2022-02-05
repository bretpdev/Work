using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace BATCHESP
{
    public class TaskHelper
    {
        private ProcessLogRun PLR;
        private DataAccessHelper.Region region;
        private ReflectionInterface RI;
        private DataAccess DA;
        private DefermentForbearanceHelper DFH;
        private string username;
        private bool skipTaskClose;
        private bool skipTaskAssign;
        private List<FutureCheck> FutureCheckQueue = new List<FutureCheck>();
        private List<EspEnrollment> CloseTaskQueue = new List<EspEnrollment>();
        private List<EspEnrollment> SupervisorTaskQueue = new List<EspEnrollment>();
        private readonly string[] ConsolidationLoanTypes = { "CNSLDN", "SPCNSL", "SUBCNS", "SUBSPC", "UNCNS", "UNSPC", "DLPCNS", "DLUSPL", "DLUCNS", "DLSSPL", "DLSCNS" };

        public TaskHelper(ProcessLogRun plr, DataAccess da, DataAccessHelper.Region region, ReflectionInterface ri, string username, bool skipTaskClose, bool skipTaskAssign)
        {
            this.PLR = plr;
            this.region = region;
            this.RI = ri;
            this.DA = da;
            this.username = username;
            this.skipTaskClose = skipTaskClose;
            this.skipTaskAssign = skipTaskAssign;
        }

        public void SetDefermentForbearanceHelper(DefermentForbearanceHelper dfh)
        {
            this.DFH = dfh;
        }

        /// <summary>
        /// Assign the given task to the Supervisor ARC and Close it.
        /// </summary>
        public void SupervisorCloseTask(EspEnrollment esp, List<Ts26LoanInformation> ts26s, TsayDefermentForbearance df, List<ParentPlusLoanDetailsInformation> pplus, string enote)
        {
            var list = new List<TsayDefermentForbearance>();
            list.Add(df);
            SupervisorCloseTask(esp, ts26s, list, pplus, enote);
        }


        /// <summary>
        /// Assign the given task to the Supervisor ARC and Close it.
        /// </summary>
        public void SupervisorCloseTask(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<TsayDefermentForbearance> dfs, List<ParentPlusLoanDetailsInformation> pplus, string enote)
        {
            SupervisorTask(esp);
            CloseTask(esp, ts26s, dfs, pplus, enote, false);
        }

        /// <summary>
        /// Assign the given task to the Supervisor ARC (ERBEQ)
        /// </summary>
        public void SupervisorTask(EspEnrollment esp)
        {
            SupervisorTaskQueue.Add(esp);
        }

        /// <summary>
        /// Close the given task and leave an ENOTE ARC with the given ENOTE comment.
        /// </summary>
        public void CloseTask(EspEnrollment esp, List<Ts26LoanInformation> ts26s, TsayDefermentForbearance df, List<ParentPlusLoanDetailsInformation> pplus, string enote)
        {
            var list = new List<TsayDefermentForbearance>();
            if (df != null)
                list.Add(df);
            CloseTask(esp, ts26s, list, pplus, enote);

        }

        /// <summary>
        /// Close the given task and leave an ENOTE ARC with the given ENOTE comment.
        /// </summary>
        public void CloseTask(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<TsayDefermentForbearance> dfs, List<ParentPlusLoanDetailsInformation> pplus, string enote, bool performChecks = true)
        {
            if (performChecks)
                FutureCheckQueue.Add(new FutureCheck(esp, ts26s, dfs, pplus));

            enote = "LnSeq " + string.Join(",", ts26s.Select(o => o.LoanSequence).OrderBy(o => o).ToArray()) + ": " + enote;
            if (!skipTaskClose)
                CloseTaskQueue.Add(esp);
            else
                Unassign(esp);

            AddEnote(esp, enote);
        }

        [Obsolete]
        public bool LoadTask(EspEnrollment esp, bool alreadyRecursed = false, bool alreadyLogged = false)
        {
            RI.FastPath("tx3z/ITX6XRB;" + esp.SubQueue + ";" + esp.TaskControlNumber + ";" + esp.Arc);
            if (RI.MessageCode == "80014" && alreadyLogged == false)
            {
                PLR.AddNotification(string.Format("Error accessing ITX6X for EspEnrollmentId {0} {1}", esp.EspEnrollmentId, RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            else if (RI.MessageCode == "01020" && alreadyLogged == false)
            {
                PLR.AddNotification($"Task did not exist on ITX6X for EspEnrollmentId {esp.EspEnrollmentId}. Encountered session message: {RI.Message}\nTask marked as processed in the BATCHESP tables.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return false; // Since task is not in Session and is in tables, this means it is an old task that BU already worked
            }
            RI.PutText(21, 18, "01", Key.Enter);
            if (RI.MessageCode == "01848")
            {
                if (!alreadyRecursed)
                {
                    Unassign(esp);
                    return LoadTask(esp, true);
                }
                else
                {
                    PLR.AddNotification(RI.Message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if a task still is present in the Session.  
        /// If it is not, then it means it was already worked by an agent.
        /// </summary>
        public bool TaskExistsInSession(EspEnrollment esp)
        {
            RI.FastPath("tx3z/ITX6XRB;" + esp.SubQueue + ";" + esp.TaskControlNumber + ";" + esp.Arc);
            if (RI.MessageCode == "01020" || !RI.CheckForText(8, 75, "U"))
            {
                AddEnote(esp, $"No records found for the given task for task control numer: {esp.TaskControlNumber}.");
                PLR.AddNotification($"Task did not exist in an unassigned status on ITX6X for EspEnrollmentId {esp.EspEnrollmentId}. Encountered session message: {RI.Message}\nTask marked as processed in the BATCHESP tables.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return false; // Since task is not in Session and is in tables, this means it is an old task that BU already worked
            }
            return true;
        }

        private void AddEnote(EspEnrollment esp, string enote)
        {
            ArcData ad = new ArcData(this.region)
            {
                AccountNumber = esp.AccountNumber,
                Arc = "ENOTE",
                Comment = enote,
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };
            var result = ad.AddArc();
            if (!result.ArcAdded)
                PLR.AddNotification(string.Format("Unable to leave ENOTE for borrower {0}.  {1}", esp.BorrowerSsn, string.Join(";", result.Errors)), NotificationType.ErrorReport, NotificationSeverityType.Warning);

        }

        /// <summary>
        /// Unassign the current user from the given task.
        /// </summary>
        public void Unassign(EspEnrollment esp)
        {
            RI.FastPath("tx3z/CTX6J;;;;;");
            RI.PutText(7, 42, esp.Queue);
            RI.PutText(8, 42, esp.SubQueue);
            RI.PutText(13, 42, username);
            RI.Hit(Key.Enter);
            if (RI.MessageCode == "01020")//already unassigned
                return;

            RI.PutText(8, 15, "", true);
            RI.Hit(Key.Enter);
        }

        public void CommitClosedTasks(EspEnrollment esp, List<Tuple<List<Ts26LoanInformation>, List<TsayDefermentForbearance>, List<Ts01Enrollment>, List<ParentPlusLoanDetailsInformation>>> processedLoanInformation, List<int> loansToExcludeFromDfChecks)
        {
            //Add semester bridge for worked ESP task, whether an update was made or not.
            //However, exclude loans that are split
            CheckSemesterBridgeDefer(esp, processedLoanInformation, loansToExcludeFromDfChecks); 

            foreach (var task in FutureCheckQueue.GroupBy(o => o.Esp))
            {
                CheckFutureSchoolDefer(task.Key, task.SelectMany(o => o.Ts26s).ToList(), task.SelectMany(o => o.Dfs).ToList());
                CheckFutureAlignForb(task.Key, task.SelectMany(o => o.Ts26s).ToList());
                AppendPostEnrollDefer(task.Key, processedLoanInformation);
            }
            FutureCheckQueue.Clear();
            AddAlignmentForb(esp, processedLoanInformation, loansToExcludeFromDfChecks); // Align forbs for worked ESP task. Exclude split loans

            CreateSupervisorTask();

            if (!skipTaskClose)
            {
                if (!skipTaskAssign)
                {
                    if (!AssignQueue(esp))
                        PLR.AddNotification($"Unable to assign queue task with control number {esp.TaskControlNumber}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    else
                        LoadTaskIntoQuecomplet(username);
                }
            }
        }

        /// <summary>
        /// Creates the DR queue task via the ERBEQ ARC.
        /// </summary>
        public void CreateSupervisorTask()
        {
            foreach (var task in SupervisorTaskQueue.Distinct())
            {
                if (DA.BorrowerAlreadyHasAReviewTask(task.BorrowerSsn))
                {
                    PLR.AddNotification($"There is already a DR task on the account {task.AccountNumber}. No new supervisor task was added.", NotificationType.Other, NotificationSeverityType.Informational);
                    continue; // We forego adding a supervisor task since session doesn't allow multiple on the same account
                }

                Func<DateTime?, string> dt = new Func<DateTime?, string>(d => d.HasValue ? d.Value.ToString("MM/dd/yy") : "");
                string comment = "{0}, ST SSN {1}, SCHL {2}, SEP REA {3}, SEP DT {4}, CERT DT {5}, ENRL STAT EFF {6}, SEP SOURCE {7}, NOTIF DT {8}";
                comment = string.Format(comment,
                    task.Message1,
                    task.StudentSsn,
                    task.SchoolCode,
                    task.Esp_Status,
                    dt(task.Esp_SeparationDate),
                    dt(task.Esp_CertificationDate),
                    dt(task.EnrollmentBeginDate),
                    task.SourceCode,
                    dt(task.ArcRequestDate)
                ); 
                ArcData ad = new ArcData(this.region)
                {
                    AccountNumber = task.AccountNumber,
                    Arc = "ERBEQ",
                    Comment = comment,
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                var result = ad.AddArc();
                if (!result.ArcAdded)
                    PLR.AddNotification(string.Format("Unable to leave ERBEQ for borrower {0}.  {1}", task.BorrowerSsn, string.Join(";", result.Errors)), NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            SupervisorTaskQueue.Clear();
        }

        /// <summary>
        /// Loads the task into the Queue Completer script's processing table.
        /// </summary>
        private bool LoadTaskIntoQuecomplet(string userName)
        {
            foreach (var esp in CloseTaskQueue.Distinct())
            {
                if (DA.InsertIntoQueueCompleter(esp))
                    Console.WriteLine($"Updated Queue Completer table by inserting task control number {esp.TaskControlNumber}");
                else
                    Console.WriteLine($"Task {esp.TaskControlNumber} failed to insert into Queue Completer table.");
            }
            CloseTaskQueue.Clear();
            return true;
        }

        /// <summary>
        /// Assigns the current Queue task to the user
        /// </summary>
        public bool AssignQueue(EspEnrollment esp)
        {
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, esp.Queue, true);
            RI.PutText(8, 42, esp.SubQueue, true);
            RI.PutText(9, 42, esp.TaskControlNumber, true);
            RI.PutText(9, 76, "D", ReflectionInterface.Key.Enter, true);

            if (RI.ScreenCode != "TXX6O")
            {
                Console.WriteLine($"Expected to be on screen TXX71, but current screen is {RI.ScreenCode}. Session Message:{RI.Message}. UserId: {username}");
                return false;
            }

            RI.PutText(8, 15, username, ReflectionInterface.Key.Enter, true);

            if (RI.MessageCode != "01005")
            {
                Console.WriteLine($"Unable to assign queue task; Session Message:{RI.Message}; UserId:{username}; Task control number: {esp.TaskControlNumber}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Applies a semester bridge deferment if there is a gap between school deferments
        /// over the summer or winter breaks.
        /// </summary>
        private void CheckSemesterBridgeDefer(EspEnrollment esp, List<Tuple<List<Ts26LoanInformation>, List<TsayDefermentForbearance>, List<Ts01Enrollment>, List<ParentPlusLoanDetailsInformation>>> processedLoanInformation, List<int> loansToExcludeFromDfChecks)
        {
            List<Ts26LoanInformation> ts26s;
            List<TsayDefermentForbearance> dfs;
            List<Ts01Enrollment> ts01s;
            List<ParentPlusLoanDetailsInformation> pplus;
            SetTargetLoans(processedLoanInformation, out ts26s, out dfs, out ts01s, out pplus, loansToExcludeFromDfChecks);

            bool loansBefore1987 = ts26s.Any(p => p.DisbursementDate < DateTime.Parse("07/01/1987"));
            bool consolBefore1993 = ts26s.Any(o => o.DisbursementDate < DateTime.Parse("07/01/1993") && o.LoanProgramType.IsIn(ConsolidationLoanTypes));
            bool updatingPplus = (esp.BorrowerSsn != esp.StudentSsn && pplus != null && pplus.Any()) ? true : false;
            string[] schoolDeferTypes = { "D45", "D15", "D18" };

            if (!ts26s.Any())
            {
                Console.WriteLine($"Skipping semester bridge deferment for account {esp.AccountNumber}.");
                return;
            }

            Console.WriteLine($"Reading Compass TSAY data for account {esp.AccountNumber} to assess need of a bridge deferment.");
            var tsayScrapes = TsayScrapedLoanInformation.ScrapeTsay(RI, esp.BorrowerSsn);
            foreach (var tsayScrape in tsayScrapes.Where(p => p.LoanSequence.IsIn(ts26s.Select(o => (int)o.LoanSequence).ToArray())))
                DA.AddTsayScrapedLoanInformation(tsayScrape.BorrowerSsn, tsayScrape.LoanSequence, tsayScrape.LoanProgramType, tsayScrape.BeginDate, tsayScrape.EndDate, tsayScrape.CertificationDate, tsayScrape.DisbursementDate, tsayScrape.DeferSchool, tsayScrape.ApprovalStatus, tsayScrape.DfType, tsayScrape.AppliedDate, "CheckSemesterBridgeDefer");

            bool successfullyProcessed = true;

            foreach (var ts26 in ts26s)
            {
                TsayDefermentForbearance df = (dfs.Any()) ? dfs.Where(p => p.LoanSequence == ts26.LoanSequence).FirstOrDefault() : new TsayDefermentForbearance();
                var gapsNeedingBridgeDefers = DA.GetSemesterGaps(ts26.BorrowerSsn, ts26.LoanSequence); // Find semester gaps for specific loan
                DA.SetTsayScrapedLoanInformationAsProcessed(ts26.BorrowerSsn, ts26.LoanSequence); // Immediately mark as processed
                if (gapsNeedingBridgeDefers != null && gapsNeedingBridgeDefers.Count != 0)
                {
                    gapsNeedingBridgeDefers = gapsNeedingBridgeDefers.Where(p => p.BeginType.IsIn(schoolDeferTypes) && p.EndType.IsIn(schoolDeferTypes)).ToList(); // Filter to only look at school defers
                    if (loansBefore1987 || consolBefore1993)
                        gapsNeedingBridgeDefers = gapsNeedingBridgeDefers.Where(o => o.BeginType != "D18" && o.EndType != "D18").ToList(); // Filter out older loans with D18 defers

                    FilterOutNonSemesterGaps(gapsNeedingBridgeDefers);
                    var mostRecentGap = gapsNeedingBridgeDefers.Where(p => p.EndDate == gapsNeedingBridgeDefers.Select(o => o.EndDate).Max()).SingleOrDefault();

                    if (gapsNeedingBridgeDefers.Count == 0) // Move on to next loan if filtered list contains no valid semester gap that needs a bridge
                        continue;

                    DateTime beginDate = mostRecentGap.EndDate.AddDays(1);
                    DateTime endDate = mostRecentGap.BeginDate.AddDays(-1);

                    Console.WriteLine($"Applying a semester bridge for loan sequence {ts26.LoanSequence}. The bridge goes from {beginDate.ToString("MM/dd/yyyy")} to {endDate.ToString("MM/dd/yyyy")}.");
                    if (updatingPplus)
                        successfullyProcessed &= DFH.DefermentForbearanceAdd(esp, "F09", df, ts26s.Where(p => p.LoanSequence == ts26.LoanSequence).ToList(), ts01s, pplus, beginDate, endDate);
                    else
                        successfullyProcessed &= DFH.DefermentForbearanceAdd(esp, "D16", df, ts26s.Where(p => p.LoanSequence == ts26.LoanSequence).ToList(), ts01s, pplus, beginDate, endDate);

                }
            }

            if (!successfullyProcessed)
            {
                AddEnote(esp, "Error while adding bridge deferment");
            }
        }

        /// <summary>
        /// Removes gaps that are too large to be single semester gaps from our list of valid deferment gaps.
        /// </summary>
        private static void FilterOutNonSemesterGaps(List<SemesterGaps> gapsNeedingBridgeDefers)
        {
            for (int i = 0; i < gapsNeedingBridgeDefers.Count; i++)
            {
                if (gapsNeedingBridgeDefers[i].EndDate.Month.IsIn(4, 5, 6) && gapsNeedingBridgeDefers[i].BeginDate.Year != gapsNeedingBridgeDefers[i].EndDate.Year)
                    gapsNeedingBridgeDefers.RemoveAt(i);
                else if (gapsNeedingBridgeDefers[i].EndDate.Month == 12 && gapsNeedingBridgeDefers[i].BeginDate.Year - 1 != gapsNeedingBridgeDefers[i].EndDate.Year)
                    gapsNeedingBridgeDefers.RemoveAt(i);
            }
        }



        /// <summary>
        /// Adds forbearances to loans with school deferments to align them 
        /// with the borrower's other loans that have a future grace end date.
        /// </summary>
        private void AddAlignmentForb(EspEnrollment esp, List<Tuple<List<Ts26LoanInformation>, List<TsayDefermentForbearance>, List<Ts01Enrollment>, List<ParentPlusLoanDetailsInformation>>> processedLoanInformation, List<int> loansToExcludeFromDfChecks)
        {
            List<Ts26LoanInformation> ts26s;
            List<TsayDefermentForbearance> dfs;
            List<Ts01Enrollment> ts01s;
            List<ParentPlusLoanDetailsInformation> pplus;
            SetTargetLoans(processedLoanInformation, out ts26s, out dfs, out ts01s, out pplus, loansToExcludeFromDfChecks);

            if (!ts26s.Any())
            {
                Console.WriteLine($"Skipping alignment forb for task control number {esp.TaskControlNumber}.");
                return;
            }

            Console.WriteLine($"Checking account {esp.AccountNumber} to see if an alignment forb needs to be added.");
            List<Ts26LoanInformation> matchingTs26s = new List<Ts26LoanInformation>();
            matchingTs26s.AddRange(ts26s);
            matchingTs26s = matchingTs26s.Where(p => !p.LoanProgramType.IsIn(ConsolidationLoanTypes)).ToList();
            bool successfullyProcessed = true;

            if (matchingTs26s.Any(p => p.GraceEndDate >= DateTime.Now) && matchingTs26s.Any(p => p.GraceEndDate < DateTime.Now)) // Check if there is a grace misalignment
            {
                DateTime maxGraceDate = DateTime.Parse("01/01/1900"); //Default date
                if (esp.Esp_SeparationDate != null)
                {
                    maxGraceDate = (DateTime)esp.Esp_SeparationDate;
                    maxGraceDate = maxGraceDate.AddMonths(6);
                }
                if (esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime))
                {
                    if (esp.EnrollmentBeginDate != null && (esp.Esp_SeparationDate == null || esp.Esp_SeparationDate > DateTime.Now))
                        maxGraceDate = (DateTime)esp.EnrollmentBeginDate;
                    else if (esp.EnrollmentBeginDate == null)
                    {
                        SupervisorCloseTask(esp, ts26s, dfs, pplus, "Please rvw for invalid enrollment begin date");
                        return;
                    }
                }
                if (esp.Esp_SeparationDate == null && !esp.Esp_Status.IsIn(StatusCode.Graduated, StatusCode.Withdrew, StatusCode.LessThanHalfTime))
                {
                    SupervisorCloseTask(esp, ts26s, dfs, pplus, "Review invalid sep dt");
                    return;
                }

                if (maxGraceDate != DateTime.Parse("01/01/1900"))
                {
                    var gracelessLoans = matchingTs26s.Where(p => p.GraceEndDate < DateTime.Now).ToList();
                    Console.WriteLine($"Reading Compass TSAY data for borrower {esp.BorrowerSsn} to assess need of alignemnt forb.");
                    var tsayScrapes = TsayScrapedLoanInformation.ScrapeTsay(RI, esp.BorrowerSsn);
                    foreach (var tsayScrape in tsayScrapes.Where(p => p.LoanSequence.IsIn(ts26s.Select(o => (int)o.LoanSequence).ToArray())))
                        DA.AddTsayScrapedLoanInformation(tsayScrape.BorrowerSsn, tsayScrape.LoanSequence, tsayScrape.LoanProgramType, tsayScrape.BeginDate, tsayScrape.EndDate, tsayScrape.CertificationDate, tsayScrape.DisbursementDate, tsayScrape.DeferSchool, tsayScrape.ApprovalStatus, tsayScrape.DfType, tsayScrape.AppliedDate, "CheckAlignmentForb");

                    foreach (var ts26 in gracelessLoans)
                    {
                        bool isPostEnrollDefElig = pplus.Any() ? pplus.Where(o => o.LoanSequence == ts26.LoanSequence).Select(p => p.PostEnrollmentDefermentEligible).SingleOrDefault() : ts26.DisbursementDate >= DateTime.Parse("07/01/2008");
                        DA.SetTsayScrapedLoanInformationAsProcessed(ts26.BorrowerSsn, ts26.LoanSequence);

                        if (ts26.LoanProgramType.IsIn("DLPLGB", "PLUSGB", "PLUS", "DLPLUS") && isPostEnrollDefElig) //Don't add align forb to post enrollment eligible PLUS loans
                            continue;

                        TsayDefermentForbearance df = (dfs.Any()) ? dfs.Where(p => p.LoanSequence == ts26.LoanSequence).FirstOrDefault() : new TsayDefermentForbearance();
                        var maxDf = tsayScrapes.Where(p => p.LoanSequence == ts26.LoanSequence && p.EndDate == tsayScrapes.Where(o => o.LoanSequence == ts26.LoanSequence).Select(e => e.EndDate).Max()).FirstOrDefault();
                        var maxSchoolDefer = tsayScrapes.Where(p => p.LoanSequence == ts26.LoanSequence && p.EndDate == tsayScrapes.Where(o => o.LoanSequence == ts26.LoanSequence && o.DfType.IsIn("D15", "D18", "D45")).Select(e => e.EndDate).Max()).FirstOrDefault();
                        bool maxDfIsShortF15 = false;
                        if (maxDf != null && maxDf.DfType == "F15")
                        {
                            var F15Duration = maxDf.EndDate - maxDf.BeginDate;
                            if (F15Duration.TotalDays < 180) // If F15 is less than six months, 
                                maxDfIsShortF15 = true;
                        }
                        if (maxDf == null || (!maxDf.DfType.IsIn("D15", "D18", "D45", "F02") && !maxDfIsShortF15)) // Only do an alignment if the most recent deferment is a school deferment or if a late school notif forb is most recent d/f; or if F15 shorter than 6 months is max d/f
                            continue;
                        if (maxSchoolDefer != null && maxSchoolDefer.EndDate < maxGraceDate)
                        {
                            var duration = (DateTime)maxGraceDate - maxSchoolDefer.EndDate.AddDays(1);
                            if (duration.TotalDays > 184) // Don't want to add align forb if it would be for more than approx six months (184 days = most number of days in a given 6 month partition of year)
                            {
                                SupervisorCloseTask(esp, ts26s, df, pplus, $"Review to align loans rs");
                                continue;
                            }
                            successfullyProcessed &= DFH.DefermentForbearanceAdd(esp, "F15", df, matchingTs26s.Where(p => p.LoanSequence == ts26.LoanSequence).ToList(), ts01s, pplus, maxSchoolDefer.EndDate.AddDays(1), maxGraceDate);
                        }
                    }
                }
            }
            if (!successfullyProcessed)
            {
                AddEnote(esp, "Error adding align rpy forb");
            }

        }

        private void CheckFutureSchoolDefer(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<TsayDefermentForbearance> dfs)
        {
            bool successfullyProcessed = true;
            List<Tuple<string, DateTime, DateTime>> toDelete = new List<Tuple<string, DateTime, DateTime>>();
            IterateItsay(esp, ts26s, () =>
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 9;
                settings.MaxRow = 20;
                bool keepProcessing = true;
                PageHelper.Iterate(RI, (row, s) =>
                {
                    var type = RI.GetText(row, 25, 3);
                    var begin = RI.GetText(row, 30, 8).ToDateNullable();
                    var end = RI.GetText(row, 40, 8).ToDateNullable();
                    if (type.IsIn("D15", "D18", "D45") && begin > DateTime.Today)
                    {
                        RI.PutText(22, 14, RI.GetText(row, 2, 2), Key.Enter);
                        var schoolCode = RI.GetText(8, 39, 8);
                        if (RI.ScreenCode == "TSXB1")
                            RI.Hit(Key.F12);
                        if (string.IsNullOrWhiteSpace(schoolCode) || dfs.AllAndAny(o => o.DeferSchool == schoolCode)) //If no school code present in the Session, we treat it like a match
                        {
                            toDelete.Add(Tuple.Create(type, begin.Value, end.Value));
                        }
                        else
                        {
                            if (begin > DateTime.Today.AddDays(30))
                            {
                                SupervisorTask(esp);
                                AddEnote(esp, "Future Defer, please review NSLDS");
                                keepProcessing = false;
                                s.ContinueIterating = false;
                            }
                        }
                    }
                }, settings);
                return keepProcessing;
            });
            string message = "";
            foreach (var delete in toDelete)
            {
                if (!DeleteDf(esp, delete.Item1, delete.Item2, delete.Item3))
                {
                    successfullyProcessed = false;
                    message += $"Error encountered trying to remove the future {delete.Item1} school deferment that has a beginning date of {delete.Item2.ToString()} and an end date of {delete.Item3}.  ";
                }
            }
            if (!successfullyProcessed)
            {
                PLR.AddNotification($"Unable to resolve future school deferments for esp task {esp.TaskControlNumber}.  The following issue(s) were encountered: {message}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                SupervisorCloseTask(esp, ts26s, dfs, null, "Review required for future dated defers");
            }
        }

        private void CheckFutureAlignForb(EspEnrollment esp, List<Ts26LoanInformation> ts26s)
        {
            bool successfullyProcessed = true;
            List<Tuple<string, DateTime, DateTime>> toDelete = new List<Tuple<string, DateTime, DateTime>>();
            IterateItsay(esp, ts26s, () =>
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 9;
                settings.MaxRow = 21;
                bool keepProcessing = true;
                PageHelper.Iterate(RI, (row, s) =>
                {
                    var type = RI.GetText(row, 25, 3);
                    var begin = RI.GetText(row, 30, 8).ToDateNullable();
                    var end = RI.GetText(row, 40, 8).ToDateNullable();
                    if ((type == "F15" || type == "D46") && begin > DateTime.Today)
                    {
                        var followingType = RI.GetText(row + 1, 25, 3);
                        var followingDate = RI.GetText(row + 1, 40, 8).ToDateNullable();
                        bool goodFollowingRow = followingType.IsIn("D15", "D18") && followingDate == begin?.AddDays(-1);
                        bool goodD46Def = type == "D46" && followingType.IsIn("D45") && followingDate == begin?.AddDays(-1);
                        if (!goodFollowingRow && !goodD46Def)
                        {
                            toDelete.Add(Tuple.Create(type, begin.Value, end.Value));
                        }
                    }
                }, settings);
                return keepProcessing;
            });
            string message = "";
            foreach (var delete in toDelete)
            {
                if (!DeleteDf(esp, delete.Item1, delete.Item2, delete.Item3))
                {
                    successfullyProcessed = false;
                    message += $"Error encountered trying to align the future {delete.Item1} forb/def that has a beginning date of {delete.Item2.ToString()} and an end date of {delete.Item3.ToString()}.  ";
                }
            }
            if (!successfullyProcessed)
            {
                PLR.AddNotification($"Unable to resolve future alignment of forbs/defs for esp task {esp.TaskControlNumber}.  The following issue(s) were encountered: {message}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                SupervisorCloseTask(esp, ts26s, new TsayDefermentForbearance(), null, "Review required. Adjust align repay forb or future defer.");
            }
        }

        /// <summary>
        /// Verifies if a D46 post enrollment deferment should be added to any loans. 
        /// Passes loans that need the deferment to ProcessPostEnrollDefer() method. 
        /// </summary>
        private bool AppendPostEnrollDefer(EspEnrollment esp, List<Tuple<List<Ts26LoanInformation>, List<TsayDefermentForbearance>, List<Ts01Enrollment>, List<ParentPlusLoanDetailsInformation>>> processedLoanInformation)
        {
            List<Ts26LoanInformation> matchingTs26s = new List<Ts26LoanInformation>();
            List<TsayDefermentForbearance> matchingDfs = new List<TsayDefermentForbearance>();
            List<Ts01Enrollment> matchingTs01s = new List<Ts01Enrollment>();
            List<ParentPlusLoanDetailsInformation> matchingPplus = new List<ParentPlusLoanDetailsInformation>();
            foreach (var processedLoan in processedLoanInformation)
            {
                matchingTs26s.AddRange(processedLoan.Item1);
                matchingDfs.AddRange(processedLoan.Item2);
                matchingTs01s.AddRange(processedLoan.Item3);
                matchingPplus.AddRange(processedLoan.Item4);
            }

            var postEnrollDefEligLoanSeqs = matchingTs26s.Where(o => o.LoanProgramType.IsIn("DLPLUS", "PLUS") && o.LoanSequence.IsIn(matchingPplus.Where(p => p.PostEnrollmentDefermentEligible).Select(p => (int?)p.LoanSequence).ToArray())).Select(o => o.LoanSequence).Where(o => o.HasValue).Select(o => o.Value).ToArray();
            var gradPostEnrollDefEligLoanSeqs = matchingTs26s.Where(o => o.LoanProgramType.IsIn("DLPLGB", "PLUSGB") && o.DisbursementDate >= DateTime.Parse("07/01/2008")).Select(o => o.LoanSequence).Where(o => o.HasValue).Select(o => o.Value).ToArray();
            var tempList = postEnrollDefEligLoanSeqs.ToList();
            tempList.AddRange(gradPostEnrollDefEligLoanSeqs);
            postEnrollDefEligLoanSeqs = tempList.ToArray();
            matchingTs26s = matchingTs26s.Where(o => o.LoanSequence.IsIn(postEnrollDefEligLoanSeqs.Select(p => (int?)p).ToArray())).ToList();
            matchingDfs = matchingDfs.Where(o => o.LoanSequence.IsIn(postEnrollDefEligLoanSeqs)).ToList();
            matchingTs01s = matchingTs01s.Where(o => o.LoanSequence.IsIn(postEnrollDefEligLoanSeqs)).ToList();
            matchingPplus = matchingPplus.Where(o => o.LoanSequence.IsIn(postEnrollDefEligLoanSeqs)).ToList();

            Tuple<string, int?, DateTime, DateTime> lastDefer = null;
            bool processed = true;

            foreach (Ts26LoanInformation ts26 in matchingTs26s)
            {
                List<Ts26LoanInformation> targetLoan = new List<Ts26LoanInformation>();
                targetLoan.Add(ts26);
                TsayDefermentForbearance df = matchingDfs.Where(p => p.LoanSequence == ts26.LoanSequence).SingleOrDefault();
                IterateItsay(esp, targetLoan, () =>
                {
                    var settings = PageHelper.IterationSettings.Default();
                    settings.MinRow = 9;
                    settings.MaxRow = 10; // Only look at max defer or possible F02 underneath
                    bool keepProcessing = true;
                    PageHelper.Iterate(RI, (row, s) =>
                    {
                        var type = RI.GetText(row, 25, 3);
                        var begin = RI.GetText(row, 30, 8).ToDateNullable();
                        var end = RI.GetText(row, 40, 8).ToDateNullable();
                        if ((type != "D46")) // If max defer is not a D46
                        {
                            if (type == "D15" || type == "D18" || type == "D45" || type == "F02")
                                lastDefer = Tuple.Create(type, ts26.LoanSequence, begin.Value, end.Value);
                        }
                    }, settings);
                    return keepProcessing;
                });
                if (lastDefer != null)
                {
                    if (!ProcessPostEnrollDefer(esp, "D46", targetLoan, matchingTs01s.Where(p => p.LoanSequence == targetLoan.SingleOrDefault().LoanSequence).ToList(), matchingPplus.Where(o => o.LoanSequence == targetLoan.SingleOrDefault().LoanSequence).ToList(), df, lastDefer))
                    {
                        processed = false;
                        PLR.AddNotification($"Unable to remove future D46 for loan {targetLoan.SingleOrDefault().LoanSequence.ToString()} for esp task number {esp.TaskControlNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    }
                    else
                        lastDefer = null;
                }
            }
            if (!processed)
                AddEnote(esp, "Review for post enroll defer.");
            return processed;
        }

        /// <summary>
        /// Posts the D46 deferment in Session.
        /// </summary>
        private bool ProcessPostEnrollDefer(EspEnrollment esp, string dfType, List<Ts26LoanInformation> ts26s, List<Ts01Enrollment> ts01s, List<ParentPlusLoanDetailsInformation> pplus, TsayDefermentForbearance df, Tuple<string, int?, DateTime, DateTime> lastDefer)
        {
            //DateTime? newBeginDate = lastDefer.Item3; 
            //DateTime? newEndDate = lastDefer.Item4;
            DateTime? newBeginDate = esp.Esp_SeparationDate.Value.AddDays(1);
            DateTime? newEndDate = newBeginDate.Value.AddMonths(6);
            int? loanSeq = lastDefer.Item2;


            RI.FastPath("tx3z/ATS0H" + esp.BorrowerSsn);
            RI.PutText(9, 33, dfType == "F02" ? "F" : "D");
            RI.PutText(11, 33, DateTime.Now.ToString("MMddyy"));
            RI.Hit(Key.Enter);
            string typeDigitsOnly = new string(dfType.Where(o => char.IsDigit(o)).ToArray());
            if (RI.ScreenCode == "TSX7E")
                RI.PutText(21, 13, typeDigitsOnly, true);
            else
                RI.PutText(22, 13, typeDigitsOnly, true);
            RI.Hit(Key.Enter);

            if (RI.ScreenCode == "TSXA5")
            {
                var settings = PageHelper.IterationSettings.Default();
                PageHelper.Iterate(RI, (row, s) =>
                {
                    var seq = RI.GetText(row, 13, 3).ToIntNullable();
                    if ((loanSeq == seq) && RI.CheckForText(row, 2, "_"))
                        RI.PutText(row, 2, "X");
                }, settings);
                RI.Hit(Key.Enter);
            }
            string screenCode = RI.ScreenCode;
            if (screenCode == "TSX4A") // SCREEN IS FOR DEFERS FOR: ENROLLED FULL TIME / PARENT OF STUDENT ENROLLED F-TIME / STUDENT LESS THAN HALF TIME / PARENT PLUS IN-SCHOOL DEFER / POST ENROLLMENT DEFER
            {
                if (newBeginDate.HasValue)
                    RI.PutText(6, 18, newBeginDate.Value.ToString("MMddyy"));
                if (newEndDate.HasValue)
                    RI.PutText(6, 47, newEndDate.Value.ToString("MMddyy"));
                RI.PutText(7, 47, esp.Esp_SeparationDate.Value.ToString("MMddyy"));
                RI.PutText(9, 18, esp.Esp_CertificationDate.Value.ToString("MMddyy"));
                RI.PutText(10, 29, (esp.BorrowerSsn == esp.StudentSsn) ? "B" : "D");
                if (esp.BorrowerSsn != esp.StudentSsn)
                    RI.PutText(11, 17, esp.StudentSsn);  // Only enter student ssn if it is not same as borrower
                RI.PutText(12, 15, esp.SchoolCode);
                RI.PutText(14, 24, "Y");
                RI.PutText(16, 22, "Y");
                RI.PutText(16, 54, "Y");
                RI.Hit(Key.Enter);

                if (RI.MessageCode == "02354")
                {
                    RI.PutText(8, 18, "", true);
                    RI.PutText(8, 21, "", true);
                    RI.PutText(8, 24, "", true);
                    RI.Hit(Key.Enter);
                }
            }
            if (RI.ScreenCode == screenCode) //screen should have changed but didn't
            {
                var msg = "Unable to add or change defer.  " + RI.Message;
                SupervisorTaskQueue.Add(esp);
                AddEnote(esp, "Ending D46 not found despite being post enroll elig. Please review.");
                return false;
            }
            else
            {
                return DFH.DefermentForbearanceNonSelectionCheck(esp, dfType, df, ts26s, ts01s, pplus, true, newBeginDate ?? df.BeginDate.Value, newEndDate ?? df.EndDate.Value);
            }
        }

        private void IterateItsay(EspEnrollment esp, List<Ts26LoanInformation> ts26s, Func<bool> tsxb0)
        {
            RI.FastPath("tx3z/ITSAY" + esp.BorrowerSsn);
            if (RI.ScreenCode == "TSXAZ")
            {
                var remainingTs26s = ts26s.ToArray().ToList();  //copy
                while (remainingTs26s.Any())
                {
                    RI.FastPath("tx3z/ITSAY" + esp.BorrowerSsn);
                    var settings = PageHelper.IterationSettings.Default();
                    settings.MinRow = 9;
                    settings.MaxRow = 21;
                    Ts26LoanInformation match = null;
                    PageHelper.Iterate(RI, (row, s) =>
                    {
                        var seq = RI.GetText(row, 14, 3).ToIntNullable();
                        match = remainingTs26s.FirstOrDefault(o => o.LoanSequence == seq);
                        if (match != null)
                        {
                            RI.PutText(22, 17, RI.GetText(row, 2, 2), Key.Enter);
                            remainingTs26s.Remove(match);
                            s.ContinueIterating = false;
                        }
                    }, settings);
                    if (match == null)
                        return;  //couldn't find any more of our loans
                    if (!tsxb0())
                        return;
                }
            }
            else if (RI.ScreenCode == "TSXB0")
            {
                tsxb0();
            }
        }

        private bool DeleteDf(EspEnrollment esp, string dfType, DateTime begin, DateTime end) // Future enhancement: Needs loan sequence-specific logic; also, does it need to delete forb/def removal correspondence?
        {
            RI.FastPath("tx3z/DTS0H" + esp.BorrowerSsn + ";;" + dfType);
            if (RI.MessageCode == "11113")
                return false; //no forb info found

            if (RI.ScreenCode == "TSX0G")
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 7;
                settings.MaxRow = 21;
                PageHelper.Iterate(RI, (row, s) =>
                {
                    var rowBegin = RI.GetText(row, 24, 8).ToDateNullable();
                    var rowEnd = RI.GetText(row, 33, 8).ToDateNullable();
                    if (rowBegin == begin && rowEnd == end)
                    {
                        RI.PutText(22, 13, RI.GetText(row, 2, 2), Key.Enter);
                        s.ContinueIterating = false;
                    }
                }, settings);
            }

            bool found = false;
            if (!RI.CheckForText(9, 18, "_"))
            {
                SupervisorTaskQueue.Add(esp);
                return false;
            }
            RI.PutText(9, 18, esp.Esp_CertificationDate.Value.ToString("MMddyy"), Key.Enter);
            bool dfWithMatchingDatesNotFound = false;
            bool targetDefRemoved = false;
            var procTsx31 = new Action(() =>
            {
                var tsx31Settings = PageHelper.IterationSettings.Default();
                tsx31Settings.MinRow = 12;
                tsx31Settings.MaxRow = 18;
                PageHelper.Iterate(RI, (row, s) =>
                {
                    var rowBegin = RI.GetText(row, 21, 8).ToDateNullable();
                    var rowEnd = RI.GetText(row, 30, 8).ToDateNullable();
                    var rowType = RI.GetText(row, 6, 1);
                    if (rowBegin == begin && rowEnd == end && dfType.StartsWith(rowType))
                    {
                        RI.PutText(row, 3, "X");
                        RI.Hit(Key.Enter);
                        found = true;
                    }
                }, tsx31Settings);
                RI.Hit(Key.F12);
                if (RI.ScreenCode == "TSX30")
                    RI.Hit(Key.F12);
                RI.Hit(Key.F6);
                if (RI.MessageCode == "01006")
                    targetDefRemoved = true;
                if (RI.MessageCode == "00000")
                    dfWithMatchingDatesNotFound = true;
            });
            if (RI.ScreenCode == "TSX30")
            {
                RI.PutText(21, 14, "01", Key.Enter, true);
                procTsx31();
                if (RI.CheckForText(2, 2, "*"))
                {
                    PLR.AddNotification("ABEND Encountered for " + esp, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false; //ABEND
                }
                else if (dfWithMatchingDatesNotFound)
                {
                    PLR.AddNotification("Matching defer/forb not found to remove. Please review future school defs and or alignment forbs for " + esp, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false; //Not found
                }
                if (!targetDefRemoved)
                    DeleteDf(esp, dfType, begin, end);
            }
            else
            {
                procTsx31();
            }
            return found;
        }

        /// <summary>
        /// Helper method that populates the lists with the target loans.
        /// </summary>
        private static void SetTargetLoans(List<Tuple<List<Ts26LoanInformation>, List<TsayDefermentForbearance>, List<Ts01Enrollment>, List<ParentPlusLoanDetailsInformation>>> processedLoanInformation, out List<Ts26LoanInformation> ts26s, out List<TsayDefermentForbearance> dfs, out List<Ts01Enrollment> ts01s, out List<ParentPlusLoanDetailsInformation> pplus, List<int> loansToExcludeFromDfChecks)
        {
            ts26s = new List<Ts26LoanInformation>();
            dfs = new List<TsayDefermentForbearance>();
            ts01s = new List<Ts01Enrollment>();
            pplus = new List<ParentPlusLoanDetailsInformation>();
            foreach (var processedLoan in processedLoanInformation.Where(p => !p.Item1.Any(l => l.LoanSequence.Value.IsIn(loansToExcludeFromDfChecks.ToArray())))) //Exclude loan seqs that should not have Df (deferment-forbearance) checks
            {
                ts26s.AddRange(processedLoan.Item1);
                dfs.AddRange(processedLoan.Item2);
                ts01s.AddRange(processedLoan.Item3);
                pplus.AddRange(processedLoan.Item4);
            }
        }
    }
}
