using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;


namespace PAYARRREHB
{
    public class DataAccess
    {
        private LogDataAccess LDA;

        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }


        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_CheckUserAccessByKey")]
        public int CheckUserAccess()
        {
            return LDA.ExecuteSingle<int>("spSYSA_CheckUserAccessByKey", DataAccessHelper.Database.Csys,
                new SqlParameter("WindowsUserName", Environment.UserName),
                new SqlParameter("UserKey", "PARHB")).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Odw, "GetCoBorrowersForBorrower")]
        public List<CoBorrower> GetCoborrowers(string account_ssn)
        {
            return LDA.ExecuteList<CoBorrower>("[GetCoBorrowersForBorrower]", DataAccessHelper.Database.Odw,
                new SqlParameter("BorrowerSSN", account_ssn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Odw, "[isCoborrowerInGA01]")]
        public List<CobInfo> CoborrowerHasLoans(string ssn)
        {
            return LDA.ExecuteList<CobInfo>("[isCoborrowerInGA01]", DataAccessHelper.Database.Odw,
                new SqlParameter("SSN", ssn)).Result;
        }
    }
}
