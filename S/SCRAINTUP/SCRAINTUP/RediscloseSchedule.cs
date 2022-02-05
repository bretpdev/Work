using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace SCRAINTUP
{
    class RediscloseSchedule
    {
        private ReflectionInterface RI { get; set; }
        private string UserId { get; set; }
        private string QueueOwner { get; set; }

        public RediscloseSchedule(ReflectionInterface ri, string userId, string queueOwner)
        {
            RI = ri;
            UserId = userId;
            QueueOwner = queueOwner;
        }

        /// <summary>
        /// Re discloses the borrower repayment schedules where an interest rate update occured.
        /// </summary>
        /// <param name="ssn">Borrowers SSN</param>
        /// <param name="validLoans">Loans that received interest rate update</param>
        /// <returns>true if was successful false if not.</returns>
        public bool RediscloseLoans(string ssn, List<BorrowerLoanData> validLoans)
        {
            //We do not want to re-disclose t this type.
            if (validLoans.Where(p => p.ScheduleType == "FS").Count() > 0)
                return false;

            RI.FastPath("TX3Z/ATS0N" + ssn);
            if (RI.ScreenCode == "TSX0P")
                return TSX0PProcessing(validLoans);
            else if (RI.ScreenCode == "TSX0S")
                return TSX0SProcessing(validLoans);
            else
                return false;
        }

        /// <summary>
        /// Processes from TSX0P.
        /// </summary>
        /// <returns>True if disclosure was successful False if not.</returns>
        private bool TSX0PProcessing(List<BorrowerLoanData> validLoans)
        {
            for (int row = 8; RI.MessageCode != "90007"; row++)
            {
                if (row > 20 || RI.CheckForText(row, 5, " "))
                {
                    row = 7;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                string program = RI.GetText(row, 11, 12);

                List<BorrowerLoanData> loans = validLoans.Where(p => p.LoanProgram == program).ToList();
                if (loans.Count > 0)
                {
                    RI.PutText(21, 12, RI.GetText(row, 4, 3), ReflectionInterface.Key.Enter, true);
                    if (RI.ScreenCode == "TSX0S")
                        if (!TSX0SProcessing(validLoans))
                            return false;
                    else if (RI.ScreenCode == "TSX0R")
                        if (!TSX0RProcessing(validLoans))
                            return false;
                    else
                        return false;

                    RI.Hit(ReflectionInterface.Key.F12);
                }
            }
            return true;
        }

        /// <summary>
        /// Processes from TSX0S.
        /// </summary>
        /// <returns>True if disclosure was successful False if not.</returns>
        private bool TSX0SProcessing(List<BorrowerLoanData> validLoans)
        {
            for (int row = 11; RI.MessageCode != "90007"; row++)
            {
                if (row > 22 || RI.CheckForText(row, 3, " "))
                {
                    row = 10;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                RI.PutText(row, 3, "X", ReflectionInterface.Key.Enter);
                if (!TSX0RProcessing(validLoans))
                    return false;
                RI.Hit(ReflectionInterface.Key.F12);
                if (RI.CheckForText(row, 3, "X"))
                    RI.PutText(row, 3, " ");
            }

            return true;
        }

        /// <summary>
        /// Processes from TSX0R.
        /// </summary>
        /// <returns>True if disclosure was successful False if not.</returns>
        private bool TSX0RProcessing(List<BorrowerLoanData> validLoans)
        {
            for (int row = 10; RI.MessageCode != "90007"; row++)
            {
                if (row > 23 || RI.CheckForText(row, 3, " "))
                {
                    row = 9;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                DateTime disbDate = RI.GetText(row, 5, 10).ToDate();
                List<BorrowerLoanData> loansForThisSchedule = validLoans.Where(p => p.DisbDate == disbDate).ToList();
                if (loansForThisSchedule.Any())
                {
                    string scheduleType = RI.GetText(row, 48, 3);
                    RI.Hit(ReflectionInterface.Key.Enter);
                    if (RI.MessageCode == "02875")
                        return false;

                    if (!TSX0TProcessing(loansForThisSchedule, scheduleType))
                        return false;
                    RI.Hit(ReflectionInterface.Key.F12);
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Processes from TSX0T.
        /// </summary>
        /// <returns>True if disclosure was successful False if not.</returns>
        private bool TSX0TProcessing(List<BorrowerLoanData> loans, string scheduleType)
        {
            RI.PutText(8, 14, scheduleType);
            RI.PutText(9, 23, RI.GetText(20, 26, 3), ReflectionInterface.Key.Enter);
            if (RI.MessageCode != "01840" && RI.MessageCode != "06579")
            {
                return false;
            }

            decimal newInstallmentAmount = GetNewPaymentAmount();
            decimal am = loans.Sum(p => p.PaymentAmount);
            decimal difference = Math.Abs(newInstallmentAmount - loans.Sum(p => p.PaymentAmount));
            if (difference > (decimal)100.00)
            {
                return false;
            }

            RI.Hit(ReflectionInterface.Key.F4);
            RI.Hit(ReflectionInterface.Key.F4);

            if (RI.MessageCode != "01832")
                return false;

            return true;
        }

        /// <summary>
        /// Cycle though all loans in the disclosure to sum there installment amounts.
        /// </summary>
        /// <returns>Sum of all the installment amounts.</returns>
        private decimal GetNewPaymentAmount()
        {
            RI.Hit(ReflectionInterface.Key.F10);

            decimal installmentAmount = RI.GetText(9, 50, 12).ToDecimal();
            RI.Hit(ReflectionInterface.Key.F12);

            return installmentAmount;
        }

    }
}
