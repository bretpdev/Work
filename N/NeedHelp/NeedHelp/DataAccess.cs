using SubSystemShared;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace NeedHelp
{
    class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) => LDA = lda;

        [UsesSproc(Csys, "spGENR_GetBusinessFunctions")]
        public List<string> GetBusinessFunctions() => LDA.ExecuteList<string>("spGENR_GetBusinessFunctions", Csys).Result.ToList();

        [UsesSproc(Csys, "spGENR_GetBusinessUnits")]
        public List<BusinessUnit> GetBusinessUnits() => LDA.ExecuteList<BusinessUnit>("spGENR_GetBusinessUnits", Csys).Result.ToList();

        [UsesSproc(Csys, "spSYSA_GetSqlUsers")]
        public List<SqlUser> GetSqlUsers(bool includeInactiveBorrowers) => LDA.ExecuteList<SqlUser>("spSYSA_GetSqlUsers", Csys,
                SqlParams.Single("IncludeInactiveRecords", includeInactiveBorrowers)).Result;
    }
}