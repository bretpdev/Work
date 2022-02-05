using IDRUSERPRO.Object_Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace IDRUSERPRO
{
    public class DataAccess
    {
        private DataAccessHelper.Database Db => DataAccessHelper.Database.IncomeBasedRepaymentUheaa;
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, false, DataAccessHelper.CurrentRegion);
        }

        /// <summary>
        /// Executes spGetApplicationSource
        /// </summary>
        /// <returns>Returns a list of Application Sources which contains all sources from the Application_Source table in the IncomeBasedRepaymentUheaa database</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetApplicationSource")]
        public List<ApplicationSource> GetApplicationSource()
        {
            return LDA.ExecuteList<ApplicationSource>("spGetApplicationSource", Db).Result;
        }

        public string GetAccountNumber(string ssn)
        {
            return LDA.ExecuteSingle<string>("spGetAccountNumberFromSSN", DataAccessHelper.Database.Udw, SqlParams.Single("Ssn", ssn)).Result;
        }

        public List<short> GetLoansForCoBorrower(string accountNumber)
        {
            return LDA.ExecuteList<short>("[idruserpro].[GetLoansForCoBorrower]", DataAccessHelper.Database.Udw, SqlParams.Single("AccountNumber", accountNumber)).Result;
        }

        /// <summary>
        /// Gets the borrowers repayment plan requested value
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetRepaymentPlanRequested")]
        public int? GetRepaymentPlanRequested(int appId)
        {
            return LDA.ExecuteSingle<int?>("spGetRepaymentPlanRequested", Db, new SqlParameter("AppId", appId)).Result;
        }

        /// <summary>
        /// Gets data specific to electronic apps
        /// </summary>
        /// <param name="eapp"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetExistingElectronicAppData")]
        public List<DuplicateEappVerification> GetDuplicateEApps(string eapp)
        {
            return LDA.ExecuteList<DuplicateEappVerification>("GetExistingElectronicAppData", Db, eapp.ToSqlParameter("Eapp")).Result;
        }

        /// <summary>
        /// Checks the user role to see if they are a supervisor
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "GetUsersRole")]
        public bool UserIsSupervisor()
        {
            return DataAccessHelper.ExecuteSingle<int>("GetUsersRole", DataAccessHelper.Database.Csys, Environment.UserName.ToSqlParameter("WindowsUserId")) == 5;
        }

        /// <summary>
        /// Executes spGetRepaymentPlanReason
        /// </summary>
        /// <returns>Returns a list of RepaymentPLanReason which contains all reasons from the Repayment_Plan_Reason table in the IncomeBasedRepaymentUheaa database</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetRepaymentPlanReason")]
        public List<RepaymentPlanReason> GetRepaymentPlanReasons()
        {
            return LDA.ExecuteList<RepaymentPlanReason>("spGetRepaymentPlanReason", Db).Result;
        }

        /// <summary>
        /// Executes spGetFilingStatuses
        /// </summary>
        /// <returns>Returns a list of FillingStatus which contains all statuses in the Filing_Statuses table in the IncomeBasedRepaymentUheaa database</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetFilingStatuses")]
        public List<FilingStatus> GetFilingStatuses()
        {
            return LDA.ExecuteList<FilingStatus>("spGetFilingStatuses", Db).Result;
        }

        /// <summary>
        /// Executes spGetIncomeSource
        /// </summary>
        /// <returns>Returns a List of IncomeSource which contains all Income Sources from Income_Source table in the IncomeBasedRepaymentUheaa database</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetIncomeSources")]
        public List<IncomeSource> GetIncomeSources()
        {
            return LDA.ExecuteList<IncomeSource>("GetIncomeSources", Db).Result;
        }

        /// <summary>
        /// Executes spGetBorrowerEligibility
        /// </summary>
        /// <returns>Returns a List of BorrowerEligibility which contains all Eligibility Code from Borrower_Eligibility table in the IncomeBasedRepaymentUheaa database</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetBorrowerEligibility")]
        public List<BorrowerEligibility> GetBorrowerEligibility()
        {
            return LDA.ExecuteList<BorrowerEligibility>("spGetBorrowerEligibility", Db).Result;
        }

        /// <summary>
        /// Executes spGetPlanStatuses
        /// </summary>
        /// <returns>Returns a List of RepaymentPlanStatus which contains all Repayment Plan Status from Repayment_Plan_Type_Status table in the IncomeBasedRepaymentUheaa database</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetPlanStatuses")]
        public List<RepaymentPlanStatus> GetRepaymentPlanStatuses()
        {
            return LDA.ExecuteList<RepaymentPlanStatus>("spGetPlanStatuses", Db).Result;
        }

        /// <summary>
        /// Executes GetMaritalStatus
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetMaritalStatus")]
        public List<MaritalStatus> GetMaritalStatus()
        {
            return LDA.ExecuteList<MaritalStatus>("GetMaritalStatus", Db).Result;
        }

        /// <summary>
        /// Executes spGetSubStatuses
        /// </summary>
        /// <param name="status">Integer value for the status see table Repayment_Plan_Type_Status for description of integer value </param>
        /// <param name="reasonId">Integer value for reason code see table Repayment_Plan_Reason  for description of integer value</param>
        /// <returns>Returns a list of SubStatuses for the given status and reason id</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetSubStatuses")]
        public List<SubStatuses> GetSubstatuses(int status, int reasonId)
        {
            return LDA.ExecuteList<SubStatuses>("spGetSubStatuses", Db,
                Sp("Status", status),
                Sp("ReasonId", reasonId)).Result.OrderBy(p => p.SubStatus).ToList();
        }

        /// <summary>
        /// Executes spGetPlanTypes
        /// </summary>
        /// <returns>Returns a list of PlanType which contains all Repayment Plan Type from Repayment_Plan_Type table in the IncomeBasedRepaymentUheaa database</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetPlanTypes")]
        public List<PlanType> GetPlanTypes()
        {
            return LDA.ExecuteList<PlanType>("spGetPlanTypes", Db).Result;
        }

        /// <summary>
        /// Checks to see if the given borrower already exist if they do not exist it will insert a new record into the database
        /// </summary>
        /// <param name="bData">Object containing all of the borrower information</param>
        /// <returns>The borrower id generated by the database, if the borrower already exists will not generate a new id it will use the existing id</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetBorrowerData")]
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spInsertBorrowerData")]
        public int CheckAndInsertBorrowerData(BorrowerInfo bData)
        {
            int borrowerId = LDA.ExecuteList<int>("spGetBorrowerData", Db, Sp("AccountNumber", bData.AccountNumber)).Result.SingleOrDefault();

            if (borrowerId < 1)
                borrowerId = LDA.ExecuteList<int>("spInsertBorrowerData", Db, DataAccessHelper.GenerateSqlParamsFromObject(bData).ToArray()).Result.SingleOrDefault();

            return borrowerId;
        }

        /// <summary>
        /// Gets an ssn for an app id
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetSsnFromAppId")]
        public string GetSsnFromAppId(int appId)
        {
            return LDA.ExecuteSingle<string>("GetSsnFromAppId", Db, Sp("AppId", appId)).Result;
        }

        /// <summary>
        /// Inserts data entered by the user into the IncomeBasedRepaymentUheaa database
        /// </summary>
        /// <param name="data">Data entered in the IdrInformation form</param>
        /// <param name="sData">Spouse Data entered.  If no Spouse data exist send empty list</param>
        /// <returns>Returns the application id generated by the database</returns>

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spInsertApplicationData")]
        public int? InsertApplicationData(ApplicationData data, string userId)
        {
            List<SqlParameter> sqlParams = SqlParams.Except(data, d => d.ApplicationId).ToList();
            sqlParams.Add(Sp("UpdatedBy", Environment.UserName));
            sqlParams.Add(Sp("borrower_paystubs", ToDataTable(data.BorrowerStubs)));
            sqlParams.Add(Sp("spouse_paystubs", ToDataTable(data.SpouseStubs)));
            sqlParams.Add(Sp("UserId", userId));

            int? result = LDA.ExecuteList<int>("spInsertApplicationData", Db, sqlParams.ToArray()).Result.SingleOrDefault();
            if (result == 0)
                result = null;
            return result;
        }
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spInsertOrUpdateSpouseData")]
        public void InsertOrUpdateSpouseData(SpouseData spouse)
        {
            //Insert data into spouse table to get spouse id.
            SqlParameter[] spouseParameters = null;
            if (spouse.SpouseId.HasValue)
                spouseParameters = SqlParams.Generate(spouse);
            else
                spouseParameters = SqlParams.Except(spouse, d => d.SpouseId);
            spouse.SpouseId = LDA.ExecuteSingle<int>("spInsertOrUpdateSpouseData", Db, spouseParameters).Result;
        }

        /// <summary>
        /// Insert data about borrowers loans.
        /// </summary>
        /// <param name="loans">Data gathered from TS26 </param>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spInsertLoanData")]
        public void InsertLoanData(List<Ts26Loans> loans)
        {
            foreach (Ts26Loans item in loans)
            {
                var sqlParams = SqlParams.Generate(item);
                LDA.Execute("spInsertLoanData", Db, sqlParams);
            }
        }

        /// <summary>
        /// Insert a record into the history table
        /// </summary>
        /// <param name="appId">App id to insert</param>
        /// <param name="active">indicates if the app is active</param>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "InsertStatusHistory")]
        public void InsertApplicationStatusHistory(int appId, bool active)
        {
            var parameters = new List<SqlParameter>()
            {
                Sp("ApplicationId", appId),
                Sp("UpdatedBy", Environment.UserName),
                Sp("Active", active)
            };

            LDA.Execute("InsertStatusHistory", Db, parameters.ToArray());
        }

        /// <summary>
        /// Executes spInsertSelectedPlan
        /// </summary>
        /// <param name="items">Object containing Application Id and Repayment Type Id</param>
        /// <returns>Retuns the repayment plan type id generated in the Repayment Plan Selected table.</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spInsertSelectedPlan")]
        public int InsertPlanSelected(object items)
        {
            return (int)LDA.ExecuteSingle<decimal>("spInsertSelectedPlan", Db, DataAccessHelper.GenerateSqlParamsFromObject(items).ToArray()).Result;
        }

        /// <summary>
        /// Insert a new history record when the Application Status changes
        /// </summary>
        /// <param name="items">Object containing RepaymentPlanTypeId, Mapping Id, and Created By string</param>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spInsertStatusHistory")]
        public void InsertStatusHistory(object items)
        {
            LDA.Execute("spInsertStatusHistory", Db, DataAccessHelper.GenerateSqlParamsFromObject(items).ToArray());
        }

        /// <summary>
        /// Will get the most recent application for a given account number.
        /// </summary>
        /// <param name="accountNumber">Account number of borrower</param>
        /// <returns>An ApplicationData object with all of the application information.</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetExistingApplication")]
        public ApplicationData GetExistingAppData(int appId)
        {
            List<ApplicationData> data = LDA.ExecuteList<ApplicationData>("spGetExistingApplication", Db, new SqlParameter("AppId", appId)).Result;
            if (data.Count == 0)
                return null;
            else
            {
                var app = data.Single();
                if (app.BorrowerAgiReflectsCurrentIncome != true)
                    app.Agi = null;
                return app;
            }
        }

        /// <summary>
        /// Gets the application status for a given application id
        /// </summary>
        /// <param name="appId">Application Id to look up</param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetExistingAppStatus")]
        public List<string> GetExistingAppStatus(int? appId)
        {
            return LDA.ExecuteList<string>("spGetExistingAppStatus", Db, new SqlParameter("AppId", appId)).Result;
        }

        /// <summary>
        /// Get the Application type processed for an existing application
        /// </summary>
        /// <param name="appId">Application Id to look up</param>
        /// <returns>Returns the type processed</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetExistingAppTypeProcessed")]
        public string GetExistingAppTypeProcessed(int? appId)
        {
            return LDA.ExecuteList<string>("spGetExistingAppTypeProcessed", Db, new SqlParameter("AppId", appId)).Result.SingleOrDefault();
        }

        /// <summary>
        /// Updates an existing application 
        /// </summary>
        /// <param name="data">Application data entered by the user in the IdrInformation form</param>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spUpdateApp")]
        public bool UpdateApp(ApplicationData data, string userId)
        {
            List<SqlParameter> sqlParams = SqlParams.Generate(data).ToList();
            sqlParams.Add(Sp("UpdatedBy", Environment.UserName));
            sqlParams.Add(Sp("borrower_paystubs", ToDataTable(data.BorrowerStubs)));
            sqlParams.Add(Sp("spouse_paystubs", ToDataTable(data.SpouseStubs)));
            sqlParams.Add(Sp("UserId", userId));
            return LDA.ExecuteSingle<bool>("spUpdateApp", Db, sqlParams.ToArray()).Result;
        }

        /// <summary>
        /// Executes spGetRepaymentPlanTypeId
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <returns>Integer values for the repayment plan type</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetRepaymentPlanTypeId")]
        public RepaymentPlanType GetRepaymentPlanTypeId(int? appId)
        {
            return LDA.ExecuteList<RepaymentPlanType>("spGetRepaymentPlanTypeId", Db, new SqlParameter("AppId", appId)).Result.SingleOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetRepaymentPlanTypeRequested")]
        public List<RepaymentPlanTypeRequested> GetRepaymentPlayTypeRequested()
        {
            return LDA.ExecuteList<RepaymentPlanTypeRequested>("GetRepaymentPlanTypeRequested", Db).Result;
        }

        /// <summary>
        /// Executes spGetExistingSpouse
        /// </summary>
        /// <param name="spouseId">Spouse Id to look up</param>
        /// <returns>A SposesData object for the given Application Id</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetExistingSpouse")]
        public SpouseData GetExistingSpouse(int? spouseId)
        {
            return LDA.ExecuteSingle<SpouseData>("spGetExistingSpouse", Db, new SqlParameter("SpouseId", spouseId ?? 0)).Result;
        }

        /// <summary>
        /// Enters other loans that were entered by the user.
        /// </summary>
        /// <param name="spouse">List of OtherLoans containing loans for the borrowers Spouse.</param>
        /// <param name="borrowers">List of OtherLoans containing loans for the borrower.</param>
        /// <param name="appId">Application Id</param>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spDeleteOtherLoans")]
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spInsertOtherLoans")]
        public void InsertOtherLoans(IEnumerable<OtherLoans> spouse, IEnumerable<OtherLoans> borrowers, int appId)
        {
            LDA.Execute("spDeleteOtherLoans", Db, new SqlParameter("AppId", appId));
            foreach (OtherLoans item in spouse.Union(borrowers))
            {
                item.ApplicationId = appId;
                LDA.Execute("spInsertOtherLoans", Db, Sp("ApplicationId", item.ApplicationId), Sp("SpouseIndicator", item.SpouseIndicator),
                    Sp("LoanType", item.LoanType), Sp("OwnerLender", item.OwnerLender), Sp("OutstandingBalance", item.OutstandingBalance),
                    Sp("OutstandingInterest", item.OutstandingInterest), Sp("MonthlyPay", item.MonthlyPay), Sp("Interestrate", item.InterestRate),
                    Sp("Ffelp", item.Ffelp));
            }
        }

        /// <summary>
        /// Executes spGetOtherLoans
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <param name="spouse"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetOtherLoans")]
        public List<OtherLoans> GetOtherLoans(int? appId, bool spouse, string ssn)
        {
            List<OtherLoans> results = new List<OtherLoans>();

            if (appId.HasValue)
                results = LDA.ExecuteList<OtherLoans>("spGetOtherLoans", Db, Sp("AppId", appId.Value), Sp("Type", spouse)).Result;

            foreach (var loan in results)
                loan.SpouseIndicator = spouse;
            return results;
        }

        /// <summary>
        /// Gets all loan programs from CSYS
        /// </summary>
        /// <returns>A list containing all of the loan programs</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetLoanPrograms")]
        public List<LoanPrograms> GetLoanPrograms()
        {
            return LDA.ExecuteList<LoanPrograms>("GetLoanPrograms", Db).Result;
        }


        /// <summary>
        /// Executes spGetEligibilityCode
        /// </summary>
        /// <param name="id">Eligibility Id</param>
        /// <returns>Returns the system code for the given integer value</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetEligibilityCode")]
        public string GetBorrowerEligibilityCode(int? id)
        {
            return LDA.ExecuteSingle<string>("spGetEligibilityCode", Db, new SqlParameter("Id", id)).Result;
        }

        /// <summary>
        /// Executes spGetArcAndComment
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <returns>Returns an object containing the dtat needed to add an activy comment and letter</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetArcAndComment")]
        public CommentLetterData GetArcAndLetterdata(int? appId)
        {
            List<CommentLetterData> data = LDA.ExecuteList<CommentLetterData>("spGetArcAndComment", Db, new SqlParameter("AppId", appId)).Result;
            return data.Where(p => p.HisId == data.Max(q => q.HisId)).LastOrDefault();
        }

        /// <summary>
        /// Updates a given application with the new information entered by the user in the IdrInformation form
        /// </summary>
        /// <param name="repayPlanTypeId"> Repayment Plan id</param>
        /// <param name="newId">New repayment plan</param>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spUpdateSelectedPlan")]
        public void UpdatePlanSelected(int repayPlanTypeId, int newId)
        {
            LDA.Execute("spUpdateSelectedPlan", Db, new SqlParameter("repayPlanTypeId", repayPlanTypeId), new SqlParameter("newRepayId", newId));
        }

        /// <summary>
        /// Executes spGetExistingLoans
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <returns>Gets loans that were entered by a user when processing an existing application</returns>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "spGetExistingLoans")]
        public List<Ts26Loans> GetExistingLoans(int? appId)
        {
            var loans = LDA.ExecuteList<Ts26Loans>("spGetExistingLoans", Db, new SqlParameter("AppId", appId)).Result;
            loans.ForEach(o => o.IsEligible = !o.LoanType.IsIn(Ts26Results.InvalidLoans));
            return loans;
        }

        /// <summary>
        /// Deletes a spouse form an application
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "DeleteExistingSpouse")]
        public void DeleteSpouse(int spouseId)
        {
            LDA.Execute("DeleteExistingSpouse", Db, new SqlParameter("SpouseId", spouseId));
        }

        /// <summary>
        /// Gets applications for a given SSN
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetIdentifiedAccounts")]
        public List<IdentifiedApplication> GetIdentifiedApplication(string ssn)
        {
            return LDA.ExecuteList<IdentifiedApplication>("GetIdentifiedAccounts", Db,
                new SqlParameter("SSN", ssn)).Result;
        }

        /// <summary>
        /// Gets Defment/Forbearance options
        /// </summary>
        public List<DefForbData> GetDefForbOptions()
        {
            return LDA.ExecuteList<DefForbData>("GetDefForbOptions", Db).Result;
        }

        /// <summary>
        /// Determines if borrower has released loans on DB.
        /// </summary>
        public BorrowerLoanDebtStatus GetBorrowerLoanDebtStatus(string ssn)
        {
            return LDA.ExecuteSingle<BorrowerLoanDebtStatus>("[idruserpro].[GetBorrowerLoanDebtStatus]", DataAccessHelper.Database.Udw, Sp("SSN", ssn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetPreviousAppFamilySize")]
        public int? GetPreviousAppFamilySize(string ssn, int appId = 0)
        {
            return LDA.ExecuteList<int?>("GetPreviousAppFamilySize", Db, SqlParams.Single("SSN", ssn), SqlParams.Single("CurrentAppId", appId)).Result.SingleOrDefault();
        }

        public List<string> GetMissingDocReasons()
        {
            return LDA.ExecuteList<string>("GetMissingDocumentationReasons", Db).Result;
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetPovertyGuideline")]
        public PovertyGuideline GetPovertyGuideline()
        {
            return LDA.ExecuteSingle<PovertyGuideline>("GetPovertyGuideline", Db).Result;
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetApplicationPaystubs")]
        public List<PayStubs> GetApplicationPaystubs(int application_id, bool isSpouse)
        {
            return LDA.ExecuteList<PayStubs>("GetApplicationPaystubs", Db, Sp("application_id", application_id), Sp("is_spouse", isSpouse)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetStateCodes")]
        public List<string> GetStateCodes()
        {
            var list = LDA.ExecuteList<string>("GetStateCodes", DataAccessHelper.Database.Bsys).Result;
            if (list.Contains("TT")) //Trust Territories used by other applications, we don't support it
                list.Remove("TT");
            if (!list.Contains("CZ")) //Panama Canal Zone
                list.Add("CZ");
            return list;
        }

        private DataTable ToDataTable(IEnumerable<PayStubs> paystubs)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ftw", typeof(decimal));
            dt.Columns.Add("gross", typeof(decimal));
            dt.Columns.Add("pre_tax_deductions", typeof(decimal));
            dt.Columns.Add("bonus", typeof(decimal));
            dt.Columns.Add("overtime", typeof(decimal));
            dt.Columns.Add("adoi_paystub_frequency_id", typeof(int));
            dt.Columns.Add("employer_name", typeof(string));
            if (paystubs != null)
                foreach (var paystub in paystubs)
                    dt.Rows.Add(paystub.Ftw, paystub.Gross, paystub.TotalPreTax, paystub.Bonus, paystub.Overtime, (int)paystub.PayFrequency, paystub.EmployerName);
            return dt;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }

        [UsesSproc(DataAccessHelper.Database.IncomeBasedRepaymentUheaa, "GetDisclosureDate")]
        public DateTime? GetDisclosureDate(int? appId)
        {
            if (appId == null)
                return DateTime.Now;
            DateTime? discDate = LDA.ExecuteSingle<DateTime?>("GetDisclosureDate", Db,
                Sp("AppId", appId)).Result;
            return discDate ?? DateTime.Now;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "[dbo].[spLTDB_GetLetterInfo]")]
        public List<LetterInfo> GetCostCenterForLetter(string letterId)
        {
            return LDA.ExecuteList<LetterInfo>("[dbo].[spLTDB_GetLetterInfo]", DataAccessHelper.Database.Bsys, Sp("LetterId", letterId)).Result;
        }
    }
}