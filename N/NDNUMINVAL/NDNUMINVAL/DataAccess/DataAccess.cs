using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
namespace NDNUMINVAL
{
    public class DataAccess
    {
        private ProcessLogRun PLR { get; set; }

        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
        }

        public enum Region
        {
            OneLink,
            UheaaCompass
        }

        /// <summary>
        /// Populate a list of objects with data from Noble's RAS database
        /// <param name="region">OneLink or UheaaCompass</param>
        /// <param name="logData">ProcessLogData Object</param>
        /// </summary>
        /// <returns>A list of NobleData objects</returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "ndnuminval.GetCalls")]
        public List<NobleData> LoadNobleData(Region region)
        {
            string campaignsRegion = region == Region.UheaaCompass ? "CompassUheaa" : "OneLink";
            return PLR.LDA.ExecuteList<NobleData>("ndnuminval.GetCalls", DataAccessHelper.Database.Uls, new SqlParameter("Region", campaignsRegion)).Result;
        }

        /// <summary>
        /// Takes and account number and returns an ssn.  Current region is taken into account
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns>SSN for account number passed in.</returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "spGetSSNFromAcctNumber")]
        public string GetSSNFromAccountNumber(string accountNumber)
        {
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
                return DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, new SqlParameter("AccountNumber", accountNumber));
            else
                return "";
        }
    }
}
