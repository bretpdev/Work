using System;
using System.Collections.Generic;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;
using static Uheaa.Common.Dialog;

namespace SMBALWO
{
    class SmallBalanceWriteOff
    {
        private DataAccess DA { get; set; }
        private ReflectionInterface RI { get; set; }
        private string UserId { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public bool LoadDataNoProcess { get; set; }

        public SmallBalanceWriteOff(ProcessLogRun logRun, bool loadData = false)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun);
            WriteLine("Pulling accounts to process. This could take a while");
            if (loadData) // For testing purposes, we might not want to always load the data
                DA.LoadAccountsFromWarehouse();
            LoadDataNoProcess = loadData;
        }

        public int Process()
        {
            if (LoadDataNoProcess && DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev)
                return 0;
            if (!LogIntoSession())
                return 1;
            UserId = RI.UserId;

            List<BorrowerData> tilpBorrowers = DA.LoadAndPullWriteOffData(true);

            foreach (BorrowerData borrower in tilpBorrowers)
                ProcessBorrower(borrower);

            List<BorrowerData> borrowers = DA.LoadAndPullWriteOffData(false);

            foreach (BorrowerData borrower in borrowers)
                ProcessBorrower(borrower);

            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev && Question.YesNo("Processing Complete. Do you want to review the processing report?"))
                System.Diagnostics.Process.Start(EnterpriseFileSystem.GetPath("smbalworeport"));

            RI.CloseSession();
            return 0;
        }

        private void ProcessBorrower(BorrowerData borrower)
        {
            WriteLine($"Verifying loan sequence {borrower.LoanSequence} for borrower: {borrower.AccountNumber}");
            if (!VerifyBalance(borrower))
                return;
            if (VerifyLoans(borrower))
                return;
            if (!CheckFinancialTransactions(borrower))
                return;

            WriteLine("Processing write off for borrower: {0}", borrower.AccountNumber);
            ProcessWriteOff(borrower);
        }

        private void ProcessWriteOff(BorrowerData borrower)
        {
            DateTime? effDate = DA.GetEffectiveDate(borrower);
            if (!effDate.HasValue)
            {
                LogError(borrower, $"The effective date was not found for borrower: {borrower.AccountNumber}.");
                return;
            }
            RI.FastPath($"TX3Z/ATS3Q{borrower.AccountNumber}{effDate.Value:MMddyy}");
            if (RI.MessageCode == "01527")
            {
                LogError(borrower, "The borrower is not eligible for a write off");
                return;
            }
            if (RI.ScreenCode == "TSX3S")
            {
                for (int row = 7; RI.MessageCode != "90007"; row++)
                {
                    string loanSeq = RI.GetText(row, 20, 4);
                    if (row > 21 || loanSeq.IsNullOrEmpty())
                    {
                        row = 6;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    if (borrower.LoanSequence == loanSeq.ToInt())
                    {
                        RI.PutText(22, 19, RI.GetText(row, 3, 3), ReflectionInterface.Key.Enter, true);
                        if (RI.ScreenCode == "TSX3S")
                        {
                            LogError(borrower, $"Was not able to write off Borrower: {borrower.AccountNumber}; Session Message:{RI.Message}.");
                            return;
                        }
                        if (!ProcessATS3Q(borrower))
                            return;

                        RI.Hit(ReflectionInterface.Key.F12);
                    }
                }
            }
            else
                ProcessATS3Q(borrower);
        }

        private bool ProcessATS3Q(BorrowerData borrower)
        {
            decimal principal = RI.GetText(12, 31, 11).ToDecimal();
            decimal interest = RI.GetText(13, 31, 11).ToDecimal();
            if (RI.CheckForText(13, 42, "CR"))
            {
                LogError(borrower, $"The interest is showing a credit in ATS3Q for borrower: {borrower.AccountNumber}, Loan Seq: {borrower.LoanSequence}.");
                return false;
            }
            int balanceCheck = (borrower.IsTilp ? 50 : 25);
            decimal total = principal + interest;
            if (total == 0 || total >= balanceCheck)
            {
                string error = total == 0 ? "is $0" : $"is greater than ${balanceCheck}";
                LogError(borrower, $"The balance for borrower: {borrower.AccountNumber}, loan seq: {borrower.LoanSequence} {error}. Please review");
                return false;
            }
            RI.PutText(8, 42, "X");
            RI.PutText(9, 45, "Z");
            if (principal > 0)
                RI.PutText(12, 48, principal.ToString());
            if (interest > 0)
                RI.PutText(13, 49, interest.ToString());

            string comment = $"Small balance write-off of ${principal} in principal and ${interest} in interest for loan seq {borrower.LoanSequence} / {UserId}";

            if (comment.Length > 77)
            {
                List<string> seqmentedComment = new List<string>();
                int take = 77;
                for (int skip = 0; skip < comment.Length; skip += 77)
                    seqmentedComment.Add(comment.SafeSubString(skip, take));

                for (int row = 18, index = 0; index < seqmentedComment.Count; row++, index++)
                    RI.PutText(row, 2, seqmentedComment[index]);
            }
            else
                RI.PutText(18, 2, comment);

            RI.Hit(ReflectionInterface.Key.F11);
            if (RI.MessageCode == "02238")
            {
                DA.MarkRecordProcessed(borrower.LoanWriteOffId, false, principal, interest);
                return true;
            }

            LogError(borrower, $"Unable to update ATS3Q Session Message: {RI.Message}; Borrower: {borrower.AccountNumber}");
            return false;
        }

        private bool CheckFinancialTransactions(BorrowerData borrower)
        {
            string trans = DA.GetMaxFinancialTransaction(borrower);
            if (trans.IsIn("1040", "1041", "1045", "1048", "0786"))
            {
                LogError(borrower, $"Borrower: {borrower.AccountNumber}; Loan Sequence: {borrower.LoanSequence} needs to be reviewed because of the most recent financial transaction.");
                return false;
            }

            return true;
        }

        private bool VerifyLoans(BorrowerData borrower)
        {
            bool needsReviewBalance = false;
            bool needsReviewCredit = false;

            RI.FastPath($"TX3Z/ITS26{borrower.AccountNumber}");
            if (RI.ScreenCode == "TSX28")
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (RI.GetText(row, 3, 1).IsNullOrEmpty() || row > 20)
                    {
                        row = 7;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }
                    if ((RI.GetText(row, 48, 11) == RI.GetText(row, 59, 11)) && (RI.GetText(row, 48, 11).ToDecimal() != 0 || RI.GetText(row, 59, 11).ToDecimal() != 0))
                        needsReviewBalance = true;
                    if (RI.GetText(row, 59, 12).Contains("CR"))
                        needsReviewCredit = true;
                }
            }
            else
            {
                if (RI.GetText(11, 11, 14) == RI.GetText(11, 35, 14))
                    needsReviewBalance = true;
                if (RI.GetText(11, 11, 16).Contains("CR"))
                    needsReviewCredit = true;
            }

            if (needsReviewBalance)
                LogError(borrower, $"Original Balance = Current Balance for Borrower: {borrower.AccountNumber} Loan Sequence {borrower.LoanSequence}. Please review");
            if (needsReviewCredit)
                LogError(borrower, $"Loan Sequence: {borrower.LoanSequence} for Borrower: {borrower.AccountNumber} has a credit.  Please review.");

            return needsReviewBalance | needsReviewCredit;
        }

        private bool VerifyBalance(BorrowerData borrower)
        {
            RI.FastPath("TX3Z/ITS26*");
            RI.PutText(8, 40, borrower.AccountNumber, ReflectionInterface.Key.Enter);
            if (RI.ScreenCode == "TSX28")
            {
                if (!CheckTotalBalance(borrower))
                    return false;
            }
            else if (RI.ScreenCode == "TSX29")
            {
                if (!CheckTotalBalance(borrower))
                    return false;
            }
            else
            {
                LogError(borrower, $"Expected to be on screen TSX28 or TSX29 on TS26 but was on screen {RI.ScreenCode}; Session Message: {RI.Message}.");
                return false;
            }

            return true;
        }

        private double GetTotalBalance(bool tilpOnly)
        {
            double totalBal = 0;
            int count = 1;
            for (int row = 8; RI.MessageCode != "90007"; row++)
            {
                if (RI.GetText(row, 3, 1).IsNullOrEmpty() || row > 20)
                {
                    row = 7;
                    RI.Hit(ReflectionInterface.Key.F8);
                    count = 1;
                    continue;
                }
                if ((tilpOnly && RI.GetText(row, 19, 6) == "TILP") || (!tilpOnly && RI.GetText(row, 19, 6) != "TILP"))
                {
                    RI.PutText(21, 12, $"{count}", ReflectionInterface.Key.Enter, true);
                    totalBal += GetBalance().Value;
                    RI.Hit(ReflectionInterface.Key.F12, 2);
                    count++;
                }
            }

            return totalBal;
        }

        private bool CheckTotalBalance(BorrowerData borrower)
        {
            double balanceToCheck = borrower.IsTilp ? 49.99 : 24.99;

            double? totalBalance;
            if (RI.ScreenCode == "TSX29")
                totalBalance = GetBalance();
            else
                totalBalance = GetTotalBalance(borrower.IsTilp);

            if (!totalBalance.HasValue)
            {
                LogError(borrower, $"Unable to determine borrowers total loan balance on ITS26.  Borrower: {borrower.AccountNumber};");
                return false;
            }
            else if (totalBalance.Value > balanceToCheck)//the write off limit is $25 for non tilp, $50 for tilp
            {
                LogError(borrower, $"Borrowers total balance is greater than {balanceToCheck}. Please review. Borrower: {borrower.AccountNumber};");
                return false;
            }
            else if (totalBalance <= 0)
            {
                LogError(borrower, $"The borrowers total balance is less than or equal to $0. Please review. Borrower: {borrower.AccountNumber}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds the balance from TSX29 with the accrued interest in TSX2A
        /// </summary>
        private double? GetBalance()
        {
            double? totalBalance;
            totalBalance = RI.GetText(11, 12, 10).ToDouble();
            RI.Hit(ReflectionInterface.Key.Enter);
            totalBalance += RI.GetText(15, 65, 10).ToDouble();
            return totalBalance;
        }

        private void LogError(BorrowerData borrower, string message, Exception ex = null)
        {
            LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            DA.MarkRecordProcessed(borrower.LoanWriteOffId, true);
        }

        private bool LogIntoSession(BatchProcessingHelper login = null, int timesTried = 0)
        {
            WriteLine("Getting User Id.");
            RI = new ReflectionInterface();
            BatchProcessingLoginHelper.Login(LogRun, RI, Program.ScriptId, "BatchUheaa");
            return RI.IsLoggedIn;
        }
    }
}