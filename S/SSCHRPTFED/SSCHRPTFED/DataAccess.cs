using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace SSCHRPTFED
{
    class DataAccess
    {
        [UsesSproc(DataAccessHelper.Database.Cls, "spSSRS_GetSchoolInfo")]
		public static List<SchoolInfo> GetSchoolInfoFromDB()
		{
			return DataAccessHelper.ExecuteList<SchoolInfo>("spSSRS_GetSchoolInfo", DataAccessHelper.Database.Cls);
		}
    }
}
