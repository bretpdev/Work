using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace CLSCHLLNFD
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Adds a single record to the SchoolClosure table in CLS
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.InsertRecord")]
        public bool AddRecordToTable(SchoolClosureData data, DisbursementData dData)
        {
            return LogRun.LDA.ExecuteSingle<bool>("clschllnfd.InsertRecord", DataAccessHelper.Database.Cls,
                SP("BF_SSN", data.BorrowerSsn.Trim()),
                SP("StudentSsn", data.StudentSsn.Trim()),
                SP("LN_SEQ", data.LoanSeq),
                SP("DisbursementSeq", dData.DisbursementSequence),
                SP("DischargeDate", dData.DischargeDate),
                SP("DischargeAmount", dData.DischargeAmount),
                SP("SchoolCode", data.SchoolCode)).Result;
        }

        /// <summary>
        /// Joins the LF_FED_AWD and LNFED_AWD_SEQ to compare the award id and return the LN_SEQ from FS10_DL_LON
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetLoanSequence")]
        public int GetLoanSequence(string awardId, string ssn)
        {
            return LogRun.LDA.ExecuteSingle<Int16>("clschllnfd.GetLoanSequence", DataAccessHelper.Database.Cls,
                SP("AwardId", awardId),
                SP("BF_SSN", ssn)).Result;
        }

        /// <summary>
        /// Gets the Disbursement Amount, Date and Sequence
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetDisbursementData")]
        public List<DisbursementData> GetDisbursements(string ssn, int loanSeq)
        {
            return LogRun.LDA.ExecuteList<DisbursementData>("clschllnfd.GetDisbursementData", DataAccessHelper.Database.Cls,
                SP("BF_SSN", ssn),
                SP("LN_SEQ", loanSeq)).Result;
        }

        /// <summary>
        /// Gets all the data that needs to be processed
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetProcessingData")]
        public ProcessingData GetProcessingData()
        {
            return LogRun.LDA.ExecuteSingle<ProcessingData>("clschllnfd.GetProcessingData", DataAccessHelper.Database.Cls).Result;
        }

        /// <summary>
        /// Gets the loan status from DW01
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "clschllnfd.GetLoanStatus")]
        public string CheckLoanStatus(ProcessingData data)
        {
            return LogRun.LDA.ExecuteSingle<string>("clschllnfd.GetLoanStatus", DataAccessHelper.Database.Cdw,
                SP("BF_SSN", data.BorrowerSsn),
                SP("LN_SEQ", data.LoanSeq)).Result;
        }

        /// <summary>
        /// Gets the date of the last payment made on the account
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "clschllnfd.GetLastPaymentDate")]
        public DateTime? GetPaymentDate(string ssn, int lnseq)
        {
            return LogRun.LDA.ExecuteSingle<DateTime?>("clschllnfd.GetLastPaymentDate", DataAccessHelper.Database.Cdw,
                SP("BF_SSN", ssn),
                SP("LN_SEQ", lnseq)).Result;
        }

        /// <summary>
        /// Updates the ArcAddProcessingId for the record that was processed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.UpdateArcAddId")]
        public void UpdateArcId(ProcessingData data, int arcAddProcessingId)
        {
            foreach (int loan in data.LoanSeqs)
            {
                LogRun.LDA.Execute("clschllnfd.UpdateArcAddId", DataAccessHelper.Database.Cls,
                    SP("BorrowerSsn", data.BorrowerSsn),
                    SP("LoanSeq", loan),
                    SP("AddedAt", data.AddedAt),
                    SP("ArcAddProcessingId", arcAddProcessingId));
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.UpdateErrorArcAddId")]
        public void UpdateErrorArcId(ProcessingData data, int loanSeq, int arcAddProcessingId)
        {
            LogRun.LDA.Execute("clschllnfd.UpdateErrorArcAddId", DataAccessHelper.Database.Cls,
                SP("BorrowerSsn", data.BorrowerSsn),
                SP("LoanSeq", loanSeq),
                SP("AddedAt", data.AddedAt),
                SP("ArcAddProcessingId", arcAddProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.[UpdateErrorLogArcAddId]")]
        public void UpdateErrorLogArcId(ProcessingData data, int arcAddProcessingId)
        {
            LogRun.LDA.Execute("clschllnfd.UpdateErrorLogArcAddId", DataAccessHelper.Database.Cls,
                SP("BorrowerSsn", data.BorrowerSsn),
                SP("ArcAddProcessingId", arcAddProcessingId));
        }

        /// <summary>
        /// Set the record as processed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.SetProcessedAt")]
        public void SetProcessedAt(int schoolClosureId)
        {
            LogRun.LDA.Execute("clschllnfd.SetProcessedAt", DataAccessHelper.Database.Cls,
                SP("SchoolClosureDataId", schoolClosureId));
        }

        /// <summary>
        /// Set the record as processed prior (indicating that it was previously written off and not written off on this run).
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.UpdateWasProcessedPrior")]
        public void UpdateWasProcessedPrior(int schoolClosureId)
        {
            LogRun.LDA.Execute("clschllnfd.UpdateWasProcessedPrior", DataAccessHelper.Database.Cls,
                SP("SchoolClosureDataId", schoolClosureId));
        }

        /// <summary>
        /// Gets the sum of all the discharge amounts for all the records added on the same day with the same loan sequence for the given borrower.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetDischargeTotal")]
        public List<DischargeRecords> GetDischargeTotal(BorrowerPrintRecord data, string school)
        {
            return LogRun.LDA.ExecuteList<DischargeRecords>("clschllnfd.GetDischargeTotal", DataAccessHelper.Database.Cls,
                SP("BF_SSN", data.BorrowerSsn),
                SP("SchoolCode", school),
                SP("Date", data.AddedAt)).Result;
        }

        /// <summary>
        /// Gets the school information tied to the borrowers loan sequence
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "clschllnfd.GetSchoolName")]
        public string GetSchoolName(string schoolCode)
        {
            return LogRun.LDA.ExecuteSingle<string>("clschllnfd.GetSchoolName", DataAccessHelper.Database.Cdw,
                SP("SchoolCode", schoolCode)).Result;
        }

        /// <summary>
        /// Adds all the data needed to print DSCSCHFED letter in CLS.[print].PrintProcessingId table
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].InsertPrintProcessingRecord")]
        public int? InsertPrintProcessing(string letterData, string accountNumber)
        {
            return EcorrProcessing.AddRecordToPrintProcessing(Program.ScriptId, "DSCSCHFED", letterData, accountNumber, "MA4481", DataAccessHelper.Region.CornerStone);
        }

        /// <summary>
        /// Updates the PrintProcessingId with the record the corresponds to the PrintProcessId in the [print].PrintProcessing table
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.UpdatePrintProcessingId")]
        public void UpdatePrintProcessingId(int schoolClosureDataId, int printProcessingId)
        {
            LogRun.LDA.Execute("clschllnfd.UpdatePrintProcessingId", DataAccessHelper.Database.Cls,
                SP("SchoolClosureDataId", schoolClosureDataId),
                SP("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Gets the loan detail data for the letter
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "clschllnfd.GetLoanDetail")]
        public LoanDetail GetLoanDetail(string borrowerSsn, int loanSeq)
        {
            return LogRun.LDA.ExecuteSingle<LoanDetail>("clschllnfd.GetLoanDetail", DataAccessHelper.Database.Cdw,
                SP("BF_SSN", borrowerSsn),
                SP("LN_SEQ", loanSeq)).Result;
        }

        /// <summary>
        /// Gets the loan sequences needed for the letter.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "clschllnfd.GetAllLoans")]
        public List<int> GetAllLoans(string borrowerSsn)
        {
            return LogRun.LDA.ExecuteList<int>("clschllnfd.GetAllLoans", DataAccessHelper.Database.Cdw,
                SP("BF_SSN", borrowerSsn)).Result;
        }

        /// <summary>
        /// Gets a list of all borrowers that have finished processing and are ready to have a letter sent
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetAccountsToPrint")]
        public List<PrintingData> GetPrintingAccounts()
        {
            return LogRun.LDA.ExecuteList<PrintingData>("clschllnfd.GetAccountsToPrint", DataAccessHelper.Database.Cls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetFinalArcData")]
        public List<FinalArcData> GetFinalArcData()
        {
            List<FinalArcData> data = LogRun.LDA.ExecuteList<FinalArcData>("clschllnfd.GetFinalArcData", DataAccessHelper.Database.Cls).Result;
            
            return data;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetAllUnprocessedLoanSequences")]
        public List<int> GetAllUnprocssedLoansForBorrower(string ssn, DateTime addedAt)
        {
            return LogRun.LDA.ExecuteList<int>("clschllnfd.GetAllUnprocessedLoanSequences", DataAccessHelper.Database.Cls, SP("BF_SSN", ssn), SP("AddedAt", addedAt)).Result;
        }

        /// <summary>
        /// Gathers info from the SchoolClosureData table to see whether or not
        /// there were write offs for any disbursements done prior to the current 
        /// run (usually manually done by the BU).
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetPriorProcessedDisbursements")]
        public List<int> GetPriorProcessedDisbursements(ProcessingData data)
        {
            return LogRun.LDA.ExecuteList<int>("clschllnfd.GetPriorProcessedDisbursements", DataAccessHelper.Database.Cls, SP("BF_SSN", data.BorrowerSsn), SP("AddedAt", data.AddedAt)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.InsertErrorRecord")]
        public void AddErrorRecord(ProcessingData data, string arc, string comment, string sessionMessage)
        {
            LogRun.LDA.Execute("clschllnfd.InsertErrorRecord", DataAccessHelper.Database.Cls, SP("BorrowerSsn", data.BorrowerSsn), SP("AccountNumber", data.AccountNumber), SP("LoanSeq", data.LoanSeq), SP("DisbursementSeq", data.DisbursementSeq), SP("Arc", arc), SP("ErrorMessage", comment), SP("SessionMessage", sessionMessage), SP("SchoolClosureDataId", data.SchoolClosureDataId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.GetAllErrorLogsForBorrower")]
        public List<ErrorData> GetErrorRecords(string borrowerSsn)
        {
            return LogRun.LDA.ExecuteList<ErrorData>("clschllnfd.GetAllErrorLogsForBorrower", DataAccessHelper.Database.Cls, SP("BorrowerSsn", borrowerSsn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "clschllnfd.UpdateFinalArcAddId")]
        public void UpdateFinalArcId(ProcessingData data, int arcAddProcessingId)
        {
            foreach (var loan in data.LoanSeqs)
            {
                var result = LogRun.LDA.Execute("clschllnfd.UpdateFinalArcAddId", DataAccessHelper.Database.Cls,
                      SP("BorrowerSsn", data.BorrowerSsn),
                      SP("LoanSeq", loan),
                      SP("AddedAt", data.AddedAt),
                      SP("ArcAddProcessingId", arcAddProcessingId));
            }
        }

        /// <summary>
        /// Gets the test school closure data for a given borrower and loan seq
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "clschllnfd.Test_GetSchoolDataForBorrower")]
        public TestSchoolDataForFile Test_GetSchoolDataForBorrower(string ssn, int loanSeq)
        {
            return LogRun.LDA.ExecuteSingle<TestSchoolDataForFile>("clschllnfd.Test_GetSchoolDataForBorrower", DataAccessHelper.Database.Cdw,
                SP("BF_SSN", ssn),
                SP("LN_SEQ", loanSeq)).Result;
        }


        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}