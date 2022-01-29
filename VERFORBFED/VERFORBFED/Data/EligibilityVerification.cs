using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace VERFORBFED
{
    class EligibilityVerification
    {
        private string Ssn { get; set; }
        private string AccountNumber { get; set; }
        public List<LoanStatusInfo> LoanStatuses { get; set; }
        public List<string> LoanPrograms { get; set; }
        private ReflectionInterface RI;
        private DataAccess DA;
        private bool HasMultipleStatuses { get; set; }
        public bool IsClaim { get; set; }

        public bool HasSpecialCase { get; set; }
        public bool IsPaidAhead { get; set; }
        public DateTime? ForbearanceEndDate { get; set; }
        public DateTime? DefermentEndDate { get; set; }
        public bool HasSpousalLoans { get; set; }
        public bool ReviewBecauseOfDefOfrb { get; set; }
        private DateTime today;

        public EligibilityVerification(string ssn, string accountNumber, DataAccess da, ReflectionInterface ri, DateTime today)
        {
            Ssn = ssn;
            AccountNumber = accountNumber;
            DA = da;
            LoanStatuses = DA.GetLoanStatuses();
            LoanPrograms = DA.GetLoanPrograms();
            RI = ri;
            HasMultipleStatuses = false;
            this.today = today;
        }

        /// <summary>
        /// Check the borrowers loans status in the DW01 table in CDW.
        /// </summary>
        public bool CheckStatuses(bool includeSpecialCase, string forbType)
        {
            bool doDisplay = (forbType == "Verbal forbearance") ? true : false;

            SetSpousalLoans(doDisplay);
            if (includeSpecialCase)
                CheckSpecialCase();
            if (includeSpecialCase)
                if (CheckDaysDelq())
                    return false;

            if (!CheckForInvalidStatus())
                return false;

            if (!CheckIfLoansAreInForbearance(forbType))
                return false;

            if (CheckIfLoansAreInDeferment(forbType))
                return false;

            if (HasFutureDatedForbearance())
                return false;

            if (HasFutureDatedDeferment())
                return false;

            if (CheckRepaymentSchedule(forbType))
                return false;

            SetPaidAhead();

            return true;
        }

        private void CheckSpecialCase()
        {
            List<int> specialCase = DA.GetSpecialStatuses();
            if (!specialCase.Where(p => p == 0).Any() && specialCase.Any())
            {
                if (Dialog.Info.YesNo("Is this a 120 Day One time verbal forbearance?"))
                {
                    HasSpecialCase = true;
                }
            }
        }

        private void SetSpousalLoans(bool doDisplay)
        {
            HasSpousalLoans = DA.CheckSpousalLoans(Ssn);
            if (HasSpousalLoans && doDisplay)
                Dialog.Info.Ok("This account has Spousal Consolidation loans.  You will need to get intent to repay from the borrower and co-borrower.");
        }


        private bool CheckDaysDelq()
        {
            int? pastDue = DA.GetDaysPastDue();
            if (pastDue.HasValue && pastDue.Value >= 270 && !HasSpecialCase)
            {
                new CustomMessageBoxDenial("The forbearance has been denied because the borrower is more than 270 days delinquent and this is not a Special 120 request.  Please advise borrower of other options. ").ShowDialog();
                AddDenialComment("Verbal forbearance denied.  borrower is more than 270 days delinquent and this is not a Special 120 request");
                return true;
            }

            return false;
        }

        private void SetPaidAhead()
        {
            if (DA.CheckPaidAhead())
            {
                IsPaidAhead = true;
            }
        }

        /// <summary>
        /// Checks CDW to see if the borrower has a future dated forbearance
        /// </summary>
        /// <returns>Returns true if the borrower has a future dated forbearance</returns>
        private bool HasFutureDatedForbearance()
        {
            if (DA.HasFutureDatedForbearance())
            {
                new CustomMessageBoxDenial("The forbearance has been denied because the borrower already has a forbearance that is scheduled to start after the forbearance was requested to start").ShowDialog();
                AddDenialComment("Forbearance denied: borrower has forbearance scheduled to begin.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks CDW to see if the borrower has a future dated deferment.
        /// </summary>
        /// <returns>Returns true if the borrower has a future dated deferment</returns>
        private bool HasFutureDatedDeferment()
        {
            if (DA.HasFutureDatedDeferment())
            {
                new CustomMessageBoxDenial("The forbearance has been denied because the borrower already has a deferment that is scheduled to start after the forbearance was requested to start").ShowDialog();
                AddDenialComment("Forbearance denied: borrower has deferment scheduled to begin.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check the borrower loan statuses to verify it has at least one valid status.  If no valid statuses are found the script will prompt the user add an denial comment and end.
        /// </summary>
        /// <param name="loanStatuses">List of statuses gathered from CDW for the current borrower.</param>
        private bool CheckForInvalidStatus()
        {
            if (LoanStatuses.Where(p => p.WX_OVR_DW_LON_STA.IsIn("CLAIM PENDING", "CLAIM SUBMITTED")).ToList().Any())
            {
                IsClaim = true;
                return true;
            }
            var helper = new StatusHelper();

            if (helper.AllStatusesMatchCollectionSuspensionDenied(LoanStatuses))
            {
                string invalidStatuses = string.Join(", ", LoanStatuses.Select(o => o.WX_OVR_DW_LON_STA).Distinct());
                new CustomMessageBoxDenial(string.Format("The forbearance has been denied because the borrower’s loans are in the following status(s) {0}", invalidStatuses)).ShowDialog();
                string comment = string.Format("Verbal forbearance denied. All loans in {0}", invalidStatuses);
                AddDenialComment(comment);
                return false;
            }

            HasMultipleStatuses = LoanStatuses.Count(p => !p.WC_DW_LON_STA.IsIn(LoanStatusInfo.REPAYMENT, LoanStatusInfo.DEFERMENT, LoanStatusInfo.FORBEARANCE, LoanStatusInfo.PAIDINFULL, "12") && !p.WX_OVR_DW_LON_STA.Contains("PRE-CLAIM SUBMITTED")) > 1;

            return true;
        }

        /// <summary>
        /// Checks to see if the borrower has loans in a forbearance status
        /// </summary>
        /// <returns>Return true if all loans are not in forbearance or returns true if all loans are in forbearance but the end date is within 30 days</returns>
        private bool CheckIfLoansAreInForbearance(string forbType)
        {
            //CHECK THE COUNT OF LOANS WHERE STATUS IS IN FORBEARANCE VS ALL OF THE LOAN STATUSES COUNTS 
            if (LoanStatuses.Any(p => p.WC_DW_LON_STA == LoanStatusInfo.FORBEARANCE))
            {
                ForbearanceEndDate = DA.GetForbearanceEndDate();

                if (ForbearanceEndDate.HasValue && (ForbearanceEndDate.Value.Subtract(today).Days > 30 || (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone && ForbearanceEndDate.Value.Month % 3 == 0)))
                {
                    if (LoanStatuses.Any(p => p.WC_DW_LON_STA == LoanStatusInfo.DEFERMENT))
                    {
                        ReviewBecauseOfDefOfrb = true;
                        return false;
                    }
                    new CustomMessageBoxDenial("The forbearance has been denied because the borrower already has an active forbearance.").ShowDialog();
                    AddDenialComment($"{forbType} denied.  All loans in active forbearance");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks to see if the borrower has loans in a deferment status
        /// </summary>
        /// <returns>Return true if all loans are not in deferment or returns true if all loans are in deferment but the end date is within 30 days</returns>
        private bool CheckIfLoansAreInDeferment(string forbType)
        {
            //CHECK THE COUNT OF LOANS WHERE STATUS IS IN FORBEARANCE VS ALL OF THE LOAN STATUSES COUNTS 
            if (LoanStatuses.Any(p => p.WC_DW_LON_STA == LoanStatusInfo.DEFERMENT))
            {
                DefermentEndDate = DA.GetDefermentEndDate();

                if (DefermentEndDate.HasValue)
                {
                    if (DefermentEndDate.Value.Subtract(today).Days > 30 || (DefermentEndDate.Value.Month % 3 == 0 && DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone))
                    {
                        if (LoanStatuses.Any(p => p.WC_DW_LON_STA == LoanStatusInfo.REPAYMENT))
                        {
                            ReviewBecauseOfDefOfrb = true;
                            return false;
                        }
                        new CustomMessageBoxDenial("The forbearance has been denied because the borrower already has an active deferment.").ShowDialog();
                        AddDenialComment($"{forbType} denied.  All loans in active deferment");
                        return true;
                    }
                }
            }

            return false;
        }

        public void ProcessPaidAhead(string monthsRequested, string ack, DateTime startDate, string reason)
        {
            new CustomMessageBoxDenial("The forbearance could not be applied because the borrower has at least 1 loan paid ahead. The request has been forwarded for review").ShowDialog();
            AddReviewComment(string.Format("{0:MM/dd/yyyy},Borrower Is Paid Ahead,{1}", startDate, reason));
        }

        public bool CheckForMultipleSchedules(string monthsRequested, string ack, DateTime startDate, string reason)
        {
            if (DA.CheckRepaymentSchedules())
            {
                if (DA.PaymentAmountEqualsZero())
                {
                    new CustomMessageBoxDenial("The forbearance could not be applied because the borrower has multiple repayment schedules. The request has been forwarded for review").ShowDialog();
                    AddReviewComment(string.Format("{0:MM/dd/yyyy},Multiple repayment schedules,{1}", startDate, reason));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks to see if the borrower has an iBR, ICR or PAYE schedule.
        /// </summary>
        /// <returns>True if they have invalid schedules</returns>
        private bool CheckRepaymentSchedule(string forbType)
        {
            if (DA.CheckRepaymentSchedules())
                return CheckPaymentAmount(forbType);

            return false;
        }

        /// <summary>
        /// Checks to see if the borrower has a 0.00 payment.
        /// </summary>
        /// <returns>True if Payment is greater than 0.00</returns>
        private bool CheckPaymentAmount(string forbType)
        {
            if (DA.PaymentAmountEqualsZero())
            {
                new CustomMessageBoxDenial("The forbearance cannot be applied because the borrower has a $0 payment").ShowDialog();
                AddDenialComment(forbType + " denied.  The borrower has a $0 payment");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a denial comment in TD22 with the ARC FBDNY.
        /// </summary>
        /// <param name="comment">Comment text to be entered into the session.</param>
        private void AddDenialComment(string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = AccountNumber,
                Arc = "FBDNY",
                Comment = comment,
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };
            ArcAddResults result = arc.AddArc();
        }

        /// <summary>
        /// Adds a comment in TD22 with the ARC XFORB, this will create a queue task for a manager to review.
        /// </summary>
        /// <param name="comment"></param>
        public void AddReviewComment(string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = AccountNumber,
                Arc = "XFORB",
                Comment = comment,
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };
            ArcAddResults result = arc.AddArc();
        }

        /// <summary>
        /// Checks to see if a borrower has multiple statuses.  If they do it will display a message to the user and create a review task.
        /// </summary>
        /// <param name="forbDetails">details about the forbearance being added.</param>
        /// <returns>false if the borrower does not have multiple statuses</returns>
        public bool CheckMultipleStatuses(ForbearanceDetails forbDetails)
        {
            if (!HasMultipleStatuses)
                return false;

            new CustomMessageBoxDenial("The forbearance could not be applied because the borrower has multiple statuses. The request has been forwarded for review").ShowDialog();
            string comment = string.Format(@"{0:MM/dd/yyyy}, ""Multiple statuses"" {1}", forbDetails.StartDate, forbDetails.ForbearanceReason);
            AddReviewComment(comment);
            return true;
        }

        /// <summary>
        /// Checks to see if the borrower has had a collection suspense forbearance during the period of delinquency.
        /// </summary>
        /// <param name="forbDetails">Object that contains info about the forbearance including all of the collection suspense forbearances</param>
        /// <returns>true if the borrower has had a collection suspense forbearance during the time of delinquency</returns>
        public bool CheckCollectionSuspenseForbearance(ForbearanceDetails forbDetails)
        {
            bool hasCollectionForb = false;
            DateTime? ddo = DA.GetDateDelinquencyOccurred();

            if (ddo.HasValue)
            {
                if (forbDetails.CollectionForbDetails.Where(p => p.CollectionSuspenseForbearanceBeginDate > ddo).Count() > 0)
                    hasCollectionForb = true;

                if (forbDetails.CollectionForbDetails.Where(p => p.CollectionSuspenseForbearanceEndDate > ddo).Count() > 0)
                    hasCollectionForb = true;

                if (hasCollectionForb)
                {
                    string type = forbDetails.CollectionForbDetails.First().LC_FOR_TYP == "25" ? "Disaster" : "Collection Suspension";
                    new CustomMessageBoxDenial(string.Format("The forbearance cannot be added because the borrower had a {0} forbearance during the delinquency period, the request has been forwarded for review", type)).ShowDialog();
                    string comment = string.Format(@"{0:MM/dd/yyyy},""{2} w/in Delq Period"",{1}", forbDetails.StartDate.Value, forbDetails.ForbearanceReason, type);
                    AddReviewComment(comment);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks to see if the borrower has used more than 36 months of forbearance.
        /// </summary>
        /// <param name="forbDetails">Object with data about the forbearance.</param>
        /// <returns>true if the borrower has used more than 36 months of forbearance.</returns>
        public bool CheckNumberOfMonthsUsed(ForbearanceDetails forbDetails)
        {
            if (forbDetails.DaysOfForbearanceUsed > 36) //36 months
            {
                if (forbDetails.DaysOfForbearanceUsed < 60) //60 months
                {
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        Comment = "Approved",
                        Arc = "FBEMA",
                        ScriptId = "VERFORBFED"
                    };
                    arc.AddArc();
                    return false;
                }
                else if (forbDetails.DaysOfForbearanceUsed < 70) //70 month
                {
                    if (DA.BorrowerMadeFullPaymentInLastYear(Ssn))
                    {
                        ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                        {
                            AccountNumber = AccountNumber,
                            ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                            Comment = "Approved",
                            Arc = "FBEMA",
                            ScriptId = "VERFORBFED"
                        };
                        arc.AddArc();
                        return false;
                    }
                    else
                    {
                        if (DA.BorrowerUsedForbearanceWithinLastYear(Ssn))
                        {
                            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                            {
                                AccountNumber = AccountNumber,
                                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                                Comment = (forbDetails.StartDate ?? DateTime.MinValue).ToShortDateString() + "," + "Borrower Requests verbal forbearance for " + forbDetails.NumberOfMonthsRequested + " month(s) due to financial difficulty.  Borrower acknowledged intent to repay this debt.",
                                Arc = "VFMGR",
                                ScriptId = "VERFORBFED"
                            };
                            arc.AddArc();
                            new CustomMessageBoxDenial("The forbearance could not be processed at this time because the borrower has already used more than 60 months of forbearance time and used a forbearance within the past year. The request has been forwarded for review").ShowDialog();
                            return true;
                        }
                        else
                        {
                            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                            {
                                AccountNumber = AccountNumber,
                                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                                Comment = "Approved",
                                Arc = "FBEMA",
                                ScriptId = "VERFORBFED"
                            };
                            arc.AddArc();
                            return false;
                        }
                    }
                }
                else
                {
                    new CustomMessageBoxDenial("The forbearance could not be processed at this time because the borrower has already used more than 70 months of forbearance time. The request has been forwarded for review.").ShowDialog();
                    string comment = string.Format("Start Date {0:MM/dd/yyyy}, {1}", forbDetails.StartDate, forbDetails.ForbearanceReason);
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = AccountNumber,
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        Comment = comment,
                        Arc = "VFMGR",
                        ScriptId = "VERFORBFED"
                    };
                    arc.AddArc();

                    return true;
                }
            }
            return false;
        }
    }
}
