using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;

namespace SCRAINTUP
{
    public class SCRAInterestUpdatesFromDOD : BatchScript
    {
        private const string EojTasksProcessed = "Queue Tasks Opened";
        private const string EojQueueTaskWorked = "Queue Tasks Worked";
        private const string EojErrors = "Number of Errors";

        private static List<string> EojFields = new List<string>() { EojTasksProcessed, EojQueueTaskWorked, EojErrors };
        private string QueueOwner { get; set; }

        public SCRAInterestUpdatesFromDOD(ReflectionInterface ri)
            : base(ri, "SCRAINTUP", "ERR_BU35", "EOJ_BU35", EojFields, DataAccessHelper.Region.Uheaa)
        {
            QueueOwner = GetQueueOwner();
        }

        public override void Main()
        {
            bool doneProcessing = false;
            AccessQueue();
            while (!doneProcessing)
            {
                if (RI.CheckForText(8, 75, "W"))
                {
                    CreateTask();

                    doneProcessing = SelectNextTask();
                    continue;
                }
                ProcessTask();
                doneProcessing = SelectNextTask();
            }

            ProcessingComplete();
        }

        private bool SelectNextTask()
        {
            RI.FastPath("TX3Z/ITX6XMS;01");
            return RI.MessageCode == "01020";//No more tasks to process
        }

        private void AddDenialComment(string accountNumber, List<int> loanSeq)
        {
            if (!Atd22ByLoan(accountNumber, "SCRAD", "The borrowers active duty begin date is prior to disbursement date", "", loanSeq, ScriptId, false))
            {
                string message = string.Format("The script was unable to add the following comment \r\n ARC: SCRAD \r\n Comment: The borrowers active duty begin date is prior to disbursement date \r\n Loan Seq(s): {0}",
                    string.Join(",", loanSeq.Select(p => p).ToArray()));
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        private void ProcessTask()
        {
            BorrowerData borrowerData = GetBorrowerData();

            if (borrowerData.EligibleLoanData.Count == 0)
            {
                AddDenialComment(borrowerData.AccountNumber, borrowerData.NonEligibleLoanData.Select(p => p.LoanSeq).ToList());
                CloseTask();
                return;
            }

            Result result = AccessInterestScreen(borrowerData);
            if (result == Result.CloseTask)
            {
                CloseTask();
                return;
            }
            else if (result == Result.TaskReassigned)
                return;

            if (AccessRepaymentScheduleScreen(borrowerData))
                CloseTask();
        }

        private void CloseTask()
        {
            RI.FastPath("TX3Z/ITX6XMS;01");
            RI.PutText(21, 18, "01", ReflectionInterface.Key.F2, true);
            RI.PutText(8, 19, "C");
            RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
            Eoj[EojQueueTaskWorked]++;
        }

        /// <summary>
        /// Checks to see if all of the borrowers loans are in a deferment or forbearance.
        /// </summary>
        /// <param name="eligibleLoans">Borrowers loans.</param>
        /// <returns>True if all loans are in a deferment or forbearance</returns>
        private bool CheckDefermentForb(List<BorrowerLoanData> eligibleLoans)
        {
            return eligibleLoans.Where(p => p.LoanStatus.ToUpper().IsIn("DEFERMENT", "FORBEARANCE")).Count() > 0;
        }

        /// <summary>
        /// Verifies that the borrower does not have loans in an invalid status.
        /// </summary>
        /// <param name="eligibleLoans">Borrowers Loans.</param>
        /// <returns>True if one loan is in a invalid status false if not.</returns>
        private bool VerifyLoanStatus(List<BorrowerLoanData> eligibleLoans)
        {
            string[] badStatuses = new string[] { "VERIFIED DEATH", "ALLEGED DEATH", "VERIFIED DISABILITY", "ALLEGED 120 TPD SUSP", "ALLEGED BANKRUPTCY", "VERIFIED BANKRUPTCY", "IN SCHOOL", "IN GRACE", "CLAIM RETURNED", "CLAIM SUBMITTED" };
            return eligibleLoans.Where(p => p.LoanStatus.ToUpper().IsIn(badStatuses)).Count() > 0;
        }

        /// <summary>
        /// Access the repayment schedule screen and re discloses to the borrower if all conditions are met,
        /// </summary>
        /// <param name="bdata">Borrowers information.</param>
        /// <returns>True if the borrower was re-disclosed too.  false if not.</returns>
        private bool AccessRepaymentScheduleScreen(BorrowerData bdata)
        {
            bool hasActiveRepaymentSchedule = false;
            RI.FastPath("TX3Z/ITS2X" + bdata.AccountNumber);
            if (RI.ScreenCode == "TSX2Y")
                hasActiveRepaymentSchedule = RI.CheckForText(8, 7, "A");
            else if (RI.ScreenCode == "TSX3W")
                hasActiveRepaymentSchedule = RI.CheckForText(7, 44, "A");

            bool hasBadLoans = VerifyLoanStatus(bdata.EligibleLoanData);
            //The borrower does not have valid loans.
            if (hasBadLoans)
                return true;

            bool loansInDefForb = CheckDefermentForb(bdata.EligibleLoanData);
            //The borrower does not have valid loans.
            if (!hasActiveRepaymentSchedule && loansInDefForb)
                return true;

            List<BorrowerRepaymentData> repaymentData = BorrowerRepaymentData.Populate(bdata.AccountNumber);
            //The borrower does not have valid loans.
            if (repaymentData == null || repaymentData.Count == 0)
                return true;

            List<BorrowerLoanData> validLoans = GetAllValidLoans(bdata, repaymentData).Where(o => o.InterestWasUpdated).ToList();
            bool success = new RediscloseSchedule(RI, UserId, QueueOwner).RediscloseLoans(bdata.Ssn, validLoans);
            if (!success)
                CreateTask();

            return success;
        }

        private List<BorrowerLoanData> GetAllValidLoans(BorrowerData bdata, List<BorrowerRepaymentData> repaymentData)
        {
            List<BorrowerLoanData> validLoans = new List<BorrowerLoanData>();
            foreach (BorrowerLoanData loanData in bdata.EligibleLoanData)
            {
                BorrowerRepaymentData loanRepaymentData = repaymentData.Where(p => p.LoanSeq == loanData.LoanSeq).FirstOrDefault();
                if (loanRepaymentData != null)
                {
                    validLoans.Add(new BorrowerLoanData()
                    {
                        LoanSeq = loanData.LoanSeq,
                        LoanStatus = loanData.LoanStatus,
                        LoanProgram = loanData.LoanProgram,
                        DisbDate = loanData.DisbDate,
                        PaymentAmount = loanRepaymentData.PaymentAmount,
                        ScheduleType = loanRepaymentData.ScheduleType,
                        InterestWasUpdated = loanData.InterestWasUpdated
                    });
                }
            }

            return validLoans;
        }

        private BorrowerData GetBorrowerData()
        {
            DateTime scraBeginDate = RI.GetText(9, 40, 10).ToDate();

            DateTime? scraEndDate = (RI.GetText(9, 76, 3) + RI.GetText(10, 2, 7)).ToDateNullable();
            RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter, true);
            Eoj[EojTasksProcessed]++;
            string accountNumber = RI.GetText(6, 10, 12).Replace(" ", "");
            string ssn = RI.GetText(4, 16, 11).Replace(" ", "");

            List<BorrowerLoanData> borrowerData = GatherDataFromTS26(accountNumber);

            List<BorrowerLoanData> eligibleLoans = borrowerData.Where(p => p.DisbDate <= scraBeginDate).ToList();
            List<BorrowerLoanData> nonEligibleLoans = borrowerData.Where(p => p.DisbDate > scraBeginDate).ToList();
            return BorrowerData.Populate(accountNumber, ssn, scraBeginDate, scraEndDate, eligibleLoans, nonEligibleLoans);
        }

        private enum Result
        {
            TaskReassigned,
            CloseTask,
            Continue
        }

        /// <summary>
        /// Accesses the interest screen and updates the borrowers interest rate if loans are valid.
        /// </summary>
        /// <param name="bData">Borrowers Information.</param>
        /// <returns>True if interest rate was updated.  False if not.</returns>
        private Result AccessInterestScreen(BorrowerData bData)
        {
            int timesToHitF8 = 0;
            bool updatedInterest = false;
            RI.FastPath("TX3Z/CTS06");
            if (RI.ScreenCode == "TSX07")
            {
                for (int row = 12; RI.MessageCode != "90007"; row++)
                {
                    if (row > 21 || RI.CheckForText(row, 3, " "))
                    {
                        row = 11;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }
                    DateTime begin = RI.GetText(row, 17, 10).ToDate();
                    DateTime? end = RI.GetText(row, 29, 10).ToDateNullable();
                    
                    if (DateTime.Now > begin && DateTime.Now < (end == null ? DateTime.Now.AddYears(2) : end))
                    {
                        if (RI.GetText(12, 74, 4).ToDecimal() > (decimal)6.00)
                        {
                            bData.EligibleLoanData.Where(p => p.LoanStatus != "DECONVERTED").First().InterestWasUpdated = true;
                            return UpdateInterestScreen(bData.EligibleLoanData.First(), bData.ScraBeginDate, bData.ScraEndDate, string.Empty)
                                ? Result.Continue : Result.TaskReassigned;   
                        }
                    }
                }

                return updatedInterest ? Result.Continue : Result.CloseTask;
            }

            for (int row = 8; RI.MessageCode != "90007"; row++)
            {
                if (RI.CheckForText(row, 4, " ") || row > 20)
                {
                    timesToHitF8++;
                    row = 7;

                    Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                BorrowerLoanData loan = bData.EligibleLoanData.Where(p => p.LoanSeq == RI.GetText(row, 47, 3).ToInt()).SingleOrDefault();
                if (loan != null && RI.GetText(row, 74, 5).ToDouble() > 6.0)
                {
                    updatedInterest = true;
                    loan.InterestWasUpdated = true;
                    string selection = RI.GetText(row, 3, 2);
                    RI.PutText(21, 18, selection, ReflectionInterface.Key.Enter, true);
                    if (!UpdateInterestScreen(loan, bData.ScraBeginDate, bData.ScraEndDate, selection))
                        return Result.TaskReassigned;
                    RI.FastPath("TX3Z/CTS06");
                    for (int time = 0; time != timesToHitF8; time++)
                        RI.Hit(ReflectionInterface.Key.F8);
                }
                else
                    bData.EligibleLoanData.Remove(loan);
            }

            return updatedInterest ? Result.Continue : Result.CloseTask;
        }

        private bool UpdateInterestScreen(BorrowerLoanData loan, DateTime scraBeginDate, DateTime? scraEndDate, string selection, string RDCCode = "M")
        {
            DateTime oldDate = new DateTime(2008, 08, 14);
            DateTime effectiveLoanAdd = RI.GetText(6, 67, 8).ToDate();

            if (effectiveLoanAdd > scraBeginDate && RDCCode != "P")
            {
                if (!UpdateInterestScreen(loan, scraBeginDate, scraEndDate, selection, "P"))
                    return false;

                RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                if (RI.ScreenCode == "TSX05")
                {
                    string sequence = loan.LoanSeq.ToString().PadLeft(3, '0');
                    PageHelper.Iterate(RI, (row, settings) =>
                    {
                        if (RI.CheckForText(row, 47, sequence))
                        {
                            string newSelection = RI.GetText(row, 3, 2);
                            RI.PutText(21, 18, newSelection, ReflectionInterface.Key.Enter, true);
                            settings.ContinueIterating = false;
                        }
                    });
                }
                scraBeginDate = effectiveLoanAdd;
            }

            RI.PutText(11, 6, "6.000", true);
            RI.PutText(11, 14, RDCCode);
            RI.PutText(11, 17, scraBeginDate < oldDate ? oldDate.ToString("MMddyyyy") : scraBeginDate.ToString("MMddyyyy"));
            RI.PutText(11, 29, RDCCode == "P" ? effectiveLoanAdd.AddDays(-1).ToString("MMddyyyy") : scraEndDate.Value.ToString("MMddyyyy"), ReflectionInterface.Key.Enter);

            if (DataAccessHelper.TestMode && (RI.MessageCode == "04236" || RI.MessageCode == "04235")) //this situation sometimes occurs during testing, but will never occur in production
                RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "01145")
            {
                for (int tries = 1; tries <= 4; tries++)//Not sure why this is in the Spec but there may be invalid dates.... I donno
                {
                    RI.PutText(11, 29, RDCCode == "P" ? effectiveLoanAdd.AddDays(-1 - tries).ToString("MMddyyyy") : scraEndDate.Value.AddDays(tries * -1).ToString("MMddyyyy"), ReflectionInterface.Key.Enter);
                    if (RI.MessageCode != "01145")
                        break;
                }
            }

            if (RI.MessageCode != "01005" && RI.MessageCode != "06207")
            {
                CreateTask();
                return false;
            }

            return true;
        }

        private void CreateTask()
        {
            string message = RI.ReAssignQueueTask("MS", "01", UserId, QueueOwner);
            if (!message.Contains("01005"))
            {
                string errorMessage = string.Format("There was an error trying to reassign the MS;01 queue task.  Please review the script will now end.  Error: {0}", message);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                EndDllScript();
            }
            Eoj[EojErrors]++;
        }

        private List<BorrowerLoanData> GatherDataFromTS26(string accountNumber)
        {
            List<BorrowerLoanData> data = new List<BorrowerLoanData>();
            RI.FastPath("TX3Z/ITS26" + accountNumber);
            if (RI.ScreenCode == "TSX28")//Selection Screen
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (RI.CheckForText(row, 3, " ") || row > 20)
                    {
                        row = 7;
                        Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    PutText(21, 12, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                    BorrowerLoanData loan = GetTS26Data();
                    if (loan != null)
                        data.Add(loan);
                    RI.Hit(ReflectionInterface.Key.F12);
                }
            }
            else
            {
                BorrowerLoanData loan = GetTS26Data();
                if (loan != null)
                    data.Add(loan);
            }
            return data;
        }

        private BorrowerLoanData GetTS26Data()
        {
            return new BorrowerLoanData()
                {
                    LoanSeq = RI.GetText(7, 35, 4).ToInt(),
                    DisbDate = RI.GetText(6, 18, 8).ToDate(),
                    LoanStatus = RI.GetText(3, 10, 30),
                    LoanProgram = RI.GetText(6, 66, 6)
                };
        }

        private void AccessQueue()
        {
            RI.FastPath("TX3Z/ITX6XMS;01");
            if (RI.MessageCode == "01020")
            {
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, "There were no tasks to process.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                EndDllScript();
            }
            else if (!RI.CheckForText(1, 74, "TXX71"))
            {
                string message = string.Format("An unexpected error occurred while trying to access ITX6XMS;01.  Please review and try again. Error:{0}", RI.Message);
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                EndDllScript();
            }
        }

        private string GetQueueOwner()
        {
            RI.FastPath("TX3Z/ITX5ZMS;01");
            return RI.GetText(5, 20, 7);
        }

    }
}
