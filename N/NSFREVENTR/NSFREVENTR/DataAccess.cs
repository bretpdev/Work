using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Data.Linq;

namespace NSFREVENTR
{
    public class DataAccess : DataAccessBase
    {

        public static List<NSFReason> GetListOfNSFReasons(bool testMode)
        {
            string query = "SELECT (CAST(CAST(Code AS INT) AS VARCHAR(50)) + ' - ' + Description) as Text, Description, Code FROM BORG_LST_NSFReasons ORDER BY Code";
            return BsysDataContext(testMode).ExecuteQuery<NSFReason>(query).ToList();
        }

    }
}
