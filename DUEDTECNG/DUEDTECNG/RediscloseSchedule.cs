using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DUEDTECNG
{
    public class RediscloseSchedule
    {
        public ReflectionInterface RI { get; set; }
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }
        private BorrowerData Borrower { get; set; }
        private AppSettings Settings { get; set; }

        public RediscloseSchedule(ReflectionInterface ri, ProcessLogRun plr, DataAccess da, AppSettings settings, BorrowerData borrower)
        {
            RI = ri;
            PLR = plr;
            DA = da;
            Settings = settings;
            Borrower = borrower;
        }

        public bool RediscloseLoans()
        {
            if (!BorrowerExists())
                return false;
            BalanceCheck();
            return RpsProcessing();
        }

        private bool BorrowerExists()
        {
            RI.FastPath("TX3Z/CTS7C" + Borrower.Ssn);
            if (RI.MessageCode == "01527")
            {
                Console.WriteLine("Unable to find borrower {0} in session.", Borrower.AccountNumber);
                return false;
            }
            return true;
        }
        private void BalanceCheck()
        {
            if (Borrower.TotalBalance >= 30000 && Borrower.FirstDisbursement >= new DateTime(1998, 10, 7))
            {
                RI.FastPath("TX3Z/CTS7C" + Borrower.Ssn);
                var procTsx7d = new Action(() =>
                {
                    RI.PutText(14, 48, "Y", ReflectionInterface.Key.Enter);
                    if (RI.MessageCode == "40054")
                        RI.PutText(18, 19, "0", ReflectionInterface.Key.Enter);
                });
                if (RI.ScreenCode == "TSX7D")
                    procTsx7d();
                else if (RI.ScreenCode == "TSX3S")
                {
                    var settings = PageHelper.IterationSettings.Default();
                    settings.MinRow = 7;
                    PageHelper.Iterate(RI, row =>
                    {
                        int? sel = RI.GetText(row, 3, 2).ToIntNullable();
                        if (sel.HasValue)
                        {
                            RI.PutText(22, 19, sel.Value.ToString().PadLeft(2, '0'), ReflectionInterface.Key.Enter);
                            procTsx7d();
                            RI.Hit(ReflectionInterface.Key.F12);
                        }
                        else
                            settings.ContinueIterating = false;
                    }, settings);
                }
            }
        }

        private bool RpsProcessing()
        {
            decimal totalInstallmentAmount = 0;
            var monthlyPayment = GetMonthlyPayment();
            var helper = new Tsx0Helper(RI, scheduleType =>
            {
                var result = Tsx0tProcessing(scheduleType);
                if (!result)
                    return false;
                RI.Hit(ReflectionInterface.Key.F10);
                decimal installmentAmount = RI.GetText(9, 49, 12).ToDecimalNullable() ?? 0;
                RI.Hit(ReflectionInterface.Key.F12);
                totalInstallmentAmount += installmentAmount;
                return true;
            }, StandardBorrowerError);
            if (!helper.Iterate(Borrower.Ssn))
                return false;
            if (totalInstallmentAmount >= monthlyPayment.Amount + Settings.MaxIncrease)
            {
                var message = string.Format("New payment exceeds max increase.  Borrower {0}, Old Payment {1}, New Payment {2}", Borrower.AccountNumber, monthlyPayment.Amount, totalInstallmentAmount);
                PLR.AddNotification(message, NotificationType.EndOfJob, NotificationSeverityType.Warning);
                return true;
            }

            helper = new Tsx0Helper(RI, scheduleType =>
            {
                var result = Tsx0tProcessing(scheduleType);
                if (!result)
                    return false;
                CommitSchedule();
                return true;
            }, StandardBorrowerError);
            return helper.Iterate(Borrower.Ssn);
        }

        private bool Tsx0tProcessing(string scheduleType)
        {
            RI.PutText(8, 14, scheduleType);
            RI.PutText(8, 27, Borrower.DueDate);

            int maximumTermRemaining = RI.GetText(20, 26, 3).ToIntNullable() ?? 0;
            int extendedMaximumTermRemaining = RI.GetText(21, 26, 3).ToIntNullable() ?? 0;
            int calculatedTerm = -1;
            if (scheduleType.IsIn("EL", "EG"))
                calculatedTerm = extendedMaximumTermRemaining;
            else
                calculatedTerm = maximumTermRemaining;
            RI.PutText(9, 23, calculatedTerm.ToString());
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode.IsIn("06701", "02556", "03664", "02553", "40140"))
            {
                RI.Hit(ReflectionInterface.Key.F12);
                RI.Hit(ReflectionInterface.Key.Enter);
                if (scheduleType == "EG")
                    RI.PutText(8, 14, "EL");
                else
                    RI.PutText(8, 14, "L");
                RI.PutText(8, 27, Borrower.DueDate);
                if (scheduleType.IsIn("EL", "EG"))
                    calculatedTerm = extendedMaximumTermRemaining;
                else
                    calculatedTerm = maximumTermRemaining;
                RI.PutText(9, 23, calculatedTerm.ToString());
                RI.Hit(ReflectionInterface.Key.Enter);
            }
            if (RI.MessageCode.IsIn("01840", "02074", "06579", "02079", "04224"))
                return true;
            StandardBorrowerError();
            return false;
        }

        private bool CommitSchedule()
        {
            RI.Hit(ReflectionInterface.Key.F4);
            RI.Hit(ReflectionInterface.Key.F4);
            if (RI.MessageCode == "01832")
                return true;
            StandardBorrowerError();
            return false;
        }

        private MonthlyPayment GetMonthlyPayment()
        {
            RI.FastPath("TX3Z/ITS26" + Borrower.Ssn);
            decimal installmentAmount = 0;
            int loanRepayTerm = 0;
            int balanceAtRepay = 0;
            Action tsx29 = new Action(() =>
            {
                installmentAmount += RI.GetText(19, 43, 10).ToDecimalNullable() ?? 0;
                loanRepayTerm = Math.Max(loanRepayTerm, RI.GetText(20, 43, 3).ToIntNullable() ?? 0);
            });
            if (RI.ScreenCode == "TSX29")
                tsx29();
            else if (RI.ScreenCode == "TSX28")
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 8;
                PageHelper.Iterate(RI, row =>
                {
                    var sel = RI.GetText(row, 2, 2).ToIntNullable();
                    if (sel.HasValue)
                    {
                        var currentBalance = RI.GetText(row, 59, 10).ToDecimalNullable();
                        if (currentBalance > 0)
                        {
                            RI.PutText(21, 12, sel.Value.ToString().PadLeft(2, '0'), ReflectionInterface.Key.Enter);
                            tsx29();
                            RI.Hit(ReflectionInterface.Key.F12);
                        }
                    }
                    else
                        settings.ContinueIterating = false;
                }, settings);
            }
            else
                return null;

            return new MonthlyPayment() { Amount = installmentAmount, RpsTerm = loanRepayTerm, BalanceAtRepayment = balanceAtRepay };
        }

        private void StandardBorrowerError()
        {
            var severity = NotificationSeverityType.Critical;
            if (RI.MessageCode.IsIn("02073", "02077", "02078", "02081", "02877", "03583", "03862"))
                severity = NotificationSeverityType.Warning;
            PLR.AddNotification(string.Format("Unable to process borrower {0}.  {1}", Borrower.AccountNumber, RI.Message), NotificationType.ErrorReport, severity);
        }
    }
}
