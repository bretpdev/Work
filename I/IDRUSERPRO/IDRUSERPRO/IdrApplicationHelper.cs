using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace IDRUSERPRO
{
    public class IdrApplicationHelper
    {

        private enum Statuses
        {
            ReviewEligibility,
            ActiveRepaymentSchedule,
            OtherError,
            Added,
            Denied
        }

        private enum Type
        {
            IBR,
            PAYE,
            REPAYE
        }

        private RecoveryLog Recovery { get; set; }
        private ReflectionInterface RI { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }

        public IdrApplicationHelper(ReflectionInterface ri, RecoveryLog recovery, DataAccess da, ProcessLogRun logRun)
        {
            RI = ri;
            Recovery = recovery;
            LogRun = logRun;
            DA = da;
        }

        /// <summary>
        /// Adds the IDR application to the system.
        /// </summary>
        /// <param name="appEntry">Application data entered by the user.</param>
        /// <param name="demos">Borrowers Demographic information</param>
        /// <returns>enum indicating if IDR was added, if invalid data was entered by the user, or if the script needs to end.</returns>
        public IDRProcessing.AppResult AddTheApp(ApplicationEntry appEntry, SystemBorrowerDemographics demos)
        {
            string results = string.Empty;
            if (Recovery.RecoveryValue.Contains("Database Updated"))
            {
                string spouseSsn = appEntry.Spouse == null ? "" : appEntry.Spouse.Ssn;
                if (appEntry.RepaymentTypeId == 1 || appEntry.RepaymentTypeId == 2)
                    results = AddIdr(demos.Ssn, appEntry.UserInputedData, spouseSsn, appEntry.LoansWithOtherServicers.SpouseLoans, appEntry.LoansWithOtherServicers.BorrowerLoans, Type.IBR);
                else if (appEntry.RepaymentTypeId == 3)
                    results = AddIcr(demos.Ssn, appEntry.UserInputedData, spouseSsn, appEntry.LoansWithOtherServicers.SpouseLoans, appEntry.LoansWithOtherServicers.BorrowerLoans); //ICR
                else
                    results = AddIdr(demos.Ssn, appEntry.UserInputedData, spouseSsn, appEntry.LoansWithOtherServicers.SpouseLoans, appEntry.LoansWithOtherServicers.BorrowerLoans, appEntry.RepaymentTypeId == 4 ? Type.PAYE : Type.REPAYE);

                return CheckApplicationResults(results, appEntry.UserInputedData.ApplicationId.Value);
            }
            else
                return IDRProcessing.AppResult.Success;//In Recovery
        }

        /// <summary>
        /// Checks the results of adding the IDR application.
        /// </summary>
        /// <param name="results">results from either AddIdr or AddIcr method</param>
        /// <returns>enum indicating if IDR was added, if invalid data was entered by the user, or if the script needs to end.</returns>
        private IDRProcessing.AppResult CheckApplicationResults(string results, int appId)
        {
            if (results.Contains("Eligibility"))
            {
                MessageBox.Show("Eligibility error received. Review error and update applicable data fields.");
                ProcessLogger.AddNotification(LogRun.ProcessLogId, "Eligibility error received. Review error and update applicable data fields.", NotificationType.Other, NotificationSeverityType.Informational);
                return IDRProcessing.AppResult.BadData;
            }
            else if (results.Contains("Active"))
            {
                MessageBox.Show("This borrower is already on an active repayment schedule. Cannot continue until the current repayment schedule is removed. Entered data will be saved in database.");
                ProcessLogger.AddNotification(LogRun.ProcessLogId, "This borrower is already on an active repayment schedule. Cannot continue until the current repayment schedule is removed. Entered data will be saved in database.", NotificationType.Other, NotificationSeverityType.Informational);
                return IDRProcessing.AppResult.EndScript;
            }
            else if (results.Contains("Denied"))
            {
                MessageBox.Show("IDR Denied. Review error and update the Status fields.");
                ProcessLogger.AddNotification(LogRun.ProcessLogId, "IDR Denied. Review error and update the Status fields.", NotificationType.Other, NotificationSeverityType.Informational);
                return IDRProcessing.AppResult.BadData;
            }
            else
            {
                if (!results.Contains("Added"))
                {
                    MessageBox.Show(string.Format("Error {0} received adding IDR to account. Review error and update applicable data fields.", results));
                    ProcessLogger.AddNotification(LogRun.ProcessLogId, string.Format("Error {0} received adding IDR to account. Review error and update applicable data fields.", results), NotificationType.Other, NotificationSeverityType.Informational);
                    return IDRProcessing.AppResult.BadData;
                }
                else
                    Recovery.RecoveryValue = string.Format("{0},IDR Added", appId);
            }

            ProcessLogger.AddNotification(LogRun.ProcessLogId, "IDR Added", NotificationType.Other, NotificationSeverityType.Informational);
            return IDRProcessing.AppResult.Success;
        }

        /// <summary>
        /// Access TS7C to verify that the borrower has loan eligible for IDR
        /// </summary>
        /// <param name="ssn">SSN of the borrower</param>
        /// <param name="loans">List of data that contains information about each loan that the borrower has.  This data has been gathered from TS26</param>
        /// <param name="eligibilityId">Eligibility Id selected in the IdrInformation form</param>
        /// <returns>A list of integers that contains all of the valid loans sequences</returns>
        public List<int> CheckTS7C(ReflectionInterface ri, string ssn, List<Ts26Loans> loans, List<LoanSequenceEligibility> indicators)
        {
            ri.FastPath("TX3Z/CTS7C" + ssn);

            if (ri.MessageCode.Contains("50108"))
            {
                MessageBox.Show("No open loans found.");
                return null;
            }

            return GetValidLoans(ri, loans, indicators);
        }

        /// <summary>
        /// Gets the loan seq for all valid loans
        /// </summary>
        /// <param name="ri">Reflection Interface Object</param>
        /// <param name="loans">List of borrowers loans from TS26</param>
        /// <param name="eligibilityId">Eligibility Id entered by the user.</param>
        /// <returns></returns>
        private List<int> GetValidLoans(ReflectionInterface ri, List<Ts26Loans> loans, List<LoanSequenceEligibility> indicators)
        {
            List<int> validLoans = new List<int>();
            //selection screen
            if (ri.ScreenCode.Contains("TSX3S"))
            {
                for (int row = 7; ri.MessageCode != "90007"; row++)
                {
                    //Code 90007 no more loans
                    if (row > 21 || ri.CheckForText(row, 3, "  "))
                    {
                        row = 6;
                        ri.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    string loanSeq = ri.GetText(row, 20, 6);
                    ri.PutText(22, 19, ri.GetText(row, 3, 2), ReflectionInterface.Key.Enter, true);
                    if (CheckEligibility(ri, loans, loanSeq, indicators))
                    {
                        validLoans.Add(int.Parse(loanSeq));
                    }
                }
            }
            else
            {   //Target screen
                string pgm = ri.GetText(6, 38, 6);
                DateTime dsbDate = ri.GetText(6, 68, 10).ToDate();
                var t = loans.First().DisbDate.ToDate();
                string loanSeq = loans.Where(p => p.DisbDate.ToDate() == dsbDate && p.LoanType.Trim() == pgm).Select(p => p.LoanSeq).First();

                if (!CheckEligibility(ri, loans, loanSeq, indicators))
                    return null;

                validLoans.Add(int.Parse(loanSeq));
            }

            return validLoans;
        }

        readonly string[] ConsolidationLoans = new string[] { "DLSCNS", "DLUCNS", "DLSSPL", "DLUSPL", "UNCNS", "SUBCNS", "CNSLDN", "SUBSPC", "SPCNSL", "UNSPC" };

        /// <summary>
        /// Checks to see if the borrower is eligible for IDR
        /// </summary>
        /// <param name="ri">Reflection Interface Object</param>
        /// <param name="loans">List of loans Gathers from TS26</param>
        /// <returns>true if the borrower has eligibility loans</returns>
        private bool CheckEligibility(ReflectionInterface ri, List<Ts26Loans> loans, string loanSeq, List<LoanSequenceEligibility> indicators)
        {
            //Checks to see if this field is not entered.  If it is not entered we need to put something in it so that the screen will validate.
            if (ri.CheckForText(14, 48, "_", " "))
                ri.PutText(14, 48, "N");

            if (ri.CheckForText(18, 19, "__"))
            {
                string numberOfGraceMonths = (ri.CheckForText(6, 38, "STFFRD", "UNSTFD") ? "6" : ri.CheckForText(6, 38, "TILP") ? "02" : "0");
                ri.PutText(18, 19, numberOfGraceMonths);
            }

            if (ri.GetText(6, 38, 6).IsIn(Ts26Results.InvalidLoans))
            {
                ri.Hit(ReflectionInterface.Key.F12);
                return false;
            }

            //Check to see if this loan was listed on TS26
            Ts26Loans loan = loans.Where(p => int.Parse(p.LoanSeq) == int.Parse(loanSeq ?? "0")).SingleOrDefault();
            string indicator = "I";
            if (loan != null && loan.IsEligible)
            {
                var matching = indicators.SingleOrDefault(o => o.LoanSequence == loan.LoanSeq.ToInt());
                if (matching != null)
                    indicator = matching.FutureEligibilityIndicator;
            }
            var indicatorIsI = indicator == "I";
            var indicatorIsChanging = !RI.CheckForText(14, 74, indicator);
            var isConsolidationLoan = loan?.LoanType?.Trim().IsIn(ConsolidationLoans) ?? false;
            var indicatorIsOk = isConsolidationLoan || !indicatorIsI;
            if (indicatorIsChanging && indicatorIsOk)
            {
                ri.PutText(14, 74, indicator, ReflectionInterface.Key.Enter);
                return ValidateVitalScreen(ri) && !indicatorIsI;
            }
            else
            {
                ri.Hit(ReflectionInterface.Key.F12);
                if (!indicatorIsI)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the data entered on TS7C was accepted
        /// </summary>
        /// <returns>True if the data was successfully added to the system, False if the data could not be added.</returns>
        private bool ValidateVitalScreen(ReflectionInterface ri)
        {
            if (!ri.MessageCode.Contains("03924"))
            {
                string message = string.Format("Error updating IDR eligibility on Non-Vital Loan Information. Session Message: {0}", ri.Message);
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                return false;
            }
            else
            {
                ri.Hit(ReflectionInterface.Key.F6);
                ri.PutText(13, 2, "IDR eligibility updated.", ReflectionInterface.Key.Enter);

                ri.Hit(ReflectionInterface.Key.Enter);

                if (!ri.MessageCode.Contains("01005"))
                {
                    MessageBox.Show("Error updating IDR eligibility on Non-Vital Loan Information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                ri.Hit(ReflectionInterface.Key.F12);
                return true;
            }
        }

        /// <summary>
        /// Will add the data gathered for ICR applications into the session.
        /// </summary>
        /// <param name="ssn">Ssn of the borrower we are adding ICR to</param>
        /// <param name="appData">Contains all of the data entered by the users in the IdrInformation form.  This data will be used to add IDR</param>
        /// <param name="spouseSsn">SSN of borrowers spouse.  If application does not have a spouse pass empty string</param>
        /// <param name="spousesLoans">Contains all of the loan information for a spouse.  If no loan information exists pass empty list</param>
        /// <param name="borrowerLoans">Contains all of the information about a borrowers loans that are not serviced at Uheaa</param>
        /// <returns>Returns the result of adding the application.  If the application was not added will return the error reason why it could not be added</returns
        private string AddIcr(string ssn, ApplicationData appData, string spouseSsn, IEnumerable<OtherLoans> spousesLoans, IEnumerable<OtherLoans> borrowerLoans)
        {
            RI.FastPath("TX3Z/ATL1G" + ssn);

            //If a message is received something went wrong.
            switch (RI.MessageCode)
            {
                case "06149":
                    return Statuses.ReviewEligibility.ToString();
                case "06365":
                    if (appData.RepaymentPlanReason == 3)
                    {
                        RI.Hit(ReflectionInterface.Key.F6);
                        if (RI.ScreenCode == "TLX38" || RI.MessageCode == "00000")
                            return Statuses.ActiveRepaymentSchedule.ToString();
                        break;
                    }
                    else
                        return Statuses.ActiveRepaymentSchedule.ToString();
                case "":
                    RI.Hit(ReflectionInterface.Key.Enter);
                    break;
                default:
                    return RI.Message;
            }

            if (appData.MaritalStatusId == 2 && spousesLoans.Any())
                RI.PutText(8, 15, spouseSsn);

            //The application does not have any external loans.
            if (!spousesLoans.Any() && !borrowerLoans.Any())
            {
                RI.PutText(7, 18, "N");
                RI.Hit(ReflectionInterface.Key.F11);
            }
            else
            {
                int startRow = 11;
                int spouseEndingRow = EnterOtherLoanInformationIcr(startRow, spousesLoans, "S", appData.SpouseId, spouseSsn);
                int borrowerEndingRow = EnterOtherLoanInformationIcr(spouseEndingRow, borrowerLoans, "B");

                foreach (OtherLoans item in spousesLoans.Where(p => p.Ffelp))
                {
                    RI.Hit(ReflectionInterface.Key.Enter);
                }

                foreach (OtherLoans item in borrowerLoans.Where(p => p.Ffelp))
                {
                    RI.Hit(ReflectionInterface.Key.Enter);
                }

                RI.Hit(ReflectionInterface.Key.F11);

                if (!RI.MessageCode.IsNullOrEmpty() && !RI.MessageCode.Contains("01021"))
                    return RI.Message;
            }

            return CreateIcrSchedule(appData);
        }

        /// <summary>
        /// Adds the ICR schedule in the AES screen
        /// </summary>
        /// <param name="appData">Data entered by the user from the IdrInformation form.</param>
        /// <returns></returns>
        private string CreateIcrSchedule(ApplicationData appData)
        {
            //Calculate ICR Repayment Schedule
            if (appData.BorrowerAgiReflectsCurrentIncome == true)
                RI.PutText(10, 7, (appData.Agi ?? 0).ToString("0.##"));
            else
                RI.PutText(10, 7, (appData.TotalIncome ?? 0).ToString("0.##"));

            RI.PutText(10, 34, appData.FamilySize.ToString());
            RI.PutText(10, 45, appData.State);

            if (!appData.DueDateRequested.IsNullOrEmpty())
                RI.PutText(10, 63, appData.DueDateRequested, ReflectionInterface.Key.None, true);

            var source = appData.IncomeSource;
            RI.PutText(10, 76, source.SourceCode);
            RI.PutText(11, 17, appData.DefForbId == 1 ? "Y" : "N");

            if (appData.FilingStatusId == 1)
                RI.PutText(12, 17, "S");
            else if (appData.FilingStatusId == 2 || appData.FilingStatusId == 3)
                RI.PutText(12, 17, "M");
            else if (appData.FilingStatusId == 4 || appData.FilingStatusId == 5)
                RI.PutText(12, 17, "H");
            else
            {
                if (appData.FamilySize == 1)
                    RI.PutText(12, 17, "S");
                else
                    RI.PutText(12, 17, "H");
            }

            RI.Hit(ReflectionInterface.Key.Enter);

            if (!RI.MessageCode.Contains("06312"))
                return RI.Message;

            HitF4(2);

            if (RI.MessageCode.Contains("06315"))
                return Statuses.Added.ToString();

            HitF4(1);

            return Statuses.Denied.ToString();
        }

        /// <summary>
        /// There are certain places within the script where we need to hit f4 a certain number of times.
        /// </summary>
        /// <param name="numberOfTimes"></param>
        private void HitF4(int numberOfTimes)
        {
            for (int n = 0; n < numberOfTimes; n++)
            {
                RI.Hit(ReflectionInterface.Key.F4);
            }
        }

        /// <summary>
        /// Enters in loans that the borrower has that are not serviced at Uheaa
        /// </summary>
        /// <param name="row">Starting row in the screen</param>
        /// <param name="loans">List with the other loan data</param>
        /// <param name="type">Type of loans (Borrowers (B) or Spouses(S))</param>
        /// <param name="spouseId">Id generated in that database</param>
        /// <param name="spouseSsn">Spouse Ssn</param>
        /// <returns></returns>
        private int EnterOtherLoanInformationIcr(int row, IEnumerable<OtherLoans> loans, string type, int? spouseId = null, string spouseSsn = "")
        {
            if (!loans.Any())
                return 11;
            //Other loans indicator
            RI.PutText(7, 18, "Y");
            if (spouseId.HasValue)
                RI.PutText(8, 16, spouseSsn);

            foreach (OtherLoans item in loans)
            {
                //Move onto the next page.
                if (row > 22)
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 11;
                }

                RI.PutText(row, 5, type);
                RI.PutText(row, 11, item.LoanType);
                RI.PutText(row, 23, string.Format("{0:0}", item.CalculatedOutstandingBalance));
                RI.PutText(row, 39, string.Format("{0:0}", item.CalculatedOutstandingBalance));
                RI.PutText(row, 56, item.InterestRate.ToString());
                row++;
            }

            return row;
        }

        /// <summary>
        /// Enters in loans that the borrower has that are not serviced at Uheaa
        /// </summary>
        /// <param name="row">Starting row in the screen</param>
        /// <param name="loans">List with the other loan data</param>
        /// <param name="type">Type of loans (Borrowers (B) or Spouses(S))</param>
        /// <param name="spouseId">Id generated in that database</param>
        /// <param name="spouseSsn">Spouse Ssn</param>
        /// <returns></returns>
        private int EnterOtherLoanInformation(int row, IEnumerable<OtherLoans> loans, string type, int? spouseId = null, string spouseSsn = "")
        {
            if (!loans.Any())
                return 11;
            //Other loans indicator
            RI.PutText(7, 19, "Y");
            if (spouseId.HasValue)
                RI.PutText(8, 15, spouseSsn);

            foreach (OtherLoans item in loans)
            {
                //Move onto the next page.
                if (row > 22)
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 11;
                }

                RI.PutText(row, 5, item.Ffelp ? "F" : "D");
                RI.PutText(row, 13, type);
                RI.PutText(row, 19, item.LoanType);
                RI.PutText(row, 27, item.OwnerLender);
                RI.PutText(row, 55, string.Format("{0:0}", item.CalculatedOutstandingBalance));
                if (item.MonthlyPay == null)
                    item.SetMonthlyPay();
                RI.PutText(row, 70, string.Format("{0:0}", item.MonthlyPay));
                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.MessageCode == "06186")
                    RI.Hit(ReflectionInterface.Key.Enter);
                row++;
            }

            return row;
        }

        /// <summary>
        /// Will add the data gathered for IBR or PAYE applications into the session.
        /// </summary>
        /// <param name="ssn">SSN of the borrower we are adding IDR to</param>
        /// <param name="appData">Contains all of the data entered by the users in the IdrInformation form.  This data will be used to add IDR</param>
        /// <param name="spouseSsn">SSN of borrowers spouse.  If application does not have a spouse pass empty string</param>
        /// <param name="spousesLoans">Contains all of the loan information for a spouse.  If no loan information exists pass empty list</param>
        /// <param name="borrowerLoans">Contains all of the information about a borrowers loans that are not serviced at Uheaa</param>
        /// <param name="type">Enum indicating what type of application we are processing</param>
        /// <returns>Returns the result of adding the application.  If the application was not added will return the error reason why it could not be added</returns>
        private string AddIdr(string ssn, ApplicationData appData, string spouseSsn, IEnumerable<OtherLoans> spousesLoans, IEnumerable<OtherLoans> borrowerLoans, Type type)
        {
            if (type == Type.IBR)
                RI.FastPath("TX3Z/ATL0E" + ssn);
            else//PAYE
                RI.FastPath("TX3Z/ATL36" + ssn);

            //If a message is received something went wrong.
            switch (RI.MessageCode)
            {
                case "06149":
                    return Statuses.ReviewEligibility.ToString();
                case "06365":
                    if (appData.RepaymentPlanReason == 3)
                    {
                        RI.Hit(ReflectionInterface.Key.F6);
                        if (RI.ScreenCode == "TLX38" || RI.MessageCode == "00000")
                            return Statuses.ActiveRepaymentSchedule.ToString();
                        break;
                    }
                    else
                        return Statuses.ActiveRepaymentSchedule.ToString();
                case "":
                    RI.Hit(ReflectionInterface.Key.Enter);
                    break;
                default:
                    return RI.Message;
            }

            if (appData.MaritalStatusId == 2 && spousesLoans.Any())
                RI.PutText(8, 15, spouseSsn);
            if (type == Type.PAYE)
                RI.PutText(7, 35, "P");
            else if (type == Type.REPAYE)
            {
                RI.PutText(7, 35, "R");
                RI.PutText(7, 45, appData.GradeLevel);
            }

            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa && !spousesLoans.Any() && !borrowerLoans.Any())
            {
                //The borrower has no external loans, so we do not need to enter anymore information on this screen.
                RI.PutText(7, 19, "N");
                RI.Hit(ReflectionInterface.Key.F11);
            }
            else
            {
                HasSpouseOrLoans(appData, spouseSsn, spousesLoans, borrowerLoans);
                if (!RI.MessageCode.IsNullOrEmpty() && !RI.MessageCode.Contains("01021"))
                    return RI.Message;
            }

            if (RI.ScreenCode == "TLX46")
                RI.Hit(ReflectionInterface.Key.F11);

            if (RI.MessageCode == "06147")
                return RI.Message;

            return CreateIdrSchedule(appData);
        }

        private void HasSpouseOrLoans(ApplicationData appData, string spouseSsn, IEnumerable<OtherLoans> spousesLoans, IEnumerable<OtherLoans> borrowerLoans)
        {
            int row = 11;
            row = EnterOtherLoanInformation(row, spousesLoans, "S", appData.SpouseId, spouseSsn);
            EnterOtherLoanInformation(row, borrowerLoans, "B");

            if (spousesLoans.Where(p => p.Ffelp).Count() > 0 || borrowerLoans.Where(p => p.Ffelp).Count() > 0)
                RI.Hit(ReflectionInterface.Key.Enter);

            //Moves to the next screen.
            RI.Hit(ReflectionInterface.Key.F11);
        }


        /// <summary>
        /// Creates the IDR (PAYE or IBR) in the session screens.
        /// </summary>
        /// <param name="appData">Application data entered by the user</param>
        /// <returns>The Message code given by the screen when the data is committed.</returns>
        private string CreateIdrSchedule(ApplicationData appData)
        {

            //Calculate/Deactivate Repayment Schedule
            if (appData.BorrowerAgiReflectsCurrentIncome == true)
                RI.PutText(10, 7, (appData.Agi ?? 0).ToString("0.##"));
            else
                RI.PutText(10, 7, (appData.TotalIncome ?? 0).ToString("0.##"));

            RI.PutText(10, 34, appData.FamilySize.ToString());
            RI.PutText(10, 45, appData.State);

            if (!appData.DueDateRequested.Replace(" ", "").IsNullOrEmpty())
                RI.PutText(10, 63, appData.DueDateRequested, ReflectionInterface.Key.None, true);

            var source = appData.IncomeSource;
            RI.PutText(10, 76, source.SourceCode);
            RI.PutText(11, 16, appData.DefForbId == 2 ? "Y" : "N");

            //Commit the application to the session.
            RI.Hit(ReflectionInterface.Key.Enter);

            if (!RI.MessageCode.IsIn("06156", "06194"))
                return RI.Message;

            HitF4(2);

            if (RI.MessageCode.Contains("06143"))
                return Statuses.Added.ToString();

            return Statuses.Denied.ToString();
        }
    }
}