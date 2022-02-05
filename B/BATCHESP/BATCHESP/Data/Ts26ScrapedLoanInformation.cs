using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace BATCHESP
{
    public class Ts26ScrapedLoanInformation
    {
        public string BorrowerSsn { get; set; }
        public int LoanSequence { get; set; }
        public string LoanStatus { get; set; }
        public DateTime? RepaymentStartDate { get; set; }

        public static List<Ts26ScrapedLoanInformation> ScrapeTs26(ReflectionInterface ri, string borrowerSsn)
        {
            var results = new List<Ts26ScrapedLoanInformation>();
            ri.FastPath("tx3z/ITS26" + borrowerSsn);
            if (ri.ScreenCode == "TSX28")
            {
                PageHelper.Iterate(ri, (row, settings) =>
                {
                    var sel = ri.GetText(row, 2, 2).ToIntNullable();
                    var currentBalance = ri.GetText(row, 59, 10).ToDecimalNullable() ?? 0;
                    if (!sel.HasValue)
                        settings.ContinueIterating = false;
                    else
                    {
                        ri.PutText(21, 12, sel.Value.ToString().PadLeft(2, '0'));
                        ri.Hit(Key.Enter);
                        results.Add(ScrapeTsx29(ri, borrowerSsn));
                        ri.Hit(Key.F12);
                    }
                });
            }
            else if (ri.ScreenCode == "TSX29")
                results.Add(ScrapeTsx29(ri, borrowerSsn));
            return results;
        }
        private static Ts26ScrapedLoanInformation ScrapeTsx29(ReflectionInterface ri, string borrowerSsn)
        {
            var result = new Ts26ScrapedLoanInformation() { BorrowerSsn = borrowerSsn };
            result.LoanSequence = ri.GetText(7, 35, 4).ToInt();
            result.LoanStatus = ri.GetText(3, 10, 40);
            result.RepaymentStartDate = ri.GetText(17, 44, 8).ToDateNullable();
            return result;

        }
    }
}

