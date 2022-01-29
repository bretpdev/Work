using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace PWRATRNY
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) => 
            LDA = lda;

        [UsesSproc(Bsys, "dbo.spGENR_GetStateCodes")]
        public List<StateCode> GetStateCodes() =>
            LDA.ExecuteList<StateCode>("dbo.spGENR_GetStateCodes", Bsys, Sp("IncludeTerritories", true)).Result;

        /// <summary>
        /// Gets a list of relationships to display on the form
        /// </summary>
        [UsesSproc(Uls, "pwratrny.GetRelationships")]
        public List<Relationship> GetRelationships() =>
            LDA.ExecuteList<Relationship>("pwratrny.GetRelationships", Uls).Result;

        [UsesSproc(Odw, "GetSystemBorrowerDemographics")]
        [UsesSproc(Udw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetDemos(string account)
        {
            SystemBorrowerDemographics demos = LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", Odw, Sp("AccountIdentifier", account)).Result;
            demos ??= LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", Udw, Sp("AccountIdentifier", account)).Result;
            return demos ?? null;
        }

        private SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}