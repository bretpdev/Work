using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace IDRXMLDATA
{
    public class DataAccess
    {
        private ProcessLogRun LogRun { get; set; }
        private LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            LDA = LogRun.LDA;
        }
        #region StoredProcedures
        [UsesSproc(DataAccessHelper.Database.Udw, "GetIDRDemosForSSN")]
        public List<BorrowerData> GetBorrowerDemos(AppBorrowerType borrower)
        {
            return LDA.ExecuteList<BorrowerData>("GetIDRDemosForSSN", DataAccessHelper.Database.Udw, new SqlParameter("SSN", borrower.SSN)).Result;
        }

        /// <summary>
        /// Look up borrower and pull back borrowerId.
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns>borrowerId or 0 if borrower doesnt exist</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.GetBorrowerIdFromSSN")]
        public List<int> GetBorrowerId(AppBorrowerType borrower)
        {
            return LDA.ExecuteList<int>("idrxmldata.GetBorrowerIdFromSSN", DataAccessHelper.Database.IncomeBasedRepaymentUheaa, new SqlParameter("SSN", borrower.SSN)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.InsertNewBorrXmlData")]
        public List<int> InsertNewBorrower(BorrowerData tempBorrower)
        {
            return LDA.ExecuteList<int>("idrxmldata.InsertNewBorrXmlData", DataAccessHelper.Database.IncomeBasedRepaymentUheaa,
                new SqlParameter("SSN", tempBorrower.SSN), new SqlParameter("AccountNumber", tempBorrower.AccountNumber),
                new SqlParameter("FirstName", tempBorrower.FirstName), new SqlParameter("LastName", tempBorrower.LastName),
                new SqlParameter("MiddleName", tempBorrower.MiddleName)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.InsertNewSpouseXmlData")]
        public List<int> InsertNewSpouse(AppBorrowerType borrower)
        {
            return LDA.ExecuteList<int>("idrxmldata.InsertNewSpouseXmlData", DataAccessHelper.Database.IncomeBasedRepaymentUheaa,
                new SqlParameter("SSN", borrower.RepaymentApplication[0].Spouse.SSN), new SqlParameter("BirthDate", borrower.RepaymentApplication[0].Spouse.Identifiers.DateOfBirth.ToShortDateString()),
                new SqlParameter("FirstName", borrower.RepaymentApplication[0].Spouse.FullName.FirstName), new SqlParameter("LastName", borrower.RepaymentApplication[0].Spouse.FullName.LastName),
                new SqlParameter("MiddleName", borrower.RepaymentApplication[0].Spouse.FullName.MiddleInitial), new SqlParameter("Separated", borrower.RepaymentApplication[0].Separated),
                new SqlParameter("AccessToIncomeInfo", borrower.RepaymentApplication[0].AccessToSpouse)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.UpdateSpouseXmlData")]
        public void UpdateSpouse(int spouseId, AppBorrowerType borrower)
        {
            LDA.Execute("idrxmldata.UpdateSpouseXmlData", DataAccessHelper.Database.IncomeBasedRepaymentUheaa,
                new SqlParameter("SpouseId", spouseId), new SqlParameter("Separated", borrower.RepaymentApplication[0].Separated), new SqlParameter("AccessToIncomeInfo", borrower.RepaymentApplication[0].AccessToSpouse));
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.InsertNewXmlApplication")]
        public int? CreateNewApplication(BorrowerData borrowerToAdd, AppBorrowerType borrower, int threadId)
        {
            int? repaymentPlanType = DetermineRepaymentPlan();
            int? repaymentReason = DetermineRepaymentReason(borrower, threadId);
            int? filingStatus = DetermineFilingStatus(borrower, threadId);
            int? deferementForbearance = DetermineDeferementForbearance(borrower, threadId);
            int? maritalStatus = DetermineMaritalStatus(borrower, threadId);
            int familySize = DetermineFamilySize(borrower, maritalStatus);
            bool includeSpouse = DetermineIncludeSpouse(borrower, maritalStatus);

            string taxYear = null, AGI = null, currentIncome = null;
            bool? AgiIncome = null;

            currentIncome = borrower.RepaymentApplication[0].IncomeInformation.AGIReflectsCurrentIncome;
            //PIR
            if (currentIncome == null)
                AgiIncome = null;
            else if (currentIncome.ToUpper() == "Y")
                AgiIncome = true;
            else if (currentIncome.ToUpper() == "N")
                AgiIncome = false;
            //Node occasionally does not exist.  Check for existence before using values
            if (borrower.RepaymentApplication[0].IncomeInformation.IRSData != null)
            {
                taxYear = borrower.RepaymentApplication[0].IncomeInformation.IRSData.TaxYear;
                AGI = borrower.RepaymentApplication[0].IncomeInformation.IRSData.AdjustedGrossIncome;
            }
            return LDA.ExecuteSingle<int>("idrxmldata.InsertNewXmlApplication", DataAccessHelper.Database.IncomeBasedRepaymentUheaa,
                new SqlParameter("ApplicationId", borrower.RepaymentApplication[0].ApplicationID),
                new SqlParameter("RepaymentPlanTypeRequested", repaymentPlanType),
                new SqlParameter("JointRepaymentPlanRequestIndicator", borrower.RepaymentApplication[0].JointRepaymentPlanRequest),
                new SqlParameter("RepaymentPlanReasonId", repaymentReason),
                new SqlParameter("TaxYear", taxYear?.ToIntNullable()),
                new SqlParameter("FilingStatusId", filingStatus),
                new SqlParameter("AdjustedGrossIncome", AGI?.ToDecimalNullable()),
                new SqlParameter("AgiReflectsCurrentIncome", AgiIncome),
                new SqlParameter("TaxesFiledFlag", taxYear != null ? true : borrower.RepaymentApplication[0].IncomeInformation.TaxesFiledFlag),
                new SqlParameter("CurrentDefermentForbearanceId", deferementForbearance),
                new SqlParameter("NumberChildren", borrower.RepaymentApplication[0].Children?.ToIntNullable() ?? 0),
                new SqlParameter("NumberDependents", borrower.RepaymentApplication[0].Dependents?.ToIntNullable() ?? 0),
                new SqlParameter("MaritalStatus", maritalStatus),
                new SqlParameter("PublicServiceEmployment", borrower.RepaymentApplication[0].PSLF),
                new SqlParameter("ReducedPaymentForbearance", borrower.RepaymentApplication[0].ReducedPaymentForbearance),
                new SqlParameter("SpouseId", borrowerToAdd.SpouseId),
                new SqlParameter("FamilySize", familySize),
                new SqlParameter("IncludeSpouse", includeSpouse),
                new SqlParameter("SupportingDocumentationRequired", !AgiIncome ?? true),
                new SqlParameter("LowestPlan", false)).Result; /*uheaa cannot add lowest plan*/
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.GetExistingEAppXmlData")]
        public List<ExistingApp> GetExistingApp(string applicationID)
        {
            return LDA.ExecuteList<ExistingApp>("idrxmldata.GetExistingEAppXmlData", DataAccessHelper.Database.IncomeBasedRepaymentUheaa, new SqlParameter("Eapp", applicationID)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.InsertNewStatusHistoryXmlData")]
        public void CreateNewStatusHistory(int? repayPlanId)
        {
            LDA.Execute("idrxmldata.InsertNewStatusHistoryXmlData", DataAccessHelper.Database.IncomeBasedRepaymentUheaa, new SqlParameter("RepaymentPlanTypeId", repayPlanId));
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.InsertNewRepaymentPlanSelectedXmlData")]
        public int CreateNewSelectedPlan(int? applicationId, AppBorrowerType borrower, BorrowerData borrowerToAdd, int threadId)
        {

            int? repayPlan = DetermineRepaymentPlan(); //Grab repayment plan from file
                                                       // int? repaymentTypeId = DataAccessHelper.ExecuteSingle<int>("GetRepaymentTypeIdForXML", conn);
            int? repaymentTypeId;
            switch (repayPlan)
            {
                case 1:
                    repaymentTypeId = 1; //IBR
                    break;
                case 2:
                    repaymentTypeId = 3; //ICR
                    break;
                case 3:
                    repaymentTypeId = 4; //PAYE
                    break;
                case 8:
                case 12:
                default:
                    repaymentTypeId = 5; //REPAYE
                    break;
            }

            return DataAccessHelper.ExecuteSingle<int>("idrxmldata.InsertNewRepaymentPlanSelectedXmlData", DataAccessHelper.Database.IncomeBasedRepaymentUheaa, new SqlParameter("ApplicationId", applicationId), new SqlParameter("RepaymentTypeId", repaymentTypeId));
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.InsertNewXmlOtherLoans")]
        public void CreateOtherLoans(int? appId, AppBorrowerType borrower, UnderlyingLoansType loan, BorrowerData borrowerToAdd, int threadId)
        {
            string LoanType = DetermineLoanType(borrower, loan, threadId);
            bool spouseIndicator = DetermineSpouseIndicator(borrower, loan);

            if (loan.LoanHolder[0].LoanHolderCode != "700126") //Dont add uheaa lender
            {
                LDA.Execute("idrxmldata.InsertNewXmlOtherLoans", DataAccessHelper.Database.IncomeBasedRepaymentUheaa,
                new SqlParameter("ApplicationId", appId), //appId is declared as nullable for compiler only.  It should never be null
                new SqlParameter("SpouseIndicator", spouseIndicator),
                new SqlParameter("OwnerLender", loan.LoanHolder[0].LoanHolderCode),
                new SqlParameter("LoanType", LoanType),
                new SqlParameter("OutstandingBalance", loan.RepaymentInformation.OutstandingPrincipalBalance),
                new SqlParameter("InterestRate", loan.InterestRate),
                new SqlParameter("OutstandingInterest", loan.RepaymentInformation.OutstandingAccruedInterest));
            }
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "idrxmldata.InsertNewXmlLoans")]
        public void CreateNewLoan(int? appId, int borrowerId, UnderlyingLoansType loan)
        {
            if (loan.LoanHolder[0].LoanHolderCode == "700126") //We are the lender 
            {
                LDA.Execute("idrxmldata.InsertNewXmlLoans", DataAccessHelper.Database.IncomeBasedRepaymentUheaa,
                new SqlParameter("BorrowerId", borrowerId),
                new SqlParameter("ApplicationId", appId)); //appId is declared as nullable for compiler only.  It should never be null

            }
        }

        #endregion

        #region EnumTranslation
        /// <summary>
        /// Converts an Enum to numerical values to insert into database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns></returns>
        public int? DetermineMaritalStatus(AppBorrowerType borrower, int threadId)
        {
            int? maritalStatus = null;
            switch (borrower.RepaymentApplication[0].MaritalStatus)
            {
                case MaritalStatusType.Single:
                    maritalStatus = 1;
                    break;
                case MaritalStatusType.Married:
                    maritalStatus = 2;
                    break;
                default:
                    string message = string.Format("ThreadId: {0}; Unknown marital status code received: {1} for applicationId: {2}.", threadId, borrower.RepaymentApplication[0].MaritalStatus, borrower.RepaymentApplication[0].ApplicationID);
                    AddNotification(message);
                    break;
            }
            return maritalStatus;
        }

        /// <summary>
        /// Converts an Enum to numerical values to insert into database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns></returns>
        private int? DetermineDeferementForbearance(AppBorrowerType borrower, int threadId)
        {
            int? deferementForbearance = null;
            switch (borrower.RepaymentApplication[0].DefermentForbearance)
            {
                case DefermentForbearanceType.No:
                    deferementForbearance = 1;
                    break;
                case DefermentForbearanceType.PayNow:
                    deferementForbearance = 2;
                    break;
                case DefermentForbearanceType.DeferPay:
                    deferementForbearance = 3;
                    break;
                default:
                    string message = string.Format("ThreadId: {0}; Unknown Deferment / Forbearance status code received: {1} for applicationId: {2}.", threadId, borrower.RepaymentApplication[0].DefermentForbearance, borrower.RepaymentApplication[0].ApplicationID);
                    AddNotification(message);
                    break;
            }
            return deferementForbearance;
        }

        /// <summary>
        /// Converts an Enum to numerical values to insert into database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns></returns>
        private int? DetermineFilingStatus(AppBorrowerType borrower, int threadId)
        {
            int? filingStatus = null;
            if (borrower.RepaymentApplication[0].IncomeInformation.IRSData != null)
            {
                switch (borrower.RepaymentApplication[0].IncomeInformation.IRSData.FilingStatus)
                {
                    case FilingStatusType.Single:
                        filingStatus = 1;
                        break;
                    case FilingStatusType.MarriedFilingJointly:
                        filingStatus = 2;
                        break;
                    case FilingStatusType.MarriedFilingSeparately:
                        filingStatus = 3;
                        break;
                    case FilingStatusType.HeadofHousehold:
                        filingStatus = 4;
                        break;
                    case FilingStatusType.QualifyingWidower:
                        filingStatus = 5;
                        break;
                    default:
                        string message = string.Format("ThreadId: {0}; Unknown Filing status code received: {1} for applicationId: {2}.", threadId, borrower.RepaymentApplication[0].IncomeInformation.IRSData.FilingStatus, borrower.RepaymentApplication[0].ApplicationID);
                        AddNotification(message);
                        break;
                }
            }
            return filingStatus;
        }

        /// <summary>
        /// Converts an Enum to numerical values to insert into database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns></returns>
        private int? DetermineRepaymentReason(AppBorrowerType borrower, int threadId)
        {
            int? repaymentReason = null;
            switch (borrower.RepaymentApplication[0].RepaymentReason)
            {
                case RepaymentReasonType.New:
                    repaymentReason = 1;
                    break;
                case RepaymentReasonType.Recertify:
                    repaymentReason = 2;
                    break;
                case RepaymentReasonType.Recalculate:
                    repaymentReason = 3;
                    break;
                case RepaymentReasonType.ChangePlan:
                    repaymentReason = 4;
                    break;
                default:
                    string message = string.Format("ThreadId: {0}; Unknown Repayment reason code received: {1} for applicationId: {2}.", threadId, borrower.RepaymentApplication[0].RepaymentReason, borrower.RepaymentApplication[0].ApplicationID);
                    AddNotification(message);
                    break;
            }
            return repaymentReason;
        }

        /// <summary>
        /// Converts an Enum to numerical values to insert into database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns></returns>
        private int? DetermineRepaymentPlan()
        {
            return 1; /*Uheaa ALWAYS processes as IBR regardless of input*/
        }

        /// <summary>
        /// Converts an Enum to string loan type values to insert into database
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns></returns>
        public string DetermineLoanType(AppBorrowerType borrower, UnderlyingLoansType loan, int threadId)
        {
            string loanType = null;
            string message = "";
            switch (loan.AwardType)
            {
                case AwardTypeType.DirectConsolidationPLUS:
                    loanType = "DLPCNS";
                    break;
                case AwardTypeType.DirectGraduatePLUS:
                    loanType = "DLPLGB";
                    break;
                case AwardTypeType.DirectParentPLUS:
                    loanType = "DLPLUS";
                    break;
                case AwardTypeType.DirectSubsidizedConsolidation:
                    loanType = "DLSCNS";
                    break;
                case AwardTypeType.DirectUnsubsidized:
                    loanType = "DLUNST";
                    break;
                case AwardTypeType.DirectUnsubsidizedConsolidation:
                    loanType = "DLUCNS";
                    break;
                case AwardTypeType.DirectSubsidized:
                case AwardTypeType.DirectSubsidizedSULA:
                    loanType = "DLSTFD";
                    break;
                case AwardTypeType.SubsidizedFederalStafford:
                    loanType = "STFFRD";
                    break;
                case AwardTypeType.UnsubsidizedFederalStafford:
                    loanType = "UNSTFD";
                    break;
                case AwardTypeType.FederalParentPLUS:
                    loanType = "PLUS";
                    break;
                case AwardTypeType.FederalGraduatePLUS:
                    loanType = "PLUSGB";
                    break;
                case AwardTypeType.FederalInsuredStudent:
                    loanType = "FISL";
                    break;
                case AwardTypeType.FederalPerkins:
                    loanType = "PERKNS";
                    break;
                case AwardTypeType.HealthEducation:
                    loanType = "HEAL";
                    break;
                case AwardTypeType.HealthProfessions:
                    loanType = "HPSL";
                    break;
                case AwardTypeType.SubsidizedFederalConsolidation:
                case AwardTypeType.DirectSubsidizedConsolidationSULA:
                    loanType = "SUBCNS";
                    break;
                case AwardTypeType.UnsubsidizedFederalConsolidation:
                    loanType = "UNCNS";
                    break;
                case AwardTypeType.FFELConsolidation:
                    loanType = "CNSLDN";
                    break;
                case AwardTypeType.FederalSupplemental:
                    loanType = "SLS";
                    break;
                case AwardTypeType.IneligibleEducationsLoans:
                case AwardTypeType.NationalDefenseStudent:
                case AwardTypeType.NationalDirectStudent:
                case AwardTypeType.AuxiliaryLoans:
                case AwardTypeType.DisadvantagedStudents:
                case AwardTypeType.NursingStudent:
                case AwardTypeType.GuaranteedStudent:
                    message = string.Format("ThreadId: {0}; Unsupported Loan type code received: {1} for applicationId: {2}.", threadId, loan.AwardType, borrower.RepaymentApplication[0].ApplicationID);
                    AddNotification(message);
                    break;
                default:
                    message = string.Format("ThreadId: {0}; Unrecognized Loan type code received: {1} for applicationId: {2}.", threadId, loan.AwardType, borrower.RepaymentApplication[0].ApplicationID);
                    AddNotification(message);
                    break;
            }
            return loanType;
        }

        private bool DetermineSpouseIndicator(AppBorrowerType borrower, UnderlyingLoansType loan)
        {
            if (borrower.RepaymentApplication[0].Spouse != null)
            {
                return borrower.RepaymentApplication[0].Spouse.SSN == loan.AwardSSN ? true : false; //Any loan thats not my spouses' is mine
            }
            return false;
        }

        private int DetermineFamilySize(AppBorrowerType borrower, int? maritalStatus)
        {
            int familySize = 1; /*default to 1 for the borrower*/
            string children = borrower.RepaymentApplication[0].Children ?? "0";
            string dependents = borrower.RepaymentApplication[0].Dependents ?? "0";
            if (borrower.RepaymentApplication[0].IncomeInformation != null)
            {
                if (maritalStatus == 1) /* single */
                    familySize = familySize + Int32.Parse(children) + Int32.Parse(dependents);
                else if (maritalStatus == 2 && borrower.RepaymentApplication[0].Separated && borrower.RepaymentApplication[0].AccessToSpouse) /* married but separated with access*/
                    familySize = familySize + Int32.Parse(children) + Int32.Parse(dependents) + 1; /*spouse included*/
                else if (maritalStatus == 2 && borrower.RepaymentApplication[0].Separated && !borrower.RepaymentApplication[0].AccessToSpouse) /* married but separated no access*/
                    familySize = familySize + Int32.Parse(children) + Int32.Parse(dependents);
                else if (maritalStatus == 2 && !borrower.RepaymentApplication[0].Separated && borrower.RepaymentApplication[0].AccessToSpouse) /*married, together, with access*/
                    familySize = familySize + Int32.Parse(children) + Int32.Parse(dependents) + 1; /*Spouse counts*/
                else if (maritalStatus == 2 && !borrower.RepaymentApplication[0].Separated && !borrower.RepaymentApplication[0].AccessToSpouse) /*married, together, no access*/
                    familySize = familySize + Int32.Parse(children) + Int32.Parse(dependents);
                else
                    familySize = familySize + Int32.Parse(children) + Int32.Parse(dependents);
            }
            return familySize;
        }

        private bool DetermineIncludeSpouse(AppBorrowerType borrower, int? maritalStatus)
        {
            bool includeSpouse = false;
            if (borrower.RepaymentApplication[0].IncomeInformation != null)
            {
                if (maritalStatus == 2 && borrower.RepaymentApplication[0].Separated && borrower.RepaymentApplication[0].AccessToSpouse) /* married but separated with access*/
                    includeSpouse = true;
                else if (maritalStatus == 2 && !borrower.RepaymentApplication[0].Separated && borrower.RepaymentApplication[0].AccessToSpouse) /*married, together, with access*/
                    includeSpouse = true;
            }
            return includeSpouse;
        }

        #endregion
        private void AddNotification(string message)
        {
            ProcessLogger.AddNotification(LogRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly());

            Console.WriteLine(message);
        }
    }
}