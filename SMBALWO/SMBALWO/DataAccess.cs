using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace SMBALWO
{
    class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun) => LDA = logRun.LDA;

        [UsesSproc(Uls, "[smbalwo].GetLoansFromWarehouse")]
        public void LoadAccountsFromWarehouse()
        {
            LDA.Execute("[smbalwo].GetLoansFromWarehouse", Uls); //NON TILP LOANS
            LDA.Execute("[smbalwo].GetLoansTILPFromWarehouse", Uls); //TILP LOANS
        }

        [UsesSproc(Uls, "[smbalwo].GetBorrowersForProcessing")]
        public List<BorrowerData> LoadAndPullWriteOffData(bool isTILP) =>
                LDA.ExecuteList<BorrowerData>("[smbalwo].GetBorrowersForProcessing", Uls, Sp("TILPOnly", isTILP)).CheckResult();

        [UsesSproc(Uls, "[smbalwo].GetEffectiveDate")]
        public DateTime? GetEffectiveDate(BorrowerData bor) =>
             LDA.ExecuteSingle<DateTime?>("[smbalwo].GetEffectiveDate", Uls,
                Sp("AccountNumber", bor.AccountNumber),
                Sp("LoanWriteOffId", bor.LoanWriteOffId),
                Sp("LN_SEQ", bor.LoanSequence)).Result;

        /// <summary>
        /// Gets the Max LN90.PC_FAT_TYP + LN90.PC_FAT_SUB_TYP
        /// </summary>
        [UsesSproc(Uls, "smbalwo.GetMaxFinancialTransaction")]
        public string GetMaxFinancialTransaction(BorrowerData bor) =>
             LDA.ExecuteSingle<string>("smbalwo.GetMaxFinancialTransaction", Uls,
                Sp("AccountNumber", bor.AccountNumber),
                Sp("LN_SEQ", bor.LoanSequence),
                Sp("LoanWriteOffId", bor.LoanWriteOffId)).Result;

        [UsesSproc(Uls, "[smbalwo].MarkRecordAsProcessed")]
        public void MarkRecordProcessed(int loanWriteOffId, bool hadError, decimal? principalWrittenOff = null, decimal? InterestWrittenOff = null)
        {
            if (hadError)//If an error occured the principal and interest were not written off
                LDA.Execute("[smbalwo].MarkRecordAsProcessed", Uls,
                    Sp("LoanWriteOffId", loanWriteOffId),
                    Sp("HadError", hadError));
            else
                LDA.Execute("[smbalwo].MarkRecordAsProcessed", Uls, Sp("LoanWriteOffId", loanWriteOffId),
                    Sp("HadError", hadError),
                    Sp("ActualPrincipalWrittenOff", principalWrittenOff.Value),
                    Sp("ActualInterestWrittenOffint", InterestWrittenOff.Value));
        }

        private SqlParameter Sp(string name, object value) => SqlParams.Single(name, value);
    }
}