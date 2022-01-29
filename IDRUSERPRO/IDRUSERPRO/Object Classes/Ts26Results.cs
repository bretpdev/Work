using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace IDRUSERPRO
{
    public class Ts26Results
    {
        public static readonly string[] InvalidLoans = new string[] { "DLPLUS", "DLPCNS", "PLUS", "TILP", "COMPLT" };
        public ReflectionInterface RI { get; set; }
        public List<Ts26Loans> Loans { get; set; }
        public Ts26Status Status { get; set; }

        public Ts26Results(ReflectionInterface ri)
        {
            RI = ri;
            Loans = new List<Ts26Loans>();
        }


        /// <summary>
        /// Goes to TS26 and gathers all loan information for a given account number
        /// </summary>
        /// <param name="accountNumber">Account Number for borrower</param>
        /// <param name="misRoutedApp">If this value is true then if a borrower does not have any loan that will not be considered and error.</param>
        /// <returns>An object with Loan Type, Seq Disb Date and Award Id</returns>
        public void LoadLoanDataFromTs26(string accountNumber, bool misRoutedApp)
        {
            List<Ts26Loans> loans = new List<Ts26Loans>();
            RI.FastPath("TX3Z/ITS26*");
            RI.PutText(8, 40, accountNumber, ReflectionInterface.Key.Enter);
            //Borrower only has 1 loan
            if (RI.ScreenCode.Contains("TSX29"))
                TS26Target();
            else if (RI.ScreenCode.Contains("T1X07"))
            {
                if (!misRoutedApp)
                {
                    Status = Ts26Status.BorrowerNotFound;
                }
            }
            else
            {
                bool hasLoans = SelectionTs26();
                if (!hasLoans)
                    Status = Ts26Status.ZeroBalance;

                if (hasLoans && !Loans.Any())
                    Status = Ts26Status.AllTilpLoans;
            }
        }

        private void TS26Target()
        {
            bool hasBalance = HasBalance(11, 12);
            if (!hasBalance)
                Status = Ts26Status.ZeroBalance;
            string loanType = RI.GetText(6, 66, 6);

            string loanSeq = RI.GetText(7, 35, 4);
            RI.Hit(ReflectionInterface.Key.Enter);
            Loans.Add(GetLoanData(loanType, loanSeq, hasBalance));
        }

        private bool SelectionTs26()
        {
            bool hasLoans = false;
            //borrower has multiple loans.
            for (int row = 8; !RI.CheckForText(23, 2, "90007"); row++)
            {
                if (RI.CheckForText(row, 2, "  ") || row > 19)
                {
                    row = 7;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                var hasBalance = HasBalance(row, 59);


                RI.PutText(21, 12, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);
                string loanType = RI.GetText(6, 66, 6);

                string loanSeq = RI.GetText(7, 35, 4);
                RI.Hit(ReflectionInterface.Key.Enter);
                if (hasBalance)
                    hasLoans = true;
                Loans.Add(GetLoanData(loanType, loanSeq, hasBalance));
                RI.Hit(ReflectionInterface.Key.F12);
                RI.Hit(ReflectionInterface.Key.F12);
            }
            return hasLoans;
        }

        /// <summary>
        /// Checks to see if given coordinates for a loan has a balance.  Screen TS26
        /// </summary>
        /// <param name="row">Row of loan to check</param>
        /// <param name="col">Column of loan to check</param>
        /// <returns>Returns true if borrower has a balance, Treats credits and false</returns>
        private bool HasBalance(int row, int col)
        {
            if (RI.GetText(row, col, 12).Contains("CR"))
                return false;

            return decimal.Parse(RI.GetText(row, col, 11).Replace(",", "")) != 0;
        }

        /// <summary>
        /// Creates a TS26Loans Object from data pulled from 2nd page of TS26
        /// </summary>
        /// <param name="loanType">Loan Type gathered from first page of TS26</param>
        /// <param name="loanSeq"Loan Deq gahtered from the first page of TS26></param>
        /// <returns>TS26 object with LoanType, AwardId LoanSeq and Disbursement  Date</returns>
        private Ts26Loans GetLoanData(string loanType, string loanSeq, bool hasBalance)
        {
            return new Ts26Loans()
            {
                LoanType = loanType,
                AwardId = RI.GetText(8, 17, 22) + RI.GetText(8, 56, 3).PadLeft(3, '0'),
                LoanSeq = loanSeq,
                DisbDate = RI.GetText(4, 45, 8),
                IsEligible = !loanType.IsIn(InvalidLoans),
                HasBalance = hasBalance
            };
        }
    }

    public enum Ts26Status
    {
        Normal,
        ZeroBalance,
        NoLoans,
        BorrowerNotFound,
        AllTilpLoans
    }

}