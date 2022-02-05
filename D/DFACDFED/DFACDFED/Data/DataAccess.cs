using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DFACDFED
{
    public class DataAccess
    {
        LogDataAccess lda;
        public DataAccess(int processLogId)
        {
            this.lda = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, true, false);
        }

        static string[] letterIds = new string[] { "TS06BD101", "TS06BF101", "TS06BF101C", "TS06BF101J" };
        [UsesSproc(DataAccessHelper.Database.Cdw, "[GetUnprocessedLettersByLetterId]")]
        public List<UnprocessedLetter> GetAllUnprocessedLetters()
        {
            List<UnprocessedLetter> results = new List<UnprocessedLetter>();
            foreach (var id in letterIds)
            {
                var letterResult = lda.ExecuteList<UnprocessedLetter>("[GetUnprocessedLettersByLetterId]",
                    DataAccessHelper.Database.Cdw, SqlParams.Single("LetterId", id));
                if (letterResult.DatabaseCallSuccessful)
                    results.AddRange(letterResult.Result);
            }
            return results;
        }

        public ManagedDataResult<List<LoanInfo>> GetLoanInfo(UnprocessedLetter letter, DateTime today)
        {
            string sproc = "GetLetterData_" + letter.LetterId;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParams.Single("AccountNumber", letter.AccountNumber));
            parameters.Add(SqlParams.Single("Today", today));
            return lda.ExecuteList<LoanInfo>(sproc, DataAccessHelper.Database.Cdw, parameters.ToArray());
        }

        public DataTable GenerateTable(List<LoanInfo> loanInfo)
        {
            DataTable dt = new DataTable();
            foreach (var col in new string[] { "ACTION", "FORB OR DEFER", "TYPE", "BEGIN DATE", "END DATE", "LOAN SEQ", "LOAN PROGRAM" })
                dt.Columns.Add(new DataColumn(col));
            foreach (var loan in loanInfo)
                dt.Rows.Add(Action(loan.LetterAction), loan.LetterTypeCode, loan.LetterType, loan.BeginDate.ToShortDateString(), loan.EndDate.ToShortDateString(), loan.LoanSequence, loan.LoanProgram);
            return dt;
        }

        private string Action(char actionCode)
        {
            switch (actionCode)
            {
                case 'A':
                    return "Approved";
                case 'C':
                    return "Changed";
                case 'D':
                    return "Denied";
            }
            return null;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "Lt20MarkLetterPrinted")]
        public bool UpdatePrinted(int letterSeq, string letterId, string accountNumber)
        {
            return lda.Execute("Lt20MarkLetterPrinted", DataAccessHelper.Database.Cdw, SqlParams.Single("LetterSeq", letterSeq),
                SqlParams.Single("LetterId", letterId), SqlParams.Single("AccountNumber", accountNumber));
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "GetAddress")]
        public AddressInfo GetAddressInfo(string accountNumber)
        {
            try
            {
                var result = lda.ExecuteSingle<AddressInfo>("GetAddress", DataAccessHelper.Database.Cdw,
                    SqlParams.Single("AccountNumber", accountNumber));

                if (result.DatabaseCallSuccessful)
                    return result.Result;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public bool InactivateSystemLetter(int letterSeq, string letterId, string accountNumber)
        {
            return lda.Execute("InactivateSystemLetter", DataAccessHelper.Database.Cdw, SqlParams.Single("SystemLetterExclusionReasonId", 5),
                SqlParams.Single("LetterSeq", letterSeq), SqlParams.Single("LetterId", letterId), SqlParams.Single("AccountNumber", accountNumber));
        }
    }
}
