using MDIntermediary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    class AlertCoordinator
    {
        public AlertCoordinator() : base()
        {

        }

        /// <summary>
        /// Calculates alerts. This is done after the demographics because one of the alerts require information provided by the user on that screen
        /// </summary>
        public static void CoordinateAlertsCreation(UheaaBorrower uheaaBorrower)
        {
            AlertCoordinator coordinator = new AlertCoordinator();

            if(uheaaBorrower != null)
            {
                coordinator.CreateAlerts(uheaaBorrower);
                if(DataAccess.DA.HasAP03Record(uheaaBorrower.SSN, DataAccessHelper.Region.Uheaa))
                {
                    uheaaBorrower.AddAlert(Borrower.AlertTypes.IsCompleteBorrower);
                }
            }
        }

        private void CreateAlerts(Borrower borrower)
        {
            //411 alert is set in 411 logic
            NSFAlertCheck(borrower);
            PaidAheadLoanAlertCheck(borrower);
            SetMiscBorrowerLevelAlert(borrower.VipAlertIndicator, Borrower.AlertTypes.HasVIP, borrower);
            SetMiscBorrowerLevelAlert(borrower.SpecialHandlingAlertIndicator, Borrower.AlertTypes.IsSpecialHandling, borrower);
            SetMiscBorrowerLevelAlert(borrower.RehabilitatedAlertIndicator, Borrower.AlertTypes.HasRehablitatedLoan, borrower);
            SetMiscBorrowerLevelAlert(borrower.CohortAlertIndicator, Borrower.AlertTypes.IsExistingCohort, borrower);
            SetMiscBorrowerLevelAlert(borrower.CoborrowerAlertIndicator, Borrower.AlertTypes.HasCoBorrower, borrower);
            SetMiscBorrowerLevelAlert(borrower.Exceeded36MonthForbAlertIndicator, Borrower.AlertTypes.HasExceeded36MonthsOfForbearance, borrower);
            SetMiscBorrowerLevelAlert(borrower.MultipleBillMethodAlertIndicator, Borrower.AlertTypes.HasSplitPaymentMethods, borrower);
            SetMiscBorrowerLevelAlert(borrower.MultipleDueDatesAlertIndicator, Borrower.AlertTypes.HasMultipleDueDate, borrower);
            if(borrower.HasPendingDisbursementCancellation)
            {
                borrower.AddAlert(Borrower.AlertTypes.HasPendingDisbursement);
            }
            if(borrower.HasSulaLoans)
            {
                borrower.AddAlert(Borrower.AlertTypes.HasSulaLoans);
            }
            if(borrower.HasPendingDisbursement == "Y")
            {
                borrower.AddAlert(Borrower.AlertTypes.HasPendingDisbursement);
            }
            if(borrower.NeedsDeconArc)
            {
                borrower.AddAlert(Borrower.AlertTypes.NeedsDeconArc);
            }
            //loan status alerts
            LoanStatusAlertCheck("06", Borrower.AlertTypes.LoanStatusCure, borrower);
            LoanStatusAlertCheck("07", Borrower.AlertTypes.LoanStatusClaimPending, borrower);
            LoanStatusAlertCheck("08", Borrower.AlertTypes.LoanStatusClaimSubmitted, borrower);
            LoanStatusAlertCheck("11", Borrower.AlertTypes.HasDmcsLoans, borrower);
            LoanStatusAlertCheck("12", Borrower.AlertTypes.LoanStatusClaimPaid, borrower);
            LoanStatusAlertCheck("13", Borrower.AlertTypes.LoanStatusPreClaimPending, borrower);
            LoanStatusAlertCheck("14", Borrower.AlertTypes.LoanStatusPreClaimSubmitted, borrower);
            LoanStatusAlertCheck("16", Borrower.AlertTypes.LoanStatusDeathAlleged, borrower);
            LoanStatusAlertCheck("17", Borrower.AlertTypes.LoanStatusDeathVerified, borrower);
            LoanStatusAlertCheck("18", Borrower.AlertTypes.LoanStatusDisabilityAlleged, borrower);
            LoanStatusAlertCheck("19", Borrower.AlertTypes.LoanStatusDisabilityVerified, borrower);
            LoanStatusAlertCheck("20", Borrower.AlertTypes.LoanStatusBankruptcyAlleged, borrower);
            LoanStatusAlertCheck("21", Borrower.AlertTypes.LoanStatusBankruptcyVerified, borrower);

            CornerstoneLoansPifAlertCheck(borrower); //Don't remove this since it looks at UHEAA borrowers too
            //forbearance alerts
            ForbearanceAlertCheck("F31", Borrower.AlertTypes.IsInReducedPaymentForbearance, borrower);
            ForbearanceAlertCheck("F28", Borrower.AlertTypes.IsInCollectionSuspensionForbearance, borrower);
            //call forwarding alerts
            if(borrower.CallForwardingUserResultData != null)
            {
                CallForwardingAlerts(borrower);
            }

            ThirdPartyAuthorizationCheck(borrower);
            PaymentsInSuspenseAlertCheck(borrower);
            if(borrower.CompassDemographics.SPAddrInd == "N" || borrower.CompassDemographics.HomePhoneValidityIndicator == "N")
            {
                borrower.AddAlert(Borrower.AlertTypes.HasSkipAddressOrPhone);
            }
        }

        private void ThirdPartyAuthorizationCheck(Borrower borrower)
        {
            AcpSelectionResult selection = borrower.AcpResponses.Selection;
            if(selection == null)
            {
                return;
            }
            if((selection.ContactCodeSelection != ContactCode.FromBorrower && selection.ContactCodeSelection != ContactCode.ToBorrower) || (selection.ContactCodeSelection == ContactCode.ToBorrower && selection.ActivityCodeSelection == ActivityCode.FailedCall))
            {
                borrower.AddAlert(Borrower.AlertTypes.ThirdPartyCalling);
            }
        }

        private void NSFAlertCheck(Borrower borrower)
        {
            if(borrower.ACHData != null)
            {
                if(borrower.ACHData.NSFCounter >= 2)
                {
                    borrower.AddAlert(Borrower.AlertTypes.Has2NSFs);
                }
            }
        }

        private void SetMiscBorrowerLevelAlert(string indicator, Borrower.AlertTypes alertType, Borrower borrower)
        {
            if(indicator.ToUpper() == "Y")
            {
                borrower.AddAlert(alertType);
            }
        }

        private void LoanStatusAlertCheck(string statusCode, Borrower.AlertTypes alertType, Borrower borrower)
        {
            if(borrower.CompassLoans.Where(p => p.StatusCode == statusCode).Count() > 0)
            {
                borrower.AddAlert(alertType);
            }
        }

        private void CornerstoneLoansPifAlertCheck(Borrower borrower) //Don't remove this since it looks at UHEAA borrowers too, maybe rename later
        {
            if (borrower.CallForwardingWarehouseData.Where(p => p.FORWARDING != "10").Count() == 0)
            {
                borrower.AddAlert(Borrower.AlertTypes.CornerStoneLoansPif);
            }
        }

        private void PaidAheadLoanAlertCheck(Borrower borrower)
        {
            if(borrower.CompassLoans.Where(p => p.PaidAhead == "Y").Count() > 0)
            {
                borrower.AddAlert(Borrower.AlertTypes.IsPaidAhead);
            }
        }

        private void ForbearanceAlertCheck(string code, Borrower.AlertTypes alertType, Borrower borrower)
        {
            if(borrower.DefermentsAndForbearences.Where(p => p.BeginDate.ToDate() <= DateTime.Today && p.EndDate.ToDate() >= DateTime.Today && p.TypeCode == code).Count() > 0)
            {
                borrower.AddAlert(alertType);
            }
        }

        private void PaymentsInSuspenseAlertCheck(Borrower borrower)
        {
            if(borrower.PaymentsInSuspense != 0)
            {
                borrower.AddAlert(Borrower.AlertTypes.HasPaymentInSuspense);
            }
        }

        private void CallForwardingAlerts(Borrower borrower)
        {
            foreach(CallForwardingResult result in borrower.CallForwardingUserResultData)
            {
                if(!result.OverrideMessage.IsNullOrEmpty())
                {
                    borrower.AddCallForwardingAlert(new Alert(result.OverrideMessage, result.AlertUrgency));
                }
                else
                {
                    borrower.AddCallForwardingAlert(new Alert($"Call Forward: {result.Location} - {result.Number}", result.AlertUrgency));
                }
            }
        }
    }
}
