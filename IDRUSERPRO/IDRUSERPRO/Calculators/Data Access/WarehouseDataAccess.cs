using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace IDRUSERPRO
{
    public class WarehouseDataAccess
    {
        private LogDataAccess lda;
        private DataAccessHelper.Database db;
        public WarehouseDataAccess(LogDataAccess lda, DataAccessHelper.Region region)
        {
            this.lda = lda;
            if (region == DataAccessHelper.Region.Uheaa)
                db = DataAccessHelper.Database.Udw;
            else
                throw new NotSupportedException();
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[idruserpro].[GetTotalLoanDebt]")]
        public decimal GetTotalDebt(string accountIdentifier)
        {
            return lda.ExecuteList<decimal>("[idruserpro].[GetTotalLoanDebt]", db, Sp("AccountIdentifier", accountIdentifier)).Result.SingleOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[idruserpro].[GetOriginalLoans]")]
        public List<OriginalLoan> GetOriginalLoans(string accountIdentifier)
        {
            return lda.ExecuteList<OriginalLoan>("[idruserpro].[GetOriginalLoans]", db, Sp("AccountIdentifier", accountIdentifier)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "[idruserpro].GetCurrentBalance")]
        public decimal GetCurrentBalance(string accountIdentifier, int loanSequence)
        {
            return lda.ExecuteSingle<decimal>("[idruserpro].GetCurrentBalance", db, Sp("AccountIdentifier", accountIdentifier), Sp("LoanSequence", loanSequence)).Result;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }


    }
}