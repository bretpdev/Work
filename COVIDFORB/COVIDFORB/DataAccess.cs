using COVIDFORB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace COVIDFORB
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.GetEndDate")]
        public Dates GetEndDate()
        {
            return LDA.ExecuteSingle<Dates>("covidforb.GetEndDate", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.InsertProcessingRecord")]
        public bool InsertProcessingRecord(string accountNumber, DateTime startDate, DateTime endDate, bool forbToClearDelinquency, bool comakerEligibility)
        {
            return LDA.ExecuteSingle<bool>("covidforb.InsertProcessingRecord", DataAccessHelper.Database.Uls, Sq("AccountNumber", accountNumber), Sq("StartDate", startDate), Sq("EndDate", endDate), Sq("ForbToClearDelq", forbToClearDelinquency ? "Y" : "N"), Sq("ComakerEligibility", comakerEligibility ? "Y" : "N")).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "spGetSSNFromAcctNumber")]
        internal string GetSsnFromAccountNumber(string accountNumber)
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, Sq("AccountNumber", accountNumber)).Result;
        }

        internal SystemBorrowerDemographics GetDemos(string accountNumber)
        {
            return LDA.ExecuteList<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", DataAccessHelper.Database.Udw, Sq("AccountIdentifier", accountNumber)).Result.FirstOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.GetDelinquentPopulation")]
        public List<ForbGenerationRecord> GetDelinquencyPopulation()
        {
            return LDA.ExecuteList<ForbGenerationRecord>("covidforb.GetDelinquentPopulation", DataAccessHelper.Database.Uls).Result;
        }


        [UsesSproc(DataAccessHelper.Database.Uls, "[covidforb].[GetClaimsManualReviewPopulation]")]
        public List<ManualReviewRecord> GetClaimsManualReviewPopulation()
        {
            return LDA.ExecuteList<ManualReviewRecord>("[covidforb].[GetClaimsManualReviewPopulation]", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.[GetUnprocessedForb]")]
        public List<ForbProcessingRecord> GetUnprocessedForb()
        {
            return LDA.ExecuteList<ForbProcessingRecord>("covidforb.[GetUnprocessedForb]", DataAccessHelper.Database.Uls).Result ?? new List<ForbProcessingRecord>();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.[GetUnprocessedForbArcs]")]
        public List<ArcProcessingRecord> GetUnprocessedForbArcs()
        {
            return LDA.ExecuteList<ArcProcessingRecord>("covidforb.[GetUnprocessedForbArcs]", DataAccessHelper.Database.Uls).Result ?? new List<ArcProcessingRecord>();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.[GetSelectedLoans]")]
        public List<int> GetSelectedLoans(long forbProcessingId)
        {
            List<short> lonSeq =  LDA.ExecuteList<short>("covidforb.[GetSelectedLoans]", DataAccessHelper.Database.Uls, Sq("ForbProcessingId", forbProcessingId)).Result;
            return lonSeq.Select(s => (int)s).ToList();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.[GetBusinessUnit]")]
        public BusinessUnits GetBusinessUnit(long businessUnitId)
        {
            return LDA.ExecuteSingle<BusinessUnits>("covidforb.[GetBusinessUnit]", DataAccessHelper.Database.Uls, Sq("BusinessUnitId", businessUnitId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.[SetProcessedOn]")]
        public bool SetProcessedOn(long forbProcessingId, string arcComment, bool failure)
        {
            return LDA.Execute("covidforb.[SetProcessedOn]", DataAccessHelper.Database.Uls, Sq("ForbProcessingId", forbProcessingId), Sq("ArcComment", arcComment), Sq("Failure", failure));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.[SetArcAddProcessingId]")]
        public bool SetArcAddProcessingId(long forbProcessingId, int arcAddProcessingId, int? printProcessingId)
        {
            return LDA.Execute("covidforb.[SetArcAddProcessingId]", DataAccessHelper.Database.Uls, Sq("ForbProcessingId", forbProcessingId), Sq("ArcAddProcessingId", arcAddProcessingId), Sq("PrintProcessingId", printProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "covidforb.[UpdateInvalidAddressNoEcorr]")]
        public bool SetInvalidAddressNoEcorr(long forbProcessingId)
        {
           return  LDA.Execute("covidforb.[UpdateInvalidAddressNoEcorr]", DataAccessHelper.Database.Uls, Sq("ForbearanceProcessingId", forbProcessingId));
        }

        public SqlParameter Sq(string dbName, object dbValue)
        {
            return SqlParams.Single(dbName, dbValue);
        }
    }
}
