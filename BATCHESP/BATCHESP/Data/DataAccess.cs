using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BATCHESP
{
    public class DataAccess
    {
        LogDataAccess LDA { get; set; }
        DataAccessHelper.Database DB { get; set; }
        DataAccessHelper.Database Warehouse_DB { get; set; }

        public DataAccess(int processLogId, DataAccessHelper.Region region)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, false, true);
            DB = DataAccessHelper.Database.Uls;
            Warehouse_DB = DataAccessHelper.Database.Udw;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[AddNewWorkToTables]")]
        public bool AddNewWorkToTables()
        {
            return LDA.Execute("[batchesp].[AddNewWorkToTables]", DB);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetUnprocessedDefermentForbearancesByBorrowerSsn]")]
        public List<TsayDefermentForbearance> GetUnprocessedDefermentForbearances(string borrowerSsn)
        {
            var result = LDA.ExecuteList<TsayDefermentForbearance>("[batchesp].[GetUnprocessedDefermentForbearancesByBorrowerSsn]", DB, SP("BorrowerSSN", borrowerSsn));
            return result.Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetUnprocessedEspEnrollments]")]
        public List<EspEnrollment> GetUnprocessedEspEnrollments()
        {
            var result = LDA.ExecuteList<EspEnrollment>("[batchesp].[GetUnprocessedEspEnrollments]", DB);
            return result.Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetUnprocessedLoanInformationByBorrowerSsn]")]
        public List<Ts26LoanInformation> GetUnprocessedLoanInformation(string borrowerSsn)
        {
            var result = LDA.ExecuteList<Ts26LoanInformation>("[batchesp].[GetUnprocessedLoanInformationByBorrowerSsn]", DB, SP("BorrowerSSN", borrowerSsn));
            return result.Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetUnprocessedTs01EnrollmentsByBorrowerSsn]")]
        public List<Ts01Enrollment> GetUnprocessedTs01Enrollment(string borrowerSsn)
        {
            var result = LDA.ExecuteList<Ts01Enrollment>("[batchesp].[GetUnprocessedTs01EnrollmentsByBorrowerSsn]", DB, SP("BorrowerSSN", borrowerSsn));
            return result.Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetUnprocessedTs2hPendingDisbursementsByBorrowerSsn]")]
        public List<Ts2hPendingDisbursement> GetUnprocessedTs2hPendingDisbursements(string borrowerSsn)
        {
            var result = LDA.ExecuteList<Ts2hPendingDisbursement>("[batchesp].[GetUnprocessedTs2hPendingDisbursementsByBorrowerSsn]", DB, SP("BorrowerSSN", borrowerSsn));
            return result.Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetUnprocessedParentPlusLoanDetailsByBorrowerSsn]")]
        public List<ParentPlusLoanDetailsInformation> GetUnprocessedParentPlusLoanDetails(string borrowerSsn)
        {
            var result = LDA.ExecuteList<ParentPlusLoanDetailsInformation>("[batchesp].[GetUnprocessedParentPlusLoanDetailsByBorrowerSsn]", DB, SP("BorrowerSSN", borrowerSsn));
            return result.Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetEspEnrollmentAsProcessed]")]
        public bool SetEspEnrollmentAsProcessed(int espEnrollmentId)
        {
            return LDA.Execute("[batchesp].[SetEspEnrollmentAsProcessed]", DB, SP("EspEnrollmentId", espEnrollmentId));
        }

        /// <summary>
        /// Gets the login info for the current User running the script
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spGetDecrpytedPassword")]
        public string GetPasswordForBatchProcessing(string userId)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, new SqlParameter("UserId", userId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetTs01EnrollmentAsProcessed]")]
        public bool SetTs01EnrollmentAsProcessed(int ts01EnrollmentId)
        {
            return LDA.Execute("[batchesp].[SetTs01EnrollmentAsProcessed]", DB, SP("Ts01EnrollmentId", ts01EnrollmentId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetTs26LoanInformationAsProcessed]")]
        public bool SetTs26LoanInformationAsProcessed(int ts26LoanInformationId)
        {
            return LDA.Execute("[batchesp].[SetTs26LoanInformationAsProcessed]", DB, SP("Ts26LoanInformationId", ts26LoanInformationId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetTsayDefermentForbearanceAsProcessed]")]
        public bool SetTsayDefermentForbearanceAsProcessed(int tsayDefermentForbearanceId)
        {
            return LDA.Execute("[batchesp].[SetTsayDefermentForbearanceAsProcessed]", DB, SP("TsayDefermentForbearanceId", tsayDefermentForbearanceId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetTs2hPendingDisbursementAsProcessed]")]
        public bool SetTs2hPendingDisbursementAsProcessed(int ts2hPendingDisbursementId)
        {
            return LDA.Execute("[batchesp].[SetTs2hPendingDisbursementAsProcessed]", DB, SP("Ts2hPendingDisbursementId", ts2hPendingDisbursementId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetParentPlusLoanDetailAsProcessed]")]
        public bool SetParentPlusLoanDetailInformationAsProcessed(int parentPlusLoanDetailInformationId)
        {
            return LDA.Execute("[batchesp].[SetParentPlusLoanDetailAsProcessed]", DB, SP("ParentPlusLoanDetailInformationId", parentPlusLoanDetailInformationId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetNonSelectionReasons]")]
        public List<NonSelectionReason> GetNonSelectionReasons()
        {
            var result = LDA.ExecuteList<NonSelectionReason>("[batchesp].[GetNonSelectionReasons]", DB);
            return result.Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetAccessQueues]")]
        public List<QueueAndSubQueue> GetAccessQueues()
        {
            var result = LDA.ExecuteList<QueueAndSubQueue>("[batchesp].[GetAccessQueues]", DB);
            return result.Result;
        }


        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[AddTs26ScrapedLoanInformation]")]
        public bool AddTs26ScrapedLoanInformation(string borrowerSsn, int loanSequence, string loanStatus, DateTime? repaymentStartDate)
        {
            var result = LDA.Execute("[batchesp].AddTs26ScrapedLoanInformation", DB,
                SP("BorrowerSsn", borrowerSsn), SP("LoanSequence", loanSequence), SP("LoanStatus", loanStatus), SP("RepaymentStartDate", repaymentStartDate));
            return result;
        }


        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[AddTsayScrapedLoanInformation]")]
        public bool AddTsayScrapedLoanInformation(string borrowerSsn, int loanSequence, string loanProgramType, DateTime? beginDate, DateTime? endDate, DateTime? certificationDate, DateTime? disbursementDate, string deferSchool, string approvalStatus, string dfType, DateTime? appliedDate, string sourceModule)
        {
            var result = LDA.Execute("[batchesp].AddTsayScrapedLoanInformation", DB,
                SP("BorrowerSsn", borrowerSsn), SP("LoanSequence", loanSequence), SP("LoanProgramType", loanProgramType), SP("BeginDate", beginDate), SP("EndDate", endDate), SP("CertificationDate", certificationDate), SP("DisbursementDate", disbursementDate), SP("DeferSchool", deferSchool), SP("ApprovalStatus", approvalStatus), SP("Type", dfType), SP("AppliedDate", appliedDate), SP("SourceModule", sourceModule));
            return result;
        }


        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[GetSemesterGaps]")]
        public List<SemesterGaps> GetSemesterGaps(string borrowerSsn, int? loanSequence)
        {
            var result = LDA.ExecuteList<SemesterGaps>("[batchesp].GetSemesterGaps", DB, SP("BorrowerSsn", borrowerSsn), SP("LoanSequence", loanSequence));
            return result.Result;
        }

        public void Test_MarkBorrowerUnprocessed(string borrowerIdentifier)
        {
            if (DataAccessHelper.TestMode)
                LDA.Execute("[batchesp].[Test_MarkBorrowerUnprocessed]", DB, SP("BorrowerIdentifier", borrowerIdentifier));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetTsayScrapedLoanInformationAsProcessed]")]
        public void SetTsayScrapedLoanInformationAsProcessed(string borrowerSsn, int? loanSequence)
        {
            LDA.Execute("[batchesp].[SetTsayScrapedLoanInformationAsProcessed]", DB, SP("BorrowerSsn", borrowerSsn), SP("LoanSequence", loanSequence));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[InsertIntoQueueCompleter]")]
        public bool InsertIntoQueueCompleter(EspEnrollment esp)
        {
            var result = LDA.Execute("[batchesp].[InsertIntoQueueCompleter]", DB, SP("Queue", esp.Queue), SP("SubQueue", esp.SubQueue), SP("TaskControlNumber", esp.TaskControlNumber), SP("AccountNumber", esp.AccountNumber));
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[batchesp].[SetTaskAsRequiresReview]")]
        public void SetTaskAsRequiresReview(int espEnrollmentId)
        {
            LDA.Execute("[batchesp].[SetTaskAsRequiresReview]", DB, SP("EspEnrollmentId", espEnrollmentId));
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[batchesp].[IsASplitLoan]")]
        public bool IsASplitConsolLoan(string borrowerSsn, int loanSeq)
        {
            return LDA.ExecuteSingle<bool>("[batchesp].[IsASplitLoan]", Warehouse_DB, SP("BorrowerSsn", borrowerSsn), SP("LoanSeq", loanSeq)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[batchesp].[HasOpenReviewTask]")]
        public bool BorrowerAlreadyHasAReviewTask(string borrowerSsn)
        {
            return LDA.ExecuteSingle<bool>("[batchesp].[HasOpenReviewTask]", Warehouse_DB, SP("BorrowerSsn", borrowerSsn)).Result;
        }

        private SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
