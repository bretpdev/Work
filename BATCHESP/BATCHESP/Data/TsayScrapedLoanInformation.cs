using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace BATCHESP
{
    /// <summary>
    /// Stores scraped information from the TSAY page in Session.
    /// </summary>
    public class TsayScrapedLoanInformation
    {
        public string BorrowerSsn { get; set; }
        public int LoanSequence { get; set; }
        public string LoanProgramType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CertificationDate { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string DeferSchool { get; set; }
        public string ApprovalStatus { get; set; }
        public string DfType { get; set; } // The type of the defer/forb; e.g.: "D45", "F02"
        public DateTime AppliedDate { get; set; } 

        /// <summary>
        /// Gathers deferment/forbearance data from TSAY in Session.
        /// </summary>
        public static List<TsayScrapedLoanInformation> ScrapeTsay(ReflectionInterface ri, string borrowerSsn)
        {
            var allDfInfo = new List<TsayScrapedLoanInformation>();
            ri.FastPath("tx3z/ITSAY" + borrowerSsn);
            if (ri.ScreenCode == "TSXAZ")
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 9;
                settings.MaxRow = 21;
                PageHelper.Iterate(ri, (row, s) =>
                {
                    var sel = ri.GetText(row, 2, 2).ToIntNullable();
                    if (!sel.HasValue)
                        s.ContinueIterating = false;
                    else
                    {
                        ri.PutText(22, 17, sel.Value.ToString().PadLeft(2, '0'));
                        ri.Hit(Key.Enter);
                        ScrapeTsxb0(ri, allDfInfo, borrowerSsn);
                        ri.Hit(Key.F12);
                    }
                }, settings);
            }
            else if (ri.ScreenCode == "TSXB0")
            {
                ScrapeTsxb0(ri, allDfInfo, borrowerSsn);
            }
            return allDfInfo;
        }

        /// <summary>
        /// Records deferment/forbearance data for each d/f on each loan.
        /// </summary>
        private static void ScrapeTsxb0(ReflectionInterface ri, List<TsayScrapedLoanInformation> allDfInfo, string borrowerSsn)
        {
            if (ri.ScreenCode == "TSXB0")
            {
                var settings = PageHelper.IterationSettings.Default();
                settings.MinRow = 9;
                settings.MaxRow = 21;
                PageHelper.Iterate(ri, (row, s) =>
                {
                    var sel = ri.GetText(row, 2, 2).ToIntNullable();
                    if (!sel.HasValue)
                        s.ContinueIterating = false;
                    else
                    {
                        var dfInfo = new TsayScrapedLoanInformation();
                        dfInfo.BorrowerSsn = borrowerSsn;
                        dfInfo.LoanSequence = ri.GetText(4, 44, 3).ToInt();
                        dfInfo.LoanProgramType = ri.GetText(4, 10, 6).Trim().ToString();
                        dfInfo.DisbursementDate = ri.GetText(4, 27, 8).ToDate();
                        dfInfo.ApprovalStatus = ri.GetText(row, 5, 8).Trim().ToString();
                        dfInfo.CertificationDate = ri.GetText(row, 15, 8).ToDateNullable();
                        dfInfo.BeginDate = ri.GetText(row, 30, 8).ToDate();
                        dfInfo.EndDate = ri.GetText(row, 40, 8).ToDate();
                        dfInfo.DfType = ri.GetText(row, 25, 3);
                        dfInfo.AppliedDate = ri.GetText(row, 72, 8).ToDate();

                        // Now step into selection to retrieve school code from TSXB1, then step out
                        ri.PutText(22, 14, sel.Value.ToString().PadLeft(2, '0'));
                        ri.Hit(Key.Enter);
                        dfInfo.DeferSchool = ri.GetText(8, 39, 8).Trim();
                        ri.Hit(Key.F12);

                        // Now add results to list of all def/forb info for bwr
                        allDfInfo.Add(dfInfo);
                    }
                }, settings);
            }
        }

    }
}
