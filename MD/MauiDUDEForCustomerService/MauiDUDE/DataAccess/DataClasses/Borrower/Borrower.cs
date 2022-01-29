using MDIntermediary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace MauiDUDE
{
    public class Borrower : IBorrower
    {
        public Borrower()
        {
            AcpResponses = new AcpResponses();
        }

        public Borrower(Borrower tempBorrower)
        {
            AcpResponses = new AcpResponses();
            SSN = tempBorrower.SSN;
            AccountNumber = tempBorrower.AccountNumber;
            NumberOf20DayLettersSent = tempBorrower.NumberOf20DayLettersSent;
            TotalNumberOfTimesLateFeesHaveBeenWaived = tempBorrower.TotalNumberOfTimesLateFeesHaveBeenWaived;
            LastRPSPrintDate = tempBorrower.LastRPSPrintDate;
            DaysDelinquent = tempBorrower.DaysDelinquent;
            BillType = tempBorrower.BillType;
            Interest = tempBorrower.Interest;
            CohortYear = tempBorrower.CohortYear;
            CurrentAmountDue = tempBorrower.CurrentAmountDue;
            AmountPastDue = tempBorrower.AmountPastDue;
            NextDueDate = tempBorrower.NextDueDate;
            TotalAmountDue = tempBorrower.TotalAmountDue;
            TotalAmountPlusLateFees = tempBorrower.TotalAmountPlusLateFees;
            Principal = tempBorrower.Principal;
            DailyInterest = tempBorrower.DailyInterest;
            MonthlyPaymentAmount = tempBorrower.MonthlyPaymentAmount;
            Rehabilitated = tempBorrower.Rehabilitated;
            RehabilitatedAlertIndicator = tempBorrower.RehabilitatedAlertIndicator;
            CohortAlertIndicator = tempBorrower.CohortAlertIndicator;
            CoborrowerAlertIndicator = tempBorrower.CoborrowerAlertIndicator;
            Exceeded36MonthForbAlertIndicator = tempBorrower.Exceeded36MonthForbAlertIndicator;
            MultipleBillMethodAlertIndicator = tempBorrower.MultipleBillMethodAlertIndicator;
            MultipleDueDatesAlertIndicator = tempBorrower.MultipleDueDatesAlertIndicator;
            DueDay = tempBorrower.DueDay;
            LastPaymentReceivedDate = tempBorrower.LastPaymentReceivedDate;
            DateDelinquencyOccured = tempBorrower.DateDelinquencyOccured;
            MonthlyInterestAccrual = tempBorrower.MonthlyInterestAccrual;
            MonthlyInterestAccrualPlus5 = tempBorrower.MonthlyInterestAccrualPlus5;
            HasPendingDisbursement = tempBorrower.HasPendingDisbursement;
            VipAlertIndicator = tempBorrower.VipAlertIndicator;
            SpecialHandlingAlertIndicator = tempBorrower.SpecialHandlingAlertIndicator;
            Info411 = tempBorrower.Info411;
            DatesOf20DayLettersSent = tempBorrower.DatesOf20DayLettersSent;
            PaymentsInSuspense = tempBorrower.PaymentsInSuspense;
        }

        //!!NOTE ASYNC UPDATES
        //This is updated asynchronously when DemographcisUI is being run
        public void Replicate(Borrower tempBorrower)
        {
            //Do not reset ACP responses, it should be filled out at this point
            NumberOf20DayLettersSent = tempBorrower.NumberOf20DayLettersSent;
            TotalNumberOfTimesLateFeesHaveBeenWaived = tempBorrower.TotalNumberOfTimesLateFeesHaveBeenWaived;
            LastRPSPrintDate = tempBorrower.LastRPSPrintDate;
            DaysDelinquent = tempBorrower.DaysDelinquent;
            BillType = tempBorrower.BillType;
            Interest = tempBorrower.Interest;
            CohortYear = tempBorrower.CohortYear;
            CurrentAmountDue = tempBorrower.CurrentAmountDue;
            AmountPastDue = tempBorrower.AmountPastDue;
            NextDueDate = tempBorrower.NextDueDate;
            TotalAmountDue = tempBorrower.TotalAmountDue;
            TotalAmountPlusLateFees = tempBorrower.TotalAmountPlusLateFees;
            Principal = tempBorrower.Principal;
            DailyInterest = tempBorrower.DailyInterest;
            MonthlyPaymentAmount = tempBorrower.MonthlyPaymentAmount;
            Rehabilitated = tempBorrower.Rehabilitated;
            RehabilitatedAlertIndicator = tempBorrower.RehabilitatedAlertIndicator;
            CohortAlertIndicator = tempBorrower.CohortAlertIndicator;
            CoborrowerAlertIndicator = tempBorrower.CoborrowerAlertIndicator;
            Exceeded36MonthForbAlertIndicator = tempBorrower.Exceeded36MonthForbAlertIndicator;
            MultipleBillMethodAlertIndicator = tempBorrower.MultipleBillMethodAlertIndicator;
            MultipleDueDatesAlertIndicator = tempBorrower.MultipleDueDatesAlertIndicator;
            DueDay = tempBorrower.DueDay;
            LastPaymentReceivedDate = tempBorrower.LastPaymentReceivedDate;
            DateDelinquencyOccured = tempBorrower.DateDelinquencyOccured;
            MonthlyInterestAccrual = tempBorrower.MonthlyInterestAccrual;
            MonthlyInterestAccrualPlus5 = tempBorrower.MonthlyInterestAccrualPlus5;
            HasPendingDisbursement = tempBorrower.HasPendingDisbursement;
            VipAlertIndicator = tempBorrower.VipAlertIndicator;
            SpecialHandlingAlertIndicator = tempBorrower.SpecialHandlingAlertIndicator;
            DatesOf20DayLettersSent = tempBorrower.DatesOf20DayLettersSent;
            PaymentsInSuspense = tempBorrower.PaymentsInSuspense;
        }

        //IBorrower fields

        /// <summary>
        /// SSN for borrower
        /// </summary>
        public string SSN { get; set; }
        public MDIntermediary.AcpResponses AcpResponses { get; set; }
        public DateTime? RelationshipBeginDate { get; set; }
        public bool EndProcessingAfterDemosPage { get; set; }
        public DemographicVerifications DemographicsVerifications { get; set; }
        public string ContactPhoneNumber { get; set; }
        public bool NeedsDeconArc { get; set; }

        /// <summary>
        /// Account number for the borrower
        /// </summary>
        public string AccountNumber { get; set; }

        //Other fields

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MI { get; set; }
        public string FullName { get; set; }
        public string DOB { get; set; }

        /// <summary>
        /// Raw call forwarding warehouse data
        /// </summary>
        public List<DataAccess.CallForwardingData> CallForwardingWarehouseData { get; set; }

        /// <summary>
        /// Refined call forwarding data to display to the user
        /// </summary>
        public List<CallForwardingResult> CallForwardingUserResultData { get; set; }

        /// <summary>
        /// Is marked true when processing should continue as usual to the home page or false when the user shouldn't be sent to the home page but to the separate Onelink demos and Call Forwarding screen.
        /// </summary>
        public bool ContinueToUseHomePage { get; set; }

        public List<string> LoanProgramsSummary { get; set; }
        public List<string> StatusSummary { get; set; }
        public List<string> CallForwardingSummary { get; set; }
        public bool HasPendingDisbursementCancellation { get; set; }
        public bool HasSulaLoans { get; set; }
        public Demographics CompassDemographics { get; set; }
        public Demographics UpdatedDemographics { get; set; }
        public MDBorrowerDemographics AltAddress { get; set; }
        public List<Reference> References { get; set; }

        //!!NOTE ASYNC UPDATES
        //This fields are updated ansynchronously when the DemographicsUI is being run       
        //Begin Used In Borrower.Replicate()
        public int DaysDelinquent { get; set; }
        public decimal TotalLateFeesWaived { get; set; }
        public int TotalNumberOfTimesLateFeesHaveBeenWaived { get; set; }
        public string BillType { get; set; }
        public decimal Interest { get; set; }
        public string CohortYear { get; set; }
        public decimal CurrentAmountDue { get; set; }
        public decimal AmountPastDue { get; set; }
        public decimal TotalAmountDue { get; set; }
        public decimal Principal { get; set; }
        public decimal TotalAmountPlusLateFees { get; set; }
        public string MonthlyPaymentAmount { get; set; }
        public decimal DailyInterest { get; set; }
        public string Rehabilitated { get; set; }
        public string RehabilitatedAlertIndicator { get; set; }
        public string CohortAlertIndicator { get; set; }
        public string CoborrowerAlertIndicator { get; set; }
        public string Exceeded36MonthForbAlertIndicator { get; set; }
        public string MultipleBillMethodAlertIndicator { get; set; }
        public string MultipleDueDatesAlertIndicator { get; set; }
        public string DueDay { get; set; }
        public string LastPaymentReceivedDate { get; set; }
        public string DateDelinquencyOccured { get; set; }
        public decimal MonthlyInterestAccrual { get; set; }
        public decimal MonthlyInterestAccrualPlus5 { get; set; }
        public string HasPendingDisbursement { get; set; }
        public string VipAlertIndicator { get; set; }
        public string SpecialHandlingAlertIndicator { get; set; }
        public List<string> DatesOf20DayLettersSent { get; set; }
        public decimal PaymentsInSuspense { get; set; }
        /// <summary>
        /// Date the current RPS was generated
        /// </summary>
        public string LastRPSPrintDate { get; set; }
        /// <summary>
        /// Number of 20 day letters sent
        /// </summary>
        public int NumberOf20DayLettersSent { get; set; }
        /// <summary>
        /// The next or current due date
        /// </summary>
        public string NextDueDate { get; set; }
        //End Used In Borrower.Replicate()
        public List<ServicingLoanDetail> CompassLoans { get; set; }
        public List<string> LoanProgramsDistinctList { get; set; }
        public List<string> LoanStatusDistinctList { get; set; }
        public List<string> LoanStatusListForLegal { get; set; }
        public List<Bill> Bills { get; set; }
        public List<DefermentForbearance> DefermentsAndForbearences { get; set; }
        public ACHInformation ACHData { get; set; }
        public List<FinancialTransaction> FinancialTransactionHistory { get; set; }
        public List<string> RPSTypes { get; set; }
        public List<string> BillTypes { get; set; }
        //!!END ASYNC UPDATES

        /// <summary>
        /// Important information about the borrower from the M1411 activity record. Displayed on the BrwInfo411
        /// </summary>
        public string Info411 { get; set; }

        //Alert Fields
        public enum AlertTypes
        {
            Has411,
            HasVIP,
            IsSpecialHandling,
            Has2NSFs,
            HasMultipleDueDate,
            HasSplitPaymentMethods,
            HasPaymentInSuspense,
            IsExistingCohort,
            IsInReducedPaymentForbearance,
            IsInCollectionSuspensionForbearance,
            HasCoBorrower,
            HasSkipAddressOrPhone,
            HasRehablitatedLoan,
            LoanStatusCure,
            LoanStatusClaimPending,
            LoanStatusClaimSubmitted,
            LoanStatusClaimPaid,
            LoanStatusPreClaimPending,
            LoanStatusPreClaimSubmitted,
            LoanStatusDeathAlleged,
            LoanStatusDeathVerified,
            LoanStatusDisabilityAlleged,
            LoanStatusDisabilityVerified,
            LoanStatusBankruptcyAlleged,
            LoanStatusBankruptcyVerified,
            IsPaidAhead,
            HasExceeded36MonthsOfForbearance,
            CallFowarding,
            ThirdPartyCalling,
            HasUheaaLoans,
            HasCornerStoneLoans,
            CornerStoneLoansPif,
            HasDmcsLoans,
            HasPendingDisbursement,
            HasPendingDisbursementCancellation,
            HasSulaLoans,
            IsCompleteBorrower,
            NeedsDeconArc
        }

        //Accessed Only through Add Alert
        private List<Alert> _alerts;

        public List<Alert> GetAlertList()
        {
            if(_alerts == null)
            {
                return new List<Alert>();
            }
            else
            {
                return _alerts;
            }
        }

        public void AddAlert(AlertTypes type)
        {
            if(_alerts == null)
            {
                _alerts = new List<Alert>();
            }

            Alert tempAlert;
            if (type == AlertTypes.Has411)
                tempAlert = new Alert("Has 411", Alert.AlertUrgency.High);
            else if (type == AlertTypes.HasVIP)
                tempAlert = new Alert("VIP Borrower", Alert.AlertUrgency.High);
            else if (type == AlertTypes.Has2NSFs)
                tempAlert = new Alert("Has 2 NSFs", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasMultipleDueDate)
                tempAlert = new Alert("Has Multiple Due Dates", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasSplitPaymentMethods)
                tempAlert = new Alert("Has Split Payment Methods", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasPaymentInSuspense)
                tempAlert = new Alert("Has Payment In Suspense", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.IsExistingCohort)
                tempAlert = new Alert("Is In Existing Cohort", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.IsInReducedPaymentForbearance)
                tempAlert = new Alert("Is In Reduced Payment Forbearance", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.IsInCollectionSuspensionForbearance)
                tempAlert = new Alert("Is In Collection Suspension Forbearance", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasCoBorrower)
                tempAlert = new Alert("Has Co-Borrower", Alert.AlertUrgency.High);
            else if (type == AlertTypes.HasSkipAddressOrPhone)
                tempAlert = new Alert("Has Skip Address/Phone", Alert.AlertUrgency.High);
            else if (type == AlertTypes.HasRehablitatedLoan)
                tempAlert = new Alert("Has Rehabilitated Loan", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.LoanStatusCure)
                tempAlert = new Alert("Has Loan In Cure Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusClaimPending)
                tempAlert = new Alert("Has Loan In Claim Pending Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusClaimSubmitted)
                tempAlert = new Alert("Has Loan In Claim Submitted Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusClaimPaid)
                tempAlert = new Alert("Has Loan In Claim Paid Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusPreClaimPending)
                tempAlert = new Alert("Has Loan In Pre-Claim Pending Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusPreClaimSubmitted)
                tempAlert = new Alert("Has Loan In Pre-Claim Submitted Status", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.LoanStatusDeathAlleged)
                tempAlert = new Alert("Has Loan In Death Alleged Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusDeathVerified)
                tempAlert = new Alert("Has Loan In Death Verified Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusDisabilityAlleged)
                tempAlert = new Alert("Has Loan In Disability Alleged Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusDisabilityVerified)
                tempAlert = new Alert("Has Loan In Disability Verified Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusBankruptcyAlleged)
                tempAlert = new Alert("Has Loan In Bankruptcy Alleged Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.LoanStatusBankruptcyVerified)
                tempAlert = new Alert("Has Loan In Bankruptcy Verified Status", Alert.AlertUrgency.High);
            else if (type == AlertTypes.IsSpecialHandling)
                tempAlert = new Alert("Special Handling Borrower", Alert.AlertUrgency.High);
            else if (type == AlertTypes.IsPaidAhead)
                tempAlert = new Alert("Is Paid Ahead", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasExceeded36MonthsOfForbearance)
                tempAlert = new Alert("Has Exceeded 36 Months Of Forbearance", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.ThirdPartyCalling)
                tempAlert = new Alert("Check Third Part Authorization", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasUheaaLoans)
                tempAlert = new Alert("Has UHEAA Loans", Alert.AlertUrgency.High);
            else if (type == AlertTypes.HasCornerStoneLoans)
                tempAlert = new Alert("Has Cornerstone Loans", Alert.AlertUrgency.High);
            else if (type == AlertTypes.HasDmcsLoans)
                tempAlert = new Alert("DMCS - 800-621-3115", Alert.AlertUrgency.High);
            else if (type == AlertTypes.CornerStoneLoansPif)
                tempAlert = new Alert("All Loan are Paid In Full", Alert.AlertUrgency.Low);
            else if (type == AlertTypes.HasPendingDisbursement)
                tempAlert = new Alert("Has Pending Disbursement", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasPendingDisbursementCancellation)
                tempAlert = new Alert("Has Pending Disbursement Cancellation", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.HasSulaLoans)
                tempAlert = new Alert("Has SULA loan(s) with loss of subsidy", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.IsCompleteBorrower)
                tempAlert = new Alert("Borrower Is Complete.", Alert.AlertUrgency.Medium);
            else if (type == AlertTypes.NeedsDeconArc)
                tempAlert = new Alert("All Loans are Deconverted", Alert.AlertUrgency.Medium);
            else
                tempAlert = new Alert("Error doing alert translation", Alert.AlertUrgency.High);

            //If the alert is not a duplicate, add it to the list
            if(_alerts.Where(p => p.Text == tempAlert.Text).Count() == 0)
            {
                _alerts.Add(tempAlert);
            }
        }

        public void AddCallForwardingAlert(Alert alertText)
        {
            if(_alerts == null)
            {
                _alerts = new List<Alert>();
            }
            _alerts.Add(alertText);
        }
    }
}
