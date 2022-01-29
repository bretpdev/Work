using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace SPECCMPFED
{
    class DataAccess
    {
        [UsesSprocAttribute(DataAccessHelper.Database.Cls, "spSCFUGetArcAndComment")]
        public static Result GetArcAndComment(string resultCode)
        {
            try
            {
                return DataAccessHelper.ExecuteSingle<Result>("spSCFUGetArcAndComment", DataAccessHelper.Database.Cls, SqlParams.Single("ResultCode", resultCode));
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
