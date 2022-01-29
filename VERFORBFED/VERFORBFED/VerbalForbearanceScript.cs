using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace VERFORBFED
{
    public class VerbalForbearanceScript
    {
        public static DataAccess DA { get; set; }
        private ReflectionInterface RI { get; set; }
        private string AccountNumber { get; set; }
        private string Ssn { get; set; }
        private bool IsCollectionSuspense { get; set; }
        EligibilityVerification Ver { get; set; }
        DateTime today;
        private string IntentComment
        {
            get
            {
                return Ver.HasSpousalLoans ? "Borrower/Co Borrower has acknowledged willingness to repay debt" : "Borrower has acknowledged willingness to repay debt";
            }

        }


        public VerbalForbearanceScript(ReflectionInterface ri, string accountNumber, ProcessLogRun PLR)
        {
            //LogDataAccess lda = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, true, false);
            DA = new DataAccess(PLR, accountNumber);
            AccountNumber = accountNumber;
            Ssn = DA.Ssn;
            RI = ri;
            today = DateTime.Now;
            Ver = new EligibilityVerification(Ssn, AccountNumber, DA, RI, today);
        }

        public int Run()
        {
            string forbType = "Verbal";
            using (ProcessingSelection sel = new ProcessingSelection())
            {
                if (sel.ShowDialog() == DialogResult.Cancel)
                    return 0;
                IsCollectionSuspense = sel.IsCollectionSuspense;
                if (IsCollectionSuspense)
                    forbType = "Collection Suspension";
            }

            var helper = new StatusHelper();
            if (helper.AllStatusesMatchCollectionSuspensionDenied(Ver.LoanStatuses))
            {
                string statusList = string.Join(",", Ver.LoanStatuses.Select(o => o.WX_OVR_DW_LON_STA).Distinct());
                string errorMessage = "The {1} forbearance has been denied because the borrower's loans are in {0} status";
                errorMessage = string.Format(errorMessage, statusList, forbType);
                new CustomMessageBoxDenial(errorMessage).ShowDialog();
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "FBDNY",
                    Comment = forbType + " forbearance denied. All loans in " + statusList,
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                ArcAddResults result = arc.AddArc();
                if (!result.ArcAdded)
                    new CustomMessageBoxDenial(string.Format("Unable to add the FBDNY ARC for the {0} denial.  Please request the denial manually", forbType)).ShowDialog();
                return 0;
            }

            if (!Ver.CheckStatuses(!IsCollectionSuspense, IsCollectionSuspense ? "Collection Suspension forbearance" : "Verbal forbearance"))
                return 0;
            if (IsCollectionSuspense)
            {
                ProcessCollectionSuspense();
                return 0;
            }

            if (Ver.LoanPrograms.Any() && Ver.LoanPrograms.All(o => o == "TILP"))
            {
                string errorMessage = "The Verbal forbearance has been denied because the borrower is a TILP borrower";
                new CustomMessageBoxDenial(errorMessage).ShowDialog();
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "FBDNY",
                    Comment = "Verbal forbearance denied. All loans are TILP",
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                ArcAddResults result = arc.AddArc();
                if (!result.ArcAdded)
                    new CustomMessageBoxDenial("Unable to add the FBDNY ARC for the verbal forbearance suspension denial.  Please request the denial manually").ShowDialog();
                return 0;
            }

            ForbearanceDetails forbDetails = new ForbearanceDetails(Ssn, AccountNumber, DA, RI, today);
            forbDetails.SetForbearanceStartDate(Ver.DefermentEndDate, Ver.ForbearanceEndDate, Ver.HasSpecialCase);

            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
            {
                if (Ver.LoanStatuses.All(o => o.WC_DW_LON_STA.IsIn("07", "08", "09", "10", "11", "12")))
                {
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        Arc = "XFORB",
                        Comment = (forbDetails.StartDate ?? DateTime.MinValue).ToShortDateString() + "," + (Ver.LoanStatuses.FirstOrDefault() ?? new LoanStatusInfo()).WX_OVR_DW_LON_STA + ",Borrower is 270+ delq requesting a one-time 120 day forbearance due to financial difficulty.  Borrower has acknowledged willingness to repay debt.",
                        ScriptId = Program.ScriptId,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                    };
                    ArcAddResults result = arc.AddArc();
                    new CustomMessageBoxDenial("The forbearance could not be applied because all loan statuses are in a claim status. The request has been forwarded for review").ShowDialog();
                    return 0;
                }
            }

            if (forbDetails.InvalidEndDate)
            {
                new CustomMessageBoxDenial("Borrower is currently in a forbearance that ends at the end of a quarter. The request has been forwarded on for review.").ShowDialog();
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "XFORB",
                    Comment = "Borrower is currently in a forbearance that ends at the end of a quarter.",
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                ArcAddResults result = arc.AddArc();
                return 0;
            }

            using (UserForm frm = new UserForm(forbDetails, today, Ver.HasSpecialCase, Ver.HasSpousalLoans, IntentComment))
            {
                if (frm.ShowDialog() == DialogResult.Cancel)
                    return 1;

                if (!Ver.LoanStatuses.Any() || string.IsNullOrEmpty(Ssn))
                {
                    string comment = "Borrower cannot be processed with the script.  The request has been forward for manual review";
                    new CustomMessageBoxDenial(comment).ShowDialog();
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        Arc = "XFORB",
                        Comment = comment,
                        ScriptId = Program.ScriptId,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                    };
                    ArcAddResults result = arc.AddArc();
                    return 0;
                }

                DateTime? nextPayDue = DA.GetNextDueDate();
                forbDetails = frm.ForbData;
                forbDetails.CalculateForbearanceEndDate(nextPayDue, Ver.HasSpecialCase);
            }

            if (Ver.IsClaim)
            {
                new CustomMessageBoxDenial("The forbearance could not be applied because the borrower has a Claim status. The request has been forwarded for review").ShowDialog();
                Ver.AddReviewComment(string.Format("{0:MM/dd/yyyy} All loans in CLAIM PENDING or CLAIM SUBMITTED Borrower is 270+ delq requesting a one-time 120 day forbearance due to financial difficulty. Borrower acknowledged intent to repay this debt.", forbDetails.StartDate));
                return 1;
            }

            if (Ver.ReviewBecauseOfDefOfrb)
            {
                new CustomMessageBoxDenial("The forbearance could not be applied because the borrower has at least one loan in a Deferment/Forbearance and Repayment status.  The request has been forwarded for review").ShowDialog();
                Ver.AddReviewComment(string.Format("{0:MM/dd/yyyy}  Borrower has at least 1 loan in Deferment/Forbearance and Repayment status Borrower acknowledged intent to repay this debt.", forbDetails.StartDate));
                return 1;
            }

            if (Ver.IsPaidAhead)
            {
                Ver.ProcessPaidAhead(forbDetails.NumberOfMonthsRequested.ToString(), IntentComment, forbDetails.StartDate.Value, forbDetails.ForbearanceReason);
                return 1;
            }

            if (Ver.CheckForMultipleSchedules(forbDetails.NumberOfMonthsRequested.ToString(), IntentComment, forbDetails.StartDate.Value, forbDetails.ForbearanceReason))
                return 1;

            if (Ver.CheckMultipleStatuses(forbDetails))
                return 1;

            if (Ver.CheckCollectionSuspenseForbearance(forbDetails))
                return 1;

            if (Ver.CheckNumberOfMonthsUsed(forbDetails))
                return 1;

            AddTheForbearance(forbDetails, Ssn, Ver.HasSpecialCase, Ver.HasSpousalLoans);

            return 0;
        }

        private void ProcessCollectionSuspense()
        {
            if (Ver.LoanPrograms.Any() && Ver.LoanPrograms.All(o => o == "TILP"))
            {
                string errorMessage = "The Collect Suspension forbearance has been denied because the borrower is a TILP borrower";
                new CustomMessageBoxDenial(errorMessage).ShowDialog();
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "FBDNY",
                    Comment = "Collection Suspension forbearance denied. All loans are TILP",
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                ArcAddResults result = arc.AddArc();
                if (!result.ArcAdded)
                    new CustomMessageBoxDenial("Unable to add the FBDNY ARC for the collection suspension denial.  Please request the denial manually").ShowDialog();
                return;
            }
            if (Ver.LoanStatuses.All(o => o.WC_DW_LON_STA.IsIn("08", "09", "10", "11", "12")))
            {
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "FBDNY",
                    Comment = "Collection Suspension forbearance denied.  All loans are in a claim status.",
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                ArcAddResults result = arc.AddArc();
                new CustomMessageBoxDenial("The Collection Suspension forbearance could not be applied because all loan statuses are in a claim status. The request has been denied.").ShowDialog();
                return;
            }
            using (CollectionSuspenseReason rea = new CollectionSuspenseReason())
            {
                if (rea.ShowDialog() != DialogResult.OK)
                {
                    Run();
                    return;
                }
                if (!Ver.LoanStatuses.Any() || string.IsNullOrEmpty(Ssn))
                {
                    string comment = "Borrower cannot be processed with the script.  The request has been forward for manual review";
                    new CustomMessageBoxDenial(comment).ShowDialog();
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        Arc = "XFORB",
                        Comment = comment,
                        ScriptId = Program.ScriptId,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                    };
                    ArcAddResults result = arc.AddArc();
                    return;
                }
                ProcessCollectionForb(rea.Reason);
            }
        }

        private void ProcessCollectionForb(CollectionSuspenseReason.SelectedReason reason)
        {
            bool HasSpousalLoans = DA.CheckSpousalLoans(Ssn);
            if (Ver.LoanStatuses.Any(o => o.WC_DW_LON_STA == "07"))
            {
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "XFORB",
                    Comment = "Manual Review, Claim pending status. Borrower requests Collection Suspension Forbearance. Borrower is sending " + reason.ToString(),
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                ArcAddResults result = arc.AddArc();
                new CustomMessageBoxDenial("Request Has been forwarded for review due to claim pending status.").ShowDialog();
                return;

            }
            if (DA.BorrowerUsedCollectionSuspensionInLast30Days())
            {
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "XFORB",
                    Comment = string.Format("Manual Review, Consecutive Request. Borrower requests Collection Suspension Forbearance. Borrower is sending {0}", reason),
                    ScriptId = Program.ScriptId,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                };
                ArcAddResults result = arc.AddArc();
                if (!result.ArcAdded)
                    new CustomMessageBoxDenial("Unable to add the XFORB ARC for the collection suspension forbearance.  Please request the Forbearance manually").ShowDialog();
                else
                    new CustomMessageBoxDenial("Request Has been forwarded for review due to Consecutive Request").ShowDialog();
            }
            else
            {
                RI.FastPath("TX3Z/ATS0H");
                var start = today.ToString("MMddyy");
                var end = today.AddDays(59).ToString("MMddyy");
                RI.PutText(9, 33, "F");
                RI.PutText(7, 33, AccountNumber);
                RI.PutText(11, 33, start, ReflectionInterface.Key.Enter, true); //date requested
                RI.PutText(21, 13, "28", ReflectionInterface.Key.Enter, true); //selection field
                if (RI.CheckForText(8, 14, "_"))
                    RI.PutText(8, 14, "X", ReflectionInterface.Key.Enter, true); //select all field
                RI.PutText(7, 18, start); //begin date
                RI.PutText(8, 18, end); //end date
                RI.PutText(9, 18, start); //date certified
                RI.PutText(16, 37, "Y"); //forbearance field
                RI.PutText(20, 17, "", true); //remove "Y" from manual review field
                if(HasSpousalLoans)
                    RI.PutText(14, 73, "Y");

                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.MessageCode == "01004") //success
                {
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        Arc = "FBAPV",
                        Comment = string.Join(",", today.ToShortDateString(), "Approved", "Processed Collection Suspension while waiting for " + reason.ToString() + " to be returned/processed."),
                        ScriptId = Program.ScriptId,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                    };
                    ArcAddResults result = arc.AddArc();
                    if (!result.ArcAdded)
                        new CustomMessageBoxDenial("Unable to add the FBAPV ARC for the collection suspension forbearance.  Please request the Forbearance manually").ShowDialog();
                    else
                        new CustomMessageBoxApproval(today.AddDays(59).ToShortDateString(), "Collection Suspension Approved", "The Collection Suspension has been applied.", "Please advise borrower interest will continue to accrue and if left unpaid, will be added to their principal balance", true).ShowDialog();
                }
                else
                {
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        Arc = "XFORB",
                        Comment = string.Join(",", today.ToShortDateString(), "Borrower requests Collection Suspension Forbearance. Borrower is sending " + reason.ToString()),
                        ScriptId = Program.ScriptId,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
                    };
                    ArcAddResults result = arc.AddArc();
                    if (!result.ArcAdded)
                        new CustomMessageBoxDenial("Unable to add the XFORB ARC for the collection suspension forbearance.  Please request the Forbearance manually").ShowDialog();
                    else
                        new CustomMessageBoxDenial("The Collection Suspension forbearance could not be applied. The request has been forwarded for review").ShowDialog();
                }
            }
        }

        /// <summary>
        /// Gets the borrowers Date Delinquency occurred and determines if an Admin forb needs to be applied
        /// </summary>
        /// <param name="forbDetails"></param>
        /// <param name="accountNumber"></param>
        /// <param name="ssn"></param>
        private void AddTheForbearance(ForbearanceDetails forbDetails, string ssn, bool is120, bool hasCoBwr)
        {
            DateTime? ddo = DA.GetDateDelinquencyOccurred();
            DateTime endDate = forbDetails.EndDate;

            if (ddo.HasValue && (Math.Abs(((ddo.Value.Year - endDate.Year) * 12) + ddo.Value.Month - endDate.Month)) > 11)
            {
                //We need to split up the forbearance into 2 forbearances
                DateTime end = today;
                if (!ActuallyAddForb(ssn, "05", ddo.Value, ref end, forbDetails.ForbearanceReason, forbDetails.NumberOfMonthsRequested, ddo, is120, hasCoBwr))
                    return;

                forbDetails.StartDate = today.AddDays(1);
            }
            //TODO Check For ABEND
            if (ActuallyAddForb(ssn, "05", forbDetails.StartDate.Value, ref endDate, forbDetails.ForbearanceReason, forbDetails.NumberOfMonthsRequested, ddo, is120, hasCoBwr))
            {
                string comment = string.Format("{0:MM/dd/yyyy},{1}", forbDetails.StartDate, forbDetails.ForbearanceReason);
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "FBAPV",
                    Comment = (is120 ? "The one-time 120 day  verbal forbearance has been approved, " : "Verbal forbearance Approved, ") + comment,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                    ScriptId = Program.ScriptId
                };
                arc.AddArc();

                new CustomMessageBoxApproval(endDate.ToShortDateString(), "Forbearance Approved", is120 ? "The one-time 120 day  verbal forbearance has been approved " : "The verbal forbearance was successfully applied. The end date for the forbearance is", is120 ? "Please advise borrower any unpaid has been capitalized to their principal balance" : "Please advise borrower interest will continue to accrue and if left unpaid, will be added to their principal balance", true).ShowDialog();
            }
        }

        /// <summary>
        /// Adds a forbearance to ATS0H
        /// </summary>
        /// <param name="ssn">Borrowers SSN</param>
        /// <param name="type">Type of forbearance to apply</param>
        /// <param name="start">Start date for the forbearance</param>
        /// <param name="end">End Date for the forbearance</param>
        /// <param name="reason">Reason the borrower is requesting forbearance</param>
        /// <param name="numberOfMonthsRequested">Number of months of forbearance the borrower is requesting</param>
        /// <returns>True of the forbearance was added</returns>
        private bool ActuallyAddForb(string ssn, string type, DateTime start, ref DateTime end, string reason, string numberOfMonthsRequested, DateTime? ddo, bool is120, bool hasCoBwr)
        {
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
            {
                if (end.Date == start.Date)
                    end = end.AddMonths(1);
                if (today.Month % 3 != 0)
                {
                    List<int> qtrs = new List<int>() { 3, 6, 9, 12 };
                    int qtr = qtrs.Where(p => p > today.Month).OrderBy(p => p).First();
                    DateTime nextQtr = new DateTime(today.Year, qtr, 28);

                    if (end > nextQtr)
                        end = nextQtr;
                }
                else
                {
                    if (today.Month % 3 == 0 && end > new DateTime(today.Year, today.Month, 28))
                    {
                        end = new DateTime(today.Year, today.Month, 28);
                    }
                }
            }
            AccessAts0h(ssn, type);

            if (RI.CheckForText(1, 72, "TSX7E"))
            {
                string errorMessage = RI.GetText(23, 2, 78);
                new CustomMessageBoxDenial(string.Format("The forbearance could not be added at this time because {0}.", errorMessage)).ShowDialog();
                return false;
            }

            //if the borrower has multiple loans we need to select all loans
            CheckForMultipleLoans();
            if (!AddForbData(type, start, ref end, ddo, is120, hasCoBwr))
                return true;

            if (RI.CheckForText(1, 72, "TSX30"))
            {
                for (int row = 8; !RI.CheckForText(23, 2, "90007"); row++)
                {
                    if (RI.CheckForText(row, 3, " ") || row > 20)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                        continue;
                    }

                    RI.PutText(21, 14, RI.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);

                    if (!CheckLoan(ssn, numberOfMonthsRequested, start, reason, true))
                        return false;
                }
            }
            else
            {
                if (RI.ScreenCode == "TSX32")
                {
                    new CustomMessageBoxDenial(string.Format("The forbearance could not be added at this time because {0}. The request has been forwarded for review.", RI.Message)).ShowDialog();
                    string comment = string.Format(@"{0:MM/dd/yyyy}, {1}, Error Message: {2}", start, reason, RI.GetText(21, 2, 80));
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        Arc = "XFORB",
                        Comment = comment,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        ScriptId = Program.ScriptId
                    };

                    arc.AddArc();

                    return false;
                }
                if (!CheckLoan(ssn, numberOfMonthsRequested, start, reason))
                    return false;
            }

            RI.Hit(ReflectionInterface.Key.F12);
            RI.Hit(ReflectionInterface.Key.F6);

            return true;
        }

        private bool AddForbData(string type, DateTime start, ref DateTime end, DateTime? ddo, bool is120, bool hasCoBwr)
        {
            RI.CheckScreenAbend(); //TODO Confirm Working
            RI.PutText(7, 18, start.ToString("MMddyy"));
            RI.PutText(8, 18, end.ToString("MMddyy"));
            //Date certified
            RI.PutText(9, 18, today.ToString("MMddyy"));

            if (type == "05")
            {
                if (hasCoBwr)//Co Bwr
                    RI.PutText(14, 73, "Y");
                //Authorized to exceed max field
                RI.PutText(15, 34, "N");
                //signature of borrower field
                RI.PutText(18, 34, "V");

                RI.PutText(16, 37, "Y");

            }

            //Cap interest
            RI.PutText(17, 34, "Y", ReflectionInterface.Key.Enter);
            return true;
        }

        private void CheckForMultipleLoans()
        {
            if (RI.CheckForText(1, 72, "TSXA5"))
            {
                for (int row = 12; !RI.CheckForText(23, 2, "90007"); row++)
                {
                    if (row > 21)
                    {
                        row = 11;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    if (RI.CheckForText(row, 2, "_") && !RI.CheckForText(row, 17, "TILP"))
                        RI.PutText(row, 2, "X");
                }
                RI.PutText(8, 14, "", true);

                RI.Hit(ReflectionInterface.Key.Enter);
            }
        }

        private void AccessAts0h(string ssn, string type)
        {
            RI.FastPath("TX3Z/ATS0H");
            RI.PutText(7, 33, ssn, true);
            //Forbearance code
            RI.PutText(9, 33, "F");
            //Date requested
            RI.PutText(11, 33, today.ToString("MMddyy"), ReflectionInterface.Key.Enter, true);
            RI.PutText(21, 13, type, ReflectionInterface.Key.Enter, true);
        }

        private bool CheckLoan(string ssn, string numberOfMonthsRequested, DateTime start, string reason, bool multiple = false)
        {
            string errorReason = RI.GetText(21, 2, 78);
            if (RI.CheckForText(1, 72, "TSX31") && !string.IsNullOrEmpty(errorReason))
            {
                new CustomMessageBoxDenial(string.Format("The forbearance could not be added at this time because {0}. The request has been forwarded for review.", errorReason)).ShowDialog();
                string comment = string.Format(@"{0:MM/dd/yyyy}, {1}, Error Message: {2}", start, reason, errorReason);
                ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    AccountNumber = AccountNumber,
                    Arc = "XFORB",
                    Comment = comment,
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                    ScriptId = Program.ScriptId
                };

                arc.AddArc();

                return false;
            }
            if (multiple)
            {
                RI.Hit(ReflectionInterface.Key.F12);
                RI.PutText(21, 14, "", true);
            }
            return true;
        }
    }
}
