using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace KEYIDCHNG
{
    class DataAccess
    {
        private readonly LogDataAccess LDA;
        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        public WarehouseBorrowerDemographics GetDemographicData_Udw(string accountIdentifier)
        {
            return GetDemographicData(accountIdentifier, DB.Udw);
        }
        public WarehouseBorrowerDemographics GetDemographicData_Odw(string accountIdentifier)
        {
            return GetDemographicData(accountIdentifier, DB.Odw);
        }

        private WarehouseBorrowerDemographics GetDemographicData(string accountIdentifier, DB database)
        {
            return LDA.ExecuteList<WarehouseBorrowerDemographics>("dbo.BorrowersSearch", database, SqlParams.Single("AccountIdentifier", accountIdentifier)).Result.FirstOrDefault();
        }

        public bool HasAccess()
        {
#if DEBUG
            return true;
#endif
            return LDA.ExecuteSingle<bool>("[keyidchng].[HasAccess]", DB.Bsys, SqlParams.Single("WindowsUsername", Environment.UserName))?.Result ?? false;
        }
    }
}
