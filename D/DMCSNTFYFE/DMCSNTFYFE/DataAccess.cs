using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;

namespace DMCSNTFYFE
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(int plId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plId, false, true);
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "GetCoBorrowerAddress")]
        public BorrowerData GetCoBorrowerData(string ssn, char MorS, int sequence)
        {
            return LDA.ExecuteSingle<BorrowerData>("GetCoBorrowerAddress", DataAccessHelper.Database.Cdw,
             new SqlParameter("BF_SSN", ssn ), new SqlParameter("EDS_TYP", MorS), new SqlParameter("LN_SEQ", sequence)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetLfEdsFromSsn")]
        public string CoBorrowerExists(string ssn, int seq, char edsType)
        {
            return LDA.ExecuteSingle<string>("spGetLfEdsFromSsn", DataAccessHelper.Database.Cdw, 
            new SqlParameter("BF_SSN", ssn), new SqlParameter("LN_SEQ", seq), new SqlParameter("LC_TYP", edsType)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
        public string getBorrowerSSN(string acct)
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw,
            new SqlParameter("AccountNumber", acct)).Result;
        }

    }
}
