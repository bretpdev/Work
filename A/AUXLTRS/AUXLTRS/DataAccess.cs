using System.Data.SqlClient;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;

namespace AUXLTRS
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public string ScriptId { get { return "AUXLTRS"; }  }

        public DataAccess(int plId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plId, false, true);
        }


        /// <summary>
        /// Gets a list of private loan types
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "LoanTypesGetByKey")]
        public List<string> GetPrivateLoanTypes()
        {
            return LDA.ExecuteList<string>("LoanTypesGetByKey", DataAccessHelper.Database.Bsys,
                SqlParams.Single("TypeKey", "Private")).Result;
        }

        /// <summary>
        /// Gets a list f all state code
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "StateCodesGetDomestic")]
        public List<string> GetStateAbbreviations()
        {
            return LDA.ExecuteList<string>("StateCodesGetDomestic", DataAccessHelper.Database.Bsys).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "GetLfEdsFromSsn")]
        public string CoBorrowerExists(string ssn, int seq, char edsType)
        {
            return LDA.ExecuteSingle<string>("spGetLfEdsFromSsn", DataAccessHelper.Database.Cdw, 
            new SqlParameter("BF_SSN", ssn), new SqlParameter("LN_SEQ", seq), new SqlParameter("LC_TYP", edsType)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetCoBorrowersForBorrower")]
        public List<CoBorrowers> GetCoBorrowers(string account)
        {
            return DataAccessHelper.ExecuteList<CoBorrowers>("[print].[GetCoBorrowersForBorrower]", DataAccessHelper.Database.Uls,
                SqlParams.Single("BorrowerSsn", account));
        }


    }
}